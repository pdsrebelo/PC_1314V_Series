using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_1.Pedro
{
    /// <summary>
    /// Usando monitores intrínsecos CLI, implemente o sincronizador Completion, que representa um 
    /// gestor de unidades que indicam a conclusão de tarefas.
    /// </summary>
    class Ex1_Completion
    {
        private int _permits; // if permits == -1, then everyone has access to the synchronizer

        public Ex1_Completion(int permits)
        {
            if (_permits > 0)
                _permits = permits;
        }

        /// <summary>
        /// A operação Complete sinaliza a conclusão de uma tarefa e viabiliza a 
        /// execução de exatamente uma chamada ao método WaitForCompletion.
        /// </summary>
        public void Complete()
        {
            lock (this)
            {
                _permits ++;
                Monitor.Pulse(this);
            }
        }

        /// <summary>
        /// A operação WaitForCompletion bloqueia a thread invocante até que exista uma unidade de conclusão 
        /// disponível, e pode terminar: com sucesso por ter sido satisfeita a condição de bloqueio, retornando
        /// true; produzindo ThreadInterruptedException caso a thread seja interrompida enquanto está 
        /// bloqueada no monitor, ou; retornando false se o tempo máximo de espera (timeout) foi atingido.
        /// </summary>
        /// <returns></returns>
        public bool WaitForCompletion(int timeout)
        {
            lock (this)
            {
                if (_permits == -1 || _permits > 0)
                {
                    _permits--;
                    return true;
                }

                if (timeout == 0) return false;

                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;

                do
                {
                    try
                    {
                        Monitor.Wait(this, timeout);
                    }
                    catch (ThreadInterruptedException)
                    {
                        Monitor.Pulse(this);
                        throw;
                    }
                    if (_permits == -1 || _permits > 0) return true;

                    if (SyncUtils.AdjustTimeout(ref lastTime, ref timeout) == 0) return false;

                } while (true);
            }
        }

        /// <summary>
        /// O sincronizador inclui ainda a operação CompleteAll que o coloca permanentemente no estado 
        /// sinalizado, ou seja, são viabilizadas todas as chamadas, anteriores ou posteriores, ao método 
        /// WaitForCompletion.
        /// </summary>
        public void CompleteAll()
        {
            _permits = -1;
            Monitor.PulseAll(this);
        }
    }
}