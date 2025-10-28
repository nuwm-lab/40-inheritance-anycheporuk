using System;

class InequalitiesSystem {
  private readonly double[,] _coefficients; // коефіцієнти при змінних
  private readonly double[] _constants;     // константи (права частина)
  public int InequalitiesCount { get; }
  public int VariablesCount { get; }

  public InequalitiesSystem(int inequalitiesCount, int variablesCount) {
    if (inequalitiesCount <= 0 || variablesCount <= 0)
      throw new ArgumentException("Кількість нерівностей і змінних має бути більшою за 0.");

    InequalitiesCount = inequalitiesCount;
    VariablesCount = variablesCount;
    _coefficients = new double[inequalitiesCount, variablesCount];
    _constants = new double[inequalitiesCount];
  }

  private double SafeReadDouble(string prompt) {
    while (true) {
      Console.Write(prompt);
      string? input = Console.ReadLine();
      if (double.TryParse(input, out double value))
        return value;
      Console.WriteLine("Некоректне число, спробуйте ще раз.");
    }
  }

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

  public virtual void PrintCoefficients() {
    Console.WriteLine("\nСистема лінійних нерівностей має вигляд:");
    for (int i = 0; i < InequalitiesCount; i++) {
      string leftSide = "";
      for (int j = 0; j < VariablesCount; j++) {
        leftSide += $"{_coefficients[i, j]}*x{j + 1}";
        if (j < VariablesCount - 1)
          leftSide += " + ";
      }
      Console.WriteLine($"{leftSide} ≤ {_constants[i]}");
    }
  }

  public virtual bool CheckVector(params double[] variables) {
    if (variables.Length != VariablesCount) {
      Console.WriteLine("❌ Кількість введених змінних не збігається з кількістю змінних у системі.");
      return false;
    }

    for (int i = 0; i < InequalitiesCount; i++) {
      double left = 0;
      for (int j = 0; j < VariablesCount; j++) {
        left += _coefficients[i, j] * variables[j];
      }
      if (left > _constants[i])
        return false;
    }
    return true;
  }
}

class Program {
  static void Main() {
    Console.WriteLine("Оберіть тип системи:");
    Console.WriteLine("1 — система двох лінійних нерівностей із двома змінними");
    Console.WriteLine("2 — система трьох лінійних нерівностей із трьома змінними");

    int choice;
    while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2)) {
      Console.WriteLine("Некоректний вибір. Введіть 1 або 2:");
    }

    InequalitiesSystem system;

    if (choice == 1)
      system = new InequalitiesSystem(2, 2);
    else
      system = new InequalitiesSystem(3, 3);

    system.InputCoefficients();
    system.PrintCoefficients();

    Console.WriteLine($"\nВведіть {system.VariablesCount} змінних (через Enter):");
    double[] vector = new double[system.VariablesCount];
    for (int i = 0; i < system.VariablesCount; i++) {
      while (true) {
        Console.Write($"x{i + 1} = ");
        if (double.TryParse(Console.ReadLine(), out double val)) {
          vector[i] = val;
          break;
        }
        Console.WriteLine("Некоректне число, спробуйте ще раз.");
      }
    }

    bool result = system.CheckVector(vector);
    Console.WriteLine(result
      ? "\n Точка задовольняє систему нерівностей."
      : "\n Точка НЕ задовольняє систему нерівностей.");
  }
}


