using System;
using System.Windows.Forms;
using ServerClientUtils;

namespace SearchFiles
{
    public partial class SearchFilesForm : Form
    {
        public SearchFilesForm()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            FolderTextBox.Text = @"C:\Users\pedrorebelo\Desktop\Work\PC\SearchFolder";
            FileExtensionTextBox.Text = @"txt";
            CharSequenceTextBox.Text = @"abc";
            
            if (String.IsNullOrEmpty(FolderTextBox.Text) || String.IsNullOrEmpty(FileExtensionTextBox.Text) ||
                String.IsNullOrEmpty(CharSequenceTextBox.Text))
                MessageBox.Show(@"Please input all the fields!");
            else
            {
                SearchButton.Enabled = false;
                LogTextBox.Clear();

                Logger logger = new Logger(new TextBoxWriter(LogTextBox));
                SearchFiles sf = new SearchFiles(logger);
                try
                {
                    sf.SearchFilesTpl(FolderTextBox.Text, FileExtensionTextBox.Text, CharSequenceTextBox.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Please input valid fields!");
                }
                SearchButton.Enabled = true;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            SearchButton.Enabled = true;
        }

        private void SearchFilesForm_Load(object sender, EventArgs e)
        {

        }
    }
}