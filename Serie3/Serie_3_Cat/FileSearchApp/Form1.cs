using System;
using System.Windows.Forms;

namespace FileSearchApp
{
    public partial class Form1 : Form
    {
        private FileSearcher _searcher;
        public Form1()
        {
            InitializeComponent();
        }

        // Search Button Clicked
        private void button1_Click(object sender, EventArgs e)
        {
            //TODO Start Searching
            string rootFolder = textBox1.Text;
            string extension = searchResultsTextBox.Text;
        }

        // Cancel Button Clicked
        private void button2_Click(object sender, EventArgs e)
        {
            //TODO Use the cancellation Token to cancel the search that is in progress 
        }
    }
}
