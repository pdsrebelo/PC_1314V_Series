package catia_36923;

import java.util.concurrent.atomic.AtomicReference;

public class ConcurrentQueue<T> {
	
	/* SERIE 2 - EXERCICIO 2 
		Implemente em Java e C# a classe ConcurrentQueue<T> que define um contentor com disciplina 
		FIFO (First-In-First-Out) suportado numa lista simplesmente ligada. A classe disponibiliza as 
		operações put, tryTake e isEmpty. A operação put coloca no fim da fila o elemento passado 
		como argumento; a operação tryTake retorna o elemento presente no início da fila, ou null caso 
		da file estar vazia; a operação isEmpty produz o valor booleano que indica se a fila contém 
		elementos. A implementação suporta acessos concorrentes e as operações disponibilizadas não 
		bloqueiam a thread invocante. 
		Nota: Para a implementação considere a explicação sobre a lock-free queue, proposta por Michael e 
		Scott, que consta no Capítulo 15 do livro Java Concurrency in Practice.
	*/
	
	private class CNode<K> {
		final K value;
		final AtomicReference<CNode<K>> nextNode;

		public CNode(K val, CNode<K> next) {
			value = val;
			nextNode = new AtomicReference<CNode<K>>(next);
		} 
	}
	
	private CNode<T>dummy = new CNode<T>(null, null);
	private AtomicReference<CNode<T>> queueHead= new AtomicReference<>(dummy); // Aponta para a cabeça da queue
	private AtomicReference<CNode<T>> queueTail= new AtomicReference<>(dummy);
	
	// A operação put coloca no fim da fila o elemento passado como argumento;
	public void put(T element){
		
		CNode<T> newNode = new CNode<T>(element, null);
		
		// Tentar adicionar o novo node à queue...
		do{
			CNode<T> currTail = queueTail.get();
			CNode<T> next = currTail.nextNode.get();
			
			// Se a informação da tail é consistente
			if(queueTail.get() == currTail){
				if(next == null){ // Se a tail é realmente o último node
					// Tentar adicionar o novo node no fim da queue
					if(queueTail.get().nextNode.compareAndSet(null, newNode)){
						queueTail.compareAndSet(currTail, newNode);
						break; //Inserção feita com sucesso
					}
				}else{
					// A tail não estava a apontar para o ultimo node
					// tentar mudar a referencia para o próximo node
					queueTail.compareAndSet(currTail, next);
				}
			}
		}while(true);
	}
	
	// A operação tryTake retorna o elemento presente no início da fila, ou null caso da file estar vazia;
	public T tryTake(){
		do{
			if(isEmpty()) // Retorna null caso a fila esteja vazia
				return null;
			
			CNode<T>headPtr = queueHead.get();
			
			// Actualiza a head, se a informação lida estiver consistente, senão tenta novamente
			if(queueHead.compareAndSet(headPtr, headPtr.nextNode.get())){
				return queueHead.get().value;
			}
		}while(true);
	}
	
	// A operação isEmpty produz o valor booleano que indica se a fila contém elementos
	public boolean isEmpty(){
		return queueHead.get().nextNode.get()==null;
	}
}
