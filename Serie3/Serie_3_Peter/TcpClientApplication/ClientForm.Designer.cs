namespace TcpClientApplication
{
    partial class ClientForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Register = new System.Windows.Forms.Button();
            this.Unregister = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.Label();
            this.ListAllFiles = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.ListFile = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 73);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(653, 280);
            this.textBox1.TabIndex = 6;
            // 
            // Register
            // 
            this.Register.Location = new System.Drawing.Point(51, 371);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(168, 55);
            this.Register.TabIndex = 8;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            this.Register.Click += new System.EventHandler(this.Register_Click);
            // 
            // Unregister
            // 
            this.Unregister.Location = new System.Drawing.Point(51, 443);
            this.Unregister.Name = "Unregister";
            this.Unregister.Size = new System.Drawing.Size(168, 58);
            this.Unregister.TabIndex = 9;
            this.Unregister.Text = "Unregister";
            this.Unregister.UseVisualStyleBackColor = true;
            this.Unregister.Click += new System.EventHandler(this.Unregister_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 25);
            this.label2.TabIndex = 11;
            // 
            // port
            // 
            this.port.AutoSize = true;
            this.port.Location = new System.Drawing.Point(226, 23);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(57, 25);
            this.port.TabIndex = 10;
            this.port.Text = "Port:";
            // 
            // ListAllFiles
            // 
            this.ListAllFiles.Location = new System.Drawing.Point(231, 443);
            this.ListAllFiles.Name = "ListAllFiles";
            this.ListAllFiles.Size = new System.Drawing.Size(168, 55);
            this.ListAllFiles.TabIndex = 12;
            this.ListAllFiles.Text = "List All Files";
            this.ListAllFiles.UseVisualStyleBackColor = true;
            this.ListAllFiles.Click += new System.EventHandler(this.ListAllFiles_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(415, 395);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(168, 31);
            this.textBox2.TabIndex = 13;
            // 
            // ListFile
            // 
            this.ListFile.Location = new System.Drawing.Point(231, 371);
            this.ListFile.Name = "ListFile";
            this.ListFile.Size = new System.Drawing.Size(168, 55);
            this.ListFile.TabIndex = 14;
            this.ListFile.Text = "List Files";
            this.ListFile.UseVisualStyleBackColor = true;
            this.ListFile.Click += new System.EventHandler(this.ListFile_Click);
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(415, 443);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(168, 55);
            this.Stop.TabIndex = 15;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 513);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.ListFile);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.ListAllFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.port);
            this.Controls.Add(this.Unregister);
            this.Controls.Add(this.Register);
            this.Controls.Add(this.textBox1);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Register;
        private System.Windows.Forms.Button Unregister;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label port;
        private System.Windows.Forms.Button ListAllFiles;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button ListFile;
        private System.Windows.Forms.Button Stop;

    }
}