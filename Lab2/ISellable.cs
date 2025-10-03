// Пункт 18: Определение собственного интерфейса.
public interface ISellable
{
    // Пункт 3: Использование свойств.
    decimal Price { get; set; }
    void Sell();
}