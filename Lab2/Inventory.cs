using System.Collections.Generic;

// Пункт 12: Использование обобщений (generic-класс).
public class Inventory<T> where T : Publication
{
    // Пункт 7: Инкапсуляция.
    private List<T> _items = new List<T>();

    // Пункт 5: Использование индексатора.
    public T this[int index]
    {
        get { return _items[index]; }
        set { _items[index] = value; }
    }

    public int Count => _items.Count;

    public void Add(T item)
    {
        _items.Add(item);
    }

    // Пункт 13: Использование обобщенного метода.
    public void PrintItemType<U>(U item)
    {
        Console.WriteLine($"Тип элемента: {item.GetType().Name}");
    }
}