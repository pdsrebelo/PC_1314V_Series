using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using ServerClientUtils;

namespace TcpClientApplication
{
    public class Client
    {
        private readonly int _port;
        private readonly IPAddress _address;
        private readonly Logger _log;

        public Client(int port, IPAddress address, Logger log)
        {
            _port = port;
            _address = address; // IPAddress.Loopback
            _log = log;

            _log.Start();
        }
        
        public void Register(IEnumerable<string> files)
        {
            _log.IncrementRequests();

            using (var client = new TcpClient())
            {
                client.Connect(_address, _port);
        
//                StreamWriter output = new StreamWriter(client.GetStream());
        
                // Send request type line
//                output.WriteLine("REGISTER");
                _log.LogMessage("REGISTER");
        
                // Send message payload
                foreach (string file in files)
                    _log.LogMessage(String.Format("{0}:{1}:{2}", file, _address, _port));
//                    output.WriteLine("{0}:{1}:{2}", file, _address, _port);
        
                // Send message end mark
//                output.WriteLine();
                _log.EndMark();

//                output.Close();
                client.Close();
            }
        }
        
        public void Unregister(string file)
        {
            using (var client = new TcpClient())
            {
                client.Connect(_address, _port);
        
//                StreamWriter output = new StreamWriter(client.GetStream());
        
                // Send request type line
//                output.WriteLine("UNREGISTER");
                _log.LogMessage("UNREGISTER");

                // Send message payload
//                output.WriteLine("{0}:{1}:{2}", file, _address, _port);
                _log.LogMessage(String.Format("{0}:{1}:{2}", file, _address, _port));
                
                // Send message end mark
//                output.WriteLine();
                _log.EndMark();

//                output.Close();
                client.Close();
            }
        }
        
        public void ListFiles()
        {
            using (var socket = new TcpClient())
            {
                socket.Connect(_address, _port);
        
//                StreamWriter output = new StreamWriter(socket.GetStream());
        
                // Send request type line
//                output.WriteLine("LIST_FILES");
                _log.LogMessage("LIST_FILES");

                // Send message end mark and flush it
//                output.WriteLine();
//                output.Flush();
                _log.EndMarkAndFlush();
        
                // Read response
                string line;
                StreamReader input = new StreamReader(socket.GetStream());
                while ((line = input.ReadLine()) != null && line != string.Empty)
                    _log.LogMessage(line);
//                    Console.WriteLine(line);
        
//                output.Close();
                input.Close();
                socket.Close();
            }
        }
        
        public void ListLocations(string fileName)
        {
            using (var socket = new TcpClient())
            {
                socket.Connect(_address, _port);
             
                _log.LogMessage(String.Format("List locations {0}", fileName));

//                StreamWriter output = new StreamWriter(socket.GetStream());
        
                // Send request type line
//                output.WriteLine("LIST_LOCATIONS");
                _log.LogMessage("LIST_LOCATIONS");
                
                // Send message payload
//                output.WriteLine(fileName);
                _log.LogMessage(fileName);

                // Send message end mark and flush it
//                output.WriteLine();
//                output.Flush();
                _log.EndMarkAndFlush();

                // Read response
                string line;
                StreamReader input = new StreamReader(socket.GetStream());
                while ((line = input.ReadLine()) != null && line != string.Empty)
                    _log.LogMessage(line);
//                    Console.WriteLine(line);
        
//                output.Close();
                input.Close();
                socket.Close();
            }
        }

        public void Stop()
        {
            _log.Stop();
        }
    }
}