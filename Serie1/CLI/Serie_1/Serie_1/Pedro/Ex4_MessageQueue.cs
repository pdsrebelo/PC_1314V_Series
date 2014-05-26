using System;
using System.Collections.Generic;
using System.Threading;

namespace Serie_1.Pedro
{
    /// <summary>
    /// As mensagens têm associado um tipo (inteiro positivo) e um campo de dados do tipo genérico T. 
    /// </summary>
    class Message<T>
    {
        public int Type { get; set; }
        public T Data { get; set; }

        public Message(int type, T data)
        {
            Type = type;
            Data = data;
        }
    }

    /// <summary>
    /// Usando monitores intrínsecos CLI, implemente o sincronizador MessageQueue que suporta a 
    /// comunicação, através de mensagens, entre threads produtoras e consumidoras. 
    /// O sincronizador deve garantir que as mensagens do mesmo tipo são entregues pela ordem de chegada
    /// e que as threads receptoras também são servidas por ordem de chegada.
    /// A operação Receive deve suportar desistência por timeout e cancelamento por interrupção das threads bloqueadas.
    /// </summary>
   class Ex4_MessageQueue<T>
    {
        private LinkedList<Message<T>> _messageQueue = new LinkedList<Message<T>>(); // Queue onde as threads produtoras guardam as mensagens produzidas -> Queue com disciplina FIFO
        private LinkedList<bool> _consumersQueue = new LinkedList<bool>(); // Queue onde as threads consumidoras se juntam à espera que existam mensagens -> Queue com disciplina FIFO 

        /// <summary>
        /// A operação Send promove a entrega de uma mensagem; esta operação nunca bloqueia a thread invocante.
        /// </summary>
        public void Send(Message<T> msg)
        {
            lock (this)
            {
                _messageQueue.AddLast(msg);
                Monitor.PulseAll(this);
            }
        }

        /// <summary>
        /// A operação Receive promove a recolha da mensagem mais antiga que satisfaça o predicado especificado num 
        /// parâmetro do tipo Predicate<int>(selector) a aplicar ao tipo das mensagens disponíveis na fila, 
        /// bloqueando a thread invocante caso não exista nenhuma mensagem que o satisfaça.
        /// </summary>
        public Message<T> Receive(Predicate<int> selector, int timeout)
        {
            lock (this)
            {
                Message<T> retMsg = null;

                if (_consumersQueue.Count == 0)
                {
                    foreach (var msg in _messageQueue)
                    {
                        if (selector(msg.Type)) // Existe uma mensagem com um predicado compativel comigo
                        {
                             retMsg = msg;
                            // We can't delete and return here because: http://www.dotnetperls.com/invalidoperationexception
                        }
                    }
                }

                if (retMsg != null)
                {
                    _messageQueue.Remove(retMsg);
                    return retMsg;
                }

                if (timeout == 0) 
                    return null;

                LinkedListNode<bool> myNode = new LinkedListNode<bool>(false);
                _consumersQueue.AddLast(myNode);

                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;

                do
                {
                    try
                    {
                        Monitor.Wait(this, timeout);
                    }
                    catch (ThreadInterruptedException) // TODO : Faz sentido a regeneração de excepção aqui?
                    {
                        _consumersQueue.Remove(myNode); // Return value: true if the element containing value is successfully removed; otherwise, false. This method also returns false if value was not found in the original LinkedList<T>.
                        Monitor.PulseAll(this);
                        throw;
                    }

                    if (_consumersQueue.First.Equals(myNode)) // Ver se sou eu que estou à frente da fila
                    {                    
                        foreach (var msg in _messageQueue)
                        {
                            if (selector(msg.Type)) // Existe uma mensagem com um predicado compativel comigo (aqui não faz sentido usar PulseAll)
                            {
                                retMsg = msg;
                            }
                        }
                    }

                    if (retMsg != null)
                    {
                        _messageQueue.Remove(retMsg);
                        _consumersQueue.Remove(myNode);
                        return retMsg;
                    }

                    if (SyncUtils.AdjustTimeout(ref lastTime, ref timeout) == 0)
                    {
                        _consumersQueue.Remove(myNode);
                        Monitor.PulseAll(this);
                        return null;
                    } 
                } while (true);
            }
        }
    }
}