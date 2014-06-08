package Cat;

import java.util.LinkedList;

public class ConcurrentQueue<T> {
	/*
	 * 	Implemente em Java e C# a classe ConcurrentQueue<T> que define um contentor com disciplina 
		FIFO (First-In-First-Out) suportado numa lista simplesmente ligada. A classe disponibiliza as 
		operações put, tryTake e isEmpty. A operação put coloca no fim da fila o elemento passado 
		como argumento; a operação tryTake retorna o elemento presente no início da fila, ou null caso 
		da file estar vazia; a operação isEmpty produz o valor booleano que indica se a fila contém 
		elementos. A implementação suporta acessos concorrentes e as operações disponibilizadas não 
		bloqueiam a thread invocante. 
		Nota: Para a implementação considere a explicação sobre a lock-free queue, proposta por Michael e 
		Scott, que consta no Capítulo 15 do livro Java Concurrency in Practice. 
	 */

	private LinkedList<T> queue; // FIFO Concurrent Queue
	
	// A operação put coloca no fim da fila o elemento passado como argumento;
	public void put(T element){
		
	}
	
	// A operação tryTake retorna o elemento presente no início da fila, ou null caso da file estar vazia;
	public T tryTake(){
		return null;
	}
	
	// A operação isEmpty produz o valor booleano que indica se a fila contém elementos
	public boolean isEmpty(){
		return false;//return queue.size()==0;
	}
}
