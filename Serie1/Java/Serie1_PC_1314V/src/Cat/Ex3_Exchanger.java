package Cat;

import java.util.LinkedList;

import Utils.SyncUtils;

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
	
	// Classe auxiliar & opcional
	private class Message<K>{
		protected K msg;
		protected boolean paired; // Indica 
		
		public Message(K m){
			msg = m;
			paired = false;
		}
	}
	
	private LinkedList<Message<T>> messages;
	
	// é chamado pelas threads para oferecer uma mensagem (myMsg) e receber uma mensagem oferecida (retorno)
	// pela thread com que emparelham
	public synchronized T exchange(T myMsg, long timeout) throws InterruptedException{

		T receivedMsg = null;
		
		// Se já existe pelo menos 1 msg na lista e a 1ª ainda ñ tem par... emparelha e retorna!
		// É garantida a ordem FIFO pois enquanto a 1ª msg não tiver par, as restantes também não terão.
		if(!messages.isEmpty() && !messages.getFirst().paired){
			receivedMsg = messages.getFirst().msg;
			messages.getFirst().msg = myMsg;
			messages.getFirst().paired = true;
			this.notifyAll(); // Acorda as threads para elas verem se foi a sua msg a ser consumida
			return receivedMsg;
		}
		
		// Senão: Adicionar esta mensagem à lista
		Message<T> offeredMsg = new Message<T>(myMsg);
		messages.add(offeredMsg);
		
		long lastTime = System.currentTimeMillis();
		do{
			this.notify();	// Notificar - Para que haja thread p/emparelhar
			try{	
				this.wait();	// Esperar
			}catch(InterruptedException iex){
				messages.remove(); // Ñ é necessário verificar se já pertence à lista; pois já há certezas disso
				throw iex;
			}
			// Verificar se a msg oferecida para troca já foi consumida
			if(offeredMsg.paired){
				receivedMsg = offeredMsg.msg;
				messages.remove(offeredMsg);
				break; // Mensagem substituída! agora "msg" é a mensagem oferecida pela outra thread! Retornar.
			}
			timeout = SyncUtils.adjustTimeout(lastTime, timeout);
		}while(timeout>0);
		return receivedMsg;
	}
}
