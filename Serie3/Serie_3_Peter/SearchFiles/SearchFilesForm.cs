using System;
using System.Threading;
using System.Windows.Forms;
using ServerClientUtils;

namespace SearchFiles
{
    public partial class SearchFilesForm : Form
    {
        public SearchFilesForm()
        {
            InitializeComponent();

            StopButton.Enabled = false;
        }

        CancellationTokenSource _taskCts = new CancellationTokenSource();

        private void SearchButton_Click(object sender, EventArgs e)
        {
            FolderTextBox.Text = @"C:\Users\pedrorebelo\Desktop\Work\PC\SearchFolder";
            FileExtensionTextBox.Text = @"txt";
            CharSequenceTextBox.Text = @"Ola";
            
            if (String.IsNullOrEmpty(FolderTextBox.Text) || String.IsNullOrEmpty(FileExtensionTextBox.Text) ||
                String.IsNullOrEmpty(CharSequenceTextBox.Text))
                MessageBox.Show(@"Please input all the fields!");
            else
            {
                // Clear previous values
                SearchButton.Enabled = false;
                StopButton.Enabled = true;
                LogTextBox.Clear();
                progressBar1.Value = 0; // 0-100 %

                Logger logger = new Logger(new TextBoxWriter(LogTextBox));
                SearchFiles sf = new SearchFiles(logger);
                try
                {
                    sf.SearchFilesTpl(_taskCts, FolderTextBox.Text, FileExtensionTextBox.Text, CharSequenceTextBox.Text, SearchButton, StopButton, progressBar1);
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Please input valid fields!");
                }
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //SearchButton.Enabled = true;

            if (_taskCts != null)
            {
                _taskCts.Cancel();
            }
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                FolderTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}