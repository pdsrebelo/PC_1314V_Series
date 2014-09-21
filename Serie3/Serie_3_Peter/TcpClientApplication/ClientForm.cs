using System;
using System.Net;
using System.Windows.Forms;
using System.Linq;
using ServerClientUtils;

namespace TcpClientApplication
{
    public partial class ClientForm : Form
    {
        private readonly Client _client;

        public ClientForm(int port)
        {
            InitializeComponent();

            label2.Text = Convert.ToString(port);

            _client = new Client(port, IPAddress.Loopback, new Logger(new TextBoxWriter(textBox1)));

            Show();
        }

        private void RegisterFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                Multiselect = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _client.Register(openFileDialog1.FileNames);

                    foreach (var file in openFileDialog1.FileNames)
                        RegisteredFilesListBox.Items.Add(file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            //_client.Register(new[] { "xpto", "ypto", "zpto" });
        }

        private void Unregister_Click(object sender, EventArgs e)
        {
            foreach (var item in RegisteredFilesListBox.CheckedItems.OfType<string>().ToList())
            {
                // Unregister the file in the server
                _client.Unregister(item);

                // Remove it from the list of registered files
                RegisteredFilesListBox.Items.Remove(item);
            }
        }

        private void ListAllFiles_Click(object sender, EventArgs e)
        {
            _client.ListFiles();
            Console.WriteLine(@"List files:");
        }

        private void ListFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
                _client.ListLocations(textBox2.Text);
            else
                MessageBox.Show(@"Please input a valid file");
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            // Stops the logger
            _client.Stop();
        }
    }
}