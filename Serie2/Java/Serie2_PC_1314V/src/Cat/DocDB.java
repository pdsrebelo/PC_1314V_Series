package Cat;

import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.atomic.AtomicReference;

public class DocDB {

	/* SERIE 2 - EXERCICIO 1
	 * Escreva uma versão thread-safe da classe DocDB usando o mínimo de sincronização, privilegiando o 
	uso de construções não bloqueantes. (Por simplificação considere que o número de chamadas ao 
	método store não excede a capacidade especificada no construtor.) 
	 */
	private static class Doc{
		private final int version;
		private final String text;
		
		public Doc(int ver, String txt){
			version = ver;
			text = txt;
		}
	}
	
	private static class DocRef{
		public  AtomicReference<Doc> ref;
	}
	
	private AtomicInteger _idx;
	private final DocRef[] _store;
	
	public DocDB(int capacity){ 
		_store = new DocRef[capacity];
		for(int i = 0; i< capacity; ++i)
			_store[i]=new DocRef();
	}
	
	public int store(String text){
		int currIdx = _idx.getAndIncrement();
		_store[currIdx].ref = new AtomicReference<Doc>(new Doc(0,text));
		return currIdx;
	}
	
	public String get(int id){
		return _store[id].ref.get().text;
	}
	
	public void update(int id, String newText){ // Problema resolvido: "downgrade" da "version"
		Doc newDoc, oldDoc;
		int oldVersion;
		do{
			oldDoc = _store[id].ref.get(); 
			oldVersion = oldDoc.version;
			newDoc = new Doc(oldVersion+1, newText);
		}while( (!_store[id].ref.compareAndSet(oldDoc, newDoc)) || (oldVersion+1 != newDoc.version));
	}
}