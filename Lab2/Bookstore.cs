// Пункт 1: Использование минимум 4 собственных классов (этот класс - четвертый).
public class Bookstore
{
    public string Name { get; set; }

    // Пункт 16: Использование агрегации. Bookstore "имеет" инвентарь,
    // но инвентарь может существовать и отдельно. Он передается в конструктор.
    private BookInventory _inventory;

    // Пункт 17: Использование композиции. Bookstore "состоит" из Financials.
    // Объект Financials создается внутри Bookstore и не может существовать без него.
    private Financials _storeFinancials;

    // Пункт 2: Конструктор с параметрами.
    public Bookstore(string name, BookInventory inventory)
    {
        Name = name;
        _inventory = inventory;
        _storeFinancials = new Financials(); // Композиция
    }

    // Пункт 5: Использование индексатора для поиска книги по названию.
    public Book this[string title]
    {
        get
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    return _inventory[i];
                }
            }
            return null;
        }
    }

    public void SellBook(string title)
    {
        Book bookToSell = this[title];
        if (bookToSell != null)
        {
            bookToSell.Sell();
            _storeFinancials.RecordSale(bookToSell.Price);
        }
        else
        {
            Console.WriteLine($"Книга '{title}' не найдена.");
        }
    }

    public void PrintRevenue()
    {
        Console.WriteLine($"Общая выручка магазина '{Name}': {_storeFinancials.TotalRevenue:C}");
    }
}