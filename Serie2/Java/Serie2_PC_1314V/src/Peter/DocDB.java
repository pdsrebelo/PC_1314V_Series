package ex1;

import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.atomic.AtomicReference;

public class DocDB_ThreadSafe {
	
	/*
	 	Escreva uma versão thread-safe da classe DocDB usando o mínimo de sincronização, privilegiando o uso de construções não bloqueantes.  
		(Por simplificação considere que o número de chamadas ao método storenão excede a capacidade especificada no construtor.)
	 */
	
	private static class Doc {
		private final int version;
		private final String text;

		public Doc(int ver, String txt) {
			version = ver;
			text = txt;
		}
	}

	private static class DocRef {
		public AtomicReference<Doc> ref;
	}

	private AtomicInteger _idx;
	private final DocRef [] _store;

	public DocDB_ThreadSafe(int sz) {
		_store = new DocRef[sz];
		for (int i = 0; i < sz; ++i)
			_store[i] = new DocRef();
	}

	public int store(String text) {
		int temp = _idx.getAndIncrement();
		
		_store[temp].ref.getAndSet(new Doc(0, text)); // _store[_idx].ref = new Doc(0, text);
		
		return temp;	// return _idx++
	}

	public String get(int id) {
		return _store[id].ref.get().text;
	}

	public void update(int id, String newText) {
		
		do{
			Doc oldVer = _store[id].ref.get();
			
			if(_store[id].ref.compareAndSet(oldVer, new Doc(oldVer.version+1, newText)))
				break;				

			Thread.yield();
		}while(true);
		// _store[id].ref = new Doc(_store[id].ref.version + 1, newText);
	}
}
