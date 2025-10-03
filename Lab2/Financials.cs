// Этот класс используется для демонстрации Композиции.
// Его жизненный цикл полностью контролируется классом Bookstore.
public class Financials
{
    public decimal TotalRevenue { get; private set; }

    public Financials()
    {
        TotalRevenue = 0;
    }

    public void RecordSale(decimal amount)
    {
        if (amount > 0)
        {
            TotalRevenue += amount;
        }
    }
}