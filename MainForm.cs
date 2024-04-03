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
        private Button copyBut;
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

        /// <summary>
        /// The function creates a new instance of the MainForm class using a Builder object initialized
        /// with a Glade file.
        /// </summary>
        /// <returns>
        /// An instance of the `MainForm` class is being returned.
        /// </returns>
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
            copyBut.Clicked += CopyBut_Clicked;
            this.DestroyEvent += MainForm_DestroyEvent;
        }

        /// <summary>
        /// The function `CopyBut_Clicked` copies the value of `ORCID.ORCID` to the clipboard using
        /// TextCopy.ClipboardService.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `CopyBut_Clicked` method refers to the
        /// object that raised the event. In this case, it would be the button that was clicked to
        /// trigger the event.</param>
        /// <param name="EventArgs">The `EventArgs` parameter in the `CopyBut_Clicked` method is an
        /// event data class that contains information about the event that was raised. It is commonly
        /// used in event handler methods to provide additional information about the event that
        /// occurred.</param>
        private void CopyBut_Clicked(object? sender, EventArgs e)
        {
            TextCopy.ClipboardService.SetText(ORCID.ORCID);
        }

        private void StatusLabel_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Clipboard clipboard = Clipboard.Get(Gdk.Selection.Clipboard);
            // Set text to the clipboard
            clipboard.Text = ORCID.ORCID;
        }

        /// <summary>
        /// The MainForm_DestroyEvent function saves data, saves wallet data with a password, and quits
        /// the application.
        /// </summary>
        /// <param name="o">The parameter "o" in the MainForm_DestroyEvent method is typically used to
        /// refer to the object that triggered the event. In this case, it could be the main form or
        /// another object related to the destruction event.</param>
        /// <param name="DestroyEventArgs">The `DestroyEventArgs` parameter in the
        /// `MainForm_DestroyEvent` method is an event argument that provides information about the
        /// event that triggered the destruction of the main form. It may contain additional data or
        /// properties related to the destruction event.</param>
        private void MainForm_DestroyEvent(object o, DestroyEventArgs args)
        {
            Save();
            wallet.Save(passwordBox.Buffer.Text);
            Application.Quit();
        }

        #endregion
        private StringWriter _writer;
        private Blockchain.Wallet wallet;
        private OAuthTokenResponse ORCID;
        string token;
        public string peer = "92.205.238.105";
        /// <summary>
        /// The function handles the login process by setting up necessary components, connecting to a
        /// chat client, loading a wallet, initializing blockchain, and performing various actions
        /// related to user authentication and transactions.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `loginBut_Click` method refers to the
        /// object that raised the event. In this case, it would typically be the button that was
        /// clicked to trigger the login process.</param>
        /// <param name="EventArgs">The `EventArgs` parameter in the `loginBut_Click` method is an
        /// object that contains event data specific to the event that was raised. In this context, it
        /// represents the event arguments for the click event that triggered the method. Event
        /// arguments provide information about the event and can be used by the event</param>
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
       /// <summary>
       /// The Timer function updates various GUI elements with real-time data at regular intervals.
       /// </summary>
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

        /// <summary>
        /// The AddItem function adds a new item to a ComboBox and updates its model accordingly.
        /// </summary>
        /// <param name="ComboBox">The `ComboBox` parameter in the `AddItem` method represents the
        /// ComboBox widget to which you want to add an item. This method is used to add a new item
        /// (specified by the `st` parameter) to the ComboBox's list of items.</param>
        /// <param name="st">The `st` parameter in the `AddItem` method represents the string value that
        /// you want to add to the ComboBox as a new item. This method adds the string `st` to the
        /// existing items in the ComboBox.</param>
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

        /// <summary>
        /// The function creates a new document, signs it, creates a new block with the document, and
        /// adds the block to a pending block list.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `createBut_Click` method refers to the
        /// object that raised the event. In this case, it would be the button that was clicked to
        /// trigger the event.</param>
        /// <param name="EventArgs">The `EventArgs` parameter in the `createBut_Click` method is an
        /// object that contains event data specific to the event that was raised. In this context, it
        /// represents the event data for the click event that triggered the execution of the method.
        /// Event data typically provides information about the event that occurred,</param>
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

        /// <summary>
        /// The function creates and signs a transaction, verifies it, and adds it to a list of
        /// transactions.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `sendBut_Click` method is of type
        /// `object?`, which means it can accept any object or `null`. It typically represents the
        /// object that raised the event, in this case, the button that was clicked to trigger the
        /// event.</param>
        /// <param name="EventArgs">The `EventArgs` parameter in the `sendBut_Click` method is an object
        /// that contains event data specific to the `Click` event. It provides information about the
        /// event that occurred, such as the sender of the event and any additional event-specific
        /// details. In this case, it is used to handle</param>
        private void sendBut_Click(object? sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.transaction, ORCID.ORCID, wallet.PublicKey, addressBox.Buffer.Text, (decimal)amountBox.Value);
            tr.SignTransaction(wallet.PrivateKey);
            bool res = VerifyTransaction(tr);

            AddTransaction(tr);
        }

        /// <summary>
        /// The function `addByNameBut_Click` searches for an ORCID ID based on a name input and adds it
        /// to a list if found.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `addByNameBut_Click` event handler refers
        /// to the object that raised the event. In this case, it would be the button that was clicked
        /// to trigger the event.</param>
        /// <param name="EventArgs">`EventArgs` is a base class for classes containing event data. It
        /// represents the arguments that are passed to an event handler when an event is raised. In
        /// this context, the `EventArgs` parameter in the `addByNameBut_Click` method represents the
        /// event data associated with the click event that triggers the</param>
        /// <returns>
        /// If the `id` variable is `null`, the method will return early and not execute the `AddItem`
        /// method.
        /// </returns>
        private async void addByNameBut_Click(object? sender, EventArgs e)
        {
            string id = await Orcid.SearchForORCID(nameBox.Text);
            if (id == null) return;
            AddItem(authorsBox, id);
        }

        /// <summary>
        /// The function checks if an ORCID exists and adds it to a list if it does.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `addByIDBut_Click` method refers to the
        /// object that raised the event. In this case, it would be the button that was clicked to
        /// trigger the event.</param>
        /// <param name="EventArgs">`EventArgs` is a base class for classes containing event data. It is
        /// used to pass information about an event that is being raised. In the context of the code
        /// snippet you provided, `EventArgs` is the second parameter of the event handler method
        /// `addByIDBut_Click`. It is commonly used in</param>
        private async void addByIDBut_Click(object? sender, EventArgs e)
        {
            bool id = await Orcid.CheckORCIDExistence(idBox.Text);
            if (id)
                AddItem(authorsBox, idBox.Text);
        }

        /// <summary>
        /// The updateBut_Click function updates the balance and reputation labels with values retrieved
        /// from the ORCID service.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `updateBut_Click` method refers to the
        /// object that raised the event. In this case, it would typically be the button that was
        /// clicked to trigger the event.</param>
        /// <param name="EventArgs">The `EventArgs` parameter in the `updateBut_Click` method is an
        /// object that contains event data and is passed to event handlers when an event is raised. It
        /// provides information about the event that occurred. In this case, it is used to handle the
        /// click event for the button that triggers the update</param>
        private void updateBut_Click(object? sender, EventArgs e)
        {
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            reputationLabel.Text = "Reputation: " + GetReputation(ORCID.ORCID).ToString();
        }

        /// <summary>
        /// This C# function asynchronously searches for an ORCID using a given name and updates the
        /// address box with the result.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `getAddrBut_Click` method refers to the
        /// object that raised the event. In this case, it would be the button that was clicked to
        /// trigger the event.</param>
        /// <param name="EventArgs">EventArgs is a base class that provides data for event handlers in
        /// C#. It contains information about the event that occurred. In this case, it is used in the
        /// event handler for the button click event to handle the click event arguments.</param>
        private async void getAddrBut_Click(object? sender, EventArgs e)
        {
            addressBox.Buffer.Text = await Orcid.SearchForORCID(sendToNameBox.Text);
        }
        /// <summary>
        /// This C# function creates a transaction for a peer review process and adds it to a list if a
        /// pending block is selected.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `peerReviewBut_Click` method is of type
        /// `object?`. It represents the object that triggered the event, typically a button or control
        /// that was clicked.</param>
        /// <param name="EventArgs">`EventArgs` is a base class that contains event data and is used as
        /// the base class for classes that represent event data. It is often used as a parameter in
        /// event handler methods to provide information about the event that occurred.</param>
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

        /// <summary>
        /// The flagBut_Click function creates a new transaction of type "flag" and adds it to the
        /// pending blocks.
        /// </summary>
        /// <param name="sender">The `sender` parameter in the `flagBut_Click` method is of type
        /// `object?`, which means it can accept any object or `null`. It typically refers to the object
        /// that raised the event that triggered the click event handler. In this case, it would likely
        /// be the button that was</param>
        /// <param name="EventArgs">The `EventArgs` parameter in the `flagBut_Click` method is an object
        /// that contains event data specific to the `Click` event. It provides information about the
        /// event and can be used to handle the event logic.</param>
        private void flagBut_Click(object? sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.flag, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            tr.Data = PendingBlocks[pendingBox.Active].GUID.ToString();
            AddTransaction(tr);
        }
    }
}
