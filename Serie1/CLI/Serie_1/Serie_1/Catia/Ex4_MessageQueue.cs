﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
    class Message<T>
    {
        public int type;   // Tipo da mensagem: inteiro positivo
        T data;     // Dados (T)
        bool received;

        public Message(int msgType, T msgData)
        {
            type = msgType;
            data = msgData;
            received = false;   // Indica se a mensagem já foi recebida por uma Thread
        }

        public void setReceived(bool recvd)
        {
            received = recvd;
        }
    }

    class Ex4_MessageQueue<T>
    {
        private LinkedList<Message<T>> messageQueue;
        private LinkedList<bool> receivers; // bool = indica se já recebeu mensagem ou não

        public Ex4_MessageQueue(){ //.CTOR
            messageQueue = new LinkedList<Message<T>>();
            receivers = new LinkedList<bool>();
        }

        public void Send(Message<T> msg)
        {
            lock (this)
            {
                messageQueue.AddLast(msg);
                Monitor.PulseAll(this);
            }
        }

        public Message<T> Receive(long timeout, Predicate<int> pred)
        {
            lock (this)
            {
                Message<T> receivedMsg;

                if (receivers.Count == 0 && messageQueue.Count>0) // Se não é necessário esperar pela sua vez (ñ é necessário colocar na queue)...
                {
                    // Ver se há alguma message cujo tipo (int) satisfaça o predicado
                    LinkedList<Message<T>>messagesCompatible = messageQueue.Where(pred(messageQueue.type)).Select();

                    if(messagesCompatible.Count>0){
                        // Colocar o boolean da Message a true = para a "Sender" Thread saber que a sua msg já foi recebida
                        ((Message<T>)messagesCompatible.First.Value).setReceived(true);
                        receivedMsg = messagesCompatible.RemoveFirst();
                        return receivedMsg;
                    }
                }

                // Se é necessário ficar na Queue à espera da sua vez...
                LinkedListNode<bool> receiverThread = new LinkedListNode<bool>(false);
                receivers.AddLast(receiverThread);

                long lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0; // Get the current time

                do
                {
                    // Ver se há alguma message cujo tipo (int) satisfaça o predicado
                    LinkedList<Message<T>>messagesCompatible = messageQueue.Where(pred(messageQueue.type)).Select();

                    if(receiverThread.Equals(receivers.First) && messagesCompatible.Count>0){
                   
                        // Colocar o boolean da Message a true = para a "Sender" Thread saber que a sua msg já foi recebida
                        ((Message<T>)messagesCompatible.First.Value).setReceived(true);
                        receivedMsg = messagesCompatible.RemoveFirst();
                        return receivedMsg;  
                    }
                    else{
                        try
                        {
                            Monitor.Wait(this, timeout);
                        }
                        catch (ThreadInterruptedException iex)
                        {
                            receivers.Remove(receiverThread); // Tirar da lista de receivers - ñ é preciso verificar pois qd chega aqui, está garantida| na lista
                            Monitor.PulseAll(this); // Notificar as outras threads - pois uma delas poderá ter condições para ler uma mensagem
                        }
                    }

                    timeout = SyncUtils.AdjustTimeout(ref lastTime, timeout);
                } while (timeout > 0);
            }
            return null;
        }
    }
}
