using System;
using System.Windows.Forms;

namespace Serie_3_Cat
{
    public partial class LoggerForm : Form
    {
        
        private readonly Logger _logger;
        private readonly int _port;

        public LoggerForm(Logger logger, int port)
        {
            InitializeComponent();
            _logger = logger; 
            _logger.SetLoggerTextBox(textBox1); 
            textBox1.Enabled = false; 
            _port = port;
            label4.Text = _port.ToString();
        }

        public void Update()
        {
            label3.Text = _logger.StartTime.ToString();
        }

        private void stopLoggerBtn_Click(object sender, EventArgs e)
        {
            _logger.Stop();
            button1.Visible = false;
        }
    }
}
