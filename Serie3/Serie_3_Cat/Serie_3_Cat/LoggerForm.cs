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
            textBox1.Enabled = false; 
            _port = port;
            _logger = logger;
            _logger.setLoggerTextBox(textBox1);
            label4.Text = _port.ToString();

           // label3.Text = DateTime.Now;
        }

        public void Update()
        {
            label3.Text = _logger.start_time.ToString();
        }

        private void stopLoggerBtn_Click(object sender, EventArgs e)
        {
            _logger.Stop();
            button1.Visible = false;
        }
    }
}
