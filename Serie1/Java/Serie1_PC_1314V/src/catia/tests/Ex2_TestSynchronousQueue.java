package catia.tests;

import junit.framework.Assert;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;

import catia.Ex2_SynchronousQueue;
import catia.tests.synchronousQueue.ConsumerThread;
import catia.tests.synchronousQueue.ProducerThread;

public class Ex2_TestSynchronousQueue {

	private static Ex2_SynchronousQueue<String> _queue;
	private static ConsumerThread<String> _consumerThread;
	private static ProducerThread<String> _producerThread;
	private final static String DEFAULT_VALUE = "VALUE";

	@Before
	public void beforeEachTest(){
		_queue = new Ex2_SynchronousQueue<String>();
		_consumerThread = new ConsumerThread<String>(_queue);
		_producerThread = new ProducerThread<String>(_queue, DEFAULT_VALUE);
	}

	@After
	public void afterTest(){
		Assert.assertFalse(_producerThread.interrupted());
		Assert.assertFalse(_consumerThread.interrupted());
	}
	
	@Test
	public void Ex2_TestPutAndTake() throws InterruptedException {
		_consumerThread.start();
		Assert.assertEquals(0, _queue.getNumberOfElements());
		_producerThread.start();
		Thread.sleep(1000);
		Assert.assertEquals(0, _queue.getNumberOfElements());
		Assert.assertNotNull(_consumerThread.getConsumedObject());
		Assert.assertEquals(DEFAULT_VALUE, _consumerThread.getConsumedObject());
	}
	
	@Test
	public void Ex2_TestPut() throws InterruptedException {
		int count = 1, total = 50;
		do{
			ProducerThread<String>prod = new ProducerThread<>(_queue, DEFAULT_VALUE);
			prod.start();
			Thread.sleep(10);
			Assert.assertEquals(count, _queue.getNumberOfElements());
			count++;
		}while(count<total);
	}
	
	@Test
	public void Ex2_TestTake() throws InterruptedException {
		_producerThread.start();
		_consumerThread.start();
		Thread.sleep(100);
		String receivedObject = _consumerThread.getConsumedObject();
		Assert.assertNotNull(receivedObject);
		Assert.assertEquals(DEFAULT_VALUE, receivedObject);
	}
	
	@Test
	public void Ex2_TestFIFO() throws InterruptedException{
		String value1 = "prod1", value2 = "prod2", value3 = "prod3";
		ProducerThread<String>prod1 = new ProducerThread<>(_queue, value1);
		prod1.start();
		ProducerThread<String>prod2 = new ProducerThread<>(_queue, value2);
		prod2.start();
		ProducerThread<String>prod3 = new ProducerThread<>(_queue, value3);
		prod3.start();
		
		_consumerThread.start();
		Thread.sleep(100);
		String taken = _consumerThread.getConsumedObject();
		Assert.assertEquals(value1, taken);
		
		_consumerThread = new ConsumerThread<>(_queue);
		_consumerThread.start();
		Thread.sleep(100);
		taken = _consumerThread.getConsumedObject();
		Assert.assertEquals(value2, taken);
		
		_consumerThread = new ConsumerThread<>(_queue);
		_consumerThread.start();
		Thread.sleep(100);
		taken = _consumerThread.getConsumedObject();
		Assert.assertEquals(value3, taken);
		
		Assert.assertEquals(0, _queue.getNumberOfElements());
	}

}
