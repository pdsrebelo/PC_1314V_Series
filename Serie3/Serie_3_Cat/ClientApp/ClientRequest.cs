using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using ClientApp;

namespace Serie_3_Cat
{
    class ClientRequest
    {
        private const string _localhost = "127.0.0.1";

        public ClientRequest(string address, int port)
        {
            if (address.Equals(""))
                address = _localhost;

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(address), port);
            try
            {
                TcpClient client = new TcpClient();
                client.ConnectAsync(IPAddress.Parse(address), port);

                Store store = Store.Instance;

//                store.Register("xpto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
//                store.Register("xpto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
//                store.Register("xpto", new IPEndPoint(IPAddress.Parse("195.1.2.3"), 1111));
//                Utils.ShowInfo(store);
//                Console.ReadLine();
//                store.Register("ypto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
//                store.Register("ypto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
//                Utils.ShowInfo(store);
//                Console.ReadLine();
//                store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("195.1.2.3"), 1111));
//                Utils.ShowInfo(store);
//                Console.ReadLine();
//                store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
//                store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
//                Utils.ShowInfo(store);
//                Console.ReadLine();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(@"Sorry! The EndPoint is invalid!", @"ERROR");
            }
            
        }
    }
}
