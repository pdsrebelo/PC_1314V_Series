using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1.Catia;

namespace Serie_1_Tests.Catia
{
    [TestClass]
    public class Ex4MessageQueueTests
    {
        private static Ex4MessageQueue<string> _messageQueue;

        //
        // Initialize the MessageQueue
        //
        private static void InitializeMessageQueue()
        {
            _messageQueue = new Ex4MessageQueue<string>();
        }

        //
        // To Create a Thread
        //
        private static Thread CreateAndStartThread(ThreadStart func)
        {
            var thread = new Thread(func);
            thread.Start();
            return thread;
        }

        [TestMethod]
        public void SendTest()
        {
            InitializeMessageQueue();

            CreateAndStartThread(MessageSender);
            CreateAndStartThread(MessageSender);
            CreateAndStartThread(MessageSender);

            Thread.SpinWait(4000);

            Assert.AreEqual(3, _messageQueue.getMessageQueue().Count);
        }

        [TestMethod]
        public void ReceiveTest_TimedOut()
        {
            InitializeMessageQueue();
            CreateAndStartThread(MessageSender);
            CreateAndStartThread(MessageSender);
            CreateAndStartThread(MessageSender);
            Assert.IsNull(_messageQueue.Receive(1000, i => i > 5000));
            Assert.AreEqual(0, _messageQueue.getReceivers().Count);
        }

        [TestMethod]
        public void ReceiveTest_NoMatchForPredicate()
        {
            InitializeMessageQueue();
            CreateAndStartThread(MessageSender); // MessageSender method prepares a Message with type = 2
            var msg = _messageQueue.Receive(5000, (i => i>5)); // We want messages with type > 5
            
            Assert.IsNull(msg);
            Assert.AreEqual(1, _messageQueue.getMessageQueue().Count);
        }

        [TestMethod]
        public void ReceiveTest_ThreadInterrupted()
        {
            InitializeMessageQueue();
            var thread = CreateAndStartThread(MessageReceiver);
            Thread.SpinWait(1000);
            thread.Interrupt();

            // Verify if the receivers' list is empty (there was only this receiver)
            Assert.IsTrue(_messageQueue.getReceivers().Count == 0);

            // The Exception Assert is done in MessageReceiver method!
        }

        [TestMethod]
        public void ReceiveTest_Successful()
        {
            InitializeMessageQueue();
            CreateAndStartThread(MessageSender);
            Thread.SpinWait(2000); 
            var msg = _messageQueue.Receive(8000, (i => i < 20 && i > 0));
            Assert.IsTrue(msg.Type<20 && msg.Type>0);
            Assert.AreEqual(0, _messageQueue.getReceivers().Count);
        }

        public void MessageReceiver()
        {
            const int timeout = 5000;
            Exception ex = null;
            Message<string> receivedMsg = null;
            var pred = new Predicate<int>(messageType => messageType>=2);
            try
            {
                receivedMsg = _messageQueue.Receive(timeout, pred);
            }
            catch (Exception exc)
            {
                ex = exc;
            }
            if (ex != null)
            {
                Assert.IsInstanceOfType(ex, typeof (ThreadInterruptedException));
                Console.WriteLine("'MessageReceiver' Thread was interrupted!");
            }
            else
            {
                Assert.IsNotNull(receivedMsg);
            }
        }

        public void MessageSender()
        {
            // Example of a message to send
            var msg = new Message<string>(2, "Test Message");
            _messageQueue.Send(msg);
        }
    }
}
