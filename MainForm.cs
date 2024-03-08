namespace SciChain
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            /*
            Blockchain scichain = new Blockchain();
            scichain.AddBlock(new Block(DateTime.Now, scichain.GetLatestBlock().Hash, "Block 1 Data"));
            scichain.AddBlock(new Block(DateTime.Now, scichain.GetLatestBlock().Hash, "Block 2 Data"));

            Console.WriteLine("Is blockchain valid? " + scichain.IsValid());

            foreach (Block block in scichain.Chain)
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Previous Hash: {block.PreviousHash}");
                Console.WriteLine($"Hash: {block.Hash}");
                Console.WriteLine($"Data: {block.Data}");
                Console.WriteLine();
            }
            */
        }

        private void loginBut_Click(object sender, EventArgs e)
        {
            string token = OAuthHelper.StartListenerAsync().Result;
            bool res = OrcidAuthentication.AuthenticateOrcidId(idBox.Text, token).Result;
        }
    }
}
