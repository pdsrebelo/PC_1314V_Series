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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.browseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(51, 610);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(6);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(348, 75);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(412, 610);
            this.StopButton.Margin = new System.Windows.Forms.Padding(6);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(348, 75);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(52, 548);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(708, 44);
            this.progressBar1.TabIndex = 2;
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(46, 29);
            this.FolderLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(73, 25);
            this.FolderLabel.TabIndex = 3;
            this.FolderLabel.Text = "Folder";
            // 
            // FolderTextBox
            // 
            this.FolderTextBox.Location = new System.Drawing.Point(131, 23);
            this.FolderTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.FolderTextBox.Name = "FolderTextBox";
            this.FolderTextBox.ReadOnly = true;
            this.FolderTextBox.Size = new System.Drawing.Size(401, 31);
            this.FolderTextBox.TabIndex = 4;
            // 
            // FileExtensionTextBox
            // 
            this.FileExtensionTextBox.Location = new System.Drawing.Point(202, 73);
            this.FileExtensionTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.FileExtensionTextBox.Name = "FileExtensionTextBox";
            this.FileExtensionTextBox.Size = new System.Drawing.Size(262, 31);
            this.FileExtensionTextBox.TabIndex = 6;
            // 
            // FileExtensionLabel
            // 
            this.FileExtensionLabel.AutoSize = true;
            this.FileExtensionLabel.Location = new System.Drawing.Point(46, 79);
            this.FileExtensionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.FileExtensionLabel.Name = "FileExtensionLabel";
            this.FileExtensionLabel.Size = new System.Drawing.Size(148, 25);
            this.FileExtensionLabel.TabIndex = 5;
            this.FileExtensionLabel.Text = "File Extension";
            // 
            // extensionExamples
            // 
            this.extensionExamples.AutoSize = true;
            this.extensionExamples.Location = new System.Drawing.Point(476, 79);
            this.extensionExamples.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.extensionExamples.Name = "extensionExamples";
            this.extensionExamples.Size = new System.Drawing.Size(168, 25);
            this.extensionExamples.TabIndex = 7;
            this.extensionExamples.Text = "(e.g. txt, jpeg,...)";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(52, 173);
            this.LogTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(704, 360);
            this.LogTextBox.TabIndex = 8;
            // 
            // CharSequenceTextBox
            // 
            this.CharSequenceTextBox.Location = new System.Drawing.Point(268, 123);
            this.CharSequenceTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.CharSequenceTextBox.Name = "CharSequenceTextBox";
            this.CharSequenceTextBox.Size = new System.Drawing.Size(196, 31);
            this.CharSequenceTextBox.TabIndex = 10;
            // 
            // CharSequenceLabel
            // 
            this.CharSequenceLabel.AutoSize = true;
            this.CharSequenceLabel.Location = new System.Drawing.Point(46, 129);
            this.CharSequenceLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.CharSequenceLabel.Name = "CharSequenceLabel";
            this.CharSequenceLabel.Size = new System.Drawing.Size(209, 25);
            this.CharSequenceLabel.TabIndex = 9;
            this.CharSequenceLabel.Text = "Character Sequence";
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(541, 19);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(116, 44);
            this.browseBtn.TabIndex = 11;
            this.browseBtn.Text = "Browse ...";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // SearchFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 700);
            this.Controls.Add(this.browseBtn);
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
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "SearchFilesForm";
            this.Text = "Search Files";
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
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button browseBtn;
    }
}