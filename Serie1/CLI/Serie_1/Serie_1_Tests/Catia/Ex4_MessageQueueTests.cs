using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1.Catia;

namespace Serie_1_Tests.Catia
{
    [TestClass]
    class Ex4MessageQueueTests
    {
        private Ex4MessageQueue<string> _messageQueue;

        //
        // Initialize the MessageQueue
        //
        private void InitializeMessageQueue()
        {
            _messageQueue = new Ex4MessageQueue<string>();
        }


    }
}
