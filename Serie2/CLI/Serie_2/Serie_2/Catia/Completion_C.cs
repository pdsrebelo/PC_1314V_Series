using System;
using System.Threading;

namespace Serie_2.Catia
{
    // SERIE 2 - EXERCICIO 3

    /*
        Usando as técnicas de sincronização non-blocking discutidas nas aulas teóricas, implemente uma 
        versão optimizada do sincronizador Completion (cuja especificação consta no exercício 1 da 
        primeira série de exercícios). As optimizações devem incidir sobre a situação em que a operação 
        WaitForCompletion não bloqueia a thread invocante e as situações em que as operações 
        Complete, CompleteAll não têm que libertar nenhuma thread bloqueada.     */
   
    #pragma warning disable 420 // Disable warnings for using "ref" on volatile vars
    class CompletionC
    {
        private volatile int _signaled;
        private volatile int _permits;

        // .CTOR
        public CompletionC(int permits)
        {
            if (permits > 0) _permits = permits;
            _signaled = 0;  // Starts unsignaled
        }

        public void Complete()
        {
            if (_signaled == 1)
                return;
            Interlocked.Increment(ref _permits);
        }

        public void CompleteAll()
        {
            var currStatus = _signaled;
            while (Interlocked.CompareExchange(ref _signaled, 1, currStatus) != currStatus){} // When the exchange is done, arg#3 is returned
        }

        //TODO VERIFY
        public bool WaitForCompletion(int timeout)
        {
            if (_signaled == 1) // Signaled to please all
                return true;

            if (timeout == 0)   // Timed out
                return false;

            if (_permits >= 1)  // Verify if there are enough permits to "Acquire"
            {
                Interlocked.Decrement(ref _permits);
                return true;
            }

            var initialTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0; // Get the current time
            do
            {
                if (_signaled == 1 || _permits >= 1)  // Verify if there are enough permits to "Acquire" or if it's Signaled to please all
                {
                    Interlocked.Decrement(ref _permits);
                    return true;
                }
                timeout = SyncUtils.AdjustTimeout(ref initialTime, ref timeout); // Update the time

            } while (timeout>0);

            return false;
        }
    }
}
