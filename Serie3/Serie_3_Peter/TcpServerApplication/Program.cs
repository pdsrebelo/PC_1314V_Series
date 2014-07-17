using System;
using System.Windows.Forms;

namespace TcpServerApplication
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartServerForm());
        }
    }

    /*
        static void TestStore()
        {
            Store store = Store.Instance;

            store.Register("xpto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
            store.Register("xpto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
            store.Register("xpto", new IPEndPoint(IPAddress.Parse("195.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();
            store.Register("ypto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
            store.Register("ypto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();
            store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("195.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();

            store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
            store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();
        }
    */
}