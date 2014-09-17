using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1.Catia;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace Serie_1_Tests.Catia{

    [TestClass]
    public class Ex5LoggerTests
    {
        private String DEFAULT_LOG_MSG = "This is a log message for tests!";
        private String filename = "test1.txt";
        private static Ex5Logger logger;

        private void InitializeLogger()
        {

            TextWriter writer = File.CreateText(filename);
            logger = new Ex5Logger(writer);
      
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

        public void LogMessageFunction()
        {
            String logMsgExample = DEFAULT_LOG_MSG;
            logger.LogMessage(logMsgExample);
        }

        [TestMethod]
        public void LogMessage_ShutdownSuccessfully()
        {
            InitializeLogger();
            logger.Start();

            Thread loggerThread = CreateAndStartThread(LogMessageFunction);

            Thread.Sleep(800);
            logger.Shutdown();

            Assert.AreEqual("SHUTDOWN",logger.getStatus());
        }

        [TestMethod]
        public void LogMessage_Unsuccessful_DueToLogAfterShutdown()
        {
            bool exceptionOccurred = false;
            InitializeLogger();
            logger.Start();
            logger.Shutdown();
            try
            {
                logger.LogMessage("Exemplo de mensagem para dar exception!");
            }
            catch (Exception e)
            {
                exceptionOccurred = true;
            }
            Assert.IsTrue(exceptionOccurred);
        }

        [TestMethod]
        public void LogMessage_SuccessfulLogMessageWritten()
        {
            InitializeLogger();
            
            logger.Start();
            
            String msg = "TESTE ESCRITA EM FICHEIRO - USANDO O LOGGER!";
            logger.LogMessage(msg);

            Thread.Sleep(500);
            logger.Shutdown();

            string line = "";
            // Ler o que foi escrito no ficheiro
            using (TextReader reader = File.OpenText(@filename))
            {
                line = reader.ReadLine();
            }
            Assert.AreEqual(msg, line);
        }

        [TestMethod]
        public void LogMessage_SuccessfullyWritesTwoLogMessages()
        {
            InitializeLogger();

            String msg1 = "TESTE ESCRITA EM FICHEIRO - USANDO O LOGGER!"
                , msg2 = DEFAULT_LOG_MSG, msg3 = DEFAULT_LOG_MSG;
            logger.LogMessage(msg1);

            CreateAndStartThread(LogMessageFunction);
            CreateAndStartThread(LogMessageFunction);

            Thread.Sleep(1000);

            logger.Shutdown();
            
            string line1 = "", line2 = "", line3 = "";
            // Ler o que foi escrito no ficheiro
            using (TextReader reader = File.OpenText(@filename))
            {
                line1 = reader.ReadLine();
                line2 = reader.ReadLine();
                line3 = reader.ReadLine();
            }
            Assert.AreEqual(msg1, line1);
            Assert.AreEqual(msg2, line2);
            Assert.AreEqual(msg3, line3);
        }

        [TestMethod]
        public void LogMessage_SuccessfullyWritesElevenLogMessages()
        {
            InitializeLogger();

            int totalLogMsgThreads = 10;
            String msg1 = "TESTE ESCRITA EM FICHEIRO - USANDO O LOGGER!... This Will write 11 log messages (total)!";
            logger.LogMessage(msg1);

            for (int j = 0; j < totalLogMsgThreads; j++)
            {
                CreateAndStartThread(LogMessageFunction);
            }

            // Make sure all the log message requests are received before shutting down
            Thread.SpinWait(2500);

            logger.Shutdown();

            string line1 = "", otherLine = "";
            // Ler o que foi escrito no ficheiro
            using (TextReader reader = File.OpenText(@filename))
            {
                line1 = reader.ReadLine();
                Assert.AreEqual(msg1, line1);

                for (int i = 0; i < totalLogMsgThreads; i++)
                {
                    otherLine = reader.ReadLine();
                    Assert.AreEqual(DEFAULT_LOG_MSG, otherLine);
                }
            }
        }
    }
}
