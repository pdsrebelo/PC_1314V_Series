using System;
using System.Threading;

namespace Serie_2.Catia
{

    class SyncUtils
    {
        private static void EnterUninterruptible(object mlock, out bool interrupted)
        {
            interrupted = false;
            do
            {
                try
                {
                    Monitor.Enter(mlock);
                    break;
                }
                catch (ThreadInterruptedException tie)
                {
                    interrupted = true;
                }
            } while (true);
        }

        public static void Wait(object mlock, object condition, int timeout)
        {
            if (mlock == condition)
            {
                Monitor.Wait(mlock, timeout);
                return;
            }
            Monitor.Enter(condition);
            Monitor.Exit(mlock); // sair de mlock só depois de entrar em condition,para ter a certeza de que faz Enter no certo!
            try
            {
                Monitor.Wait(condition, timeout);
            }
            finally
            {
                Monitor.Exit(mlock);
                bool interrupted = false;
                EnterUninterruptible(mlock, out interrupted);

                if (interrupted)
                {
                    throw new ThreadInterruptedException();
                }
            }
        }

        public static void Notify(Object mLock, Object condition)
        {
            if (mLock == condition)
            {
                Monitor.Pulse(mLock);
                return;
            }

            bool interrupted;

            EnterUninterruptible(condition, out interrupted);
            Monitor.Pulse(condition);
            Monitor.Exit(condition);

            if (interrupted)
                Thread.CurrentThread.Interrupt();
        }

        public static void Broadcast(Object mLock, Object condition)
        {
            if (mLock == condition)
            {
                Monitor.PulseAll(mLock);
                return;
            }

            bool interrupted;

            EnterUninterruptible(condition, out interrupted);
            Monitor.PulseAll(condition);
            Monitor.Exit(condition);

            if (interrupted)
                Thread.CurrentThread.Interrupt();
        }

        public static int AdjustTimeout(ref int lastTime, ref int timeout)
        {
            if (timeout != Timeout.Infinite)
            {
                int now = Environment.TickCount;

                int elapsed = (now > lastTime) ? 1 : (now - lastTime);

                if (elapsed >= timeout)
                    timeout = 0;
                else
                {
                    timeout -= elapsed;
                    lastTime = now;
                }
            }
            return timeout;
        }
    }
}
