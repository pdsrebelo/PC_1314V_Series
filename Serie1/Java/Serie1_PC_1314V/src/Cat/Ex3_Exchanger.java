package Cat;

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

	public T exchange(T myMsg, long timeout){
		
		return null;	
	}
	
	
}
