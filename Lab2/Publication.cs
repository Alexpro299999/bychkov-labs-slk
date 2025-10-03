// Пункт 1: Использование минимум 4 собственных классов (этот класс - первый).
// Пункт 6: Использование абстрактного класса.
// Пункт 7: Использование инкапсуляции (модификатор protected).
public abstract class Publication
{
    // Пункт 7: Поле защищено, доступно только в классе и его наследниках.
    protected string _title;

    // Пункт 4: Свойство с логикой в set-блоке.
    public string Title
    {
        get { return _title; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Название не может быть пустым.");
            }
            _title = value;
        }
    }

    // Пункт 2: Использование конструктора с параметрами.
    public Publication(string title)
    {
        Title = title;
    }

    // Пункт 6: Использование абстрактного члена класса (метода).
    // Пункт 10: Этот метод предназначен для переопределения в классах-наследниках.
    public abstract string GetDescription();
}