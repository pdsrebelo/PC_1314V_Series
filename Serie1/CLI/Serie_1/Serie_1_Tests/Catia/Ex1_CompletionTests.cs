using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1.Catia;
using Serie_1_Tests.Catia;

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

        [TestMethod]
        public void Ex1_CompleteTest() // Verifies that the Complete method is adding a permit
        {
            
            // Start the synchronizer with no permits
            StartCompletionSynchronizer(0);

            // Will call 'Complete()' and permits will be = 1
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            // Add one more permit!
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionSetter);

            // Will wait for completion, for a maximum time of 30000 ms, and then take 1 permit
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);

            // Take again a permit!
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);

            
            Thread.Sleep(1000);
            // Number of permits will be 0 because all the given permits were taken in the end!
            Assert.AreEqual(0, _completionSynchronizer.GetPermits());
        
        }

        [TestMethod]
        public void Ex1_CompleteAllTest() // Verifies that after calling completeAll, all calls to "WaitForCompletion" return 'true'
        {
            StartCompletionSynchronizer(0);
            Assert.IsFalse(_completionSynchronizer.WaitForCompletion(500));

            _completionSynchronizer.CompleteAll();

            for (int j = 0; j < 50; j++)
                Assert.IsTrue(_completionSynchronizer.WaitForCompletion(500));
            
        }

        [TestMethod]
        public void Ex1_WaitForCompletionTest()
        {
            int nWaitingThreads = 6, nPermits = 4;

            // Start the synchronizer
            StartCompletionSynchronizer(nPermits);

            // 6 Threads want permits! But there are only 4...

            for(var j = 0; j < nWaitingThreads; j++)
                TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);
            
            // Release some permits...
           
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionSetter);
            // 2 of these are consumed!... There is 1 permit left now.

            // Another waiter thread:
            Assert.IsTrue(_completionSynchronizer.WaitForCompletion(1000));
            // It consumes the last permit!


            // wait a bit..
            Thread.Sleep(2000);

            // Another waiter: will wait but won't get a permit
            Assert.IsFalse(_completionSynchronizer.WaitForCompletion(100));

            // Assert: no permits left.
            Assert.AreEqual(0, _completionSynchronizer.GetPermits());
        }

        [TestMethod]
        public void Ex1_TestInterruptedWaitingThread()
        {
            StartCompletionSynchronizer(0);
            Thread t = TestUtils.CreateAndStartThread(WorkerThreadFunction_CompletionWaiter);
            Thread.SpinWait(1000);
            t.Interrupt();
            // Asserts feitos no metodo WorkerThreadFunction_CompletionWaiter
        }

        //
        // Waits for completion, for up to 30000 milliseconds.
        //
        private void WorkerThreadFunction_CompletionWaiter()
        {
            const int timeout = 30000;
            Exception ex = null;
            bool success = false;
            try
            {
                success = _completionSynchronizer.WaitForCompletion(timeout);
            }
            catch (Exception e)
            {
                Console.WriteLine("CompletionWaiter -> Thread Was Interrupted!");
                ex = e;
            }
            if (ex != null)
            {
                Assert.IsInstanceOfType(ex, typeof(ThreadInterruptedException));
            }
            else
            {
                Assert.IsTrue(success);
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
