using System;
using System.Collections.Generic;
using System.Linq;

// Класс 1: Абонент
public class Subscriber
{
    public string FullName { get; set; }
    public decimal Balance { get; set; }
    public string TariffName { get; set; }

    public override string ToString()
    {
        return $"Абонент: {FullName}, Баланс: {Balance:C}, Тариф: {TariffName}";
    }
}

// Класс 2: Тариф
public class Tariff
{
    public string Name { get; set; }
    public int SpeedMbps { get; set; }
    public decimal MonthlyCost { get; set; }

    public override string ToString()
    {
        return $"Тариф: '{Name}', Скорость: {SpeedMbps} Мбит/с, Цена: {MonthlyCost:C}/мес.";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // ИСХОДНЫЕ ДАННЫЕ 

        var tariffs = new List<Tariff>
        {
            new Tariff { Name = "Базовый", SpeedMbps = 50, MonthlyCost = 500 },
            new Tariff { Name = "Стандарт", SpeedMbps = 100, MonthlyCost = 750 },
            new Tariff { Name = "Премиум", SpeedMbps = 500, MonthlyCost = 1200 },
            new Tariff { Name = "Социальный", SpeedMbps = 10, MonthlyCost = 300 }
        };

        var subscribers = new List<Subscriber>
        {
            new Subscriber { FullName = "Иванов Иван Иванович", Balance = 1500, TariffName = "Стандарт" },
            new Subscriber { FullName = "Петров Петр Петрович", Balance = -200, TariffName = "Базовый" },
            new Subscriber { FullName = "Сидорова Анна Викторовна", Balance = 5000, TariffName = "Премиум" },
            new Subscriber { FullName = "Кузнецова Мария Львовна", Balance = 0, TariffName = "Стандарт" },
            new Subscriber { FullName = "Васильев Сергей Евгеньевич", Balance = 800, TariffName = "Базовый" },
            new Subscriber { FullName = "Павлова Ольга Игоревна", Balance = -50, TariffName = "Премиум" }
        };

        Console.WriteLine("--- ИСХОДНЫЕ СПИСКИ ---");
        Console.WriteLine("\n--- Список тарифов ---");
        tariffs.ForEach(Console.WriteLine);
        Console.WriteLine("\n--- Список абонентов ---");
        subscribers.ForEach(Console.WriteLine);
        Console.WriteLine("\n" + new string('=', 50));


        // 1. Фильтрация по одному критерию (LINQ-запрос)
        Console.WriteLine("\n1. Абоненты с отрицательным балансом:");
        var debtors = from s in subscribers
                      where s.Balance < 0
                      select s;
        debtors.ToList().ForEach(Console.WriteLine);

        // 2. Фильтрация по двум критериям, один от пользователя (LINQ-запрос)
        Console.WriteLine("\n2. Поиск абонентов по тарифу с положительным балансом.");
        Console.Write("Введите название тарифа (например, 'Стандарт'): ");
        string tariffInput = Console.ReadLine();
        var filteredSubscribers = from s in subscribers
                                  where s.TariffName.Equals(tariffInput, StringComparison.OrdinalIgnoreCase) && s.Balance > 0
                                  select s;
        Console.WriteLine("Результат:");
        filteredSubscribers.ToList().ForEach(Console.WriteLine);

        // 3. Сортировка (метод расширения)
        Console.WriteLine("\n3. Абоненты, отсортированные по ФИО:");
        var sortedSubscribers = subscribers.OrderBy(s => s.FullName);
        sortedSubscribers.ToList().ForEach(Console.WriteLine);

        // 4. Получение размера выборки (метод расширения)
        Console.WriteLine("\n4. Количество абонентов на тарифе 'Премиум':");
        int premiumCount = subscribers.Count(s => s.TariffName == "Премиум");
        Console.WriteLine($"Результат: {premiumCount}");

        // 5. Использование Max(), Average(), Sum() (методы расширения)
        Console.WriteLine("\n5. Агрегатные данные по балансам абонентов:");
        decimal maxBalance = subscribers.Max(s => s.Balance);
        decimal avgBalance = subscribers.Average(s => s.Balance);
        decimal sumBalance = subscribers.Sum(s => s.Balance);
        Console.WriteLine($"Максимальный баланс: {maxBalance:C}");
        Console.WriteLine($"Средний баланс: {avgBalance:C}");
        Console.WriteLine($"Суммарный баланс: {sumBalance:C}");

        // 6. Использование оператора let (LINQ-запрос)
        Console.WriteLine("\n6. Стоимость 1 Мбит/с для каждого тарифа:");
        var costPerMbps = from t in tariffs
                          let cost = t.MonthlyCost / t.SpeedMbps
                          select new { Tariff = t.Name, CostPerMbps = cost };
        costPerMbps.ToList().ForEach(item => Console.WriteLine($"Тариф '{item.Tariff}': {item.CostPerMbps:C} за 1 Мбит/с"));

        // 7. Группировка по одному полю (LINQ-запрос)
        Console.WriteLine("\n7. Группировка абонентов по тарифам:");
        var subscribersByTariff = from s in subscribers
                                  group s by s.TariffName;
        foreach (var group in subscribersByTariff)
        {
            Console.WriteLine($"Тариф: {group.Key}");
            foreach (var subscriber in group)
            {
                Console.WriteLine($"  - {subscriber.FullName}");
            }
        }

        // 8. Использование метода Join (метод расширения)
        Console.WriteLine("\n8. Список 'Абонент - Скорость его тарифа':");
        var subscriberWithSpeed = subscribers.Join(tariffs,
            sub => sub.TariffName,
            tar => tar.Name,
            (sub, tar) => new {
                SubscriberName = sub.FullName,
                Speed = tar.SpeedMbps
            });
        subscriberWithSpeed.ToList().ForEach(item => Console.WriteLine($"{item.SubscriberName} - {item.Speed} Мбит/с"));

        // 9. Использование метода GroupJoin (LINQ-запрос)
        Console.WriteLine("\n9. Список тарифов и всех их абонентов:");
        var tariffsWithSubscribers = from t in tariffs
                                     join s in subscribers on t.Name equals s.TariffName into subsOfTariff
                                     select new
                                     {
                                         TariffName = t.Name,
                                         Subscribers = subsOfTariff
                                     };
        foreach (var item in tariffsWithSubscribers)
        {
            Console.WriteLine($"Тариф: {item.TariffName}");
            if (item.Subscribers.Any())
            {
                foreach (var sub in item.Subscribers)
                {
                    Console.WriteLine($"  - {sub.FullName}");
                }
            }
            else
            {
                Console.WriteLine("  - (нет абонентов)");
            }
        }

        // 10. Использование метода All (метод расширения)
        Console.WriteLine("\n10. Проверка: все ли абоненты имеют баланс больше -500?");
        bool allHavePositiveBalance = subscribers.All(s => s.Balance > -500);
        Console.WriteLine($"Результат: {allHavePositiveBalance}");

        // 11. Использование метода Any (метод расширения)
        Console.WriteLine("\n11. Проверка: есть ли хотя бы один абонент на тарифе 'Социальный'?");
        bool anyOnSocial = subscribers.Any(s => s.TariffName == "Социальный");
        Console.WriteLine($"Результат: {anyOnSocial}");

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}