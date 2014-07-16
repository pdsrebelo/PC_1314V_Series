using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Serie_3_Cat;

namespace ClientApp
{
    public partial class ClientPanelForm : Form
    {

        public ClientPanelForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
//            textBox2.Text = @"<This thread>";
            ClientRequest newClient = new ClientRequest(textBox2.Text, Convert.ToInt32(textBox1.Text));

            // Do stuff with the newly created client request
        }

    }
}
