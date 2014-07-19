namespace SearchFiles
{
    partial class SearchFilesForm
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
            this.SearchButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.FolderTextBox = new System.Windows.Forms.TextBox();
            this.FileExtensionTextBox = new System.Windows.Forms.TextBox();
            this.FileExtensionLabel = new System.Windows.Forms.Label();
            this.extensionExamples = new System.Windows.Forms.Label();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.CharSequenceTextBox = new System.Windows.Forms.TextBox();
            this.CharSequenceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(206, 313);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(174, 39);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(26, 314);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(174, 39);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(26, 285);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(354, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(23, 15);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(36, 13);
            this.FolderLabel.TabIndex = 3;
            this.FolderLabel.Text = "Folder";
            // 
            // FolderTextBox
            // 
            this.FolderTextBox.Location = new System.Drawing.Point(67, 8);
            this.FolderTextBox.Name = "FolderTextBox";
            this.FolderTextBox.Size = new System.Drawing.Size(256, 20);
            this.FolderTextBox.TabIndex = 4;
            // 
            // FileExtensionTextBox
            // 
            this.FileExtensionTextBox.Location = new System.Drawing.Point(101, 34);
            this.FileExtensionTextBox.Name = "FileExtensionTextBox";
            this.FileExtensionTextBox.Size = new System.Drawing.Size(133, 20);
            this.FileExtensionTextBox.TabIndex = 6;
            // 
            // FileExtensionLabel
            // 
            this.FileExtensionLabel.AutoSize = true;
            this.FileExtensionLabel.Location = new System.Drawing.Point(23, 41);
            this.FileExtensionLabel.Name = "FileExtensionLabel";
            this.FileExtensionLabel.Size = new System.Drawing.Size(72, 13);
            this.FileExtensionLabel.TabIndex = 5;
            this.FileExtensionLabel.Text = "File Extension";
            // 
            // extensionExamples
            // 
            this.extensionExamples.AutoSize = true;
            this.extensionExamples.Location = new System.Drawing.Point(240, 37);
            this.extensionExamples.Name = "extensionExamples";
            this.extensionExamples.Size = new System.Drawing.Size(83, 13);
            this.extensionExamples.TabIndex = 7;
            this.extensionExamples.Text = "(e.g. txt, jpeg,...)";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(26, 90);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(354, 189);
            this.LogTextBox.TabIndex = 8;
            // 
            // CharSequenceTextBox
            // 
            this.CharSequenceTextBox.Location = new System.Drawing.Point(134, 64);
            this.CharSequenceTextBox.Name = "CharSequenceTextBox";
            this.CharSequenceTextBox.Size = new System.Drawing.Size(100, 20);
            this.CharSequenceTextBox.TabIndex = 10;
            // 
            // CharSequenceLabel
            // 
            this.CharSequenceLabel.AutoSize = true;
            this.CharSequenceLabel.Location = new System.Drawing.Point(23, 67);
            this.CharSequenceLabel.Name = "CharSequenceLabel";
            this.CharSequenceLabel.Size = new System.Drawing.Size(105, 13);
            this.CharSequenceLabel.TabIndex = 9;
            this.CharSequenceLabel.Text = "Character Sequence";
            // 
            // SearchFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 364);
            this.Controls.Add(this.CharSequenceTextBox);
            this.Controls.Add(this.CharSequenceLabel);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.extensionExamples);
            this.Controls.Add(this.FileExtensionTextBox);
            this.Controls.Add(this.FileExtensionLabel);
            this.Controls.Add(this.FolderTextBox);
            this.Controls.Add(this.FolderLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.SearchButton);
            this.Name = "SearchFilesForm";
            this.Text = "Search Files";
            this.Load += new System.EventHandler(this.SearchFilesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label FolderLabel;
        private System.Windows.Forms.TextBox FolderTextBox;
        private System.Windows.Forms.TextBox FileExtensionTextBox;
        private System.Windows.Forms.Label FileExtensionLabel;
        private System.Windows.Forms.Label extensionExamples;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.TextBox CharSequenceTextBox;
        private System.Windows.Forms.Label CharSequenceLabel;
    }
}