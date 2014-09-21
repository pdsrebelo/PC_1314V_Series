using System;
using System.Net;
using System.Net.Sockets;
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
                int port = Convert.ToInt32(textBox1.Text);

                // Test if the given port has a server associated
                var client = new TcpClient();
                client.Connect(IPAddress.Loopback, port);
                client.Close();

                new ClientForm(port);
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Please input a valid port!");
            }
            catch (SocketException)
            {
                MessageBox.Show(@"There's no server in that port!");
            }
        }
    }
}