using System;
using System.Threading.Tasks;

namespace Lab9
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Задание 1: Обед дикарей ===");
            DiningSavages.Run(5, 3);

            Console.WriteLine("\n=== Задание 2.1: Async/Await ===");
            await AsyncDemonstration.RunAsync();

            Console.WriteLine("\n=== Задание 2.2: AutoResetEvent (Пинг-Понг) ===");
            PingPongEvents.Run();

            Console.WriteLine("\nВсе задания выполнены. Нажмите Enter для выхода.");
            Console.ReadLine();
        }
    }
}