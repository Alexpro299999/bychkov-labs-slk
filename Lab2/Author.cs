// Пункт 1: Использование минимум 4 собственных классов (этот класс - второй).
public class Author
{
    // Пункт 3: Использование свойств.
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Пункт 2: Использование конструктора с параметрами.
    public Author(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    // Пункт 10: Переопределение метода ToString() от базового класса object.
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}