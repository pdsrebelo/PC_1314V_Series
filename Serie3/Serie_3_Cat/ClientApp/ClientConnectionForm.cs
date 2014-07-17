using System.Windows.Forms;
using Serie_3_Cat;

namespace ClientApp
{
    public partial class ClientConnectionForm : Form
    {
        private ClientRequest _connection;

        public ClientConnectionForm(ClientRequest conn)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            _connection = conn;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //TODO Register File!
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            //TODO Unregister File!
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            _connection.Cancel();
            //TODO Cancel the connection! (Maybe we can use a Cancellation Token??)
        }
    }
}
