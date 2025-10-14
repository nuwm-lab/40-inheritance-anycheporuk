using System;

class TwoInequalitiesSystem {
  protected double[,] a = new double[2, 2];  //Добрий день
  protected double[] b = new double[2];     

  public virtual void InputCoefficients() {
    Console.WriteLine("Введіть коефіцієнти для системи двох нерівностей:");
    for (int i = 0; i < 2; i++) {
      Console.WriteLine($"Для {i + 1}-ї нерівності введіть a{i+1}1, a{i+1}2, b{i+1}:");
      a[i, 0] = double.Parse(Console.ReadLine());
      a[i, 1] = double.Parse(Console.ReadLine());
      b[i] = double.Parse(Console.ReadLine());
    }
  }

  public virtual void PrintCoefficients() {
    Console.WriteLine("\nСистема двох лінійних нерівностей має вигляд:");
    for (int i = 0; i < 2; i++) {
      Console.WriteLine($"{a[i, 0]}*x1 + {a[i, 1]}*x2 ≤ {b[i]}");
    }
  }

  public virtual bool CheckVector(double x1, double x2) {
    for (int i = 0; i < 2; i++) {
      double left = a[i, 0] * x1 + a[i, 1] * x2;
      if (left > b[i])
        return false;
    }
    return true;
  }
}

class ThreeInequalitiesSystem : TwoInequalitiesSystem {
  private double[,] a3 = new double[3, 3];
  private double[] b3 = new double[3];

  public override void InputCoefficients() {
    Console.WriteLine("Введіть коефіцієнти для системи трьох нерівностей:");
    for (int i = 0; i < 3; i++) {
      Console.WriteLine($"Для {i + 1}-ї нерівності введіть a{i+1}1, a{i+1}2, a{i+1}3, b{i+1}:");
      a3[i, 0] = double.Parse(Console.ReadLine());
      a3[i, 1] = double.Parse(Console.ReadLine());
      a3[i, 2] = double.Parse(Console.ReadLine());
      b3[i] = double.Parse(Console.ReadLine());
    }
  }

  public override void PrintCoefficients() {
    Console.WriteLine("\nСистема трьох лінійних нерівностей має вигляд:");
    for (int i = 0; i < 3; i++) {
      Console.WriteLine($"{a3[i, 0]}*x1 + {a3[i, 1]}*x2 + {a3[i, 2]}*x3 ≤ {b3[i]}");
    }
  }

  public bool CheckVector(double x1, double x2, double x3) {
    for (int i = 0; i < 3; i++) {
      double left = a3[i, 0] * x1 + a3[i, 1] * x2 + a3[i, 2] * x3;
      if (left > b3[i])
        return false;
    }
    return true;
  }
}

class Program {
  static void Main() {
    Console.WriteLine("Оберіть тип системи:");
    Console.WriteLine("1 -- система двох лінійних нерівностей");
    Console.WriteLine("2 -- система трьох лінійних нерівностей");
    int choice = int.Parse(Console.ReadLine());

    if (choice == 1) {
      TwoInequalitiesSystem system = new TwoInequalitiesSystem();
      system.InputCoefficients();
      system.PrintCoefficients();

      Console.WriteLine("\nВведіть вектор X = (x1, x2):");
      double x1 = double.Parse(Console.ReadLine());
      double x2 = double.Parse(Console.ReadLine());

      bool result = system.CheckVector(x1, x2);
      Console.WriteLine(result ? "Точка задовольняє систему нерівностей."
                               : "Точка НЕ задовольняє систему нерівностей.");
    } else if (choice == 2) {
      ThreeInequalitiesSystem system3 = new ThreeInequalitiesSystem();
      system3.InputCoefficients();
      system3.PrintCoefficients();

      Console.WriteLine("\nВведіть вектор X = (x1, x2, x3):");
      double x1 = double.Parse(Console.ReadLine());
      double x2 = double.Parse(Console.ReadLine());
      double x3 = double.Parse(Console.ReadLine());

      bool result = system3.CheckVector(x1, x2, x3);
      Console.WriteLine(result ? "Точка задовольняє систему нерівностей."
                               : "Точка НЕ задовольняє систему нерівностей.");
    } else {
      Console.WriteLine("Невірний вибір!");
    }
  }
}

