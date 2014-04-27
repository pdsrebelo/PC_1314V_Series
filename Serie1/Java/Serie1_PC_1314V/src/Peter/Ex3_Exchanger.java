package Peter;

/*
Implemente em Java o sincronizador exchanger Exchanger<T> que permite a troca, entre pares de 
threads, de mensagens definidas por instâncias do tipo T.
*/
public class Ex3_Exchanger<T> {
	/*
	A classe que implementa o sincronizador inclui o método T exchange(T myMsg, long timeout), 
	que é chamado pelas threads para oferecer uma mensagem (parâmetro myMsg) e receber a mensagem 
	oferecida (valor de retorno) pela thread com que emparelham. Quando a troca de mensagens não 
	pode ser realizada de imediato (porque não existe ainda  uma thread bloqueada), a thread corrente 
	fica bloqueada até que seja interrompida, expire o limite de tempo especificado através do parâmetro 
	timeout, ou até que outra thread invoque o método exchange.
	*/
	
	T messageHolder = null;
	
	public synchronized T exchange(T myMsg, long timeout) throws InterruptedException{
		if(messageHolder != null){
			T retMsg = messageHolder;
			messageHolder = myMsg;
			this.notify();
			return retMsg;
		}
		
		messageHolder = myMsg;
		
		long lasttime = System.currentTimeMillis();
		
		do{
			try{
				this.wait(timeout);
			}catch(InterruptedException ie){
				if(!messageHolder.equals(myMsg)){ /* Fui interrompido, no entanto a minha mensagem já não se encontra no holder, logo retorno sucesso e regenero a interupção*/
					T retMsg = messageHolder;
					messageHolder = null;
					return retMsg;
				}			
				throw ie;
			}
			
			if(!messageHolder.equals(myMsg)){ /* Não é necessário notificar nenhuma thread, pois neste ponto ninguem está bloqueado */
				T retMsg = messageHolder;
				messageHolder = null;
				return retMsg;
			}
			
			if(SyncUtils.adjustTimeout(lasttime, timeout) == 0){
				messageHolder = null;
				return null;
			}
			
		}while(true);
		
	}
}