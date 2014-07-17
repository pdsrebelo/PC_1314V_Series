using System;
using System.Windows.Forms;

namespace TcpClientApplication
{
    public partial class StartClientForm : Form
    {
        public StartClientForm()
        {
            InitializeComponent();
        }

        private void StartClient_Click(object sender, EventArgs e)
        {
            try
            {
                new ClientForm(Convert.ToInt32(textBox1.Text));
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Please input a valid port!");
            }
        }
    }
}