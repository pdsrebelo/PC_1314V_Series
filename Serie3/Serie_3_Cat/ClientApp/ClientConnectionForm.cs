using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Serie_3_Cat;

namespace ClientApp
{
    public partial class ClientConnectionForm : Form
    {
        public ClientConnection _connection { get; private set; }
    

        public ClientConnectionForm(ClientConnection conn)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            _connection = conn;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //TODO Register File!
            List<string> filesToRegister = new List<string>(textBox1.Lines);
            foreach (string s in textBox1.Lines)
            {
                filesToRegister.Add(s);
            }
            _connection.Register(filesToRegister);
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            //TODO Unregister File!
            _connection.Unregister(textBox2.Text);
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            _connection.CloseConnection();
            Close();
        }
    }
}
