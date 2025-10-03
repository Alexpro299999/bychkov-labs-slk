using System;
using System.Linq;

namespace DelegateAndEventsLab
{
    public class InvalidChoiceException : Exception
    {
        public InvalidChoiceException(string message) : base(message)
        {
        }
    }

    public class VectorEventManager
    {
        public delegate void VectorActionHandler(int[] vector, int choice);
        public event VectorActionHandler ActionRequested;

        public void ProcessUserInput(int[] vector)
        {
            Console.WriteLine("\n--- Задание 3: События и исключения ---");
            Console.WriteLine("1. Подсчет суммы отрицательных элементов");
            Console.WriteLine("2. Подсчет количества нечетных элементов");
            Console.Write("Введите ваш выбор (1 или 2): ");

            try
            {
                int choice = int.Parse(Console.ReadLine());
                if (choice != 1 && choice != 2)
                {
                    throw new InvalidChoiceException("Ошибка: необходимо ввести 1 или 2.");
                }
                ActionRequested?.Invoke(vector, choice);
            }
            catch (FormatException)
            {
                Console.WriteLine("Критическая ошибка: Введено не число!");
            }
            catch (InvalidChoiceException ex)
            {
                Console.WriteLine($"Ошибка пользовательского ввода: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Блок обработки ввода завершил работу.");
            }
        }
    }

    class Program
    {
        public delegate double FuncDelegate(double x, double y);

        public static double FunctionF(double x, double y)
        {
            return 13 * x * y;
        }

        public static double FunctionG(double x, double y)
        {
            return 4 * Math.Pow(x, 2) * y;
        }

        public static double CalculateZ(double x, double y, FuncDelegate func)
        {
            return 3 - func(2 * x, y) + func(x, y + 5);
        }

        public static void RunTask1()
        {
            Console.WriteLine("\n--- Задание 1: Использование делегатов ---");
            try
            {
                Console.Write("Введите значение x: ");
                double x = double.Parse(Console.ReadLine());
                Console.Write("Введите значение y: ");
                double y = double.Parse(Console.ReadLine());

                Console.Write("Какую функцию использовать для подсчета z (1 - f(x,y), 2 - g(x,y))? ");
                string choice = Console.ReadLine();

                double result;
                if (choice == "1")
                {
                    result = CalculateZ(x, y, FunctionF);
                    Console.WriteLine($"Результат z с использованием f(x,y): {result}");
                }
                else if (choice == "2")
                {
                    result = CalculateZ(x, y, FunctionG);
                    Console.WriteLine($"Результат z с использованием g(x,y): {result}");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный выбор.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода. Пожалуйста, вводите числа.");
            }
        }

        public static int SumByCondition(int[] array, Predicate<int> condition)
        {
            int sum = 0;
            foreach (int number in array)
            {
                if (condition(number))
                {
                    sum += number;
                }
            }
            return sum;
        }

        public static int CountByCondition(int[] array, Predicate<int> condition)
        {
            int count = 0;
            foreach (int number in array)
            {
                if (condition(number))
                {
                    count++;
                }
            }
            return count;
        }

        public static void RunTask2()
        {
            Console.WriteLine("\n--- Задание 2: Лямбда-выражения ---");
            int[] vector = { 5, -12, 8, -3, 0, 15, -4, 7, 91, -20 };
            Console.WriteLine($"Исходный вектор: [{string.Join(", ", vector)}]");

            Console.WriteLine("1. Подсчет суммы отрицательных элементов");
            Console.WriteLine("2. Подсчет количества нечетных элементов");
            Console.Write("Введите ваш выбор (1 или 2): ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                int sum = SumByCondition(vector, x => x < 0);
                Console.WriteLine($"Сумма отрицательных элементов: {sum}");
            }
            else if (choice == "2")
            {
                int count = CountByCondition(vector, x => x % 2 != 0);
                Console.WriteLine($"Количество нечетных элементов: {count}");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный выбор.");
            }
        }

        public static void OnActionRequested(int[] vector, int choice)
        {
            Console.WriteLine("Событие получено! Выполняется действие...");
            switch (choice)
            {
                case 1:
                    int sum = SumByCondition(vector, x => x < 0);
                    Console.WriteLine($"Сумма отрицательных элементов: {sum}");
                    break;
                case 2:
                    int count = CountByCondition(vector, x => x % 2 != 0);
                    Console.WriteLine($"Количество нечетных элементов: {count}");
                    break;
            }
        }

        public static void RunTask3()
        {
            int[] vector = { 5, -12, 8, -3, 0, 15, -4, 7, 91, -20 };
            Console.WriteLine($"Исходный вектор: [{string.Join(", ", vector)}]");

            var eventManager = new VectorEventManager();
            eventManager.ActionRequested += OnActionRequested;
            eventManager.ProcessUserInput(vector);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n--- Главное меню ---");
                Console.WriteLine("1. Задание 1 (Делегаты)");
                Console.WriteLine("2. Задание 2 (Лямбда-выражения)");
                Console.WriteLine("3. Задание 3 (События и исключения)");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите задание: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        RunTask1();
                        break;
                    case "2":
                        RunTask2();
                        break;
                    case "3":
                        RunTask3();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                        break;
                }
            }
        }
    }
}