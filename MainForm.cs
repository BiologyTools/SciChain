using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using static SciChain.Orcid;

namespace SciChain
{
    public partial class MainForm : Form
    {
        private Blockchain chain;
        private Blockchain.Wallet wallet;
        private OAuthTokenResponse ORCID;
        string token;
        public MainForm()
        {
            InitializeComponent();
            chain = new Blockchain();
            chain.Load();
            if (chain.Chain.Count == 0)
                chain.AddGenesisBlock();
            wallet = new Blockchain.Wallet();
        }

        private async void loginBut_Click(object sender, EventArgs e)
        {
            token = OAuthHelper.StartListenerAsync().Result;
            ORCID = await Orcid.GetAccessToken(token);
            toolStripStatusLabel.Text = "Logged In:" + ORCID.Name + " " + ORCID.ORCID;
            balanceLabel.Text = chain.GetBalance(ORCID.ORCID).ToString();
            sendBut.Enabled = true;
            addByIDBut.Enabled = true;
            addByNameBut.Enabled = true;
            createBut.Enabled = true;
            wallet.Load(maskedTextBox.Text);    
        }

        private void createBut_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var item in authorsBox.Items)
            {
                list.Add(item.ToString());
            }
            Block.Document doc = new Block.Document(doiBox.Text,list);
            chain.MineBlock(ORCID.ORCID, doc,wallet.PublicKey);
        }

        private void sendBut_Click(object sender, EventArgs e)
        {
            chain.ProcessTransaction(new Block.Transaction(Block.Transaction.Type.transaction, ORCID.ORCID, wallet.PublicKey, addrBox.Text, amountBox.Value));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            chain.Save();
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
            if(id)
                authorsBox.Items.Add(id);
        }
    }
}
