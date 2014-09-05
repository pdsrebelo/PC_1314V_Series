package catia_36923;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

public class ConcurrentQueueTests {
	private static ConcurrentQueue<Integer> emptyList;
	private static ConcurrentQueue<Integer> nonEmptyList;
	
	@Before
	private void initializeList(){
		emptyList = new ConcurrentQueue<Integer>();
		nonEmptyList = new ConcurrentQueue<Integer>();
		for(int i = 0; i< 100; i++){
			nonEmptyList.put(i*i);
		}
	}
	
	@Test
	private void testEmptyListTake(){
		Assert.assertNull(emptyList.tryTake());
	}
	
	@Test
	private void testEmptyListPut(){
		Integer number = new Integer(10);
		emptyList.put(number);
		Assert.assertEquals(number, emptyList.tryTake());
	}
	
	@Test
	private void testNonEmptyListTake(){
	}
	
	@Test
	private void testNonEmptyListPut(){

	}
}
