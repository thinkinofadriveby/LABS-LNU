using lab4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab4
{
    public static class Matrix
    {
        public static void GenerateRandomMatrix(out double[,] A, int Size, out double[] B)
        {
            A = new double[Size, Size];
            B = new double[Size];
            Random rnd = new Random();
            for (int i = 0; i < Size; i++)
            {
                B[i] = rnd.Next(1, 10);
                for (int j = 0; j < Size; j++)
                {
                    A[i, j] = rnd.Next(1, 10);
                }
            }
        }

        public static void SubtractionFromMatrix(double[,] A, int Size, double[] B, int index, int j, double max)
        {
            for (int i = j + 1 + index; i < Size; i += 5)
            {
                double mult = A[i, j] / max;
                for (int k = j; k < Size; k++)
                {
                    A[i, k] -= mult * A[j, k];
                }
                B[i] -= mult * B[j];
            }
            //Matrix.ShowMatrix(A, Size, B);
        }

        public static void GetResult(double[,] A, int Size, double[] B, double[] X, int index, int j)
        {
            for (int i = index; i < j; i += 4)
            {
                B[i] -= A[i, j] * X[j];
            }
        }
      
    }
    public static class Gauss
    {
        public static double[] GaussSeidelMethod(double[,] A, int Size, double[] B, double[] X)
        {
            for (int j = 0; j < Size - 1; j++)
            {             
                double max = Math.Abs(A[j, j]);
                int maxLine = j;
                for (int i = j + 1; i < Size; i++)
                {
                    if (Math.Abs(A[i, j]) > max)
                    {
                        max = Math.Abs(A[i, j]);
                        maxLine = i;
                    }
                }

                if (maxLine != j)
                {
                    for (int k = j; k < Size; k++)
                    {
                        double toSwap = A[j, k];
                        A[j, k] = A[maxLine, k];
                        A[maxLine, k] = toSwap;
                    }
                    double toSwapB = B[j];
                    B[j] = B[maxLine];
                    B[maxLine] = toSwapB;
                }

                for (int i = j + 1; i < Size; i++)
                {
                    double mult = A[i, j] / max;
                    for (int k = j; k < Size; k++)
                    {
                        A[i, k] -= mult * A[j, k];
                    }
                    B[i] -= mult * B[j];
                }
            }
            for (int j = Size - 1; j >= 0; j--)
            {
                if (A[j, j] == 0)
                {
                    Console.WriteLine("Invalid input!");
                    return B;
                }
                X[j] = B[j] / A[j, j];
                for (int i = 0; i < j; i++)
                {
                    B[i] -= A[i, j] * X[j];
                }
            }
            double[] arr = B;
            return arr;
        }
    }
}

    class Program
    {
        static void Main(string[] args)
        {
        Console.Write("Input size of matrix: ");
        int size = Convert.ToInt32(Console.ReadLine());
        Console.Write("Input number of threads: ");
        int threadCount = Convert.ToInt32(Console.ReadLine());
        double[,] A;
        double[] B;
        double[] X = new double[size];
        Matrix.GenerateRandomMatrix(out A, size, out B);

        var watch1 = Stopwatch.StartNew();
        Gauss.GaussSeidelMethod(A, size, B, X);
        // Console.WriteLine();
        watch1.Stop();

        Console.WriteLine($"One thread: {watch1.Elapsed}");


        double[,] AParal = A;
        double[] BParal = B;
        double[] XParal = new double[size];

        var watch2 = Stopwatch.StartNew();

        List<WaitCallback> s = new List<WaitCallback>();

        int move = X.GetLength(0) / threadCount;
        int[] begins = new int[threadCount];
        int[] stops = new int[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            int begin = i * move;
            int stop;
            if (i * 1 == threadCount)
            {
                stop = X.GetLength(0);
            }
            else
            {
                stop = begin + move;
            }
            WaitCallback thread = new WaitCallback((x) => Gauss.GaussSeidelMethod(A, size, B, X));
            s.Add(thread);
            begins[i] = begin;
            stops[i] = stop;

        }

        int counter = 0;
        double[] result = Gauss.GaussSeidelMethod(A, size, B, X); 
        foreach (var item in s)
        {
            ThreadPool.QueueUserWorkItem(item, new object[] { X, begins[counter], stops[counter], result });
            counter++;
        }
        watch2.Stop();
        Console.WriteLine($"{threadCount} thread: {watch2.Elapsed}");

        Console.ReadLine();

        /*
         *                                ВИСНОВОК
         * 
         * При малому розмірі матриці (100) та малій кількості потоків (2) головний потік рахує майже так само як і на 2 потоках
         * При малому розмірі матриці (100) та великій кількості потоків (24) головний потік рахує скоріше
         * При середньому розмірі матриці (1000) та малій кількості потоків (2) головний потік рахує трохи повільніше
         * При середньому розмірі матриці (1000) та великій кількості потоків (24) головний потік рахує трохи повільніше
         * При великому розмірі матриці (2000) та малій кількості потоків (2) головний потік рахує трохи повільніше
         * При великому розмірі матриці (2000) та великій кількості потоків (24) головний потік рахує трохи повільніше
         * Підкріпив це все скріном, який прикріплю разом з файлом
         */
    }
}

