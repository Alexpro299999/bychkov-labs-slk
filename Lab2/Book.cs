// Пункт 1: Использование минимум 4 собственных классов (этот класс - третий).
// Пункт 9: Использование наследования (Book наследуется от Publication).
// Пункт 18: Класс реализует интерфейс ISellable.
// Пункт 17: Использование композиции. Класс Book "состоит" из автора.
// Если удалить книгу, автор как сущность остается. Это слабая композиция, или агрегация.
// Для демонстрации двух видов, здесь пусть будет Агрегация.
public class Book : Publication, ISellable
{
    // Пункт 16: Использование агрегации классов (Book "имеет" автора).
    public Author BookAuthor { get; private set; }
    public string ISBN { get; private set; }

    // Пункт 3 и 4: Свойство с логикой. Цена не может быть отрицательной.
    private decimal _price;
    public decimal Price
    {
        get { return _price; }
        set
        {
            if (value < 0)
            {
                Console.WriteLine("Ошибка: Цена не может быть отрицательной.");
                _price = 0;
            }
            else
            {
                _price = value;
            }
        }
    }

    // Пункт 2: Конструктор с параметрами, вызывающий конструктор базового класса.
    public Book(string title, Author author, string isbn, decimal price) : base(title)
    {
        BookAuthor = author;
        ISBN = isbn;
        Price = price;
    }

    // Пункт 10: Переопределение абстрактного метода из базового класса Publication.
    public override string GetDescription()
    {
        return $"Книга: '{Title}' от автора {BookAuthor}, ISBN: {ISBN}";
    }

    // Реализация метода из интерфейса ISellable
    public void Sell()
    {
        Console.WriteLine($"Продана книга '{Title}' по цене {Price:C}");
    }

    // Пункт 11: Использование перегруженных операторов.
    // Перегружаем операторы == и != для сравнения книг по ISBN.
    public static bool operator ==(Book b1, Book b2)
    {
        if (ReferenceEquals(b1, null) && ReferenceEquals(b2, null)) return true;
        if (ReferenceEquals(b1, null) || ReferenceEquals(b2, null)) return false;
        return b1.ISBN == b2.ISBN;
    }

    public static bool operator !=(Book b1, Book b2)
    {
        return !(b1 == b2);
    }
}