using System;
using System.Threading;

namespace Lab9
{
    public class PingPongEvents
    {
        private static AutoResetEvent _event1 = new AutoResetEvent(true);
        private static AutoResetEvent _event2 = new AutoResetEvent(false);
        private static bool _running = true;

        public static void Run()
        {
            _running = true;
            Thread t1 = new Thread(Worker1);
            Thread t2 = new Thread(Worker2);

            t1.Start();
            t2.Start();

            Thread.Sleep(3000);
            _running = false;

            _event1.Set();
            _event2.Set();

            t1.Join();
            t2.Join();
        }

        private static void Worker1()
        {
            while (_running)
            {
                _event1.WaitOne();
                if (!_running) break;

                Console.WriteLine("Поток 1: Пинг");
                Thread.Sleep(300);
                _event2.Set();
            }
        }

        private static void Worker2()
        {
            while (_running)
            {
                _event2.WaitOne();
                if (!_running) break;

                Console.WriteLine("Поток 2: Понг");
                Thread.Sleep(300);
                _event1.Set();
            }
        }
    }
}