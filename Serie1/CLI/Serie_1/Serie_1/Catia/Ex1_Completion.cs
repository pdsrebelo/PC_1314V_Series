using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_1
{
    /*
     * 1.   Usando monitores intrínsecos CLI, implemente o sincronizador Completion, que representa um 
            gestor de unidades que indicam a conclusão de tarefas. A operação Complete sinaliza a conclusão de 
            uma tarefa e viabiliza a execução de exatamente uma chamada ao método WaitForCompletion. A 
            operação WaitForCompletion bloqueia a thread invocante até que exista uma unidade de conclusão 
            disponível, e pode terminar: com sucesso por ter sido satisfeita a condição de bloqueio, retornando 
            true; produzindo ThreadInterruptedException caso a thread seja interrompida enquanto está 
            bloqueada no monitor, ou; retornando false se o tempo máximo de espera (timeout) foi atingido. 
            O sincronizador inclui ainda a operação CompleteAll que o coloca permanentemente no estado 
            sinalizado, ou seja, são viabilizadas todas as chamadas, anteriores ou posteriores, ao método 
            WaitForCompletion
     * */
    class Ex1Completion
    {
        private bool _signaled;
        private int _permits;

        public Ex1Completion(int permits, bool signaled)
        {
            if (permits > 0) _permits = permits;
            _signaled = false;  // Starts unsignaled
        }

        // Bloqueia a Thread invocante até que exista uma unidade de conclusão disponível.
        // Pode terminar:   com sucesso por ter sido satisfeita a condição de bloqueiro, retornando true.
          //                retornando false, caso o tempo máximo de espera (timeout) tenha sido atingido
        public bool WaitForCompletion(int timeout)
        {
            lock (this)
            {
                if (_signaled)      // Signaled to please all
                    return true;

                if (timeout == 0)   // Timeout
                    return false;

                if (_permits >= 1)  // Verify if there are enough permits to "Acquire"
                {
                    _permits--;
                    return true;
                }

                int initialTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0; // Get the current time
                do
                {
                    try  // Wait for more permits...
                    {
                        Monitor.Wait(this, timeout);
                    }
                    catch (ThreadInterruptedException iex)
                    {
                        if(_permits >= 1)   // If there were enough permits, "Pulse", so that another Thread will have the chance to "Acquire"!
                            Monitor.Pulse(this);

                        throw;  // Rethrow the exception
                    }

                    if (_signaled || _permits >= 1)  // Verify if there are enough permits to "Acquire" or if it's Signaled to please all
                    {
                        _permits--;
                        return true;
                    }

                    timeout = SyncUtils.AdjustTimeout(ref initialTime, ref timeout); // Update the time

                } while (timeout>0);
            }
            
        }

        // sinaliza a conclusão de uma tarefa e viabiliza a execução de uma chamada ao WaitForCompletion
        public void Complete()
        {
            lock (this)
            {
//                // If "signaled" then we don't need to count the permits - We notify all the threads!
//                if (_signaled)
//                {
//                    Monitor.PulseAll(this);
//                    return;
//                }

                // A Task was concluded, so we have another permit available!
                _permits++;
                
                // Now let's notify one of the waiting threads
                Monitor.Pulse(this);
            }
        }

        // Coloca o sincronizador no estado sinalizado, permanentemente. Ou seja, são viabilizadas todas as chamadas,
        // anteriores ou posteriores, ao WaitForCompletion
        public void CompleteAll()
        {
            lock (this)
            {
                // Put the synchronizer in "signaled" state and notify all the waiting threads!
                _signaled = true;
                Monitor.PulseAll(this);
            }
        }
    }
}
