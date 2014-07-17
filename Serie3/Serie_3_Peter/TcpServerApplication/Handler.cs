using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ServerClientUtils;

namespace TcpServerApplication
{
    /// <summary>
    /// Handles client requests.
    /// </summary>
    public sealed class Handler
    {
        #region Message Handlers

        /// <summary>
        /// Data structure that supports message processing dispatch.
        /// </summary>
        private static readonly Dictionary<string, Action<StreamReader, StreamWriter, Logger>> MESSAGE_HANDLERS;

        static Handler()
        {
            MESSAGE_HANDLERS = new Dictionary<string, Action<StreamReader, StreamWriter, Logger>>();
            MESSAGE_HANDLERS["REGISTER"] = ProcessRegisterMessage;
            MESSAGE_HANDLERS["UNREGISTER"] = ProcessUnregisterMessage;
            MESSAGE_HANDLERS["LIST_FILES"] = ProcessListFilesMessage;
            MESSAGE_HANDLERS["LIST_LOCATIONS"] = ProcessListLocationsMessage;
        }

        /// <summary>
        /// Handles REGISTER messages.
        /// </summary>
        private static void ProcessRegisterMessage(StreamReader input, StreamWriter output, Logger log)
        {
            // Read message payload, terminated by an empty line. 
            // Each payload line has the following format
            // <filename>:<ipAddress>:<portNumber>
            string line;
            while ((line = input.ReadLine()) != null && line != string.Empty)
            {
                string[] triple = line.Split(':');

                if (triple.Length != 3)
                {
                    log.LogMessage("Handler - Invalid REGISTER message.");
                    return;
                }

                IPAddress ipAddress = IPAddress.Parse(triple[1]);

                ushort port;

                if (!ushort.TryParse(triple[2], out port))
                {
                    log.LogMessage("Handler - Invalid REGISTER message.");
                    return;
                }

                Store.Instance.Register(triple[0], new IPEndPoint(ipAddress, port));
            }

            // This request message does not have a corresponding response message, hence, 
            // nothing is sent to the client.
        }

        /// <summary>
        /// Handles UNREGISTER messages.
        /// </summary>
        private static void ProcessUnregisterMessage(StreamReader input, StreamWriter output, Logger log)
        {
            // Read message payload, terminated by an empty line. 
            // Each payload line has the following format
            // <filename>:<ipAddress>:<portNumber>
            string line;
            while ((line = input.ReadLine()) != null && line != string.Empty)
            {
                string[] triple = line.Split(':');

                if (triple.Length != 3)
                {
                    log.LogMessage("Handler - Invalid UNREGISTER message.");
                    return;
                }

                IPAddress ipAddress = IPAddress.Parse(triple[1]);

                ushort port;

                if (!ushort.TryParse(triple[2], out port))
                {
                    log.LogMessage("Handler - Invalid UNREGISTER message.");
                    return;
                }

                Store.Instance.Unregister(triple[0], new IPEndPoint(ipAddress, port));
            }

            // This request message does not have a corresponding response message, hence, 
            // nothing is sent to the client.
        }

        /// <summary>
        /// Handles LIST_FILES messages.
        /// </summary>
        private static void ProcessListFilesMessage(StreamReader input, StreamWriter output, Logger log)
        {
            // Request message does not have a payload.
            // Read end message mark (empty line)
            input.ReadLine();

            string[] trackedFiles = Store.Instance.GetTrackedFiles();

            // Send response message. 
            // The message is composed of multiple lines and is terminated by an empty one.
            // Each line contains a name of a tracked file.
            foreach (string file in trackedFiles)
                output.WriteLine(file);

            // End response and flush it.
            output.WriteLine();
            output.Flush();
        }

        /// <summary>
        /// Handles LIST_LOCATIONS messages.
        /// </summary>
        private static void ProcessListLocationsMessage(StreamReader input, StreamWriter output, Logger log)
        {
            // Request message payload is composed of a single line containing the file name.
            // The end of the message's payload is marked with an empty line
            string line = input.ReadLine();
            input.ReadLine();

            IPEndPoint[] fileLocations = Store.Instance.GetFileLocations(line);

            // Send response message. 
            // The message is composed of multiple lines and is terminated by an empty one.
            // Each line has the following format
            // <ipAddress>:<portNumber>
            foreach (IPEndPoint endpoint in fileLocations)
                output.WriteLine("{0}:{1}", endpoint.Address, endpoint.Port);

            // End response and flush it.
            output.WriteLine();
            output.Flush();
        }

        #endregion

        /// <summary>
        /// The handler's input (from the TCP connection)
        /// </summary>
        private readonly StreamReader _input;

        /// <summary>
        /// The handler's output (to the TCP connection)
        /// </summary>
        private readonly StreamWriter _output;

        /// <summary>
        /// The Logger instance to be used.
        /// </summary>
        private readonly Logger _log;

        /// <summary>
        ///	Initiates an instance with the given parameters.
        /// </summary>
        /// <param name="connection">The TCP connection to be used.</param>
        /// <param name="log">the Logger instance to be used.</param>
        public Handler(Stream connection, Logger log)
        {
            _log = log;
            _output = new StreamWriter(connection);
            _input = new StreamReader(connection);
        }

        /// <summary>
        /// Performs request servicing.
        /// </summary>
        public void Run()
        {
            try
            {
                string requestType;
                // Read request type (the request's first line)
                while ((requestType = _input.ReadLine()) != null && requestType != string.Empty)
                {
                    requestType = requestType.ToUpper();
                    if (!MESSAGE_HANDLERS.ContainsKey(requestType))
                    {
                        _log.LogMessage("Handler - Unknown message type. Servicing ending.");
                        return;
                    }
                    // Dispatch request processing
                    MESSAGE_HANDLERS[requestType](_input, _output, _log);
                }
            }
            catch (IOException ioe)
            {
                // Connection closed by the client. Log it!
                _log.LogMessage(String.Format("Handler - Connection closed by client {0}", ioe));
            }
            finally
            {
                _input.Close();
                _output.Close();
            }
        }
    }
}
