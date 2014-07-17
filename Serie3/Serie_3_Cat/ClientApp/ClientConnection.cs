using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Serie_3_Cat
{
    public class ClientConnection
    {
        private const string _localhost = "127.0.0.1";
        public TcpClient _clientSocket { get; private set; }
        public int _port { get; private set; }
        public string _address { get; private set; }

        public ClientConnection(string address, int port)
        {
            _address = address;
            if (_address.Equals(""))
                _address = _localhost;

            _port = port;


            var endPoint = new IPEndPoint(IPAddress.Parse(_address), _port);
            try
            {
                _clientSocket = new TcpClient();
              //  _clientSocket.ConnectAsync(IPAddress.Parse(_address), _port);

                //Store storeOperation = new Store();
                //storeOperation.Register("xpto_file_example1.txt", endPoint);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(@"Sorry! The EndPoint is invalid!", @"ERROR");
            }
        }

        public void Register(IEnumerable<string> files)//, string adress, ushort port)
        {
            _clientSocket.ConnectAsync(_address, _port);//IPAddress.Loopback, _port);

            StreamWriter output = new StreamWriter(_clientSocket.GetStream());

            // Send request type line
            output.WriteLine("REGISTER");

            // Send message payload
            foreach (string file in files)
                output.WriteLine(string.Format("{0}:{1}:{2}", file, _address, _port));

            // Send message end mark
            output.WriteLine();

            output.Close();
            _clientSocket.Close();
        }

        public void Unregister(string file)//, string address, ushort port)
        {
            _clientSocket.ConnectAsync(_address, _port);//(IPAddress.Loopback, _port);

            StreamWriter output = new StreamWriter(_clientSocket.GetStream());

            // Send request type line
            output.WriteLine("UNREGISTER");
            // Send message payload
            output.WriteLine(string.Format("{0}:{1}:{2}", file, _address, _port));
            // Send message end mark
            output.WriteLine();

            output.Close();
            _clientSocket.Close();
        }

        public void ListFiles()
        {
            _clientSocket.ConnectAsync(_address, _port);//(IPAddress.Loopback, _port);

            StreamWriter output = new StreamWriter(_clientSocket.GetStream());

            // Send request type line
            output.WriteLine("LIST_FILES");
            // Send message end mark and flush it
            output.WriteLine();
            output.Flush();

            // Read response
            string line;
            StreamReader input = new StreamReader(_clientSocket.GetStream());
            while ((line = input.ReadLine()) != null && line != string.Empty)
                Console.WriteLine(line);

            output.Close();
            _clientSocket.Close();
            
        }

        public void ListLocations(string fileName)
        {

            _clientSocket.ConnectAsync(_address, _port);//(IPAddress.Loopback, _port);

            StreamWriter output = new StreamWriter(_clientSocket.GetStream());

            // Send request type line
            output.WriteLine("LIST_LOCATIONS");
            // Send message payload
            output.WriteLine(fileName);
            // Send message end mark and flush it
            output.WriteLine();
            output.Flush();

            // Read response
            string line;
            StreamReader input = new StreamReader(_clientSocket.GetStream());
            while ((line = input.ReadLine()) != null && line != string.Empty)
                Console.WriteLine(line);

            output.Close();
            _clientSocket.Close();
            
        }


//        static void Main()
//        {
//            Register(new[] { "xpto", "ypto", "zpto" }, "192.1.1.1", 5555);
//            Register(new[] { "xpto", "ypto" }, "192.1.1.2", 5555);
//            Register(new[] { "xpto" }, "192.1.1.3", 5555);
//
//            Console.WriteLine("List files:");
//            ListFiles();
//            Console.WriteLine("List locations xpto");
//            ListLocations("xpto");
//            Console.WriteLine("List locations ypto");
//            ListLocations("ypto");
//            Console.WriteLine("List locations zpto");
//            ListLocations("zpto");
//            Console.ReadLine();
//
//            Unregister("zpto", "192.1.1.1", 5555);
//
//            Console.WriteLine("List files:");
//            ListFiles();
//            Console.WriteLine("List locations xpto");
//            ListLocations("xpto");
//            Console.WriteLine("List locations ypto");
//            ListLocations("ypto");
//            Console.WriteLine("List locations zpto");
//            ListLocations("zpto");
//            Console.ReadLine();
//
//            Unregister("xpto", "192.1.1.1", 5555);
//            Unregister("ypto", "192.1.1.1", 5555);
//
//            Console.WriteLine("List files:");
//            ListFiles();
//            Console.WriteLine("List locations xpto");
//            ListLocations("xpto");
//            Console.WriteLine("List locations ypto");
//            ListLocations("ypto");
//            Console.WriteLine("List locations zpto");
//            ListLocations("zpto");
//            Console.ReadLine();
//        }
    }
}
