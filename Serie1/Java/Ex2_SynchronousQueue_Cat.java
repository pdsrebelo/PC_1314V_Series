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
public class Ex2_SynchronousQueue <T> {

	// FIFO Queues:
	private LinkedList<T> consumerThreads;
	private LinkedList<T> objectsProduced;
	
	public Ex2_SynchronousQueue(){
		objectsProduced = new LinkedList<T>();
		consumerThreads = new LinkedList<T>();
	}
	
	// As threads consumidoras declaram disponibilidade 
	// para receber um objeto invocando a operação take, bloqueando-se até que o objeto seja recebido por 
	// uma thread consumidora.
	public synchronized T take() throws InterruptedException{
		T takenObject = null;
		T tId = (T) Thread.currentThread();//.getId();
		consumerThreads.add(tId);
		
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
			if(!objectsProduced.isEmpty()){
				if(consumerThreads.getFirst().equals(tId))
				{
					consumerThreads.removeFirst();
					takenObject = objectsProduced.removeFirst();
					this.notifyAll();
					break;
				}
			}
		}while(true);
		
		return takenObject;
	}
	
	// A operação put oferece um objeto e bloqueia a thread produtora até que exista uma 
	// thread consumidora disponível para o receber.
	public synchronized void put(T obj) throws InterruptedException{
		objectsProduced.add(obj);
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
