using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Serie_1.Catia
{
    /*
     * Implemente em C# a classe Logger que fornece funcionalidade de registo de relatórios suportada 
    por uma thread de baixa prioridade criada para o efeito (logger thread). 
     * 
     * As mensagens com os 
    relatórios são passadas por threads que chamam a operação LogMessage(string msg) e são 
    escritas pela logger thread na instância de TextWriter especificada como argumento no construtor 
    de Logger. 
     * 
     * A classe inclui ainda as operações Start e Shutdown que promovem o início e a 
    terminação do registo de relatórios, respectivamente. A operação Shutdown bloqueia a thread 
    invocante até que todos os relatórios submetidos até ao momento sejam efectivamente escritos; 
    relatórios submetidos posteriormente são rejeitados (i.e. chamadas a LogMessage produzem 
    excepção). 
    Note que a preocupação principal da solução a apresentar deverá ser o de minimizar o custo do 
    registo de relatórios por threads cuja prioridade se admite maior que a prioridade da logger thread. 
    Pretende-se, por isso, que seja usado um mecanismo de comunicação que minimize o tempo de 
    bloqueio das threads produtoras, admitindo-se inclusivamente a possibilidade de ignorar relatórios. 

     http://www.junit.org/ 1
     http://www.nunit.org/ 2
     * 
     */    
    class Ex5Logger
    {
        enum LoggerState
        {
            SHUTDOWN = 1,
            ACTIVE = 2,
            NOT_ACTIVE = 0,
        }

        private class MsgSenderThread
        {
            public int ThreadId { get; set; }
            public string Msg { get; set; }

            public MsgSenderThread(int tId, string m)
            {
                Msg = m;
                ThreadId = tId;
            }
        }

        private LinkedList<String> messagesToWrite; 
        private LoggerState _state;
        private Thread _loggerThread; 
        private readonly TextWriter _writer;

        public Ex5Logger(TextWriter writer, LinkedList<string> messagesToWrite)
        {
            _state = LoggerState.NOT_ACTIVE;
            _writer = writer;
            this.messagesToWrite = messagesToWrite;
            _loggerThread.Priority = ThreadPriority.Lowest;
        }

        public void LogMessage(string msg) // Recebe mensagem com relatório
        {
            lock (this)
            {
                if (_state == LoggerState.SHUTDOWN) throw new Exception("LOGGER IS SHUTDOWN");
                if(_state == LoggerState.NOT_ACTIVE) Start();
                _loggerThread.Start(new MsgSenderThread(Thread.CurrentThread.ManagedThreadId,msg)); 
            }          
        }

        void Start()
        {   
            _loggerThread = new Thread((msgSenderThread) =>
            {
                lock (this)
                {
                    if (_state == LoggerState.NOT_ACTIVE)
                        _state = LoggerState.ACTIVE;

                    messagesToWrite.AddLast(((MsgSenderThread)msgSenderThread).Msg);

                    do
                    {
                        if (messagesToWrite.First == msgSenderThread)
                        {
                            _writer.WriteLine(msgSenderThread);
                            messagesToWrite.Remove(((MsgSenderThread) msgSenderThread).Msg);
                        }
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException iex)
                        {
                            messagesToWrite.Remove(((MsgSenderThread)msgSenderThread).Msg);
                            Monitor.PulseAll(this);
                            //TODO 
                        }

                    } while (true);
                }
            });
            _state = LoggerState.ACTIVE;
        }

        public void Shutdown()
        {
            lock (this)
            {
                if (_state == LoggerState.SHUTDOWN) return;

                _state = LoggerState.SHUTDOWN;

                do
                {
                    // Esperar pelo fim de todas as escritas 
                    if (messagesToWrite.Count == 0)
                        return;
                    try
                    {
                        Monitor.Wait(this);
                    }
                    catch (ThreadInterruptedException iex)
                    {
                        _writer.Close();
                        throw;
                    }
                } while (true); 
            }
        }
    }
}
