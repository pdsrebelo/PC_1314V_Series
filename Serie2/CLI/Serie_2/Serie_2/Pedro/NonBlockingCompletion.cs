using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_2.Pedro
{
    /// <summary>
    /// 
    /// </summary>
    class NonBlockingCompletion
    {
         private volatile int _permits;
         private volatile bool _isSignaled;
         private volatile int _waiters;

         public NonBlockingCompletion(int initial){
             if (initial > 0)
                 _permits = initial;
             _isSignaled = false;
         }

         public void Complete(){
             if (_isSignaled) return;
             Interlocked.Increment(ref _permits); 
             if (_waiters != 0)
             {
                 lock (this)
                 {
                     if (_waiters > 0)
                         Monitor.Pulse(this); 
                 }
             }
         }

         public void CompleteAll(){
             if (!_isSignaled)
                 _isSignaled = true;
         }

         private bool TryCompletion(){
             SpinWait sw = new SpinWait();
             do
             {
                 int p;
                 if ((p = _permits) == 0)
                     return false;
                 if (Interlocked.CompareExchange(ref _permits, p - 1, p) == p)
                     return true;
                
                 sw.SpinOnce();
             } while (true);
         }

         public bool WaitForCompletion(int timeout){

             if (TryCompletion())
                 return true;
            
             if (_isSignaled) return true;
             if (timeout == 0)
                 return false;
            
             int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
             lock (this)
             {
                 do
                 {
                     if (TryCompletion())
                         return true;
                        
                     if (SyncUtils.AdjustTimeout(ref lastTime, ref timeout) == 0)
                         return false;
                    
                     _waiters++;
                     Thread.MemoryBarrier();
                     if (TryCompletion())
                     {
                         _waiters--;
                         return true;
                     }
                     try
                     {
                         Monitor.Wait(this, timeout);
                     }
                     catch (ThreadInterruptedException)
                     {
                         if (_permits > 0)
                             Monitor.Pulse(this);
                        
                         throw;
                     }
                     finally
                     {
                         _waiters--;
                     }
                 } while (true);
             }
         }
     }
 }