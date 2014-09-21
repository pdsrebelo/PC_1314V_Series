package catia.tests.synchronousQueue;

import catia.Ex2_SynchronousQueue;

public 	class ConsumerThread<T> extends Thread{
	
	Ex2_SynchronousQueue<T> _queue;
	public boolean consumerInterrupted;
	private T consumedObject;
	
	public ConsumerThread(Ex2_SynchronousQueue<T> queue){
		_queue = queue;
		consumerInterrupted = false;
		consumedObject = null;
	}
	
	public void run(){
		// DO STUFF
		try {
			consumedObject = _queue.take();
		} catch (InterruptedException e) {
			consumerInterrupted = true;
		}
	}
	
	public T getConsumedObject(){
		return consumedObject;
	}
}
