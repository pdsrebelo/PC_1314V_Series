using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Serie_3_Cat;

namespace ClientApp
{
    public partial class ClientConnectionForm : Form
    {
        public ClientConnection _connection { get; private set; }
        private TextBoxStreamWriter _writer;

        public ClientConnectionForm(ClientConnection conn)
        {
            InitializeComponent();
            _connection = conn;
            _writer = new TextBoxStreamWriter(textBox3);
            _writer.WriteLine("~~Client Connection ESTABLISHED~~");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //TODO Register File!
            List<string> filesToRegister = new List<string>(textBox1.Lines);
            _writer.WriteLine("Sent a REGISTER Message for:");

            foreach (string s in textBox1.Lines)
            {
                _writer.WriteLine(s);
                filesToRegister.Add(s);
            }
            
            _connection.Register(filesToRegister);
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            _writer.WriteLine("Sent an UNREGISTER Message for: " + textBox2.Text);
            _connection.Unregister(textBox2.Text);
        }

        private void button_CloseConnection_Click(object sender, System.EventArgs e)
        {
            _writer.WriteLine("~~Client Connection CLOSED~~");
            _connection.CloseConnection();
            Close();
        }

        private void button_ListFiles_Click(object sender, EventArgs e)
        {
            _writer.WriteLine("Sending 'LIST ALL FILES' Request...");
            _connection.ListFiles();
        }

        private void button_ListLocations_Click(object sender, EventArgs e)
        {
            _writer.WriteLine("Sending 'LIST LOCATIONS' Requests...");
            foreach(string fileName in textBox1.Lines)
                _connection.ListLocations(fileName);
        }
    }
}
