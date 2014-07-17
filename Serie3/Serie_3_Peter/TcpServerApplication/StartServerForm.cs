using System;
using System.Windows.Forms;

namespace TcpServerApplication
{
    public partial class StartServerForm : Form
    {
        public StartServerForm()
        {
            InitializeComponent();
        }

        private void StartServer_Click(object sender, EventArgs e)
        {
            new ServerForm(textBox1.Text);
        }
    }
}