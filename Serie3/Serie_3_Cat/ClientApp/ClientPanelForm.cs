using System;
using System.Windows.Forms;
using Serie_3_Cat;

namespace ClientApp
{
    public partial class ClientPanelForm : Form
    {
        public ClientPanelForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            int port;
            try
            {
                port = Convert.ToInt32(textBox1.Text);
            }
            catch (FormatException fex)
            {
                textBox1.Clear();
                MessageBox.Show(@"Invalid TCP Port! Please try again!", @"ERROR");
                return;
            }

            ClientConnection newClient = new ClientConnection("", port);

            textBox1.Clear();

            // Do stuff with the newly created client request: Show a client connection form

            ClientConnectionForm connectionForm = new ClientConnectionForm(newClient);
            connectionForm.Show();
        }
    }
}
