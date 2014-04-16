package Cat;
import java.util.LinkedList;


/*
 * 2. 
 * 	Implemente em Java o sincronizador synchronous queue, com base na classe 
	SynchronousQueue<T> que define as operações T take() e void put(T obj). O sincronizador 
	é usado para comunicar através de objetos do tipo T entre threads produtoras e threads 
	consumidoras. A operação put oferece um objeto e bloqueia a thread produtora até que exista uma 
	thread consumidora disponível para o receber. As threads consumidoras declaram disponibilidade 
	para receber um objeto invocando a operação take, bloqueando-se até que o objeto seja recebido por 
	uma thread consumidora. O sincronizador implementa disciplina FIFO, ou seja, são sempre 
	satisfeitos os pedidos das threads bloqueadas há mais tempo. 

 */

	/***
	 * Classe auxiliar
	 * 
	 * protected T object;	// Objecto oferecido, para ser consumido por uma Thread.
	 * protected boolean consumed; // Indica se o objecto (do tipo T) foi consumido por uma Thread.
	 * 
	 * @author Cátia
	 *
	 * @param <T>
	 */
	public class Ex2_SynchronousQueue <T> {

		public class ObjectProduced{
			protected T object;
			protected boolean consumed;
			
			public ObjectProduced(T obj){
				object = obj;
				consumed = false;
			}
	}
	
	// FIFO Queues:
	private LinkedList<Integer> consumerThreads;		// Queue FIFO de thread ID's que pretendem consumir objectos
	private LinkedList<ObjectProduced> objectsProduced;	// Queue FIFO com os objectos a consumir
	
	public Ex2_SynchronousQueue(){
		objectsProduced = new LinkedList<ObjectProduced>();
		consumerThreads = new LinkedList<Integer>();
	}
	
	// As threads consumidoras declaram disponibilidade 
	// para receber um objeto invocando a operação take, bloqueando-se até que o objeto seja recebido por 
	// uma thread consumidora.
	public synchronized T take() throws InterruptedException{
		
		T takenObject = null;
		int tId = (int) Thread.currentThread().getId();
		consumerThreads.add(tId);
		
		if(!objectsProduced.isEmpty() && consumerThreads.getFirst().equals(tId)){	
			return objectsProduced.getFirst().object;
		}
		
		do{
			if(objectsProduced.isEmpty()){

				try {
					this.notifyAll();
					this.wait();
				} catch (InterruptedException e) {
					consumerThreads.remove(tId);
					throw e;
				}
			}			
			if(!objectsProduced.isEmpty() && consumerThreads.getFirst().equals(tId)){
				consumerThreads.removeFirst();
				takenObject = objectsProduced.getFirst().object;
				objectsProduced.getFirst().consumed = true;
				this.notifyAll();
				break;
			}
		}while(true);
		
		return takenObject;
	}
	
	// A operação put oferece um objeto e bloqueia a thread produtora até que exista uma 
	// thread consumidora disponível para o receber.
	public synchronized void put(T obj) throws InterruptedException{
		objectsProduced.add(new ObjectProduced(obj));
		do{
			if(consumerThreads.isEmpty()){
				try{
					this.wait();
				}catch(InterruptedException iex){
					objectsProduced.remove(obj);
					throw iex;
				}
			}
			if(!consumerThreads.isEmpty()){
				if(objectsProduced.getFirst().equals(obj)){
					break;
				}
			}
		}while(true);
		this.notifyAll();
	}
}
