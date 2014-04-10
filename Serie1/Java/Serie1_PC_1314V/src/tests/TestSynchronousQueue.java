package tests;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;

public class TestSynchronousQueue {

	private Thread t1;
	private Thread t2;
	
	@Before 
	public void start(){
		//TODO TESTS
		t1 = new Thread();
		t2 = new Thread();
	}
	
	@After
	public void end(){
		
	}
	
	@Test
	public void twoThreadsTest(){
		
	}
	
}
