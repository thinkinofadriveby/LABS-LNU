using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using MathNet.Numerics.LinearAlgebra.Double.Solvers;
using MathNet.Numerics.LinearAlgebra.Solvers;

namespace LAB3
{

class Matrix
    {
        private static int n;
        double[,] a;
        public Matrix(int _n)
        {
            n = _n;
            a = new double[n + 1, n + 1];
        }
        public override string ToString()
        {
            StringBuilder matrixToStringBuilder = new StringBuilder();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    matrixToStringBuilder.Append(Convert.ToString(a[i, j]) + "\t");
                }
                matrixToStringBuilder.Append("\n");
            }
            return matrixToStringBuilder.ToString();
        }


        public double[] SolveTridiagonalMatrix()
        {
            Random random = new Random();
            double[] b = new double[n + 1];
            double[] x = new double[n + 1];
            double[] k = new double[n + 1];
            double[] m = new double[n + 1];
            double[] t = new double[n + 1];
            double[] p = new double[n + 1];
            double[] q = new double[n + 1];

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    a[i, j] = Math.Round(random.NextDouble(), 2);
                    //Console.Write(a[i, j] + "\t");
                }
                b[i] = Math.Round(random.NextDouble(), 2);
                //Console.WriteLine();
            }
            for (int i = 1; i <= n; i++)
            {
                if (i == 1)
                {
                    k[i] = 0;
                }
                else
                {
                    k[i] = a[i, i - 1];
                }
                m[i] = -a[i, i];
                if (i == n)
                {
                    t[i] = 0;
                }
                else
                {
                    t[i] = a[i, i + 1];
                }
            }
            p[1] = t[1] / m[1];
            q[1] = -b[1] / m[1];
            for (int i = 2; i <= n; i++)
            {
                p[i] = -t[i] / (k[i] * p[i - 1] - m[i]);
                q[i] = (b[i] - k[i] * q[i - 1]) / (k[i] * p[i - 1] - m[i]);
            }
            x[n] = (b[n] - k[n] * q[n - 1]) / (k[n] * p[n - 1] - m[n]);
            for (int i = n - 1; i >= 1; i--)
            {
                x[i] = p[i] * x[i + 1] + q[i];
            }

            //for (int i = 1; i < x.Length; i++)
            //{
            //    Console.WriteLine($"\nSOLVE: x[{i}] = {Math.Round(x[i], 2)}"); 
            //}
            return x;
        }
        public static void QuasiMinimalResidualMethod(int order)
        {
            var matrixA = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.Random(order, order, 1);
            var matrixB = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.Random(order, order, 1);

            var monitor = new Iterator<double>(
                new IterationCountStopCriterion<double>(1000),
                new ResidualStopCriterion<double>(1e-10)); // new ResidualStopCriterion<double>(1e-10));

            var solver = new TFQMR();
            var matrixX = matrixA.SolveIterative(matrixB, solver, monitor);
        }
    }
class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input number of equations: ");
            int n = Convert.ToInt32(Console.ReadLine());

            Console.Write("Input number of streams: ");
            int k = Convert.ToInt32(Console.ReadLine());

            Matrix a = new Matrix(n);

            //================================ Tridiagonal matrix algorithm
            var watch1 = Stopwatch.StartNew();
            a.SolveTridiagonalMatrix();
            watch1.Stop();
            Console.WriteLine("\n[Exact method]: Tridiagonal Matrix Algorithm:");
            Console.WriteLine($"\t\tTime elapsed in main stream: {watch1.Elapsed}");

            List<Thread> threadsForTMA = new List<Thread>();

            var watch2 = Stopwatch.StartNew();
            for (int i = 0; i < k; i++)
            {
                Thread thread = new Thread(() => a.SolveTridiagonalMatrix());
                threadsForTMA.Add(thread);
                thread.Start();
            }
            threadsForTMA.ForEach(t => t.Join());
            watch2.Stop();
            Console.WriteLine($"\t\tTime elapsed in {k} streams: {watch2.Elapsed}");

            // ================================ Quasi Minimal Residual Method
            List<Thread> threadsForQMR = new List<Thread>();
            var watch3 = Stopwatch.StartNew();
            Matrix.QuasiMinimalResidualMethod(n);
            watch3.Stop();
            Console.WriteLine("\n[Iterative method]: Quasi-Minimal Residual Algorithm:");
            Console.WriteLine($"\t\tTime elapsed in main stream: {watch3.Elapsed}");

            var watch4 = Stopwatch.StartNew();
            for (int i = 0; i < k; i++)
            {
                Thread thread = new Thread(() => Matrix.QuasiMinimalResidualMethod(n));
                threadsForQMR.Add(thread);
                thread.Start();
            }
            threadsForQMR.ForEach(t => t.Join());
            watch4.Stop();
            Console.WriteLine($"\t\tTime elapsed in {k} streams: {watch4.Elapsed}");

            Console.ReadLine();

            //   ВИСНОВОК

            // Точний метод (метод прогонки)

            // Для малої кількості систем (100) і малої кількості потоків (2) головний потік, як і очікувалось, обраховує швидше
            // Для малої кількості систем (100) і великої кількості потоків (24) головний потік працює помітно швидше
            // Для середньої кількості систем (4000) і малої кількості потоків (2) головний потік обраховує повільніше
            // Для середньої кількості систем (4000) і великої кількості потоків (8) головний потік обраховує трохи повільніше
            // Для великої кількості систем (10000) і малої кількості потоків (2) головний потік теж обраховує трохи повільніше
            // Для великої кількості систем (10000) і великої кількості потоків (8) головний потік обраховує трохи повільніше

            // А ось з ітераційним методом (в моєму випадку це метод квазі-мінімальних лишків) все не так однозначно.
            // Справа в тому, що, схоже, він не розбивається на потоки, оскільки навіть при малих кількостях систем (100) він видає
            // якийсь неадекватний час виконання (скрін я прикріплю разом з файлом у завдання)
            // наприклад, щоб розв'язати систему зі 100 рівнянь, 2 потокам потрібно 1,5 ХВИЛИНИ, головному потоку 1 хвилина
            // щоб розв'язати систему зі 100 рівнянь, 8 потокам потрібно ЦІЛИХ 6,5 ХВИЛИНИ, головному потоку 1 хвилина
        }
    }
}
