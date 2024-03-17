using static SciChain.Orcid;
using static SciChain.Blockchain;
using NetCoreServer;
namespace SciChain
{
    public partial class MainForm : Form
    {
        private Blockchain.Wallet wallet;
        private OAuthTokenResponse ORCID;
        string token;
        public string peer = "92.205.238.105";
        public MainForm()
        {
            InitializeComponent();
            Initialize(false);
            ConnectToPeer(peer,new ChatClient(peer, 8333),8333);
            Blockchain.Load();
            wallet = new Blockchain.Wallet();
            timer.Start();
        }
        private async void loginBut_Click(object sender, EventArgs e)
        {
            token = OAuthHelper.StartListenerAsync().Result;
            ORCID = await Orcid.GetAccessToken(token);
            toolStripStatusLabel.Text = "Logged In:" + ORCID.Name + " " + ORCID.ORCID;
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            sendBut.Enabled = true;
            addByIDBut.Enabled = true;
            addByNameBut.Enabled = true;
            createBut.Enabled = true;
            updateBut.Enabled = true;
            getAddrBut.Enabled = true;
            wallet.Load(maskedTextBox.Text);
            ProcessTransaction(new Block.Transaction(Block.Transaction.Type.registration, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift));
            
        }

        private void createBut_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var item in authorsBox.Items)
            {
                list.Add(item.ToString());
            }
            Block.Document doc = new Block.Document(doiBox.Text, list);
            MineBlock(ORCID.ORCID, doc, wallet.PublicKey);
        }

        private void sendBut_Click(object sender, EventArgs e)
        {
            ProcessTransaction(new Block.Transaction(Block.Transaction.Type.transaction, ORCID.ORCID, wallet.PublicKey, addrBox.Text, amountBox.Value));
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
        }

        private async void getAddrBut_Click(object sender, EventArgs e)
        {
            addrBox.Text = await Orcid.SearchForORCID(sendToNameBox.Text);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Connections:" + Peers.Count.ToString() + " Height: " + Chain.Count;
        }
    }
}
