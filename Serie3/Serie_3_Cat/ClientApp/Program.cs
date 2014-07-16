using System;
using System.Net;
using System.Windows.Forms;
using ClientApp;

namespace Serie_3_Cat
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the Client application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientPanelForm());
        }
    }
}
