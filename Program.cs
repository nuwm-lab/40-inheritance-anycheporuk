using System;
using System.Globalization;
using System.Text;

namespace InequalitiesApp {

    /// <summary>
    /// Знак нерівності.
    /// </summary>
    public enum SignType {
        LessOrEqual,
        GreaterOrEqual
    }

    /// <summary>
    /// Базовий клас для системи лінійних нерівностей.
    /// </summary>
    public class InequalitiesSystem {
        private readonly double[,] _coefficients;
        private readonly double[] _constants;
        private readonly double _epsilon = 1e-9;

        /// <summary>Кількість нерівностей у системі.</summary>
        public int InequalitiesCount { get; }

        /// <summary>Кількість змінних у системі.</summary>
        public int VariablesCount { get; }

        /// <summary>Ініціалізує систему з вказаними розмірами.</summary>
        public InequalitiesSystem(int inequalitiesCount, int variablesCount) {
            if (inequalitiesCount <= 0 || variablesCount <= 0)
                throw new ArgumentException("Кількість нерівностей і змінних має бути більшою за 0.");

            InequalitiesCount = inequalitiesCount;
            VariablesCount = variablesCount;
            _coefficients = new double[inequalitiesCount, variablesCount];
            _constants = new double[inequalitiesCount];
        }

        /// <summary>
        /// Безпечне введення чисел з клавіатури з урахуванням CultureInfo.
        /// </summary>
        protected static double SafeReadDouble(string prompt) {
            while (true) {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    return value;
                Console.WriteLine("Некоректне число. Використовуйте формат з крапкою (наприклад, 2.5). Спробуйте ще раз.");
            }
        }

        /// <summary>
        /// Зчитує коефіцієнти системи з консолі.
        /// </summary>
        public virtual void InputCoefficients() {
            Console.WriteLine($"Введіть коефіцієнти для системи з {InequalitiesCount} нерівностей " +
                              $"та {VariablesCount} змінних:");

            for (int i = 0; i < InequalitiesCount; i++) {
                Console.WriteLine($"\nНерівність {i + 1}:");
                for (int j = 0; j < VariablesCount; j++) {
                    _coefficients[i, j] = SafeReadDouble($"  Введіть a{i + 1}{j + 1}: ");
                }
                _constants[i] = SafeReadDouble($"  Введіть b{i + 1}: ");
            }
        }

        /// <summary>
        /// Виводить систему на екран у зручному форматі.
        /// </summary>
        public virtual void PrintCoefficients() {
            Console.WriteLine("\nСистема лінійних нерівностей має вигляд:");

            for (int i = 0; i < InequalitiesCount; i++) {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < VariablesCount; j++) {
                    double coeff = _coefficients[i, j];
                    if (j > 0)
                        sb.Append(coeff >= 0 ? " + " : " - ");
                    else if (coeff < 0)
                        sb.Append("-");

                    sb.Append($"{Math.Abs(coeff).ToString(CultureInfo.InvariantCulture)}*x{j + 1}");
                }
                sb.Append($" ≤ {_constants[i].ToString(CultureInfo.InvariantCulture)}");
                Console.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// Перевіряє, чи вектор задовольняє всі нерівності системи.
        /// </summary>
        public virtual bool CheckVector(params double[] variables) {
            if (variables.Length != VariablesCount)
                throw new ArgumentException("Кількість змінних не збігається з кількістю у системі.");

            for (int i = 0; i < InequalitiesCount; i++) {
                double left = 0;
                for (int j = 0; j < VariablesCount; j++)
                    left += _coefficients[i, j] * variables[j];

                if (left - _constants[i] > _epsilon)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Зчитує вектор змінних із консолі.
        /// </summary>
        public double[] InputVector() {
            double[] vector = new double[VariablesCount];
            Console.WriteLine($"\nВведіть {VariablesCount} змінних (через Enter):");
            for (int i = 0; i < VariablesCount; i++) {
                vector[i] = SafeReadDouble($"x{i + 1} = ");
            }
            return vector;
        }
    }

    /// <summary>
    /// Розширена версія системи, яка підтримує різні типи знаків (≤ або ≥).
    /// </summary>
    public class AdvancedInequalitiesSystem : InequalitiesSystem {
        private readonly SignType[] _signs;

        public AdvancedInequalitiesSystem(int inequalitiesCount, int variablesCount)
            : base(inequalitiesCount, variablesCount) {
            _signs = new SignType[inequalitiesCount];
        }

        public override void InputCoefficients() {
            base.InputCoefficients();

            Console.WriteLine("\nВведіть типи знаків для кожної нерівності (1 — ≤, 2 — ≥):");
            for (int i = 0; i < _signs.Length; i++) {
                while (true) {
                    Console.Write($"  Нерівність {i + 1}: ");
                    string? input = Console.ReadLine();
                    if (input == "1") { _signs[i] = SignType.LessOrEqual; break; }
                    if (input == "2") { _signs[i] = SignType.GreaterOrEqual; break; }
                    Console.WriteLine("Некоректний вибір. Введіть 1 або 2.");
                }
            }
        }

        public override void PrintCoefficients() {
            Console.WriteLine("\nСистема лінійних нерівностей має вигляд:");
            for (int i = 0; i < InequalitiesCount; i++) {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < VariablesCount; j++) {
                    double coeff = GetCoefficient(i, j);
                    if (j > 0)
                        sb.Append(coeff >= 0 ? " + " : " - ");
                    else if (coeff < 0)
                        sb.Append("-");

                    sb.Append($"{Math.Abs(coeff).ToString(CultureInfo.InvariantCulture)}*x{j + 1}");
                }

                string sign = _signs[i] == SignType.LessOrEqual ? "≤" : "≥";
                double constant = GetConstant(i);
                sb.Append($" {sign} {constant.ToString(CultureInfo.InvariantCulture)}");
                Console.WriteLine(sb.ToString());
            }
        }

        public override bool CheckVector(params double[] variables) {
            if (variables.Length != VariablesCount)
                throw new ArgumentException("Кількість змінних не збігається з кількістю у системі.");

            for (int i = 0; i < InequalitiesCount; i++) {
                double left = 0;
                for (int j = 0; j < VariablesCount; j++)
                    left += GetCoefficient(i, j) * variables[j];

                double diff = left - GetConstant(i);
                if (_signs[i] == SignType.LessOrEqual && diff > 1e-9)
                    return false;
                if (_signs[i] == SignType.GreaterOrEqual && diff < -1e-9)
                    return false;
            }
            return true;
        }

        // Приватні допоміжні методи для доступу до базових полів
        private double GetCoefficient(int i, int j) {
            var field = typeof(InequalitiesSystem).GetField("_coefficients",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var matrix = (double[,])field!.GetValue(this)!;
            return matrix[i, j];
        }

        private double GetConstant(int i) {
            var field = typeof(InequalitiesSystem).GetField("_constants",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var array = (double[])field!.GetValue(this)!;
            return array[i];
        }
    }

    /// <summary>
    /// Точка входу в програму.
    /// </summary>
    internal static class Program {
        private static void Main() {
            Console.WriteLine("Оберіть тип системи:");
            Console.WriteLine("1 — базова система (≤)");
            Console.WriteLine("2 — розширена система (≤ або ≥)");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2)) {
                Console.WriteLine("Некоректний вибір. Введіть 1 або 2:");
            }

            InequalitiesSystem system = choice == 1
                ? new InequalitiesSystem(2, 2)
                : new AdvancedInequalitiesSystem(3, 3);

            system.InputCoefficients();
            system.PrintCoefficients();

            double[] vector = system.InputVector();
            bool result = system.CheckVector(vector);

            Console.WriteLine(result
                ? "\n✅ Точка задовольняє систему нерівностей."
                : "\n❌ Точка НЕ задовольняє систему нерівностей.");
        }
    }
}
