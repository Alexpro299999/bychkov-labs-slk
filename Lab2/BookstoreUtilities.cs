// Пункт 8: Использование статического класса.
public static class BookstoreUtilities
{
    // Пункт 8: Статическое свойство.
    public static string DefaultCurrency { get; } = "RUB";

    // Пункт 15: Использование метода расширения для класса Book.
    // Добавляет новый метод IsExpensive к любому объекту типа Book.
    public static bool IsExpensive(this Book book, decimal limit)
    {
        return book.Price > limit;
    }
}