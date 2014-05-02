using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    public class MsgSenderThread
    {
        public int ThreadId { get; set; }
        public string Msg { get; set; }

        public MsgSenderThread(int tId, string m)
        {
            Msg = m;
            ThreadId = tId;
        }
    }
    
    class Ex5_Logger
    {

        enum Logger_State
        {
            SHUTDOWN = 1,
            ACTIVE = 2,
            NOT_ACTIVE = 0,
        }

        private LinkedList<String> messagesToWrite; 
        private Logger_State state;
        private Thread loggerThread; 
        private TextWriter writer;

        public Ex5_Logger(TextWriter writer)
        {
            state = Logger_State.NOT_ACTIVE;
            this.writer = writer;
            loggerThread.Priority = ThreadPriority.Lowest;
        }

        // Operação LogMessage
        void LogMessage(string msg) // Recebe mensagem com relatório
        {
            lock (this)
            {
                if (state == Logger_State.SHUTDOWN) throw new Exception("LOGGER IS SHUTDOWN");
                if(state == Logger_State.NOT_ACTIVE) Start();
                loggerThread.Start(new MsgSenderThread(Thread.CurrentThread.ManagedThreadId,msg)); 
            }          
        }

        void Start()
        {   
            loggerThread = new Thread((msgSenderThread) =>
            {
                lock (this)
                {
                    if (state == Logger_State.NOT_ACTIVE)
                        state = Logger_State.ACTIVE;

                    messagesToWrite.AddLast(((MsgSenderThread)msgSenderThread).Msg);

                    do
                    {
                        if (messagesToWrite.First == msgSenderThread)
                        {
                            writer.WriteLine(msgSenderThread);
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
            state = Logger_State.ACTIVE;
        }

        void Shutdown()
        {
            lock (this)
            {
                if (state == Logger_State.SHUTDOWN) return;

                state = Logger_State.SHUTDOWN;

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
                        writer.Close();
                        throw;
                    }
                } while (true); 
            }
        }
    }
}
