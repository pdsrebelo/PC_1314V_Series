using System;
using System.IO;
using System.Net;

namespace Serie_3_Cat
{
    public class Utils
    {
        public static void ShowInfo(Store store, TextWriter writer)
        {
            foreach (string fileName in store.GetTrackedFiles())
            {
                writer.WriteLine(fileName);//Console.WriteLine(fileName);
                foreach (IPEndPoint endPoint in store.GetFileLocations(fileName))
                    writer.Write(endPoint + " ; ");//Console.Write(endPoint + " ; ");

                writer.WriteLine();//Console.WriteLine();
            }
            writer.WriteLine();// Console.WriteLine();
        }
    }
}
