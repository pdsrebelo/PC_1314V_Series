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
    public class Ex5Logger
    {
        private String LOGGER_SHUTDOWN_EXCEPTION_MSG = "LOGGER IS SHUTDOWN";
        enum LoggerState
        {
            SHUTDOWN = 1,
            ACTIVE = 2,
            NOT_ACTIVE = 0,
        }

        private class MsgSenderThread
        {
            public string Msg { get; set; }
            public bool Processed { get; set; }

            public MsgSenderThread(int tId, string m)
            {
                Msg = m;
                Processed = false;
            }
        }

        private volatile LinkedList<MsgSenderThread> messagesToWrite; 
        private LoggerState _state;
        private Thread _loggerThread; 
        private volatile TextWriter _writer;

        public Ex5Logger(TextWriter writer)
        {
            messagesToWrite = new LinkedList<MsgSenderThread>();
            _state = LoggerState.NOT_ACTIVE;
            _writer = writer;
        }

        public String getStatus()
        {
            lock (this)
            {
                return _state.ToString();
            }
        }

        public void LogMessage(string msg) // Recebe mensagem com relatório
        {
            lock (this)
            {
                if (_state == LoggerState.SHUTDOWN) throw new Exception(LOGGER_SHUTDOWN_EXCEPTION_MSG);
                if (_state == LoggerState.NOT_ACTIVE) Start();

                MsgSenderThread node = new MsgSenderThread(Thread.CurrentThread.ManagedThreadId, msg);
                messagesToWrite.AddLast(node);
                Monitor.Pulse(this);

                while(true){
                    if (node.Processed)
                    {
                        messagesToWrite.Remove(node);
                        return;
                    }
                    Monitor.Wait(this);
                }
            }          
        }

        public void Start()
        {
   
            _loggerThread = new Thread( () =>
            {
                lock (this)
                {
                    if (_state == LoggerState.NOT_ACTIVE)
                        _state = LoggerState.ACTIVE;
                    do
                    {
                        if (messagesToWrite.Count>0)
                        {
                            foreach(MsgSenderThread msgNode in messagesToWrite){
                                if(!msgNode.Processed){            
                                    _writer.WriteLine(msgNode.Msg);
                                    msgNode.Processed = true;
                                    Monitor.PulseAll(this); // Notificar threads, para elas verem se a sua msg ja foi escrita no ficheiro
                                }
                            }
                        }
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException iex)
                        {
                            foreach (MsgSenderThread msg in messagesToWrite)
                                messagesToWrite.Remove(msg);
                            Monitor.PulseAll(this);
                        }
                        
                    } while (true);
                }
            });
            _loggerThread.Priority = ThreadPriority.Lowest;
            _state = LoggerState.ACTIVE;
            _loggerThread.Start();
        }

        public void Shutdown()
        {
            lock (this)
            {
                if (_state == LoggerState.SHUTDOWN) 
                    return;

                _state = LoggerState.SHUTDOWN;
                do
                {
                    // Esperar pelo fim de todas as escritas 
                    if (messagesToWrite.Count == 0)
                    {
                        _writer.Close();
                        return;
                    }
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
