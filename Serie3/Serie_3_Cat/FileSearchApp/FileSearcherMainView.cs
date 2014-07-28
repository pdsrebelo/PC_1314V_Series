using System;
using System.Windows.Forms;
using Serie_3_Cat;

namespace FileSearchApp
{
    public partial class FileSearcherMainView : Form
    {
        public FileSearcherMainView()
        {
            InitializeComponent();
        }

        // Search Button Clicked
        private void button1_Click(object sender, EventArgs e)
        {
            //TODO Start Searching
            string rootFolder = rootFolderTextBox.Text;
            string extension = fileExtensionTextBox.Text;
            string charSequenceToSearch = charSequenceTextBox.Text;

            if (rootFolder.Equals("") || extension.Equals("") || charSequenceToSearch.Equals(""))
            {
                MessageBox.Show(@"Please fill all the necessary fields!", @"WARNING");
                return;
            }

            searchResultsTextBox.Clear();

            var search = FileSearcher.startSearch(rootFolder, extension, charSequenceToSearch);

            TextBoxStreamWriter writer = new TextBoxStreamWriter(searchResultsTextBox);
            foreach (var file in search._filesWithExtensionAndSequence)
            {
                writer.WriteLine(file);
            }
        }

        // Cancel Button Clicked
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Cancel button isn't working yet!", @"Warning");
            
            //TODO Use the cancellation Token to cancel the search that is in progress 
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
