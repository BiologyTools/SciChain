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
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // loginBut
            // 
            loginBut.Location = new Point(113, 3);
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
            statusStrip1.Location = new Point(0, 142);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(349, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 17);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(349, 164);
            Controls.Add(statusStrip1);
            Controls.Add(loginBut);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
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
    }
}
