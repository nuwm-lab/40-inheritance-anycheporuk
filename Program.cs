using System;
using System.Globalization;

class InequalitiesSystem
{
    private readonly double[,] _coefficients; // коефіцієнти при змінних
    private readonly double[] _constants;     // константи (права частина)

    public int InequalitiesCount { get; }
    public int VariablesCount { get; }

    public InequalitiesSystem(int inequalitiesCount, int variablesCount)
    {
        if (inequalitiesCount <= 0 || variablesCount <= 0)
            throw new ArgumentException("Кількість нерівностей і змінних має бути більшою за 0.");

        InequalitiesCount = inequalitiesCount;
        VariablesCount = variablesCount;
        _coefficients = new double[inequalitiesCount, variablesCount];
        _constants = new double[inequalitiesCount];
    }

    private static double SafeReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;

            Console.WriteLine("❌ Некоректне число. Використовуйте крапку для дробових значень.");
        }
    }

    public void InputCoefficients()
    {
        Console.WriteLine($"\nВведіть коефіцієнти для системи з {InequalitiesCount} нерівностей " +
                          $"та {VariablesCount} змінних:");

        for (int i = 0; i < InequalitiesCount; i++)
        {
            Console.WriteLine($"\nНерівність {i + 1}:");
            for (int j = 0; j < VariablesCount; j++)
            {
                _coefficients[i, j] = SafeReadDouble($"  Введіть a{i + 1}{j + 1}: ");
            }
            _constants[i] = SafeReadDouble($"  Введіть b{i + 1}: ");
        }
    }

    public void PrintSystem()
    {
        Console.WriteLine("\nСистема лінійних нерівностей має вигляд:");

        for (int i = 0; i < InequalitiesCount; i++)
        {
            string leftSide = "";
            for (int j = 0; j < VariablesCount; j++)
            {
                string sign = _coefficients[i, j] >= 0 && j > 0 ? " + " : " ";
                leftSide += $"{sign}{_coefficients[i, j]}*x{j + 1}";
            }
            Console.WriteLine($"{leftSide} ≤ {_constants[i]}");
        }
    }

    public bool CheckVector(params double[] variables)
    {
        if (variables.Length != VariablesCount)
            throw new ArgumentException("Кількість змінних не збігається з кількістю змінних у системі.");

        for (int i = 0; i < InequalitiesCount; i++)
        {
            double left = 0;
            for (int j = 0; j < VariablesCount; j++)
                left += _coefficients[i, j] * variables[j];

            if (left > _constants[i])
                return false;
        }
        return true;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Перевірка системи лінійних нерівностей ===\n");

        int choice = ReadChoice("Оберіть тип системи:\n" +
                                "1 — система двох лінійних нерівностей із двома змінними\n" +
                                "2 — система трьох лінійних нерівностей із трьома змінними\n" +
                                "Ваш вибір: ", 1, 2);

        InequalitiesSystem system = choice switch
        {
            1 => new InequalitiesSystem(2, 2),
            2 => new InequalitiesSystem(3, 3),
            _ => throw new InvalidOperationException()
        };

        system.InputCoefficients();
        system.PrintSystem();

        Console.WriteLine($"\nВведіть значення для {system.VariablesCount} змінних:");
        double[] vector = new double[system.VariablesCount];
        for (int i = 0; i < vector.Length; i++)
            vector[i] = ReadDouble($"x{i + 1} = ");

        bool result = system.CheckVector(vector);
        Console.WriteLine(result
            ? "\n✅ Точка задовольняє систему нерівностей."
            : "\n❌ Точка НЕ задовольняє систему нерівностей.");
    }

    private static int ReadChoice(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                return choice;

            Console.WriteLine($"Введіть число від {min} до {max}.");
        }
    }

    private static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;

            Console.WriteLine("❌ Некоректне число. Використовуйте крапку для дробових значень.");
        }
    }
}
