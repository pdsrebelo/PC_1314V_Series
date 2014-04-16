package Cat;

import java.util.LinkedList;

public class Ex3_Exchanger<T> {

	/*
	 * 3. 
	 * 	Implemente em Java o sincronizador exchanger Exchanger<T> que permite a troca, entre pares de 
		threads, de mensagens definidas por instâncias do tipo T. A classe que implementa o sincronizador 
		inclui o método T exchange(T myMsg, long timeout), que é chamado pelas threads para 
		oferecer uma mensagem (parâmetro myMsg) e receber a mensagem oferecida (valor de retorno) pela 
		thread com que emparelham. Quando a troca de mensagens não pode ser realizada de imediato 
		(porque não existe ainda uma thread bloqueada), a thread corrente fica bloqueada até que seja 
		interrompida, expire o limite de tempo especificado através do parâmetro timeout, ou até que outra 
		thread invoque o método exchange. 
	 */
	
	public class Message<T>{
		protected T msg;
		protected boolean consumed;
		
		public Message(T m){
			msg = m;
			consumed = false;
		}
	}
	private LinkedList<Message<T>> messages; // Cada posição tem a mensagem e o número da thread
	
	// é chamado pelas threads para oferecer uma mensagem (myMsg) e receber uma mensagem oferecida (retorno)
	// pela thread com que emparelham
	public synchronized T exchange(T myMsg, long timeout) throws InterruptedException{

		T receivedMsg = null;
		
		if(!messages.isEmpty()){
			receivedMsg = messages.getFirst().msg;
			messages.getFirst().msg = myMsg;
			messages.getFirst().consumed = true;
			this.notifyAll();
			return receivedMsg;
		}
		
		Message<T> offeredMsg = new Message<T>(myMsg);
		do{
			if(messages.isEmpty()){ // Adicionar esta mensagem à lista
				messages.add(offeredMsg);
			}
			try{
				this.notify();	// Notificar - Para que haja thread p/emparelhar
				this.wait();
			}catch(InterruptedException iex){
				messages.remove();
				throw iex;
			}
			
			// Wait for another message to be offered for consumption 
			if(offeredMsg.consumed){
				T msg = offeredMsg.msg;
				messages.remove(offeredMsg);
				return msg; // Mensagem substituída! agora "msg" é a mensagem oferecida pela outra thread!
			}
			if(timeout==0){
				//timeout= SyncUtils.;
			}
		}while(true);
	}
}
