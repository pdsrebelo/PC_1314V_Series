package ex2;

import java.util.concurrent.atomic.AtomicReference;

/*
 	A classe ConcurrentQueue define um contentor com disciplina FIFO (First-In-First-Out) suportado numa lista simplesmente ligada. 
 	A implementação suporta acessos concorrentes e as operações disponibilizadas não bloqueiam a thread invocante. 
*/

public class ConcurrentQueue<T> {
	
	private static class Node <T>{
		final T item;
		final AtomicReference<Node<T>> next;
		
		public Node(T item, Node<T>next){
			this.item = item;
			this.next = new AtomicReference<Node<T>>(next);
		}
	}
	
	private final Node<T> dummy = new Node<T>(null, null);
	
	private final AtomicReference<Node<T>> head = new AtomicReference<Node<T>>(dummy);
	private final AtomicReference<Node<T>> tail = new AtomicReference<Node<T>>(dummy);
		
	public boolean put(T item) {
		Node<T> newNode = new Node<T>(item, null);
		while (true){
			Node<T> curTail = tail.get();
			Node<T> tailNext = curTail.next.get();
			if (curTail == tail.get()) {
				if (tailNext != null) {
					// Queue in intermediate state, advance tail
					tail.compareAndSet(curTail, tailNext);
				}else {
					// In quiescent state, try inserting new node
					if (curTail.next.compareAndSet(null, newNode)) {
						// Insertion succeeded, try advancing tail
						tail.compareAndSet(curTail, newNode);
						return true;
					}
				}
			}
		}
	}
    
    /* 	A operação tryTake retorna o elemento presente no início da fila, ou null caso 
    	da fila estar vazia. */
    public T tryTake()
    {
    	do{											// Keep trying until Dequeue is done
			Node<T> curHead = head.get();			// Read Head
			Node<T> curTail = tail.get();			// Read Tail
			Node<T> headNext = curHead.next.get();	// Read Head.ptr->next
			if(curHead == head.get()){				// Are head, tail, and next consistent?
				if(curHead == curTail){				// Is queue empty or Tail falling behind?
					if(headNext == null)			// Is queue empty?
						return null;				// Queue is empty, couldn't dequeue			

					tail.compareAndSet(curTail, curTail.next.get());	// Tail is falling behind. Try to advance it
				}
				else{							// No need to deal with Tail
					T elem = headNext.item;		// Read value before CAS, otherwise, another dequeue might free the next node
					if(head.compareAndSet(curHead, headNext))	// Try to swing Head to the next node
						break;
				}
			}
    	}while(true);
    	
    	return head.get().item;
    }

    /* A operação isEmpty produz o valor booleano que indica se a fila contém elementos. */
    public boolean IsEmpty()
    {
    	if(head.get() == tail.get())
    		return true;
        return false;
    }

}