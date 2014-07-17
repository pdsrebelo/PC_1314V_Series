using System;
using System.Net;
using System.Windows.Forms;

namespace TcpClientApplication
{
    public partial class ClientForm : Form
    {
        private readonly Client _client;

        public ClientForm(int port)
        {
            InitializeComponent();

            label2.Text = Convert.ToString(port);

            _client = new Client(port, IPAddress.Loopback);

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

//            Console.WriteLine(@"List locations xpto");
//            client.ListLocations("xpto");
//            
//            Console.WriteLine(@"List locations ypto");
//            client.ListLocations("ypto");
//            
//            Console.WriteLine(@"List locations zpto");
//            client.ListLocations("zpto");
//            Console.ReadLine();
    }
}