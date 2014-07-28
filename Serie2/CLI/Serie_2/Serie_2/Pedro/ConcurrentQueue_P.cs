using System.Threading;

namespace Serie_2.Pedro
{
    /// <summary>
    /// A classe ConcurrentQueue define um contentor com disciplina FIFO (First-In-First-Out) suportado numa lista simplesmente ligada. 
    /// A implementação suporta acessos concorrentes e as operações disponibilizadas não bloqueiam a thread invocante. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ConcurrentQueue_P<T>
    {
        private class Node<TS>
        {
            public readonly TS Item;
            public Node<TS> Next;

            public Node(TS item)
            {
                Item = item;
            }
        }

        private volatile Node<T> _dummy;
        private volatile Node<T> _head;
        private volatile Node<T> _tail;

        public ConcurrentQueue_P()
        {
            _dummy = new Node<T>(default(T));
            _tail = new Node<T>(default(T)){ Next = _dummy };
            _head = new Node<T>(default(T)){ Next = _dummy };
        }

        /// <summary>
        /// A operação Put coloca no fim da fila o elemento passado como argumento.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Put(T item)
        {
            Node<T> newNode = new Node<T>(item);
            do
            {
                Node<T> currTail = _tail;   
                Node<T> tailNext = currTail.Next;
                if (currTail == _tail)
                {
                    if (tailNext != null)
                    {
                        // Queue in intermediate state, advance tail
                        Interlocked.CompareExchange(ref currTail, tailNext, _tail);
                    }
                    else
                    {
                        // In quiescent state, try inserting new node
                        if (Interlocked.CompareExchange(ref newNode, null, currTail.Next) == null)
                        {
                            // Insertion succeeded, try advancing tail
                            Interlocked.CompareExchange(ref currTail, newNode, _tail);
                            return true;
                        }
                    }
                }
            } while (true);
        }

        /// <summary>
        /// A operação TryTake retorna o elemento presente no início da fila, ou null caso 
    	/// da fila estar vazia.
        /// </summary>
        /// <returns></returns>
        public T TryTake()
        {
            do{                                     // Keep trying until Dequeue is done  
                Node<T> currHead = _head;           // Read Head
                Node<T> currTail = _tail;           // Read Tail
                Node<T> headNext = currHead.Next;   // Read Head.ptr->next
                if (currHead == _head){             // Are head, tail, and next consistent?
                    if (currHead == currTail){      // Is queue empty or Tail falling behind?
                        if (headNext == null)       // Is queue empty?
                            return default(T);      // Queue is empty, couldn't dequeue

                        Interlocked.CompareExchange(ref currTail, currTail.Next, _tail);    // Tail is falling behind. Try to advance it
                    }
                    else{                                   // No need to deal with Tail
                        T elem = headNext.Item;             // Read value before CAS, otherwise, another dequeue might free the next node
                        if (Interlocked.CompareExchange(ref currHead, headNext,_head) == null)  // Try to swing Head to the next node
                            break;
                    }
                }
            } while (true);

            return _head.Item;
        }

        /// <summary>
        /// A operação isEmpty produz o valor booleano que indica se a fila contém elementos.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _head == _tail;
        }
    }
}