using static SciChain.OrcidAuthentication;

namespace SciChain
{
    public partial class MainForm : Form
    {
        private Blockchain chain;
        private OAuthTokenResponse ORCID;
        string token;
        public MainForm()
        {
            InitializeComponent();
            chain = new Blockchain();
            chain.Load();
            if (chain.Chain.Count == 0)
                chain.AddGenesisBlock();

        }

        private async void loginBut_Click(object sender, EventArgs e)
        {
            token = OAuthHelper.StartListenerAsync().Result;
            ORCID = await OrcidAuthentication.GetAccessToken(token);
            toolStripStatusLabel.Text = "Logged In:" + ORCID.Name + " " + ORCID.ORCID;
        }

        private void createBut_Click(object sender, EventArgs e)
        {

        }
    }
}
