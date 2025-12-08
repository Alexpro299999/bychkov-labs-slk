using System;
using System.Threading;

namespace Lab9
{
    public class DiningSavages
    {
        private static int _servings;
        private static int _potCapacity;
        private static Semaphore _mutex;
        private static Semaphore _emptyPot;
        private static Semaphore _fullPot;
        private static volatile bool _isRunning = true;

        public static void Run(int n, int m)
        {
            _potCapacity = m;
            _servings = m;
            _isRunning = true;

            _mutex = new Semaphore(1, 1);
            _emptyPot = new Semaphore(0, n); 
            _fullPot = new Semaphore(0, 1);

            Thread cookThread = new Thread(Cook);
            cookThread.Start();

            Thread[] savages = new Thread[n];
            for (int i = 0; i < n; i++)
            {
                savages[i] = new Thread(Savage);
                savages[i].Name = $"Дикарь {i + 1}";
                savages[i].Start();
            }

            Thread.Sleep(3000);

            Console.WriteLine("\n--- Остановка задачи 1 (ожидание завершения потоков) ---\n");
            _isRunning = false;

            SafeRelease(_mutex);
            try { _emptyPot.Release(n); } catch { }
            try { _fullPot.Release(); } catch { }

            cookThread.Join();
            for (int i = 0; i < n; i++)
            {
                savages[i].Join();
            }
        }

        private static void Savage()
        {
            while (_isRunning)
            {
                _mutex.WaitOne();

                if (!_isRunning) { SafeRelease(_mutex); break; }

                if (_servings == 0)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} видит пустой горшок и будит повара.");
                    SafeRelease(_emptyPot);
                    _fullPot.WaitOne();
                }

                if (!_isRunning) { SafeRelease(_mutex); break; }

                _servings--;
                Console.WriteLine($"{Thread.CurrentThread.Name} обедает. Кусков осталось: {_servings}");

                SafeRelease(_mutex);
                Thread.Sleep(100);
            }
        }

        private static void Cook()
        {
            while (_isRunning)
            {
                _emptyPot.WaitOne();
                if (!_isRunning) break;

                Console.WriteLine("Повар наполняет горшок...");
                Thread.Sleep(500);
                _servings = _potCapacity;
                Console.WriteLine("Горшок полон.");

                SafeRelease(_fullPot);
            }
        }

        private static void SafeRelease(Semaphore sem)
        {
            try { sem.Release(); } catch (SemaphoreFullException) { }
        }
    }
}