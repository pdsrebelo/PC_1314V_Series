using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Serie_1.Pedro
{
    /// <summary>
    /// Implemente em C# a classe Logger que fornece funcionalidade de registo de relatórios suportada 
    /// por uma thread de baixa prioridade criada para o efeito (logger thread). 
    /// 
    /// Note que a preocupação principal da solução a apresentar deverá ser o de minimizar o custo do 
    /// registo de relatórios por threads cuja prioridade se admite maior que a prioridade da logger thread. 
    /// Pretende-se, por isso, que seja usado um mecanismo de comunicação que minimize o tempo de 
    /// bloqueio das threads produtoras, admitindo-se inclusivamente a possibilidade de ignorar relatórios.
    /// </summary>
    class Ex5Logger
    {
        private Thread _loggerThread;

        private enum LoggerState
        {
            NotStarted = 0,
            Working = 1,
            Shutdown = 2
        }

        private TextWriter _myTw;
        private LinkedList<string> _msgQueue;
        private LoggerState _state;

        public Ex5Logger (TextWriter tw)
        {
            _msgQueue = new LinkedList<string>();
            _myTw = tw;
            _state = LoggerState.NotStarted;
        }

        /// <summary>
        /// As mensagens com os relatórios são passadas por threads que chamam a operação LogMessage(string msg) e 
        /// são escritas pela logger thread na instância de TextWriter especificada como argumento no construtor de Logger. 
        /// </summary>
        /// <param name="msg"></param>
        public void LogMessage(string msg)
        {
            lock (this)
            {
                if (_state == LoggerState.Shutdown)
                {
                    throw new Exception("Logger is shutting down and not accepting anymore messages!");
                }

                if (_state == LoggerState.NotStarted)
                {
                    Start();
                }
                _loggerThread.Start(msg);
            }
        }

        /// <summary>
        /// A classe inclui ainda as operações Start e Shutdown que promovem o início e a terminação do 
        /// registo de relatórios, respectivamente.
        /// </summary>
        public void Start()
        {
            _state = LoggerState.Working;
            _loggerThread = new Thread((msg) =>
            {
                lock (this)
                {
                    LinkedListNode<string> myNode = new LinkedListNode<string>(((string) msg));
                    _msgQueue.AddLast(myNode);

                    do
                    {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (ThreadInterruptedException)
                        {
                            _msgQueue.Remove(myNode);
                            throw;
                        }

                        if (_msgQueue.First == myNode)
                        {
                            // TODO : Comparar a thread actual com a loggerthread (priority), NOT FINISHED! (Não faz sentido!!)
                            if (Thread.CurrentThread.Priority < _loggerThread.Priority)
                            {
                                _myTw.Write(msg);
                                Monitor.PulseAll(this);
                            }
                        }
                    } while (true);
                }
            });
            _loggerThread.Priority = ThreadPriority.BelowNormal;
        }

        /// <summary>
        /// A operação Shutdown bloqueia a thread invocante até que todos os relatórios submetidos até ao momento sejam efectivamente 
        /// escritos; relatórios submetidos posteriormente são rejeitados (i.e. chamadas a LogMessage produzem excepção).
        /// </summary>
        public void Shutdown()
        {
            lock (this)
            {
                _state = LoggerState.Shutdown;

                if (_msgQueue.Count == 0)
                {
                    _myTw.Close();
                    return;
                }

                do
                {
//                    try
//                    {
                        Monitor.Wait(this);
//                    }
//                    catch (ThreadInterruptedException)
//                    {
//                        foreach (var msg in _msgQueue)
//                        {
//                            _msgQueue.Remove(msg);
//                        }
//                        throw;
//                    }

                    if (_msgQueue.Count == 0)
                    {
                        _myTw.Close();
                        return;
                    }

                } while (true);
            }
        }
    }
}
