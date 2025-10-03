// Пункт 14: Использование наследования обобщений.
// BookInventory — это конкретная реализация Inventory для книг.
public class BookInventory : Inventory<Book>
{
    public void PrintAllAuthors()
    {
        Console.WriteLine("--- Авторы в инвентаре ---");
        for (int i = 0; i < this.Count; i++)
        {
            Console.WriteLine(this[i].BookAuthor.ToString());
        }
    }
}