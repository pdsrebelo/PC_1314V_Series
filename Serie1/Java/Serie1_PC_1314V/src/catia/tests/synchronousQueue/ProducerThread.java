package catia.tests.synchronousQueue;

import catia.Ex2_SynchronousQueue;

public class ProducerThread<T> extends Thread {

	private T DEFAULT_VALUE;
	private Ex2_SynchronousQueue<T> _queue;
	private boolean producerInterrupted;
	
	public ProducerThread(Ex2_SynchronousQueue<T> queue, T defValue){
		_queue = queue;
		setProducerInterrupted(false);
		DEFAULT_VALUE = defValue;
	}
	
	public void run(){
		// DO STUFF
		try {
			_queue.put(DEFAULT_VALUE);
		} catch (InterruptedException e) {
			setProducerInterrupted(true);
		}
	}

	public boolean isProducerInterrupted() {
		return producerInterrupted;
	}

	public void setProducerInterrupted(boolean producerInterrupted) {
		this.producerInterrupted = producerInterrupted;
	}
}
