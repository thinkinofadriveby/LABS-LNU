using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace domZavd
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

        public Action GenerateEditedMatrix = () =>
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                {

                    if (j < m)
                    {
                        matrix[i, j] = matrix[i, j] + matrix[i, j + 1];
                    }
                    else if (j == m)
                    {
                        matrix[i, j] = matrix[i, j] + matrix[i, 1];
                    }
                }
            }
        };

        class Program
        {
            static void Main(string[] args)
            {
                Console.Write("Input number of rows: ");
                int m = Convert.ToInt32(Console.ReadLine());

                Console.Write("Input number of columns: ");
                int n = Convert.ToInt32(Console.ReadLine());

                Console.Write("Input number of streams: ");
                int k = Convert.ToInt32(Console.ReadLine());

                Matrix startMatrix = new Matrix(m, n);
                startMatrix.GenerateRandomMatrix();

                //Console.WriteLine($"   \n   Start matrix: \n{startMatrix}");

                Matrix editedMatrix = startMatrix;

                var watch1 = System.Diagnostics.Stopwatch.StartNew();
                startMatrix.GenerateEditedMatrix();
                watch1.Stop();
                //Console.WriteLine($"   Edited matrix: \n{editedMatrix}");
                Console.WriteLine($"Main stream: {watch1.Elapsed}");

                Task[] task = new Task[k];

                var watch2 = System.Diagnostics.Stopwatch.StartNew();

                for (int i = 0; i < task.Length - 1; i++)
                {
                    task[i] = Task.Run(() => startMatrix.GenerateEditedMatrix.Invoke());
                }
                while (task.All(t => t == Task.CompletedTask))
                {
                    Task.Delay(50);
                }
                watch2.Stop();

                Console.WriteLine($"{k} stream(s): {watch2.Elapsed}");

                Console.ReadLine();
            }
        }

    }
}