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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            loginBut = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            createBut = new Button();
            openFileDialog = new OpenFileDialog();
            label2 = new Label();
            fileBox = new TextBox();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // loginBut
            // 
            loginBut.Location = new Point(0, 168);
            loginBut.Name = "loginBut";
            loginBut.Size = new Size(117, 34);
            loginBut.TabIndex = 0;
            loginBut.Text = "Login";
            loginBut.UseVisualStyleBackColor = true;
            loginBut.Click += loginBut_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip1.Location = new Point(0, 205);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(403, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 17);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 2;
            label1.Text = "Balance:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 36);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(297, 23);
            textBox1.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(315, 36);
            button1.Name = "button1";
            button1.Size = new Size(81, 23);
            button1.TabIndex = 4;
            button1.Text = "Send";
            button1.UseVisualStyleBackColor = true;
            // 
            // createBut
            // 
            createBut.Location = new Point(315, 65);
            createBut.Name = "createBut";
            createBut.Size = new Size(81, 23);
            createBut.TabIndex = 5;
            createBut.Text = "Create Block";
            createBut.UseVisualStyleBackColor = true;
            createBut.Click += createBut_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 69);
            label2.Name = "label2";
            label2.Size = new Size(28, 15);
            label2.TabIndex = 6;
            label2.Text = "File:";
            // 
            // fileBox
            // 
            fileBox.Location = new Point(46, 65);
            fileBox.Name = "fileBox";
            fileBox.Size = new Size(263, 23);
            fileBox.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(403, 227);
            Controls.Add(fileBox);
            Controls.Add(label2);
            Controls.Add(createBut);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Controls.Add(loginBut);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "SciChain";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button loginBut;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private Button createBut;
        private OpenFileDialog openFileDialog;
        private Label label2;
        private TextBox fileBox;
    }
}
