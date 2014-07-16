using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Serie_3_Cat
{
    public partial class ServerForm : Form
    {
        private static Listener _server;
        public ServerForm()
        {
            InitializeComponent();
        }

        private void connectToServerBtn_Click(object sender, EventArgs e)
        {
            // Checking command line arguments
            var insertedPortNumber = textBox1.Text;
            var fileOutputName = textBox2.Text;

            int port;
            try
            {
                port = Convert.ToInt32(insertedPortNumber);

                textBox1.Clear();
                textBox2.Clear();

                _server = new Listener(port);


                // Start servicing
                Logger logger = !fileOutputName.Equals("") ? new Logger(fileOutputName) : new Logger();
                LoggerForm loggerForm = new LoggerForm(logger, port);

                _server.Run(logger);

                loggerForm.Show();
                logger.Start();
                loggerForm.Update();


            }
            catch (FormatException ex)
            {
                Console.WriteLine(@"\nERRO: Porto TCP invalido! Formato inserido nao e compativel com Int! {0}",
                    ex.Message);

                Console.WriteLine(@"Usage: {0} <TCPPortNumber>", AppDomain.CurrentDomain.FriendlyName);
                DialogResult result1 = MessageBox.Show(@"Invalid TCP Port! Exit Program?", "ERROR",
                    MessageBoxButtons.YesNo);

                if (result1 == DialogResult.Yes)
                    Environment.Exit(1);
                return;
            }
            catch (SocketException s)
            {
                return;
            }

            if (port <= 0)
            {
                Console.WriteLine(@"Usage: {0} <TCPPortNumber>", AppDomain.CurrentDomain.FriendlyName);
                Environment.Exit(1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _server.Stop();
        }
    }
}
