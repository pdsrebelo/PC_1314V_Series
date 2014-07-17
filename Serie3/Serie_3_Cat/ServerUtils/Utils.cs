using System;
using System.Net;

namespace Serie_3_Cat
{
    public class Utils
    {
        public static void ShowInfo(Store store)
        {
            foreach (string fileName in store.GetTrackedFiles())
            {
                Console.WriteLine(fileName);
                foreach (IPEndPoint endPoint in store.GetFileLocations(fileName))
                    Console.Write(endPoint + " ; ");

                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
