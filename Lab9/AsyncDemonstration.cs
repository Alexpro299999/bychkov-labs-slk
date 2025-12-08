using System;
using System.Threading.Tasks;

namespace Lab9
{
    public class AsyncDemonstration
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("Main: Запуск асинхронной операции...");
            await DoWorkAsync();
            Console.WriteLine("Main: Асинхронная операция завершена.");
        }

        private static async Task DoWorkAsync()
        {
            Console.WriteLine("AsyncMethod: Работа началась.");
            await Task.Delay(2000);
            Console.WriteLine("AsyncMethod: Работа завершена после задержки.");
        }
    }
}