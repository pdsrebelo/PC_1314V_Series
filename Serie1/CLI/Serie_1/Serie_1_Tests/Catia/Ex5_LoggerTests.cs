using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie_1.Catia;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Serie_1_Tests.Catia;

namespace Serie_1_Tests.Catia{

    [TestClass]
    public class Ex5LoggerTests
    {
        private String DEFAULT_LOG_MSG = "This is a log message for tests!";
        private static Ex5Logger logger;

        private void InitializeLogger(String filename)
        {
            TextWriter writer = File.CreateText(filename);
            logger = new Ex5Logger(writer);
        }

        public void LogMessageFunction()
        {
            String logMsgExample = DEFAULT_LOG_MSG;
            logger.LogMessage(logMsgExample);
        }

        [TestMethod]
        public void Ex5_LogMessage_ShutdownSuccessfully()
        {
            InitializeLogger("LogMessage_ShutdownSuccessfully");
            logger.Start();

            Thread loggerThread = TestUtils.CreateAndStartThread(LogMessageFunction);

            Thread.Sleep(800);
            logger.Shutdown();

            Assert.AreEqual("SHUTDOWN",logger.getStatus());
        }

        [TestMethod]
        public void Ex5_LogMessage_Unsuccessful_DueToLogAfterShutdown()
        {
            bool exceptionOccurred = false;
            InitializeLogger("LogMessage_Unsuccessful_DueToLogAfterShutdown");
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
        public void Ex5_LogMessage_SuccessfulLogMessageWritten()
        {
            String filename = "LogMessage_SuccessfulLogMessageWritten.txt";
            InitializeLogger(filename);
            
            logger.Start();
            
            String msg = "TESTE ESCRITA EM FICHEIRO - USANDO O LOGGER!";
            logger.LogMessage(msg);

            Thread.Sleep(500);
            logger.Shutdown();

            string line = "";
            // Ler o que foi escrito no ficheiro
            using (TextReader reader = File.OpenText(filename))
            {
                line = reader.ReadLine();
            }
            Assert.AreEqual(msg, line);
        }

        [TestMethod]
        public void Ex5_LogMessage_SuccessfullyWritesTwoLogMessages()
        {
            String filename = "LogMessage_SuccessfullyWritesTwoLogMessages.txt";
            InitializeLogger(filename);

            String msg1 = "TESTE ESCRITA EM FICHEIRO - USANDO O LOGGER!"
                , msg2 = DEFAULT_LOG_MSG, msg3 = DEFAULT_LOG_MSG;
            logger.LogMessage(msg1);

            TestUtils.CreateAndStartThread(LogMessageFunction);
            TestUtils.CreateAndStartThread(LogMessageFunction);

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
        public void Ex5_LogMessage_SuccessfullyWritesElevenLogMessages()
        {
            String filename = "LogMessage_SuccessfullyWritesElevenLogMessages.txt";
            InitializeLogger(filename);

            int totalLogMsgThreads = 10;
            String msg1 = "TESTE ESCRITA EM FICHEIRO - USANDO O LOGGER!... This Will write 11 log messages (total)!";
            logger.LogMessage(msg1);

            for (int j = 0; j < totalLogMsgThreads; j++)
            {
                TestUtils.CreateAndStartThread(LogMessageFunction);
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
