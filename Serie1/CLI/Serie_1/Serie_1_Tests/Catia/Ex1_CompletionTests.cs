using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1.Catia;

namespace Serie_1_Tests.Catia
{
    [TestClass]
    public class Ex1CompletionTests
    {
        private static Ex1Completion _completionSynchronizer;

        //
        // To Call in before starting each test
        //
        private void StartCompletionSynchronizer(int nPermits)
        {
            _completionSynchronizer = new Ex1Completion(nPermits);
        }

        //
        // To Create a Thread
        //
        private Thread CreateAndStartThread(ThreadStart func)
        {
            var thread = new Thread(func);
            thread.Start();
            return thread;
        }

        [TestMethod]
        public void CompleteTest() // Verifies that the Complete method is adding a permit
        {
            
            // Start the synchronizer with no permits
            StartCompletionSynchronizer(0);

            // Will call 'Complete()' and permits will be = 1
            CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            // Add one more permit!
            CreateAndStartThread(WorkerThreadFunction_CompletionSetter);

            // Will wait for completion, for a maximum time of 30000 ms, and then take 1 permit
            CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);

            // Take again a permit!
            CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);

            Thread.SpinWait(2000);

            // Number of permits will be 0 because all the given permits were taken in the end!
            Assert.AreEqual(0, _completionSynchronizer.GetPermits());
        
        }

        [TestMethod]
        public void CompleteAllTest() // Verifies that after calling completeAll, all calls to "WaitForCompletion" return 'true'
        {
            StartCompletionSynchronizer(0);
            Assert.IsFalse(_completionSynchronizer.WaitForCompletion(500));

            _completionSynchronizer.CompleteAll();

            for (int j = 0; j < 50; j++)
                Assert.IsTrue(_completionSynchronizer.WaitForCompletion(500));
            
        }

        [TestMethod]
        public void WaitForCompletionTest()
        {
            int nWaitingThreads = 6, nPermits = 4;

            // Start the synchronizer
            StartCompletionSynchronizer(nPermits);

            // The first threads receive a permit instantly

            for(var j = 0; j < nWaitingThreads; j++)
                CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);
            
            // Release 2 permits

            CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            CreateAndStartThread(WorkerThreadFunction_CompletionSetter);

            // Another waiter thread:
            Assert.IsTrue(_completionSynchronizer.WaitForCompletion(1000));
            Thread.SpinWait(2000);

            // Another waiter: will wait but won't get a permit
            Assert.IsFalse(_completionSynchronizer.WaitForCompletion(100));

            Assert.AreEqual(0, _completionSynchronizer.GetPermits());
        }

        [TestMethod]
        public void TestInterruptedWaitingThread()
        {
            StartCompletionSynchronizer(0);
            Thread t = CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);
            try
            {
                t.Interrupt();
            }
            catch (Exception iex)
            {
                Assert.IsInstanceOfType(iex, typeof (ThreadInterruptedException));
            }
            
        }

        //
        // Waits for completion, for up to 30000 milliseconds.
        //
        private void WorkerThreadFunction_CompletionWaiter()
        {
            const int timeout = 30000;
            try
            {
                _completionSynchronizer.WaitForCompletion(timeout);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("CompletionWaiter -> Thread Was Interrupted!");
                throw;
            }
        }

        //
        // Calls "Complete()" method
        //
        private static void WorkerThreadFunction_CompletionSetter()
        {
            _completionSynchronizer.Complete();
        }
    }
}
