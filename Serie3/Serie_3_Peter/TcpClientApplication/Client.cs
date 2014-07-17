using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpClientApplication
{
    public class Client
    {
        private readonly int _port;
        private readonly IPAddress _address;

        public Client(int port, IPAddress address)
        {
            _port = port;
            _address = address; // IPAddress.Loopback
        }
        
        public void Register(IEnumerable<string> files)
        {
            using (var client = new TcpClient())
            {
                client.Connect(_address, _port);
        
                StreamWriter output = new StreamWriter(client.GetStream());
        
                // Send request type line
                output.WriteLine("REGISTER");
        
                // Send message payload
                foreach (string file in files)
                    output.WriteLine("{0}:{1}:{2}", file, _address, _port);
        
                // Send message end mark
                output.WriteLine();
        
                output.Close();
                client.Close();
            }
        }
        
        public void Unregister(string file)
        {
            using (var client = new TcpClient())
            {
                client.Connect(_address, _port);
        
                StreamWriter output = new StreamWriter(client.GetStream());
        
                // Send request type line
                output.WriteLine("UNREGISTER");
                
                // Send message payload
                output.WriteLine("{0}:{1}:{2}", file, _address, _port);
                
                // Send message end mark
                output.WriteLine();
        
                output.Close();
                client.Close();
            }
        }
        
        public void ListFiles()
        {
            using (var socket = new TcpClient())
            {
                socket.Connect(_address, _port);
        
                StreamWriter output = new StreamWriter(socket.GetStream());
        
                // Send request type line
                output.WriteLine("LIST_FILES");
                // Send message end mark and flush it
                output.WriteLine();
                output.Flush();
        
                // Read response
                string line;
                StreamReader input = new StreamReader(socket.GetStream());
                while ((line = input.ReadLine()) != null && line != string.Empty)
                    Console.WriteLine(line);
        
                output.Close();
                socket.Close();
            }
        }
        
        public void ListLocations(string fileName)
        {
            using (var socket = new TcpClient())
            {
                socket.Connect(_address, _port);
        
                StreamWriter output = new StreamWriter(socket.GetStream());
        
                // Send request type line
                output.WriteLine("LIST_LOCATIONS");
                // Send message payload
                output.WriteLine(fileName);
                // Send message end mark and flush it
                output.WriteLine();
                output.Flush();
        
                // Read response
                string line;
                StreamReader input = new StreamReader(socket.GetStream());
                while ((line = input.ReadLine()) != null && line != string.Empty)
                    Console.WriteLine(line);
        
                output.Close();
                socket.Close();
            }
        }
    }
}