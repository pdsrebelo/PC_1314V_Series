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
            int timeout = 1000;
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
        public void Ex6_Request_Sent_Accepted_And_Received()
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
            Assert.AreEqual(responseFromServer, responseFromServerToRequest);
            Assert.AreEqual(0, _rChannel.getNumberOfRequests()); 
            Assert.IsTrue(resultFromRequest);
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

        [TestMethod]
        public void Ex6_Server_Accepted_Request_NoTimeOut()
        {
            InitializeRendezvousChannel();

            int serviceRequested = -1;
            object rendezvousToken = null;

            Thread server = TestUtils.CreateAndStartThread(new ThreadStart(
                    () =>
                    {
                        rendezvousToken = _rChannel.Accept(50000, out serviceRequested);
                    }));

            String responseReceivedFromServer = null;
            int serviceRequestedFromClient = 99;
            Thread client = TestUtils.CreateAndStartThread(new ThreadStart(
                () =>
                {
                    _rChannel.Request(serviceRequestedFromClient, 3000, out responseReceivedFromServer);
                }
                ));

            Thread.Sleep(600);

            // Verify the Unsuccess!
            Assert.AreEqual(serviceRequestedFromClient, serviceRequested);
            Assert.IsNotNull(rendezvousToken);
        }

        [TestMethod]
        public void Ex6_Server_Two_Requests_Sent_OneAcceptedOnly()
        {
            InitializeRendezvousChannel();

            int serviceRequested = -1;
            int serviceRequested1 = 512;
            int serviceRequested2 = 1024;
            String response1, response2;
            bool successReq1 = false, successReq2 = false;

            object rendezvousToken = null;

            Thread client1 = TestUtils.CreateAndStartThread(new ThreadStart(
                ()=>{
                    successReq1 = _rChannel.Request(serviceRequested1, 5000, out response1);
                }));

            Thread client2 = TestUtils.CreateAndStartThread(new ThreadStart(
                ()=>{
                    successReq2 = _rChannel.Request(serviceRequested2, 2000, out response2);
                }));

            Thread.Sleep(600);
            Assert.AreEqual(2, _rChannel.getNumberOfRequests());

            Thread server = TestUtils.CreateAndStartThread(new ThreadStart(
                    () =>
                    {
                        rendezvousToken = _rChannel.Accept(5000000, out serviceRequested);
                    }));
            Thread.Sleep(600);

            Assert.AreEqual(serviceRequested1, serviceRequested);
            Assert.IsNotNull(rendezvousToken);

            client2.Interrupt();

            Thread.Sleep(400);
            Assert.IsFalse(successReq2);
            Assert.IsFalse(successReq1);
        }

        [TestMethod]
        public void Ex6_Server_Two_Requests_Sent_AllAcceptedAndResponded()
        {
            InitializeRendezvousChannel();

            int serviceRequested = -1;
            int serviceRequested1 = 512, service2 = -1, service1 = -1, serviceRequested2 = 1024;
            String response1 = null, response2 = null;
            String sentReply1 = "This is the first response.", sentReply2 = "REPLY #2!!";
            object rendezvousToken1 = null, rendezvousToken2 = null; 
            bool successReq1 = false, successReq2 = false;

            Thread client1 = TestUtils.CreateAndStartThread(new ThreadStart(
                () =>
                {
                    successReq1 = _rChannel.Request(serviceRequested1, 9999999, out response1);
                }));

            Thread client2 = TestUtils.CreateAndStartThread(new ThreadStart(
                () =>
                {
                    successReq2 = _rChannel.Request(serviceRequested2, 9999999, out response2);
                }));

            Thread.Sleep(300);
            Assert.AreEqual(2, _rChannel.getNumberOfRequests());

            Thread server1 = TestUtils.CreateAndStartThread(new ThreadStart(
                    () =>
                    {
                        rendezvousToken1 = _rChannel.Accept(9999999, out service1);
                        _rChannel.Reply(rendezvousToken1, sentReply1);
                    }));
            Thread server2 = TestUtils.CreateAndStartThread(new ThreadStart(
                    () =>
                    {
                        rendezvousToken2 = _rChannel.Accept(9999999, out service2);
                        _rChannel.Reply(rendezvousToken2, sentReply2);
                    }));

            Thread.Sleep(200);

            Assert.AreEqual(serviceRequested1, service1);
            Assert.AreEqual(serviceRequested2, service2); 
            
            Assert.IsNotNull(rendezvousToken1);
            Assert.IsNotNull(rendezvousToken2);
           
            Assert.AreEqual(sentReply1, response1);
            Assert.AreEqual(sentReply2, response2);

            Assert.IsTrue(successReq1);
            Assert.IsTrue(successReq2);
        }
    }
}
