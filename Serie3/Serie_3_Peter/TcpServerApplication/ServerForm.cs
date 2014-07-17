using System;
using System.Windows.Forms;
using ServerClientUtils;

namespace TcpServerApplication
{
    public partial class ServerForm : Form
    {
        private readonly Listener _listener;
        private readonly Logger _log;
        /// <summary>
        ///	Application's starting point. Starts a tracking server that listens at the TCP port 
        ///	specified as a command line argument.
        /// </summary>
        public ServerForm(string portnumber)
        {
            InitializeComponent();

            label2.Text = portnumber;

            Show();

            // Checking command line arguments
            if (String.IsNullOrEmpty(portnumber))
            {
                string errorMsg = String.Format(@"Utilização: {0} <numeroPortoTCP>", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine(errorMsg);
                textBox1.Text += errorMsg;
                Environment.Exit(1);
            }
            
            ushort port;
            if (!ushort.TryParse(portnumber, out port))
            {
                string usage = String.Format(@"Usage: {0} <TCPPortNumber>", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine(usage);
                textBox1.Text += usage;
                return;
            }

            // Start servicing
            _log = new Logger(new TextBoxWriter(textBox1));

//            try
//            {
                _listener = new Listener(port);
                _listener.Run(_log);
//            }
//            finally
//            {
//                _log.Stop();
//            }
        }

        private void StartServer_Click(object sender, EventArgs e)
        {
            _listener.Stop(_log);
        }
    }
}