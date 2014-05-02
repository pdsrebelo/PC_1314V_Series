using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Serie_1.Catia
{
    /*
     * Usando monitores intrínsecos CLI, implemente o sincronizador MessageQueue que suporta a 
    comunicação, através de mensagens, entre threads produtoras e consumidoras. 
    
    As mensagens têm associado um tipo (inteiro positivo) e um campo de dados do tipo genérico T. 
     * 
    A operação Send promove a entrega de uma mensagem; esta operação nunca bloqueia a thread invocante. 
    
    A operação Receive promove a recolha da mensagem mais antiga que satisfaça o predicado especificado num 
    parâmetro do tipo Predicate<int> (selector) a aplicar ao tipo das mensagens disponíveis na fila, 
    bloqueando a thread invocante caso não exista nenhuma mensagem que o satisfaça. O sincronizador 
    deve garantir que as mensagens do mesmo tipo são entregues pela ordem de chegada e que as threads 
    receptoras também são servidas por ordem de chegada. A operação Receive deve suportar 
    desistência por timeout e por cancelamento devido à interrupção de threads bloqueadas
     */
    

    class Ex4MessageQueue<T>
    {
        public class Message<T>
        {
            public int Type { get; set; } // Tipo da mensagem: inteiro positivo
            protected T Data { get; set; } // Dados (T)

            public Message(int msgType, T msgData)
            {
                Type = msgType;
                Data = msgData;
            }
        }

        private LinkedList<Message<T>> messageQueue;
        private LinkedList<bool> receivers; // bool = só porque sim... para ter uma lista com alguma coisa.

        public Ex4MessageQueue(){ //.CTOR
            messageQueue = new LinkedList<Message<T>>();
            receivers = new LinkedList<bool>();
        }

        public void Send(Message<T> msg)
        {
            lock (this)
            {
                messageQueue.AddLast(msg);
                Monitor.PulseAll(this); // PulseAll: Acordar várias threads "receivers", pois se acordármos só 1 e ela ñ for a que tem condições,será preciso haver mais threads a ser acordadas
            }
        }

        public Message<T> Receive(int timeout, Predicate<int> pred)
        {
            Message<T> receivedMsg = null;
            lock (this)
            {
                if (receivers.Count == 0 && messageQueue.Count>0) // Se não é necessário esperar pela sua vez (ñ é necessário colocar na queue)...
                {
                    foreach (var msg in messageQueue.Where(msg => pred(msg.Type))) // Ver se há alguma message cujo tipo (int) satisfaça o predicado
                    {
                        receivedMsg = msg; break;
                    }
                    if (receivedMsg != null)
                    {
                        messageQueue.Remove(receivedMsg);
                        return receivedMsg;
                    }
                }

                // Se é necessário ficar na Queue à espera da sua vez...
                var receiverThread = new LinkedListNode<bool>(false);
                receivers.AddLast(receiverThread);

                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0; // Get the current time

                do
                {
                    // Ver se há alguma message cujo tipo (int) satisfaça o predicado
                    foreach (var msg in messageQueue.Where(msg => pred(msg.Type))) // Ver se há alguma message cujo tipo (int) satisfaça o predicado
                    {
                        receivedMsg = msg; break;
                    }
                    if (receivedMsg != null)
                    {
                        messageQueue.Remove(receivedMsg); break;
                    }
                    try
                    {
                        Monitor.Wait(this, timeout);
                    }
                    catch (ThreadInterruptedException iex)
                    {
                        receivers.Remove(receiverThread); // Tirar da lista de receivers - ñ é preciso verificar pois qd chega aqui, está garantida| na lista
                        Monitor.PulseAll(this); // Notificar as outras threads - pois uma delas poderá ter condições para ler uma mensagem
                        throw;
                    }
                    
                    if ((timeout = SyncUtils.AdjustTimeout(ref lastTime, ref timeout)) != 0) continue;

                    receivers.Remove(receiverThread);
                    Monitor.PulseAll(this);
                } while (timeout > 0);
            }
            return receivedMsg;
        }
    }
}
