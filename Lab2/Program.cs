using System;

// Пункт 19: В функции Main демонстрируется использование всех разработанных элементов.
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("--- Демонстрация работы системы 'Книжный магазин' ---\n");

        // 1. Создание авторов и книг (Классы: Author, Book, Publication)
        var author1 = new Author("Лев", "Толстой"); // Пункт 2 (конструктор)
        var author2 = new Author("Фёдор", "Достоевский");

        // Пункт 9 (наследование), 10 (переопределение), 18 (интерфейс)
        var book1 = new Book("Война и мир", author1, "978-5-04-116810-9", 1500.50m);
        var book2 = new Book("Преступление и наказание", author2, "978-5-389-04932-3", -100); // Пункт 4 (логика в свойстве)
        book2.Price = 750.00m; 

        Console.WriteLine("Информация о книгах:");
        Console.WriteLine(book1.GetDescription());
        Console.WriteLine(book2.GetDescription());
        Console.WriteLine();

        // 2. Демонстрация обобщений и наследования обобщений (Inventory<T>, BookInventory)
        var bookInventory = new BookInventory(); // Пункт 14 (наследование обобщений)

        // 3. Демонстрация добавления элементов в инвентарь
        bookInventory.Add(book1);
        bookInventory.Add(book2);

        Console.WriteLine($"В инвентаре теперь {bookInventory.Count} книги.");

        // 4. Демонстрация индексатора в Inventory
        Console.WriteLine($"Первая книга в инвентаре: {bookInventory[0].Title}"); // Пункт 5
        Console.WriteLine();

        // 5. Создание магазина (Агрегация и Композиция)
        // Пункт 16 (Агрегация - передаем готовый инвентарь)
        // Пункт 17 (Композиция - Financials создается внутри)
        var myBookstore = new Bookstore("Читай-Город", bookInventory);

        // 6. Демонстрация индексатора в Bookstore для поиска
        Console.WriteLine("Поиск книги 'Война и мир' в магазине...");
        var foundBook = myBookstore["Война и мир"]; // Пункт 5
        if (foundBook != null)
        {
            Console.WriteLine($"Найдена книга: {foundBook.GetDescription()}");
        }
        Console.WriteLine();

        // 7. Продажа книг и демонстрация интерфейса
        myBookstore.SellBook("Война и мир"); // Пункт 18 (использование метода из ISellable)
        myBookstore.PrintRevenue();
        Console.WriteLine();

        // 8. Демонстрация статического класса и метода расширения
        Console.WriteLine($"Валюта по умолчанию: {BookstoreUtilities.DefaultCurrency}"); // Пункт 8
        bool isExpensive = book1.IsExpensive(1000); // Пункт 15 (метод расширения)
        Console.WriteLine($"Книга '{book1.Title}' дорогая (дороже 1000 {BookstoreUtilities.DefaultCurrency})? - {isExpensive}");
        Console.WriteLine();

        // 9. Демонстрация перегруженных операторов '==' и '!=' (Пункт 11)
        var book1_copy = new Book("Война и мир", author1, "978-5-04-116810-9", 1500.50m);
        Console.WriteLine($"Сравнение двух книг по ISBN:");
        Console.WriteLine($"Оригинал == Копия? -> {book1 == book1_copy}"); // Пункт 11
        Console.WriteLine($"Оригинал != Вторая книга? -> {book1 != book2}");
        Console.WriteLine();

        // 10. Демонстрация обобщенного метода
        Console.WriteLine("Демонстрация обобщенного метода:");
        bookInventory.PrintItemType(123); 
        bookInventory.PrintItemType("тестовая строка"); 
        bookInventory.PrintItemType(book1); 
    }
}