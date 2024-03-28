using static SciChain.Orcid;
using static SciChain.Blockchain;
using NetCoreServer;
using System.Transactions;
namespace SciChain
{
    public partial class MainForm : Form
    {
        private StringWriter _writer;
        private Blockchain.Wallet wallet;
        private OAuthTokenResponse ORCID;
        string token;
        public string peer = "92.205.238.105";
        public MainForm()
        {
            InitializeComponent();
        }
        private async void loginBut_Click(object sender, EventArgs e)
        {
            _writer = new StringWriter();
            Console.SetOut(_writer);
            token = OAuthHelper.StartListenerAsync().Result;
            ORCID = await Orcid.GetAccessToken(token);
            wallet = new Blockchain.Wallet();
            wallet.Load(maskedTextBox.Text);
            Initialize(wallet);
            ChatClient cl = new ChatClient(peer, 8333);
            cl.ConnectAsync();
            ConnectToPeer(peer, cl, 8333);
            Blockchain.Load();
            timer.Start();
            toolStripStatusLabel.Text = "Logged In:" + ORCID.Name + " " + ORCID.ORCID;
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            sendBut.Enabled = true;
            addByIDBut.Enabled = true;
            addByNameBut.Enabled = true;
            createBut.Enabled = true; 
            updateBut.Enabled = true;
            getAddrBut.Enabled = true;
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.registration, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            AddTransaction(tr);
            GetPending(Peers.First().Value, PendingBlocks.Count);
        }

        private void createBut_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var item in authorsBox.Items)
            {
                list.Add(item.ToString());
            }
            Block.Document doc = new Block.Document(doiBox.Text, list,wallet.PublicKey);
            doc.SignDocument(wallet.PrivateKey,ORCID.ORCID);
            Block bl = new Block(DateTime.Now, GetLatestBlock().Hash, null);
            bl.BlockDocument = doc;
            AddPendingBlock(bl);
            //MineBlock(ORCID.ORCID, doc, wallet.PublicKey);
        }

        private void sendBut_Click(object sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.transaction, ORCID.ORCID, wallet.PublicKey, addrBox.Text, amountBox.Value);
            tr.SignTransaction(wallet.PrivateKey);
            ProcessTransaction(tr);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
            wallet.Save(maskedTextBox.Text);
        }

        private async void addByNameBut_Click(object sender, EventArgs e)
        {
            string id = await Orcid.SearchForORCID(nameBox.Text);
            if (id == null) return;
            authorsBox.Items.Add(id);
        }

        private async void addByIDBut_Click(object sender, EventArgs e)
        {
            bool id = await Orcid.CheckORCIDExistence(idBox.Text);
            if (id)
                authorsBox.Items.Add(id);
        }

        private void updateBut_Click(object sender, EventArgs e)
        {
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            reputationLabel.Text = "Reputation: " + GetReputation(ORCID.ORCID).ToString();
        }

        private async void getAddrBut_Click(object sender, EventArgs e)
        {
            addrBox.Text = await Orcid.SearchForORCID(sendToNameBox.Text);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Connections:" + Peers.Count.ToString() + " Height: " + Chain.Count;
            updateBut.PerformClick();
            pendingBox.Items.Clear();
            pendingBox.Items.AddRange(PendingBlocks.ToArray());
            textBox1.Text = _writer.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
        int block = -1;
        private void peerReviewBut_Click(object sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.review, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            if (block > -1)
            {
                tr.Data = PendingBlocks[block].GUID.ToString();
                AddTransaction(tr);
            }
        }

        private void toolStripStatusLabel_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ORCID.ORCID.ToString());
        }

        private void flagBut_Click(object sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.flag, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.SignTransaction(wallet.PrivateKey);
            tr.Data = PendingBlocks[pendingBox.SelectedIndex].GUID.ToString();
            AddTransaction(tr);
        }

        private void pendingBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            block = pendingBox.SelectedIndex;
        }
    }
}
