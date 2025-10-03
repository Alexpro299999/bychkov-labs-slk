using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.WriteLine("\n--- Главное меню ---");
                Console.WriteLine("1. Задание 1: Вычисление функций f и g");
                Console.WriteLine("2. Задание 2: Меню с векторами и матрицами");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите пункт меню: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        RunTask1();
                        break;
                    case "2":
                        RunTask2();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Ошибка: Неверный пункт меню. Попробуйте еще раз.");
                        break;
                }
            }
        }

        public static void RunTask1()
        {
            try
            {
                Console.WriteLine("\n--- Задание 1: Вычисление функций ---");
                Console.Write("Введите xn (начало диапазона x): ");
                double xn = double.Parse(Console.ReadLine());
                Console.Write("Введите xk (конец диапазона x, xk > xn): ");
                double xk = double.Parse(Console.ReadLine());
                Console.Write("Введите h (шаг для x, h > 0): ");
                double h = double.Parse(Console.ReadLine());

                Console.Write("Введите yn (начало диапазона y): ");
                double yn = double.Parse(Console.ReadLine());
                Console.Write("Введите yk (конец диапазона y, yk > yn): ");
                double yk = double.Parse(Console.ReadLine());
                Console.Write("Введите t (шаг для y, t > 0): ");
                double t = double.Parse(Console.ReadLine());

                if (h <= 0 || t <= 0 || xk <= xn || yk <= yn)
                {
                    Console.WriteLine("Ошибка: неверно заданы диапазоны или шаги.");
                    return;
                }

                CalculateAndPrintFunctions(xn, xk, h, yn, yk, t);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода. Пожалуйста, вводите числовые значения.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
        }

        private static void CalculateAndPrintFunctions(double xn, double xk, double h, double yn, double yk, double t)
        {
            Console.WriteLine("\n" + new string('-', 49));
            Console.WriteLine("| {0,-10} | {1,-10} | {2,-10} | {3,-10} |", "x", "y", "f", "g");
            Console.WriteLine(new string('-', 49));

            List<string> errors = new List<string>();

            double y = yn;
            for (double x = xn; x <= xk; x += h)
            {
                if (y > yk) y = yn;

                string f_str, g_str;

                try
                {
                    double f_val = CalculateF(x, y);
                    f_str = f_val.ToString("F3");
                }
                catch (ArgumentException ex)
                {
                    f_str = "не опр.";
                    errors.Add($"Для (x={x:F2}, y={y:F2}): {ex.Message}");
                }

                try
                {
                    double g_val = CalculateG(x, y);
                    g_str = g_val.ToString("F3");
                }
                catch (ArgumentException ex)
                {
                    g_str = "не опр.";
                    if (!errors.Any(e => e.StartsWith($"Для (x={x:F2}, y={y:F2})")))
                    {
                        errors.Add($"Для (x={x:F2}, y={y:F2}): {ex.Message}");
                    }
                }

                Console.WriteLine("| {0,-10:F2} | {1,-10:F2} | {2,-10} | {3,-10} |", x, y, f_str, g_str);

                y += t;
            }
            Console.WriteLine(new string('-', 49));

            if (errors.Count > 0)
            {
                Console.WriteLine("\n--- Сообщения об ошибках ---");
                foreach (var err in errors)
                {
                    Console.WriteLine(err);
                }
            }
        }

        private static double CalculateF(double x, double y)
        {
            double xy = x * y;
            if (xy < 1)
            {
                if (Math.Abs(1 - xy) < 1e-9)
                    throw new ArgumentException("Функция f не определена (деление на 0)");
                return (x + y) / (1 - xy);
            }
            if (xy >= 1 && xy <= 8)
            {
                return Math.Sqrt(Math.Abs(Math.Pow(x, 3) + Math.Pow(y, 3)));
            }

            if (Math.Abs(12 + x + xy) < 1e-9)
                throw new ArgumentException("Функция f не определена (деление на 0)");
            return (x + y) / (12 + x + xy);
        }

        private static double CalculateG(double x, double y)
        {
            double xy = x * y;
            if (xy < 1)
            {
                return Math.Pow(x, 2) - Math.Pow(y, 2);
            }
            if (xy >= 1 && xy <= 8)
            {
                if (Math.Abs(1 + Math.Pow(y, 2)) < 1e-9)
                    throw new ArgumentException("Функция g не определена (деление на 0)");
                return (y - x) / (1 + Math.Pow(y, 2));
            }

            return Math.Sqrt(Math.Abs(3.14 - xy) + Math.Pow(y, 2));
        }

        public static void RunTask2()
        {
            while (true)
            {
                Console.WriteLine("\n--- Задание 2: Меню ---");
                Console.WriteLine("1. Вектор: Найти макс. четный элемент до первого нечетного");
                Console.WriteLine("2. Матрица: Построить вектор из кол-ва четных элементов в столбцах");
                Console.WriteLine("3. Матрица: Поменять местами главную и побочную диагонали");
                Console.WriteLine("4. Назад в главное меню");
                Console.Write("Выберите пункт: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        HandleVectorTask();
                        break;
                    case "2":
                        HandleMatrixToVectorTask();
                        break;
                    case "3":
                        HandleMatrixSwapTask();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Ошибка: Неверный пункт меню.");
                        break;
                }
            }
        }

        private static void HandleVectorTask()
        {
            try
            {
                Console.WriteLine("\n--- Задача 1: Поиск в векторе ---");
                int[] vector = InputHelper.ReadVector("Введите элементы вектора через пробел:");

                Console.WriteLine("Исходный вектор:");
                OutputHelper.PrintVector(vector);

                if (FindMaxEvenBeforeFirstOdd(vector, out int maxElement))
                {
                    Console.WriteLine($"Результат: Максимальный четный элемент до первого нечетного = {maxElement}");
                }
                else
                {
                    Console.WriteLine("Результат: Четные элементы до первого нечетного не найдены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void HandleMatrixToVectorTask()
        {
            try
            {
                Console.WriteLine("\n--- Задача 2: Анализ столбцов матрицы ---");
                int[,] matrix = InputHelper.ReadSquareMatrix("Введите размерность квадратной матрицы n:");

                Console.WriteLine("Исходная матрица:");
                OutputHelper.PrintMatrix(matrix);

                int[] resultVector = CountEvenInColumns(matrix);

                Console.WriteLine("Результирующий вектор b (количество четных элементов в столбцах):");
                OutputHelper.PrintVector(resultVector);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void HandleMatrixSwapTask()
        {
            try
            {
                Console.WriteLine("\n--- Задача 3: Замена диагоналей в матрице ---");
                int[,] matrix = InputHelper.ReadSquareMatrix("Введите размерность квадратной матрицы n:");

                Console.WriteLine("Исходная матрица:");
                OutputHelper.PrintMatrix(matrix);

                SwapDiagonals(ref matrix);

                Console.WriteLine("Матрица после замены диагоналей:");
                OutputHelper.PrintMatrix(matrix);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static bool FindMaxEvenBeforeFirstOdd(int[] vector, out int max)
        {
            max = 0;
            bool foundEven = false;
            foreach (int item in vector)
            {
                if (item % 2 != 0)
                {
                    break;
                }

                if (!foundEven)
                {
                    max = item;
                    foundEven = true;
                }
                else if (item > max)
                {
                    max = item;
                }
            }
            return foundEven;
        }

        private static int[] CountEvenInColumns(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[] b = new int[n];

            for (int j = 0; j < n; j++)
            {
                int count = 0;
                for (int i = 0; i < n; i++)
                {
                    if (matrix[i, j] % 2 == 0)
                    {
                        count++;
                    }
                }
                b[j] = count;
            }
            return b;
        }

        private static void SwapDiagonals(ref int[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                int mainDiagElement = matrix[i, i];
                int antiDiagElement = matrix[i, n - 1 - i];

                matrix[i, i] = antiDiagElement;
                matrix[i, n - 1 - i] = mainDiagElement;
            }
        }

        static class InputHelper
        {
            public static int[] ReadVector(string message)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    throw new FormatException("Введена пустая строка.");

                return input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();
            }

            public static int[,] ReadSquareMatrix(string message)
            {
                Console.WriteLine(message);
                int n = int.Parse(Console.ReadLine());
                if (n <= 0) throw new ArgumentException("Размерность матрицы должна быть положительной.");

                int[,] matrix = new int[n, n];
                Console.WriteLine("Введите элементы матрицы построчно (числа через пробел):");
                for (int i = 0; i < n; i++)
                {
                    int[] row = ReadVector($"Строка {i + 1}:");
                    if (row.Length != n) throw new ArgumentException($"Ошибка: в строке {i + 1} должно быть {n} элементов.");
                    for (int j = 0; j < n; j++)
                    {
                        matrix[i, j] = row[j];
                    }
                }
                return matrix;
            }
        }

        static class OutputHelper
        {
            public static void PrintVector(int[] vector)
            {
                Console.WriteLine(string.Join(" ", vector));
            }

            public static void PrintMatrix(int[,] matrix)
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write($"{matrix[i, j],-5}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}