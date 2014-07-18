using System;
using System.Windows.Forms;
using Serie_3_Cat;

namespace FileSearchApp
{
    public partial class FileSearcherMainView : Form
    {
        private FileSearcher _searcher;
        public FileSearcherMainView()
        {
            InitializeComponent();
        }

        // Search Button Clicked
        private void button1_Click(object sender, EventArgs e)
        {
            //TODO Start Searching
            string rootFolder = rootFolderTextBox.Text;
            string extension = searchResultsTextBox.Text;
            string charSequenceToSearch = charSequenceTextBox.Text;

            if (rootFolder.Equals("") || extension.Equals("") || charSequenceToSearch.Equals(""))
            {
                MessageBox.Show(@"Please fill all the necessary fields!", @"WARNING");
                return;
            }

            var search = FileSearcher.startSearch(rootFolder, extension, charSequenceToSearch, 
                new TextBoxStreamWriter(searchResultsTextBox));
        }

        // Cancel Button Clicked
        private void button2_Click(object sender, EventArgs e)
        {
            //TODO Use the cancellation Token to cancel the search that is in progress 
            FileSearcher.startSearch();
        }
    }
}
