package Peter;

public class SyncUtils {
	public static long adjustTimeout(long lasttime, long timeout) {
	    if (timeout != -1) {
	    	long now = System.currentTimeMillis();
	    	long elapsed = (now == lasttime) ? 1 : (now - lasttime);
	        if (elapsed >= lasttime)
	        {
	            timeout = 0;
	        }
	        else {
	            timeout -= elapsed;
	            lasttime = now;
	        }
	    }
	    return timeout;
	}
}
