using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_2.Catia
{
    #pragma warning disable 420
    class ConcurrentQueueC<T>
    {
        class Node<TK>
        {
            public readonly object Value;
            public Node<TK> Next;

            public Node(object val) // .CTOR
            {
                Value = val;
            }
        }

        private volatile Node<T> _dummyNode;
        private volatile Node<T> _headNode;
        private volatile Node<T> _tailNode;

        /*
         *  Implemente em Java e C# a classe ConcurrentQueue<T> que define um contentor com disciplina 
            FIFO (First-In-First-Out) suportado numa lista simplesmente ligada. A classe disponibiliza as 
            operações put, tryTake e isEmpty. A operação put coloca no fim da fila o elemento passado 
            como argumento; a operação tryTake retorna o elemento presente no início da fila, ou null caso 
            da file estar vazia; a operação isEmpty produz o valor booleano que indica se a fila contém 
            elementos. A implementação suporta acessos concorrentes e as operações disponibilizadas não 
            bloqueiam a thread invocante. 
            Nota: Para a implementação considere a explicação sobre a lock-free queue, proposta por Michael e 
            Scott, que consta no Capítulo 15 do livro Java Concurrency in Practice. 
         */

        // .CTOR
        public ConcurrentQueueC()
        {
            _dummyNode = new Node<T>(null) {Next = null};
            _headNode = new Node<T>(null) {Next = _dummyNode};
            _tailNode = new Node<T>(null) {Next = _dummyNode};
        } 

        void put(object newValue)
        {
            bool success = false;
            do
            {
                Node<T> readTailNode = _tailNode;

                if (_tailNode == readTailNode && _tailNode.Next == null)
                {
                    if (Interlocked.CompareExchange(ref _tailNode.Next, new Node<T>(newValue), null) == null)
                        success = true;
                }
                else
                {
                    // Update the tail node
                    if (_tailNode != readTailNode) continue;
                    if (Interlocked.CompareExchange(ref _tailNode, _tailNode.Next, readTailNode) == readTailNode)
                        success = true;
                }
            }
            while (!success) ;
        }

        object tryTake()
        {
            object valueToReturn = null;
            return valueToReturn;
        }

        bool IsEmpty()
        {
            return _headNode.Next==_tailNode.Next && _headNode.Next ==_dummyNode;
        }
    }
}
