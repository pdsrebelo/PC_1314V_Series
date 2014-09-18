using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1_Tests.Catia;
using Serie_1.Catia;
using System.Threading;

namespace Serie_1_Tests.Catia
{
    [TestClass]
    public class Ex6_RendezvousChannel
    {
        private Ex6RendezvousChannel<int,String> _rChannel;

        private void InitializeRendezvousChannel()
        {
            _rChannel = new Ex6RendezvousChannel<int,string>();
        }

        [TestMethod]
        public void Ex6_Request_TimedOut()
        {
            InitializeRendezvousChannel();
            String response;
            int service = 100;
            int timeout = 1000000;
            bool result =_rChannel.Request(service, timeout, out response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Ex6_Request_Interrupted()
        {
            bool interruptedThreadSuccess = true;
            Thread t = TestUtils.CreateAndStartThread(new ThreadStart(
                () =>
                {
                    int timeout = 100000;
                    String response = null;
                    try
                    {
                        interruptedThreadSuccess = _rChannel.Request(1, timeout, out response);
                    }
                    catch (Exception e)
                    {
                        interruptedThreadSuccess = false;
                    }
                    Assert.IsFalse(interruptedThreadSuccess);
                    Assert.IsNull(response);
                }
                ));
            Thread.SpinWait(1000);
            t.Interrupt();
            // Assert é feito no código da lambda, da thread
        }

        public void RequestSenderAndBeInterrupted()
        {
            int timeout = 100000;
            String response;
            bool res = _rChannel.Request(1, timeout, out response);

        }

        [TestMethod]
        public void Ex6_Request_Sent_And_Received()
        {
            InitializeRendezvousChannel();
            int serviceType = 5;
            bool resultFromRequest = false;
            String responseFromServerToRequest = null;
            Thread sender = TestUtils.CreateAndStartThread(new ThreadStart(
                () =>{
                    int timeout = 100000;
                    resultFromRequest = _rChannel.Request(serviceType, timeout, out responseFromServerToRequest);
                }));

            int serviceRequested = -1;
            object rendezvousToken = null;
            String responseFromServer = "ANSWER A!";

            Thread receiver = TestUtils.CreateAndStartThread(new ThreadStart(
                    ()=>{
                        rendezvousToken = _rChannel.Accept(1000, out serviceRequested);
                        _rChannel.Reply(rendezvousToken, responseFromServer);
                    }));

            Thread.Sleep(3000);
            Assert.AreEqual(serviceType, serviceRequested);
            Assert.IsTrue(resultFromRequest);
            Assert.AreEqual(responseFromServer, responseFromServerToRequest);
            Assert.AreEqual(0, _rChannel.getNumberOfRequests());
        }

        [TestMethod]
        public void Ex6_Server_Accept_TimedOut()
        {
            InitializeRendezvousChannel();
            
            int serviceRequested = -1;
            object rendezvousToken = null;

            Thread server = TestUtils.CreateAndStartThread(new ThreadStart(
                    () => {
                        rendezvousToken = _rChannel.Accept(500, out serviceRequested);
                    }));

            Thread.Sleep(600);

            // Verify the Unsuccess!
            Assert.AreEqual(default(int), serviceRequested); // invalid service number!
            Assert.IsNull(rendezvousToken);
        }
    }
}
