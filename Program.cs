using System;
using System.Globalization;
using System.Text;

namespace LinearInequalitiesApp
{
    /// <summary>
    /// Представляє систему лінійних нерівностей вигляду A*x ≤ b.
    /// </summary>
    public class InequalitiesSystem
    {
        private readonly double[,] _coefficients; // коефіцієнти при змінних
        private readonly double[] _constants;     // константи (права частина)

        /// <summary>
        /// Кількість нерівностей у системі.
        /// </summary>
        public int InequalitiesCount { get; }

        /// <summary>
        /// Кількість змінних у системі.
        /// </summary>
        public int VariablesCount { get; }

        /// <summary>
        /// Ініціалізує новий екземпляр системи лінійних нерівностей.
        /// </summary>
        /// <param name="inequalitiesCount">Кількість нерівностей.</param>
        /// <param name="variablesCount">Кількість змінних.</param>
        /// <exception cref="ArgumentException">Якщо значення непозитивні.</exception>
        public InequalitiesSystem(int inequalitiesCount, int variablesCount)
        {
            if (inequalitiesCount <= 0 || variablesCount <= 0)
                throw new ArgumentException("Кількість нерівностей і змінних має бути більшою за 0.");

            InequalitiesCount = inequalitiesCount;
            VariablesCount = variablesCount;
            _coefficients = new double[inequalitiesCount, variablesCount];
            _constants = new double[inequalitiesCount];
        }

        /// <summary>
        /// Безпечно зчитує дійсне число з консолі.
        /// </summary>
        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                    && double.IsFinite(value))
                    return value;

                Console.WriteLine("❌ Некоректне число. Використовуйте крапку для дробових значень (наприклад: 2.5).");
            }
        }

        /// <summary>
        /// Зчитує коефіцієнти системи з консолі.
        /// </summary>
        public void InputCoefficients()
        {
            Console.WriteLine($"\nВведіть коефіцієнти для системи з {InequalitiesCount} нерівностей " +
                              $"та {VariablesCount} змінних:");

            for (int i = 0; i < InequalitiesCount; i++)
            {
                Console.WriteLine($"\nНерівність {i + 1}:");
                for (int j = 0; j < VariablesCount; j++)
                    _coefficients[i, j] = ReadDouble($"  Введіть a{i + 1}{j + 1}: ");

                _constants[i] = ReadDouble($"  Введіть b{i + 1}: ");
            }
        }

        /// <summary>
        /// Форматує рядкове представлення окремої нерівності.
        /// </summary>
        private string FormatInequality(int index)
        {
            var sb = new StringBuilder();

            for (int j = 0; j < VariablesCount; j++)
            {
                double coeff = _coefficients[index, j];
                string sign = coeff >= 0 && j > 0 ? " + " : (j > 0 ? " - " : coeff < 0 ? "-" : "");
                double absCoeff = Math.Abs(coeff);

                sb.Append($"{sign}{absCoeff.ToString(CultureInfo.InvariantCulture)}*x{j + 1}");
            }

            sb.Append($" ≤ {_constants[index].ToString(CultureInfo.InvariantCulture)}");
            return sb.ToString();
        }

        /// <summary>
        /// Виводить систему лінійних нерівностей у консоль.
        /// </summary>
        public void PrintSystem()
        {
            Console.WriteLine("\nСистема лінійних нерівностей має вигляд:");
            for (int i = 0; i < InequalitiesCount; i++)
                Console.WriteLine($"  {FormatInequality(i)}");
        }

        /// <summary>
        /// Перевіряє, чи задовольняє вектор змінних усі нерівності системи.
        /// </summary>
        /// <param name="variables">Масив значень змінних.</param>
        /// <returns>true, якщо вектор задовольняє систему; інакше false.</returns>
        /// <exception cref="ArgumentException">Якщо кількість змінних не збігається.</exception>
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

    /// <summary>
    /// Точка входу в програму. Зчитує дані, будує систему та перевіряє вектор.
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Перевірка системи лінійних нерівностей ===\n");

            int choice = ReadChoice("Оберіть тип системи:\n" +
                                    "1 — система двох лінійних нерівностей із двома змінними\n" +
                                    "2 — система трьох лінійних нерівностей із трьома змінними\n" +
                                    "Ваш вибір: ", 1, 2);

            var system = choice == 1
                ? new InequalitiesSystem(2, 2)
                : new InequalitiesSystem(3, 3);

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
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Введіть число від {min} до {max}.");
            }
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                    && double.IsFinite(value))
                    return value;

                Console.WriteLine("❌ Некоректне число. Використовуйте крапку для дробових значень (наприклад: 1.5).");
            }
        }
    }
}
