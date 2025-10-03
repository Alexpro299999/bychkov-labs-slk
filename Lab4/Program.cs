using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

public class Train
{
    public string NumberTrain { get; set; }
    public string DepartureStation { get; set; }
    public string DepartureTime { get; set; }
}

public class Program
{
    public static string ReplaceIngWithEd(string inputText)
    {
        if (string.IsNullOrWhiteSpace(inputText))
        {
            return inputText;
        }
        return Regex.Replace(inputText, @"(?i)\b(\w+)ing\b", "$1ed");
    }

    public static void RunStringTask()
    {
        Console.WriteLine("\n--- Задание 1: Замена ING на ED ---");
        Console.WriteLine("Введите текст для обработки:");
        string userInput = Console.ReadLine();
        string result = ReplaceIngWithEd(userInput);
        Console.WriteLine("\nРезультат:");
        Console.WriteLine(result);
    }

    public static string FilterLinesByConsonantEndingWords(string fileContent)
    {
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return "";
        }

        const string vowels = "aeiouyаеёиоуыэюя";
        var resultLines = new List<string>();
        string[] allLines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in allLines)
        {
            string[] words = line.Split(new[] { ' ', '\t', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) continue;

            bool allWordsEndWithConsonant = true;
            foreach (var word in words)
            {
                char lastChar = word.LastOrDefault();
                if (!char.IsLetter(lastChar) || vowels.Contains(char.ToLower(lastChar)))
                {
                    allWordsEndWithConsonant = false;
                    break;
                }
            }

            if (allWordsEndWithConsonant)
            {
                resultLines.Add(line);
            }
        }
        return string.Join(Environment.NewLine, resultLines);
    }

    public static void RunTextFileTask()
    {
        Console.WriteLine("\n--- Задание 2: Фильтрация строк в файле ---");
        string inputFileName = "source_text.txt";
        string outputFileName = "result_text.txt";

        string sampleText = "This is a test sentence for checking.\n" +
                            "Work is starting now.\n" +
                            "Every word must end with consonant.\n" +
                            "Этот кот любит спать.\n" +
                            "Один два три.\n" +
                            "Мой дом стоит тут.";
        File.WriteAllText(inputFileName, sampleText, Encoding.UTF8);
        Console.WriteLine($"Создан демонстрационный файл '{inputFileName}'.");

        string content = File.ReadAllText(inputFileName, Encoding.UTF8);
        string result = FilterLinesByConsonantEndingWords(content);
        File.WriteAllText(outputFileName, result, Encoding.UTF8);

        Console.WriteLine($"Результат записан в файл '{outputFileName}'.");
    }

    public static void CreateSampleJsonFile(string filePath)
    {
        if (File.Exists(filePath)) return;

        var trains = new List<Train>
        {
            new Train { NumberTrain = "729М", DepartureStation = "Москва", DepartureTime = "08:30" },
            new Train { NumberTrain = "015А", DepartureStation = "Санкт-Петербург", DepartureTime = "12:45" },
            new Train { NumberTrain = "112П", DepartureStation = "Москва", DepartureTime = "15:00" },
            new Train { NumberTrain = "035Г", DepartureStation = "Нижний Новгород", DepartureTime = "21:10" },
            new Train { NumberTrain = "001А", DepartureStation = "Санкт-Петербург", DepartureTime = "23:55" }
        };

        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        string jsonString = JsonSerializer.Serialize(trains, options);
        File.WriteAllText(filePath, jsonString, Encoding.UTF8);
        Console.WriteLine($"Создан демонстрационный файл '{filePath}'.");
    }

    public static void ProcessTrainJsonFile(string jsonFilePath)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string trainsDirectoryPath = Path.Combine(desktopPath, "Trains");
        Directory.CreateDirectory(trainsDirectoryPath);

        if (!File.Exists(jsonFilePath))
        {
            Console.WriteLine($"Ошибка: Файл {jsonFilePath} не найден.");
            return;
        }
        string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);
        List<Train> allTrains = JsonSerializer.Deserialize<List<Train>>(jsonContent);

        if (allTrains == null || allTrains.Count == 0)
        {
            Console.WriteLine("В файле нет данных о поездах.");
            return;
        }

        var trainsByStation = allTrains.GroupBy(train => train.DepartureStation);

        foreach (var group in trainsByStation)
        {
            string stationName = group.Key;
            string stationFilePath = Path.Combine(trainsDirectoryPath, $"{stationName}.txt");
            var linesForFile = group.Select(train => $"{train.NumberTrain}, {train.DepartureTime}").ToList();
            File.WriteAllLines(stationFilePath, linesForFile, Encoding.UTF8);
        }
    }

    public static void RunJsonTask()
    {
        Console.WriteLine("\n--- Задание 3: Обработка JSON-файла с поездами ---");
        string jsonFileName = "trains.json";

        CreateSampleJsonFile(jsonFileName);
        ProcessTrainJsonFile(jsonFileName);

        string trainsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Trains");
        Console.WriteLine($"Операция завершена. Проверьте папку 'Trains' на вашем рабочем столе: {trainsFolderPath}");
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n--- Главное меню ---");
            Console.WriteLine("1. Строки (замена ING на ED)");
            Console.WriteLine("2. Текстовый файл (поиск строк по условию)");
            Console.WriteLine("3. Json файл (обработка данных о поездах)");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите пункт меню: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RunStringTask();
                    break;
                case "2":
                    RunTextFileTask();
                    break;
                case "3":
                    RunJsonTask();
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