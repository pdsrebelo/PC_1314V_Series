using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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

        private CancellationToken ct;
        private void button1_Click(object sender, System.EventArgs e)
        {
//          textBox2.Text = @"<This thread>";
            ct = new CancellationToken();

            int port = Convert.ToInt32(textBox1.Text);
            ClientConnection newClient = new ClientConnection("", port);

            textBox1.Clear();

            // Do stuff with the newly created client request

            ClientConnectionForm connectionForm = new ClientConnectionForm(newClient);
            connectionForm.Show();
        }
    }
}
