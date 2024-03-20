namespace SciChain
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            loginBut = new Button();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            balanceLabel = new Label();
            addrBox = new TextBox();
            sendBut = new Button();
            createBut = new Button();
            openFileDialog = new OpenFileDialog();
            label1 = new Label();
            amountBox = new NumericUpDown();
            doiBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            authorsBox = new ComboBox();
            addByNameBut = new Button();
            nameBox = new TextBox();
            idBox = new TextBox();
            addByIDBut = new Button();
            label4 = new Label();
            label5 = new Label();
            maskedTextBox = new MaskedTextBox();
            label6 = new Label();
            updateBut = new Button();
            label7 = new Label();
            sendToNameBox = new TextBox();
            getAddrBut = new Button();
            timer = new System.Windows.Forms.Timer(components);
            label8 = new Label();
            peerReviewBox = new TextBox();
            peerReviewBut = new Button();
            flagBut = new Button();
            reputationLabel = new Label();
            pendingBox = new ComboBox();
            label10 = new Label();
            textBox1 = new TextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)amountBox).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // loginBut
            // 
            loginBut.Location = new Point(345, 233);
            loginBut.Name = "loginBut";
            loginBut.Size = new Size(132, 23);
            loginBut.TabIndex = 0;
            loginBut.Text = "Login";
            loginBut.UseVisualStyleBackColor = true;
            loginBut.Click += loginBut_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripStatusLabel2 });
            statusStrip.Location = new Point(0, 292);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(493, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 17);
            toolStripStatusLabel.Click += toolStripStatusLabel_Click;
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(0, 17);
            // 
            // balanceLabel
            // 
            balanceLabel.AutoSize = true;
            balanceLabel.Location = new Point(6, 3);
            balanceLabel.Name = "balanceLabel";
            balanceLabel.Size = new Size(51, 15);
            balanceLabel.TabIndex = 2;
            balanceLabel.Text = "Balance:";
            // 
            // addrBox
            // 
            addrBox.Location = new Point(71, 31);
            addrBox.Name = "addrBox";
            addrBox.Size = new Size(232, 23);
            addrBox.TabIndex = 3;
            // 
            // sendBut
            // 
            sendBut.Enabled = false;
            sendBut.Location = new Point(396, 31);
            sendBut.Name = "sendBut";
            sendBut.Size = new Size(81, 23);
            sendBut.TabIndex = 4;
            sendBut.Text = "Send";
            sendBut.UseVisualStyleBackColor = true;
            sendBut.Click += sendBut_Click;
            // 
            // createBut
            // 
            createBut.Enabled = false;
            createBut.Location = new Point(345, 205);
            createBut.Name = "createBut";
            createBut.Size = new Size(132, 23);
            createBut.TabIndex = 5;
            createBut.Text = "Create Block";
            createBut.UseVisualStyleBackColor = true;
            createBut.Click += createBut_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 34);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 8;
            label1.Text = "Address:";
            // 
            // amountBox
            // 
            amountBox.Location = new Point(309, 31);
            amountBox.Name = "amountBox";
            amountBox.Size = new Size(81, 23);
            amountBox.TabIndex = 9;
            // 
            // doiBox
            // 
            doiBox.Location = new Point(71, 90);
            doiBox.Name = "doiBox";
            doiBox.Size = new Size(406, 23);
            doiBox.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 93);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 11;
            label2.Text = "DOI:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 121);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 12;
            label3.Text = "Authors:";
            // 
            // authorsBox
            // 
            authorsBox.FormattingEnabled = true;
            authorsBox.Location = new Point(71, 118);
            authorsBox.Name = "authorsBox";
            authorsBox.Size = new Size(406, 23);
            authorsBox.TabIndex = 13;
            // 
            // addByNameBut
            // 
            addByNameBut.Enabled = false;
            addByNameBut.Location = new Point(345, 146);
            addByNameBut.Name = "addByNameBut";
            addByNameBut.Size = new Size(132, 23);
            addByNameBut.TabIndex = 14;
            addByNameBut.Text = "Add Author by Name";
            addByNameBut.UseVisualStyleBackColor = true;
            addByNameBut.Click += addByNameBut_Click;
            // 
            // nameBox
            // 
            nameBox.Location = new Point(71, 147);
            nameBox.Name = "nameBox";
            nameBox.Size = new Size(268, 23);
            nameBox.TabIndex = 15;
            // 
            // idBox
            // 
            idBox.Location = new Point(71, 176);
            idBox.Name = "idBox";
            idBox.Size = new Size(268, 23);
            idBox.TabIndex = 16;
            // 
            // addByIDBut
            // 
            addByIDBut.Enabled = false;
            addByIDBut.Location = new Point(345, 176);
            addByIDBut.Name = "addByIDBut";
            addByIDBut.Size = new Size(132, 23);
            addByIDBut.TabIndex = 17;
            addByIDBut.Text = "Add Author by ID";
            addByIDBut.UseVisualStyleBackColor = true;
            addByIDBut.Click += addByIDBut_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 150);
            label4.Name = "label4";
            label4.Size = new Size(42, 15);
            label4.TabIndex = 18;
            label4.Text = "Name:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 179);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 19;
            label5.Text = "ORCID ID:";
            // 
            // maskedTextBox
            // 
            maskedTextBox.Location = new Point(105, 233);
            maskedTextBox.Name = "maskedTextBox";
            maskedTextBox.PasswordChar = '*';
            maskedTextBox.Size = new Size(234, 23);
            maskedTextBox.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 236);
            label6.Name = "label6";
            label6.Size = new Size(93, 15);
            label6.TabIndex = 21;
            label6.Text = "Wallet Password";
            // 
            // updateBut
            // 
            updateBut.Enabled = false;
            updateBut.Location = new Point(396, 2);
            updateBut.Name = "updateBut";
            updateBut.Size = new Size(81, 23);
            updateBut.TabIndex = 22;
            updateBut.Text = "Update";
            updateBut.UseVisualStyleBackColor = true;
            updateBut.Click += updateBut_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 64);
            label7.Name = "label7";
            label7.Size = new Size(42, 15);
            label7.TabIndex = 23;
            label7.Text = "Name:";
            // 
            // sendToNameBox
            // 
            sendToNameBox.Location = new Point(71, 60);
            sendToNameBox.Name = "sendToNameBox";
            sendToNameBox.Size = new Size(232, 23);
            sendToNameBox.TabIndex = 24;
            // 
            // getAddrBut
            // 
            getAddrBut.Enabled = false;
            getAddrBut.Location = new Point(309, 60);
            getAddrBut.Name = "getAddrBut";
            getAddrBut.Size = new Size(81, 23);
            getAddrBut.TabIndex = 25;
            getAddrBut.Text = "Get Address";
            getAddrBut.UseVisualStyleBackColor = true;
            getAddrBut.Click += getAddrBut_Click;
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(8, 34);
            label8.Name = "label8";
            label8.Size = new Size(59, 15);
            label8.TabIndex = 27;
            label8.Text = "ORCID ID:";
            // 
            // peerReviewBox
            // 
            peerReviewBox.Location = new Point(73, 31);
            peerReviewBox.Name = "peerReviewBox";
            peerReviewBox.Size = new Size(268, 23);
            peerReviewBox.TabIndex = 26;
            // 
            // peerReviewBut
            // 
            peerReviewBut.Location = new Point(347, 31);
            peerReviewBut.Name = "peerReviewBut";
            peerReviewBut.Size = new Size(132, 23);
            peerReviewBut.TabIndex = 28;
            peerReviewBut.Text = "Peer Review Block";
            peerReviewBut.UseVisualStyleBackColor = true;
            peerReviewBut.Click += peerReviewBut_Click;
            // 
            // flagBut
            // 
            flagBut.Location = new Point(347, 60);
            flagBut.Name = "flagBut";
            flagBut.Size = new Size(132, 23);
            flagBut.TabIndex = 31;
            flagBut.Text = "Fail Peer Review";
            flagBut.UseVisualStyleBackColor = true;
            flagBut.Click += flagBut_Click;
            // 
            // reputationLabel
            // 
            reputationLabel.AutoSize = true;
            reputationLabel.Location = new Point(168, 3);
            reputationLabel.Name = "reputationLabel";
            reputationLabel.Size = new Size(68, 15);
            reputationLabel.TabIndex = 32;
            reputationLabel.Text = "Reputation:";
            // 
            // pendingBox
            // 
            pendingBox.FormattingEnabled = true;
            pendingBox.Location = new Point(73, 3);
            pendingBox.Name = "pendingBox";
            pendingBox.Size = new Size(406, 23);
            pendingBox.TabIndex = 33;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(8, 6);
            label10.Name = "label10";
            label10.Size = new Size(54, 15);
            label10.TabIndex = 34;
            label10.Text = "Pending:";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Black;
            textBox1.Dock = DockStyle.Fill;
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(3, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(479, 258);
            textBox1.TabIndex = 35;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(493, 292);
            tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(balanceLabel);
            tabPage1.Controls.Add(reputationLabel);
            tabPage1.Controls.Add(loginBut);
            tabPage1.Controls.Add(getAddrBut);
            tabPage1.Controls.Add(addrBox);
            tabPage1.Controls.Add(sendToNameBox);
            tabPage1.Controls.Add(sendBut);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(createBut);
            tabPage1.Controls.Add(updateBut);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(amountBox);
            tabPage1.Controls.Add(maskedTextBox);
            tabPage1.Controls.Add(doiBox);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(addByIDBut);
            tabPage1.Controls.Add(authorsBox);
            tabPage1.Controls.Add(idBox);
            tabPage1.Controls.Add(addByNameBut);
            tabPage1.Controls.Add(nameBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(485, 264);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Wallet";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(485, 264);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Console";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(pendingBox);
            tabPage3.Controls.Add(label10);
            tabPage3.Controls.Add(peerReviewBox);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(peerReviewBut);
            tabPage3.Controls.Add(flagBut);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(485, 264);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Peer Review";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AcceptButton = loginBut;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(493, 314);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "SciChain";
            FormClosing += MainForm_FormClosing;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)amountBox).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button loginBut;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Label balanceLabel;
        private TextBox addrBox;
        private Button sendBut;
        private Button createBut;
        private OpenFileDialog openFileDialog;
        private Label label1;
        private NumericUpDown amountBox;
        private TextBox doiBox;
        private Label label2;
        private Label label3;
        private ComboBox authorsBox;
        private Button addByNameBut;
        private TextBox nameBox;
        private TextBox idBox;
        private Button addByIDBut;
        private Label label4;
        private Label label5;
        private MaskedTextBox maskedTextBox;
        private Label label6;
        private Button updateBut;
        private Label label7;
        private TextBox sendToNameBox;
        private Button getAddrBut;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Timer timer;
        private Label label8;
        private TextBox peerReviewBox;
        private Button peerReviewBut;
        private Button flagBut;
        private Label reputationLabel;
        private ComboBox pendingBox;
        private Label label10;
        private TextBox textBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
    }
}
