using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Serie_3_Cat;


/// <summary>
/// This class instances are file tracking servers. They are responsible for accepting 
/// and managing established TCP connections.
/// </summary>
public class Listener
{
    /// <summary>
    /// TCP port number in use.
    /// </summary>
    private readonly int portNumber;

    private TcpListener _server;

    /// <summary> Initiates a tracking server instance.</summary>
    /// <param name="_portNumber"> The TCP port number to be used.</param>
    public Listener(int _portNumber)
    {
        portNumber = _portNumber;
    }

    /// <summary>
    ///	Server's main loop implementation.
    /// </summary>
    /// <param name="log"> The Logger instance to be used.</param>

    public void Run(Logger log)
    {
        try{
            try
            {
                _server = new TcpListener(IPAddress.Loopback, portNumber);
                _server.Start();
            }
            catch (ArgumentOutOfRangeException argex)
            {
                MessageBox.Show(@"The port number inserted is not valid!", @"ERROR");
                _server = null;
                throw argex;
            }
            catch (SocketException sock)
            {
                MessageBox.Show(@"The specified port is already in use by another server!",
                    @"ERROR");
                _server = null;
                throw sock;
            }

            // Define the callback method - which will be called whenever a client request ends;
            AsyncCallback onAcceptCallback = null;
            int nseconds = 10;

            onAcceptCallback = delegate(IAsyncResult ar)
            {
                log.IncrementRequests();
                TcpClient socket = null;
                EndPoint clientEndPoint = null;
                try
                {
                    // Asynchronously accept an incoming connection attempt 
                    // and create a new TcpClient to handle remote host communication
                    socket = _server.EndAcceptTcpClient(ar);

                    // Begin an async operation: to accept an incoming connection attempt
                    _server.BeginAcceptTcpClient(onAcceptCallback, _server);

                    // Process the accepted connection.
                    /////////////////////////////////////////////////////////////////////
                    log.LogMessage("Listener - Processing the accepted connection...");
                    
                    socket.LingerState = new LingerOption(true, nseconds);

                    clientEndPoint = socket.Client.RemoteEndPoint;
                    log.LogMessage(String.Format("Listener - Connection established with {0}.",
                        clientEndPoint));

                    // Instantiate protocol handler and associate it to the current TCP connection
                    var protocolHandler = new Program.Handler(socket.GetStream(), log);
                        
                    // Synchronously process request made through the connection
                    protocolHandler.Run();

                    
                    Utils.ShowInfo(Store.Instance, log.Writer);
                    /////////////////////////////////////////////////////////////////////
                }
                catch (SocketException sockex)
                {
                    Console.WriteLine("***socket exception occurred: {0}", sockex.Message);

                }
                catch (ObjectDisposedException)
                {
                    //
                    // This exception happens when the listener socket is closed.
                    // So, we just ignore it!
                    //
                }
                log.LogMessage(socket != null && socket.Connected
                    ? String.Format("Listener - Time left for socket to be disconnected: {0} seconds.",
                        socket.Client.LingerState.LingerTime)
                    : String.Format("Listener - Connection closed with {0}.", clientEndPoint));
            };

            // Begins an asynchronous operation to accept an incoming connection attempt.
            _server.BeginAcceptTcpClient(onAcceptCallback, null);
        }
       
        finally
        {
            if (_server == null)
            {
                log.LogMessage("listener - ERROR: Server is undefined!");
            }
  
            //                    else
            //                    {
            //                        log.LogMessage("Listener - Ending.");
            //                        srv.Stop();
            //                    }
        }
    }

    internal void Stop()
    {
        _server.Stop();
    }
}

