namespace Serie_3_Cat
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectToServerBtn = new System.Windows.Forms.Button();
            this.welcomeMessageLbl = new System.Windows.Forms.Label();
            this.clickBtnMessageLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // connectToServerBtn
            // 
            this.connectToServerBtn.Location = new System.Drawing.Point(208, 109);
            this.connectToServerBtn.Name = "connectToServerBtn";
            this.connectToServerBtn.Size = new System.Drawing.Size(109, 21);
            this.connectToServerBtn.TabIndex = 0;
            this.connectToServerBtn.Text = "Start Server";
            this.connectToServerBtn.UseVisualStyleBackColor = true;
            this.connectToServerBtn.Click += new System.EventHandler(this.connectToServerBtn_Click);
            // 
            // welcomeMessageLbl
            // 
            this.welcomeMessageLbl.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.welcomeMessageLbl.AutoSize = true;
            this.welcomeMessageLbl.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.welcomeMessageLbl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.welcomeMessageLbl.ForeColor = System.Drawing.Color.Purple;
            this.welcomeMessageLbl.Location = new System.Drawing.Point(143, 9);
            this.welcomeMessageLbl.Name = "welcomeMessageLbl";
            this.welcomeMessageLbl.Size = new System.Drawing.Size(55, 13);
            this.welcomeMessageLbl.TabIndex = 1;
            this.welcomeMessageLbl.Text = "Welcome!";
            this.welcomeMessageLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clickBtnMessageLabel
            // 
            this.clickBtnMessageLabel.AutoSize = true;
            this.clickBtnMessageLabel.Location = new System.Drawing.Point(99, 93);
            this.clickBtnMessageLabel.Name = "clickBtnMessageLabel";
            this.clickBtnMessageLabel.Size = new System.Drawing.Size(218, 13);
            this.clickBtnMessageLabel.TabIndex = 2;
            this.clickBtnMessageLabel.Text = "Press the following button to start the Server!";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(146, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkOrchid;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "TCP Port #";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkOrchid;
            this.label2.Location = new System.Drawing.Point(15, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "File Name <Optional>";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(146, 52);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 142);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.clickBtnMessageLabel);
            this.Controls.Add(this.welcomeMessageLbl);
            this.Controls.Add(this.connectToServerBtn);
            this.ForeColor = System.Drawing.Color.DarkBlue;
            this.Name = "ServerForm";
            this.Text = "~*~ Server App ~*~";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectToServerBtn;
        private System.Windows.Forms.Label welcomeMessageLbl;
        private System.Windows.Forms.Label clickBtnMessageLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
    }
}

