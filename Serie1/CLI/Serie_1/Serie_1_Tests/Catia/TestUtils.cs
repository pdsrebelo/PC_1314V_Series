using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_1_Tests.Catia
{
    public static class TestUtils
    {
        //
        // To Create a Thread
        //
        public static Thread CreateAndStartThread(ThreadStart func)
        {
            var thread = new Thread(func);
            thread.Start();
            return thread;
        }

    }
}
