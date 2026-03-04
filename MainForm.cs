using static SciChain.Orcid;
using static SciChain.Blockchain;
using NetCoreServer;
using System.Transactions;
using Gtk;
using Pango;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciChain
{
    public partial class MainForm : Window
    {
        #region Properties

        private Builder _builder;
        private static MainForm form;
        int line = 0;
#pragma warning disable 649

        // --- Existing widgets ---
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

        // --- New publishing widgets ---
        [Builder.Object]
        private Entry titleBox;
        [Builder.Object]
        private Entry abstractBox;
        [Builder.Object]
        private Entry contentHashBox;

        // --- New review widgets ---
        [Builder.Object]
        private TextView reviewCommentsBox;
        [Builder.Object]
        private ComboBox reviewDecisionBox;
        [Builder.Object]
        private Button viewReviewsBut;

        // --- New governance widgets ---
        [Builder.Object]
        private Entry proposalTitleBox;
        [Builder.Object]
        private TextView proposalDescBox;
        [Builder.Object]
        private SpinButton votingDaysBox;
        [Builder.Object]
        private Button createProposalBut;
        [Builder.Object]
        private ComboBox proposalsBox;
        [Builder.Object]
        private Button voteYesBut;
        [Builder.Object]
        private Button voteNoBut;
        [Builder.Object]
        private Label proposalStatusLabel;

        // --- Browse widgets ---
        [Builder.Object]
        private Entry searchBox;
        [Builder.Object]
        private Button searchBut;
        [Builder.Object]
        private ComboBox publishedDocsBox;
        [Builder.Object]
        private Label docInfoLabel;

#pragma warning restore 649

        #endregion

        #region Constructors / Destructors

        public static MainForm Create()
        {
            Builder builder = new Builder(new FileStream(System.IO.Path.GetDirectoryName(Environment.ProcessPath) + "/" + "Glade/MainForm.glade", FileMode.Open));
            return new MainForm(builder, builder.GetObject("mainform").Handle);
        }

        protected MainForm(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
            form = this;
        }

        #endregion

        #region Handler Setup

        protected void SetupHandlers()
        {
            // Existing handlers
            getAddressBut.Clicked += getAddrBut_Click;
            sendBut.Clicked += sendBut_Click;
            addByIDBut.Clicked += addByIDBut_Click;
            addByNameBut.Clicked += addByNameBut_Click;
            createBlockBut.Clicked += createBut_Click;
            loginBut.Clicked += loginBut_Click;
            peerReviewBut.Clicked += peerReviewBut_Click;
            failReviewBut.Clicked += flagBut_Click;
            copyBut.Clicked += CopyBut_Clicked;
            this.Destroyed += MainForm_Destroyed;

            // New handlers — only wire up if the widgets exist in the glade file
            if (viewReviewsBut != null) viewReviewsBut.Clicked += viewReviewsBut_Click;
            if (createProposalBut != null) createProposalBut.Clicked += createProposalBut_Click;
            if (voteYesBut != null) voteYesBut.Clicked += voteYesBut_Click;
            if (voteNoBut != null) voteNoBut.Clicked += voteNoBut_Click;
            if (searchBut != null) searchBut.Clicked += searchBut_Click;
            if (publishedDocsBox != null) publishedDocsBox.Changed += publishedDocsBox_Changed;
            if (proposalsBox != null) proposalsBox.Changed += proposalsBox_Changed;
        }

        private void MainForm_Destroyed(object? sender, EventArgs e)
        {
            Save();
            if (wallet != null)
                wallet.Save(passwordBox.Buffer.Text);
            Application.Quit();
        }

        private void CopyBut_Clicked(object? sender, EventArgs e)
        {
            TextCopy.ClipboardService.SetText(ORCID.ORCID);
        }

        #endregion

        private StringWriter _writer;
        private Blockchain.Wallet wallet;
        private OAuthTokenResponse ORCID;
        string token;
        public string peer = "92.205.238.105";

        // Cached lists for index-based combobox lookups
        private List<Block> cachedPendingList = new List<Block>();
        private List<Block> cachedPublishedList = new List<Block>();
        private List<ChainQuery.ProposalRecord> cachedProposalList = new List<ChainQuery.ProposalRecord>();

        #region Login and Timer

        private async void loginBut_Click(object? sender, EventArgs e)
        {
            _writer = new StringWriter();
            Console.SetOut(_writer);
            token = await OAuthHelper.StartListenerAsync();
            ORCID = await Orcid.GetAccessToken(token);
            wallet = new Blockchain.Wallet();
            wallet.Load(passwordBox.Buffer.Text);
            Initialize(wallet, passwordBox.Buffer.Text);
            ChatClient cl = new ChatClient(peer, 8333);
            cl.ConnectAsync();
            ConnectToPeer(peer, cl, 8333);
            Blockchain.Load();
            StartTimer();
            statusLabel.Text = "Logged In:" + ORCID.Name + " " + ORCID.ORCID;
            balanceLabel.Text = "Balance: " + GetBalance(ORCID.ORCID).ToString();
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.registration, Blockchain.treasuryAddress, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
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
                    Application.Invoke(delegate
                    {
                        try
                        {
                            var stats = ChainQuery.GetChainStats();
                            form.statusLabel.Text = $"Peers:{stats.ConnectedPeers} Height:{stats.TotalBlocks} " +
                                                    $"Docs:{stats.PublishedDocuments} Treasury:{stats.TreasuryBalance}";
                            form.balanceLabel.Text = "Balance: " + GetBalance(form.ORCID.ORCID).ToString();
                            form.reputationLabel.Text = "Reputation: " + GetReputation(form.ORCID.ORCID).ToString();

                            form.RefreshPendingBlocksList();
                            form.RefreshProposalsList();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    });

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
            th.IsBackground = true;
            th.Start();
        }

        #endregion

        #region Shared UI Helpers

        private void AddItem(ComboBox box, string st)
        {
            ListStore ls = new ListStore(typeof(string));
            TreeIter iter;
            if (box.Model == null)
            {
                box.Model = new ListStore(typeof(string));
            }
            if (box.Model.GetIterFirst(out iter))
            {
                do
                {
                    string item = (string)box.Model.GetValue(iter, 0);
                    ls.AppendValues(item);
                }
                while (box.Model.IterNext(ref iter));
            }
            ls.AppendValues(st);
            CellRendererText cell = new CellRendererText();
            box.PackStart(cell, false);
            box.AddAttribute(cell, "text", 0);
            box.Model = ls;
        }

        private void PopulateComboBox(ComboBox box, IEnumerable<string> items)
        {
            box.Clear();
            ListStore ls = new ListStore(typeof(string));
            foreach (var item in items)
            {
                ls.AppendValues(item);
            }
            CellRendererText cell = new CellRendererText();
            box.PackStart(cell, false);
            box.AddAttribute(cell, "text", 0);
            box.Model = ls;
        }

        private void RefreshPendingBlocksList()
        {
            if (pendingBox == null) return;

            cachedPendingList = GetPendingBlocksList();
            var displayItems = cachedPendingList.Select(b =>
            {
                string label = b.GUID.Substring(0, 8);
                if (b.BlockDocument != null)
                {
                    string title = b.BlockDocument.Title ?? b.BlockDocument.DOI ?? "Untitled";
                    label = title.Length > 30 ? title.Substring(0, 30) + "..." : title;
                }
                int revs = GetReviews(b.GUID);
                int fls = GetFlags(b.GUID);
                return $"{label} [R:{revs} F:{fls}]";
            });

            PopulateComboBox(pendingBox, displayItems);
        }

        private void RefreshProposalsList()
        {
            if (proposalsBox == null) return;

            cachedProposalList = ChainQuery.GetProposals();
            var displayItems = cachedProposalList.Select(p =>
            {
                string status = p.Status.ToString();
                return $"[{status}] {p.Proposal.Title} ({p.VotesFor}Y/{p.VotesAgainst}N)";
            });

            PopulateComboBox(proposalsBox, displayItems);
        }

        #endregion

        #region Publishing — Create Block with Rich Document

        private void createBut_Click(object? sender, EventArgs e)
        {
            // Gather author list from the authorsBox combobox
            List<string> authorList = new List<string>();
            TreeIter iter;
            if (authorsBox.Model != null && authorsBox.Model.GetIterFirst(out iter))
            {
                do
                {
                    string item = (string)authorsBox.Model.GetValue(iter, 0);
                    authorList.Add(item);
                }
                while (authorsBox.Model.IterNext(ref iter));
            }

            // Read the new document fields, falling back gracefully if widgets aren't present
            string doi = doiBox?.Text ?? "";
            string title = titleBox?.Text ?? "";
            string abstract_ = abstractBox?.Text ?? "";
            string contentHash = contentHashBox?.Text ?? "";

            Block.Document doc = new Block.Document(doi, title, abstract_, contentHash, authorList, wallet.PublicKey);
            doc.SignDocument(wallet.PrivateKey, ORCID.ORCID);
            Block bl = new Block(DateTime.Now, GetLatestBlock().Hash, null);
            bl.BlockDocument = doc;
            AddPendingBlock(bl);
        }

        #endregion

        #region Transactions — Send

        private void sendBut_Click(object? sender, EventArgs e)
        {
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.transaction, ORCID.ORCID, wallet.PublicKey, addressBox.Buffer.Text, (decimal)amountBox.Value);
            tr.SignTransaction(wallet.PrivateKey);
            bool res = VerifyTransaction(tr);
            AddTransaction(tr);
        }

        private async void getAddrBut_Click(object? sender, EventArgs e)
        {
            addressBox.Buffer.Text = await Orcid.SearchForORCID(sendToNameBox.Text);
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

        #endregion

        #region Peer Review — Structured Reviews with Comments and Decisions

        private void peerReviewBut_Click(object? sender, EventArgs e)
        {
            if (pendingBox.Active < 0 || pendingBox.Active >= cachedPendingList.Count)
                return;

            Block pendingBlock = cachedPendingList[pendingBox.Active];

            // Determine the review decision from the combobox, defaulting to Approve
            ReviewData.Decision decision = ReviewData.Decision.Approve;
            if (reviewDecisionBox != null && reviewDecisionBox.Active >= 0)
            {
                switch (reviewDecisionBox.Active)
                {
                    case 0: decision = ReviewData.Decision.Approve; break;
                    case 1: decision = ReviewData.Decision.RequestRevision; break;
                    case 2: decision = ReviewData.Decision.Reject; break;
                }
            }

            // Get reviewer comments if the widget exists
            string comments = "";
            if (reviewCommentsBox?.Buffer != null)
                comments = reviewCommentsBox.Buffer.Text;

            ReviewData reviewData = new ReviewData(pendingBlock.GUID, ORCID.ORCID, decision, comments);

            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.review, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.Data = reviewData.Serialize();
            tr.SignTransaction(wallet.PrivateKey);
            AddTransaction(tr);

            // Clear the comments box after submitting
            if (reviewCommentsBox?.Buffer != null)
                reviewCommentsBox.Buffer.Text = "";
        }

        private void flagBut_Click(object? sender, EventArgs e)
        {
            if (pendingBox.Active < 0 || pendingBox.Active >= cachedPendingList.Count)
                return;

            Block pendingBlock = cachedPendingList[pendingBox.Active];
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.flag, null, wallet.PublicKey, ORCID.ORCID, Blockchain.gift);
            tr.Data = pendingBlock.GUID;
            tr.SignTransaction(wallet.PrivateKey);
            AddTransaction(tr);
        }

        private void viewReviewsBut_Click(object? sender, EventArgs e)
        {
            if (pendingBox.Active < 0 || pendingBox.Active >= cachedPendingList.Count)
                return;

            Block pendingBlock = cachedPendingList[pendingBox.Active];
            var reviews = ChainQuery.GetReviewHistory(pendingBlock.GUID);
            var flags = ChainQuery.GetFlagHistory(pendingBlock.GUID);
            var revisions = ChainQuery.GetRevisionHistory(pendingBlock.GUID);

            ShowReviewHistoryDialog(pendingBlock, reviews, flags, revisions);
        }

        private void ShowReviewHistoryDialog(Block block, List<ChainQuery.ReviewRecord> reviews,
            List<ChainQuery.FlagRecord> flagRecords, List<ChainQuery.RevisionRecord> revisions)
        {
            Dialog dialog = new Dialog("Review History", this, DialogFlags.Modal,
                "Close", ResponseType.Close);
            dialog.SetDefaultSize(600, 400);

            var content = dialog.ContentArea;

            // Document info header
            string docTitle = block.BlockDocument?.Title ?? block.BlockDocument?.DOI ?? block.GUID;
            Label headerLabel = new Label();
            headerLabel.Markup = $"<b>{GLib.Markup.EscapeText(docTitle)}</b>";
            content.PackStart(headerLabel, false, false, 5);

            // Scrollable text area for review history
            ScrolledWindow scrolled = new ScrolledWindow();
            scrolled.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            TextView textView = new TextView();
            textView.Editable = false;
            textView.WrapMode = Gtk.WrapMode.Word;

            var buffer = textView.Buffer;
            string text = "";

            text += $"=== Reviews ({reviews.Count}) ===\n\n";
            foreach (var review in reviews)
            {
                string mined = review.IsMined ? "[Mined]" : "[Pending]";
                string decisionStr = review.Decision.ToString();
                text += $"{mined} {review.ReviewerAddress} — {decisionStr}\n";
                if (!string.IsNullOrEmpty(review.Comments))
                    text += $"  Comments: {review.Comments}\n";
                if (review.Timestamp != DateTime.MinValue)
                    text += $"  Date: {review.Timestamp:yyyy-MM-dd HH:mm}\n";
                text += "\n";
            }

            if (flagRecords.Count > 0)
            {
                text += $"=== Flags ({flagRecords.Count}) ===\n\n";
                foreach (var flag in flagRecords)
                {
                    string mined = flag.IsMined ? "[Mined]" : "[Pending]";
                    text += $"{mined} Flagged by: {flag.FlaggerAddress}\n";
                }
                text += "\n";
            }

            if (revisions.Count > 0)
            {
                text += $"=== Revisions ({revisions.Count}) ===\n\n";
                foreach (var rev in revisions)
                {
                    string mined = rev.IsMined ? "[Mined]" : "[Pending]";
                    text += $"{mined} Revision #{rev.RevisionNumber} by {rev.AuthorAddress}\n";
                    text += $"  Notes: {rev.RevisionNotes}\n";
                    if (!string.IsNullOrEmpty(rev.UpdatedDOI))
                        text += $"  Updated DOI: {rev.UpdatedDOI}\n";
                    text += $"  Date: {rev.Timestamp:yyyy-MM-dd HH:mm}\n\n";
                }
            }

            buffer.Text = text;
            scrolled.Add(textView);
            content.PackStart(scrolled, true, true, 5);

            dialog.ShowAll();
            dialog.Run();
            dialog.Destroy();
        }

        #endregion

        #region Governance — Proposals and Voting

        private void createProposalBut_Click(object? sender, EventArgs e)
        {
            string title = proposalTitleBox?.Text ?? "";
            string description = proposalDescBox?.Buffer?.Text ?? "";
            int votingDays = (int)(votingDaysBox?.Value ?? 7);

            if (string.IsNullOrWhiteSpace(title))
            {
                statusLabel.Text = "Proposal title cannot be empty.";
                return;
            }

            ProposalData propData = new ProposalData(title, description, ORCID.ORCID, votingDays);
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.proposal, ORCID.ORCID, wallet.PublicKey, ORCID.ORCID, 0);
            tr.Data = propData.Serialize();
            tr.SignTransaction(wallet.PrivateKey);
            AddTransaction(tr);

            // Clear input fields
            if (proposalTitleBox != null) proposalTitleBox.Text = "";
            if (proposalDescBox?.Buffer != null) proposalDescBox.Buffer.Text = "";
            statusLabel.Text = "Proposal submitted: " + title;
        }

        private void voteYesBut_Click(object? sender, EventArgs e)
        {
            CastVote(true);
        }

        private void voteNoBut_Click(object? sender, EventArgs e)
        {
            CastVote(false);
        }

        private void CastVote(bool inFavor)
        {
            if (proposalsBox == null || proposalsBox.Active < 0 || proposalsBox.Active >= cachedProposalList.Count)
                return;

            var proposal = cachedProposalList[proposalsBox.Active];

            if (proposal.Status != ProposalData.ProposalStatus.Active)
            {
                statusLabel.Text = "Cannot vote on a closed proposal.";
                return;
            }

            if (ChainQuery.HasVoted(proposal.Proposal.ProposalId, ORCID.ORCID))
            {
                statusLabel.Text = "You have already voted on this proposal.";
                return;
            }

            VoteData voteData = new VoteData(proposal.Proposal.ProposalId, ORCID.ORCID, inFavor);
            Block.Transaction tr = new Block.Transaction(Block.Transaction.Type.vote, ORCID.ORCID, wallet.PublicKey, ORCID.ORCID, 0);
            tr.Data = voteData.Serialize();
            tr.SignTransaction(wallet.PrivateKey);
            AddTransaction(tr);

            string voteStr = inFavor ? "YES" : "NO";
            statusLabel.Text = $"Vote cast: {voteStr} on '{proposal.Proposal.Title}'";
        }

        private void proposalsBox_Changed(object? sender, EventArgs e)
        {
            if (proposalsBox == null || proposalStatusLabel == null)
                return;
            if (proposalsBox.Active < 0 || proposalsBox.Active >= cachedProposalList.Count)
                return;

            var proposal = cachedProposalList[proposalsBox.Active];
            proposalStatusLabel.Text = $"Status: {proposal.Status} | " +
                $"Votes: {proposal.VotesFor}Y / {proposal.VotesAgainst}N | " +
                $"Deadline: {proposal.Proposal.VotingDeadline:yyyy-MM-dd} | " +
                $"Threshold: {proposal.Proposal.ApprovalThreshold:P0}";
        }

        #endregion

        #region Browse — Search and View Published Documents

        private void searchBut_Click(object? sender, EventArgs e)
        {
            if (searchBox == null || publishedDocsBox == null) return;

            string keyword = searchBox.Text;
            if (string.IsNullOrWhiteSpace(keyword))
                cachedPublishedList = ChainQuery.GetPublishedDocuments();
            else
                cachedPublishedList = ChainQuery.SearchDocuments(keyword);

            var displayItems = cachedPublishedList.Select(b =>
            {
                string title = b.BlockDocument?.Title ?? "Untitled";
                string doi = b.BlockDocument?.DOI ?? "";
                return $"[{b.Index}] {title} ({doi})";
            });

            PopulateComboBox(publishedDocsBox, displayItems);

            statusLabel.Text = $"Found {cachedPublishedList.Count} document(s).";
        }

        private void publishedDocsBox_Changed(object? sender, EventArgs e)
        {
            if (publishedDocsBox == null || docInfoLabel == null) return;
            if (publishedDocsBox.Active < 0 || publishedDocsBox.Active >= cachedPublishedList.Count)
                return;

            Block block = cachedPublishedList[publishedDocsBox.Active];
            var doc = block.BlockDocument;
            if (doc == null) return;

            string publishers = doc.Publishers != null ? string.Join(", ", doc.Publishers) : "Unknown";
            int reviewCount = GetReviews(block.GUID);

            string info = $"Title: {doc.Title ?? "N/A"}\n" +
                         $"DOI: {doc.DOI ?? "N/A"}\n" +
                         $"Authors: {publishers}\n" +
                         $"Abstract: {doc.Abstract ?? "N/A"}\n" +
                         $"Content Hash: {doc.ContentHash ?? "N/A"}\n" +
                         $"Block Index: {block.Index}\n" +
                         $"Reviews: {reviewCount}\n" +
                         $"Timestamp: {block.TimeStamp:yyyy-MM-dd HH:mm}";

            docInfoLabel.Text = info;
        }

        #endregion
    }
}
