using System;
using System.Net;
using System.Windows.Forms;
using ServerClientUtils;

namespace TcpClientApplication
{
    public partial class ClientForm : Form
    {
        private readonly Client _client;
        private readonly Logger _log;

        public ClientForm(int port)
        {
            InitializeComponent();

            label2.Text = Convert.ToString(port);

            _log = new Logger(new TextBoxWriter(textBox1));

            _client = new Client(port, IPAddress.Loopback, _log);

            Show();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            _client.Register(new[] { "xpto", "ypto", "zpto" });
        }

        private void Unregister_Click(object sender, EventArgs e)
        {
            _client.Unregister("zpto");
            _client.Unregister("xpto");
            _client.Unregister("ypto");
        }

        private void ListAllFiles_Click(object sender, EventArgs e)
        {
            _client.ListFiles();
            Console.WriteLine(@"List files:");
        }

        private void ListFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
                _client.ListLocations(textBox2.Text);
            else
                MessageBox.Show(@"Please input a valid file");
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            // Stops the logger
            _client.Stop();
        }
    }
}