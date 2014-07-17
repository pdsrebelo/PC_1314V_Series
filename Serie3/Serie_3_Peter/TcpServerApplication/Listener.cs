using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using ServerClientUtils;

namespace TcpServerApplication
{
    /// <summary>
    /// This class instances are file tracking servers. They are responsible for accepting 
    /// and managing established TCP connections.
    /// </summary>
    public sealed class Listener
    {
        private TcpListener _srv;

        /// <summary>
        /// The amount of time to remain connected after calling 
        /// the Socket.Close method if data remains to be sent.
        /// </summary>
        private const int LingerTime = 3; 

        /// <summary>
        /// TCP port number in use.
        /// </summary>
        private readonly int _portNumber;

        /// <summary> Initiates a tracking server instance.</summary>
        /// <param name="portNumber"> The TCP port number to be used.</param>
        public Listener(int portNumber)
        {
            _portNumber = portNumber;
        }

        public static void ShowInfo(Store store)
        {
            foreach (string fileName in store.GetTrackedFiles())
            {
                Console.WriteLine(fileName);
                foreach (IPEndPoint endPoint in store.GetFileLocations(fileName))
                {
                    Console.Write(endPoint + @" ; ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        ///	Server's main loop implementation.
        /// </summary>
        /// <param name="log"> The Logger instance to be used.</param>
        public void Run(Logger log)
        {
            try
            {
                _srv = new TcpListener(IPAddress.Loopback, _portNumber);
                _srv.Start();
                
                log.Start();

                AsyncCallback callback = null;

                callback = delegate(IAsyncResult iar)
                {
                    TcpClient socket = _srv.EndAcceptTcpClient(iar);
                    log.LogMessage("Listener - Waiting for connection requests.");
                    _srv.BeginAcceptTcpClient(callback, _srv);

                    // Process the previous request
                    try
                    {
                        socket.LingerState = new LingerOption(true, LingerTime);
                        log.LogMessage(String.Format("Listener - Connection established with {0}.",
                            socket.Client.RemoteEndPoint));

                        log.LogMessage(String.Format("Listener - Connection is going to last {0} seconds", 
                            LingerTime));

                        // Instantiating protocol handler and associate it to the current TCP connection
                        Handler protocolHandler = new Handler(socket.GetStream(), log);

                        // Synchronously process requests made through de current TCP connection
                        protocolHandler.Run();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    ShowInfo(Store.Instance);
                };

                log.LogMessage("Listener - Waiting for connection requests.");
                _srv.BeginAcceptTcpClient(callback, _srv);
            }
            catch (Exception)
            {
                log.LogMessage("Listener - Ending.");
                log.Stop();
                _srv.Stop();
            }
        }

        public void Stop(Logger log)
        {
            log.LogMessage("Listener - Ending.");
            _srv.Stop();
        }
    }
}