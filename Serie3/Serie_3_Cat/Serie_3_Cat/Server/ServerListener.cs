using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace Serie_3_Cat.Server
{
    class ServerListener
    {
        private readonly int _port;
        public ServerListener(int port)
        {
            _port = port;
        }

        public void Run(Logger log)
        {
            TcpListener srv = null;
            try
            {
                srv = new TcpListener(IPAddress.Loopback, portNumber);
                srv.Start();
                while (true)
                {
                    log.LogMessage("Listener - Waiting for connection requests.");
                    using (TcpClient socket = srv.AcceptTcpClient())
                    {
                        socket.LingerState = new LingerOption(true, 10);
                        log.LogMessage(String.Format("Listener - Connection established with {0}.",
                            socket.Client.RemoteEndPoint));
                        // Instantiating protocol handler and associate it to the current TCP connection
                        Handler protocolHandler = new Handler(socket.GetStream(), log);
                        // Synchronously process requests made through de current TCP connection
                        protocolHandler.Run();
                    }

                    Program.ShowInfo(Store.Instance);
                }
            }
            finally
            {
                log.LogMessage("Listener - Ending.");
                srv.Stop();
            }
        }
    }
}
