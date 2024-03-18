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
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)amountBox).BeginInit();
            SuspendLayout();
            // 
            // loginBut
            // 
            loginBut.Location = new Point(366, 337);
            loginBut.Name = "loginBut";
            loginBut.Size = new Size(117, 23);
            loginBut.TabIndex = 0;
            loginBut.Text = "Login";
            loginBut.UseVisualStyleBackColor = true;
            loginBut.Click += loginBut_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripStatusLabel2 });
            statusStrip.Location = new Point(0, 388);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(828, 22);
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
            balanceLabel.Location = new Point(12, 9);
            balanceLabel.Name = "balanceLabel";
            balanceLabel.Size = new Size(51, 15);
            balanceLabel.TabIndex = 2;
            balanceLabel.Text = "Balance:";
            // 
            // addrBox
            // 
            addrBox.Location = new Point(77, 37);
            addrBox.Name = "addrBox";
            addrBox.Size = new Size(232, 23);
            addrBox.TabIndex = 3;
            // 
            // sendBut
            // 
            sendBut.Enabled = false;
            sendBut.Location = new Point(402, 37);
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
            createBut.Location = new Point(351, 211);
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
            label1.Location = new Point(12, 40);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 8;
            label1.Text = "Address:";
            // 
            // amountBox
            // 
            amountBox.Location = new Point(315, 37);
            amountBox.Name = "amountBox";
            amountBox.Size = new Size(81, 23);
            amountBox.TabIndex = 9;
            // 
            // doiBox
            // 
            doiBox.Location = new Point(77, 96);
            doiBox.Name = "doiBox";
            doiBox.Size = new Size(406, 23);
            doiBox.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 99);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 11;
            label2.Text = "DOI:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 127);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 12;
            label3.Text = "Authors:";
            // 
            // authorsBox
            // 
            authorsBox.FormattingEnabled = true;
            authorsBox.Location = new Point(77, 124);
            authorsBox.Name = "authorsBox";
            authorsBox.Size = new Size(406, 23);
            authorsBox.TabIndex = 13;
            // 
            // addByNameBut
            // 
            addByNameBut.Enabled = false;
            addByNameBut.Location = new Point(351, 152);
            addByNameBut.Name = "addByNameBut";
            addByNameBut.Size = new Size(132, 23);
            addByNameBut.TabIndex = 14;
            addByNameBut.Text = "Add Author by Name";
            addByNameBut.UseVisualStyleBackColor = true;
            addByNameBut.Click += addByNameBut_Click;
            // 
            // nameBox
            // 
            nameBox.Location = new Point(77, 153);
            nameBox.Name = "nameBox";
            nameBox.Size = new Size(268, 23);
            nameBox.TabIndex = 15;
            // 
            // idBox
            // 
            idBox.Location = new Point(77, 182);
            idBox.Name = "idBox";
            idBox.Size = new Size(268, 23);
            idBox.TabIndex = 16;
            // 
            // addByIDBut
            // 
            addByIDBut.Enabled = false;
            addByIDBut.Location = new Point(351, 182);
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
            label4.Location = new Point(12, 156);
            label4.Name = "label4";
            label4.Size = new Size(42, 15);
            label4.TabIndex = 18;
            label4.Text = "Name:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 185);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 19;
            label5.Text = "ORCID ID:";
            // 
            // maskedTextBox
            // 
            maskedTextBox.Location = new Point(111, 337);
            maskedTextBox.Name = "maskedTextBox";
            maskedTextBox.PasswordChar = '*';
            maskedTextBox.Size = new Size(249, 23);
            maskedTextBox.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 340);
            label6.Name = "label6";
            label6.Size = new Size(93, 15);
            label6.TabIndex = 21;
            label6.Text = "Wallet Password";
            // 
            // updateBut
            // 
            updateBut.Enabled = false;
            updateBut.Location = new Point(402, 8);
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
            label7.Location = new Point(12, 70);
            label7.Name = "label7";
            label7.Size = new Size(42, 15);
            label7.TabIndex = 23;
            label7.Text = "Name:";
            // 
            // sendToNameBox
            // 
            sendToNameBox.Location = new Point(77, 66);
            sendToNameBox.Name = "sendToNameBox";
            sendToNameBox.Size = new Size(232, 23);
            sendToNameBox.TabIndex = 24;
            // 
            // getAddrBut
            // 
            getAddrBut.Enabled = false;
            getAddrBut.Location = new Point(315, 66);
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
            label8.Location = new Point(12, 271);
            label8.Name = "label8";
            label8.Size = new Size(59, 15);
            label8.TabIndex = 27;
            label8.Text = "ORCID ID:";
            // 
            // peerReviewBox
            // 
            peerReviewBox.Location = new Point(77, 268);
            peerReviewBox.Name = "peerReviewBox";
            peerReviewBox.Size = new Size(268, 23);
            peerReviewBox.TabIndex = 26;
            // 
            // peerReviewBut
            // 
            peerReviewBut.Enabled = false;
            peerReviewBut.Location = new Point(351, 268);
            peerReviewBut.Name = "peerReviewBut";
            peerReviewBut.Size = new Size(132, 23);
            peerReviewBut.TabIndex = 28;
            peerReviewBut.Text = "Peer Review Block";
            peerReviewBut.UseVisualStyleBackColor = true;
            peerReviewBut.Click += peerReviewBut_Click;
            // 
            // flagBut
            // 
            flagBut.Enabled = false;
            flagBut.Location = new Point(351, 297);
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
            reputationLabel.Location = new Point(174, 9);
            reputationLabel.Name = "reputationLabel";
            reputationLabel.Size = new Size(68, 15);
            reputationLabel.TabIndex = 32;
            reputationLabel.Text = "Reputation:";
            // 
            // pendingBox
            // 
            pendingBox.FormattingEnabled = true;
            pendingBox.Location = new Point(77, 240);
            pendingBox.Name = "pendingBox";
            pendingBox.Size = new Size(406, 23);
            pendingBox.TabIndex = 33;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 243);
            label10.Name = "label10";
            label10.Size = new Size(54, 15);
            label10.TabIndex = 34;
            label10.Text = "Pending:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(489, 8);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(327, 352);
            textBox1.TabIndex = 35;
            // 
            // MainForm
            // 
            AcceptButton = loginBut;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 410);
            Controls.Add(textBox1);
            Controls.Add(label10);
            Controls.Add(pendingBox);
            Controls.Add(reputationLabel);
            Controls.Add(flagBut);
            Controls.Add(peerReviewBut);
            Controls.Add(label8);
            Controls.Add(peerReviewBox);
            Controls.Add(getAddrBut);
            Controls.Add(sendToNameBox);
            Controls.Add(label7);
            Controls.Add(updateBut);
            Controls.Add(label6);
            Controls.Add(maskedTextBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(addByIDBut);
            Controls.Add(idBox);
            Controls.Add(nameBox);
            Controls.Add(addByNameBut);
            Controls.Add(authorsBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(doiBox);
            Controls.Add(amountBox);
            Controls.Add(label1);
            Controls.Add(createBut);
            Controls.Add(sendBut);
            Controls.Add(addrBox);
            Controls.Add(balanceLabel);
            Controls.Add(statusStrip);
            Controls.Add(loginBut);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "SciChain";
            FormClosing += MainForm_FormClosing;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)amountBox).EndInit();
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
    }
}
