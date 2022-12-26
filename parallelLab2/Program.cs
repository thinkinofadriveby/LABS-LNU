using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2Parallel
{
    class Matrix
    {
        public static int m;
        private static int n;
        private static int[,] matrix;
        private static Random random = new Random();

        public Matrix()
        {
            m = 0;
            n = 0;
            matrix = new int[m, n];
        }
        public Matrix(int _m, int _n)
        {
            m = _m;
            n = _n;
            matrix = new int[m, n];
        }

        public override string ToString()
        {
            StringBuilder matrixToStringBuilder = new StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrixToStringBuilder.Append(Convert.ToString(matrix[i, j]) + "\t");
                }
                matrixToStringBuilder.Append("\n");
            }
            return matrixToStringBuilder.ToString();
        }

        public void GenerateRandomMatrix()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = random.Next(-100, 100);
                }
            }
        }

        public delegate void PerformMatrixDelegate();
        public PerformMatrixDelegate PerformMatrix() => () =>
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != matrix[i, 0])
                    {
                        matrix[i, j] += matrix[i, 0];
                    }
                }
            }
        };

        public static void ShowInfoAboutThread(IAsyncResult asyncResult)
        {
            string showInfo = (string)asyncResult.AsyncState;
            Console.WriteLine(showInfo + " in thread {0}.", Thread.CurrentThread.ManagedThreadId);
        }

        class Program
        {

            public delegate void CallBack();
            static void Main(string[] args)
            {
                Console.Write("Input number of rows: ");
                int m = Convert.ToInt32(Console.ReadLine());

                Console.Write("Input number of columns: ");
                int n = Convert.ToInt32(Console.ReadLine());

                Console.Write("Input number of streams: ");
                int k = Convert.ToInt32(Console.ReadLine());

                Matrix startMatrix = new Matrix(m, n);

                startMatrix.PerformMatrix().Invoke();

                CallBack callback1 = new CallBack(startMatrix.PerformMatrix());

                var watch1 = Stopwatch.StartNew();
                callback1.Invoke();

                watch1.Stop();
                Console.WriteLine($"\n[!] One thread: {watch1.Elapsed}");

                Thread[] threads = new Thread[k];
                CallBack callback2 = new CallBack(startMatrix.PerformMatrix());
                IAsyncResult[] result = new IAsyncResult[k];
                var watch2 = Stopwatch.StartNew();

                for (int i = 0; i < threads.Length; i++)
                {
                    IAsyncResult asyncResult =
                        callback2.BeginInvoke(ShowInfoAboutThread, "\nMethod ShowInfoAboutThread run");
                    result[i] = asyncResult;
                    threads[i] = new Thread(() => callback2.EndInvoke(asyncResult));
                }
                watch2.Stop();

                foreach (var varAsyncResult in result)
                {
                    callback2.EndInvoke(varAsyncResult);
                }

                while (threads.All(t => t.IsAlive))
                {
                    Thread.Sleep(50);
                }

                Console.WriteLine($"\n[!] {threads.Length} threads: {watch2.Elapsed}");

                {
                    //startMatrix.GenerateRandomMatrix();
                    //Console.WriteLine($"   \n   Start matrix: \n{startMatrix}");
                    //startMatrix.PerformMatrix();                                       // вивід матриці
                    //Matrix performedMatrix = startMatrix;
                    //Console.WriteLine($"   Performed matrix: \n{performedMatrix}");
                }

                //                                                              ВИСНОВОК

                // Для малої матриці (100 елементів) і малої кількості потоків (2 потоки) головний потік обраховує швидше
                // Для малої матриці (100 елементів) і великої кількості потоків (24 потоки) головний потік також обраховує швидше
                // Для середнього розміру матриці (4000 елементів) і малої кількості потоків (2 потоки) головний потік обраховує НАБАГАТО повільніше
                // Для середнього розміру матриці (4000 елементів) і великої кількості потоків (24 потоки) головний потік не так сильно, але все ж рахує повільніше 
                // Для великого розміру матриці (10000 елементів) і малої кількості потоків (2 потоки) головний потік обраховує НАБАГАТО повільніше
                // Для великого розміру матриці (10000 елементів) і великої кількості потоків (24 потоки) головний потік також обраховує НАБАГАТО повільніше
                // Всі результати підтверджуються скріном, який я додам у завдання разом з цим файлом

                Console.ReadLine();
            }
        }
    }
}
