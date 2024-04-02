using static SciChain.Orcid;
using static SciChain.Blockchain;
using NetCoreServer;
using System.Transactions;
using Gtk;
using Pango;
using System.Reflection;
using System;
namespace SciChain
{
    public partial class MainForm : Window
    {
        #region Properties

        /// <summary> Used to load in the glade file resource as a window. </summary>
        private Builder _builder;
        private static MainForm form;
        int line = 0;
#pragma warning disable 649
        [Builder.Object]
        private Label balanceLabel;
        [Builder.Object]
        private Label reputationLabel;
        [Builder.Object]
        private Entry addressBox;
        [Builder.Object]
        private SpinButton amountBox;
        [Builder.Object]
        private Entry sendToNameBox;
        [Builder.Object]
        private Button sendBut;
        [Builder.Object]
        private Button getAddressBut;
        [Builder.Object]
        private Entry doiBox;
        [Builder.Object]
        private ComboBox authorsBox;
        [Builder.Object]
        private Entry nameBox;
        [Builder.Object]
        private Button addByNameBut;
        [Builder.Object]
        private Entry idBox;
        [Builder.Object]
        private Button addByIDBut;
        [Builder.Object]
        private Button createBlockBut;
        [Builder.Object]
        private Entry passwordBox;
        [Builder.Object]
        private Button loginBut;
        [Builder.Object]
        private ComboBox pendingBox;
        [Builder.Object]
        private Button peerReviewBut;
        [Builder.Object]
        private Button failReviewBut;
        [Builder.Object]
        private Label statusLabel;
#pragma warning restore 649

        #endregion

        #region Constructors / Destructors

        /// Create a new BioConsole object using the Glade file "BioGTK.Glade.BioConsole.glade"
        /// @return A new instance of the BioConsole class.
        public static MainForm Create()
        {
            Builder builder = new Builder(new FileStream(System.IO.Path.GetDirectoryName(Environment.ProcessPath) + "/" + "Glade/MainForm.glade", FileMode.Open));
            return new MainForm(builder, builder.GetObject("mainform").Handle);
        }

        /// <summary> Specialised constructor for use only by derived class. </summary>
        /// <param name="builder"> The builder. </param>
        /// <param name="handle">  The handle. </param>
        protected MainForm(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
            form = this;
        }

        #endregion

        #region Handlers

        /// <summary> Sets up the handlers. </summary>
        protected void SetupHandlers()
        {
            getAddressBut.Clicked += getAddrBut_Click;
            sendBut.Clicked += sendBut_Click;
            addByIDBut.Clicked += addByIDBut_Click;
            addByNameBut.Clicked += addByNameBut_Click;
            createBlockBut.Clicked += createBut_Click;
            loginBut.Clicked += loginBut_Click;
            peerReviewBut.Clicked += peerReviewBut_Click;
            failReviewBut.Clicked += flagBut_Click;
            statusLabel.ButtonPressEvent += StatusLabel_ButtonPressEvent;
            this.DestroyEvent += MainForm_DestroyEvent;
        }

        private void StatusLabel_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Clipboard clipboard = Clipboard.Get(Gdk.Selection.Clipboard);
            // Set text to the clipboard
            clipboard.Text = ORCID.ORCID;
        }

        private void MainForm_DestroyEvent(object o, DestroyEventArgs args)
        {
            Save();
            wallet.Save(passwordBox.Buffer.Text);
        }

        #endregion
        private StringWriter _writer;
        private Blockchain.Wallet wallet;
        private OAuthTokenResponse ORCID;
        string token;
        public string peer = "92.205.238.105";
        private async void loginBut_Click(object? sender, EventArgs e)
        {
            _writer = new StringWriter();
            Console.SetOut(_writer);
            token = OAuthHelper.StartListenerAsync().Result;
            ORCID = await Orcid.GetAccessToken(token);
            wallet = new Blockchain.Wallet();
            wallet.Load(passwordBox.Buffer.Text);
            Initialize(wallet);
            ChatClient cl = new ChatClient(peer, 8333);
            cl.ConnectAsync();
            ConnectToPeer(peer, cl, 8333);
            Blockchain.Load();
            StartTimer();
            statusLabel.Text = "Logged In:" + ORCID.Name + " " + ORCID.ORCID;
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.registration, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            AddTransaction(tr);
            GetPending(Peers.First().Value, PendingBlocks.Count);
        }
        private static void Timer()
        {
            do
            {
                try
                {
                    form.statusLabel.Text = "Connections:" + Peers.Count.ToString() + " Height: " + Chain.Count + " Treasury:" + GetTreasury();
                    form.balanceLabel.Text = "Balance: " + GetBalance(form.ORCID.ORCID).ToString();
                    form.reputationLabel.Text = "Reputation: " + GetReputation(form.ORCID.ORCID).ToString();
                    form.pendingBox.Clear();
                    ListStore ls = new ListStore(typeof(string));
                    foreach (var item in PendingBlocks)
                    {
                        ls.AppendValues(item);
                    }
                    // Create a CellRendererText and pack it into the ComboBox
                    CellRendererText cell = new CellRendererText();
                    form.pendingBox.PackStart(cell, false);
                    form.pendingBox.AddAttribute(cell, "text", 0); // Associate the renderer with the first column of the model
                    form.pendingBox.Model = ls;
                    
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            } while (true);
            
        }
        private void StartTimer()
        {
            Thread th = new Thread(Timer);
            th.Start();
        }

        private void AddItem(ComboBox box, string st)
        {
            ListStore ls = new ListStore(typeof(string));
            TreeIter iter;
            if(box.Model == null)
            {
                box.Model = new ListStore(typeof(string));
            }
            // Check if the model has a first row
            if (box.Model.GetIterFirst(out iter))
            {
                do
                {
                    // Retrieve the value in the first column of the current row
                    string item = (string)box.Model.GetValue(iter, 0);
                    ls.AppendValues(item);
                }
                while (box.Model.IterNext(ref iter)); // Move to the next row
            }
            ls.AppendValues(st);
            // Create a CellRendererText and pack it into the ComboBox
            CellRendererText cell = new CellRendererText();
            box.PackStart(cell, false);
            box.AddAttribute(cell, "text", 0); // Associate the renderer with the first column of the model
            box.Model = ls;
        }

        private void createBut_Click(object? sender, EventArgs e)
        {
            List<string> list = new List<string>();
            ListStore ls = new ListStore(typeof(string));
            TreeIter iter;
            // Check if the model has a first row
            if (pendingBox.Model.GetIterFirst(out iter))
            {
                do
                {
                    // Retrieve the value in the first column of the current row
                    string item = (string)pendingBox.Model.GetValue(iter, 0);
                    list.Add(item);
                }
                while (pendingBox.Model.IterNext(ref iter)); // Move to the next row
            }
            Block.Document doc = new Block.Document(doiBox.Text, list,wallet.PublicKey);
            doc.SignDocument(wallet.PrivateKey,ORCID.ORCID);
            Block bl = new Block(DateTime.Now, GetLatestBlock().Hash, null);
            bl.BlockDocument = doc;
            AddPendingBlock(bl);
            //MineBlock(ORCID.ORCID, doc, wallet.PublicKey);
        }

        private void sendBut_Click(object? sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.transaction, ORCID.ORCID, wallet.PublicKey, addressBox.Buffer.Text, (decimal)amountBox.Value);
            tr.SignTransaction(wallet.PrivateKey);
            bool res = VerifyTransaction(tr);

            AddTransaction(tr);
        }

        private async void addByNameBut_Click(object? sender, EventArgs e)
        {
            string id = await Orcid.SearchForORCID(nameBox.Text);
            if (id == null) return;
            AddItem(authorsBox, id);
        }

        private async void addByIDBut_Click(object? sender, EventArgs e)
        {
            bool id = await Orcid.CheckORCIDExistence(idBox.Text);
            if (id)
                AddItem(authorsBox, idBox.Text);
        }

        private void updateBut_Click(object? sender, EventArgs e)
        {
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            reputationLabel.Text = "Reputation: " + GetReputation(ORCID.ORCID).ToString();
        }

        private async void getAddrBut_Click(object? sender, EventArgs e)
        {
            addressBox.Buffer.Text = await Orcid.SearchForORCID(sendToNameBox.Text);
        }
        private void peerReviewBut_Click(object? sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.review, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            if (pendingBox.Active > -1)
            {
                tr.Data = PendingBlocks[pendingBox.Active].GUID.ToString();
                AddTransaction(tr);
            }
        }
        /*
        private void toolStripStatusLabel_Click(object? sender, EventArgs e)
        {
            Clipboard.SetText(ORCID.ORCID.ToString());
        }
        */
        private void flagBut_Click(object? sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.flag, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            tr.Data = PendingBlocks[pendingBox.Active].GUID.ToString();
            AddTransaction(tr);
        }
    }
}
