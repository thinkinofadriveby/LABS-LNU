using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dz1_matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter N: ");
            int N = int.Parse(Console.ReadLine());

            int[,] array = new int[N, N];

            Random random = new Random();

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = random.Next(-100, 100);
                }
            }

            Console.WriteLine("\nYour matrix with random elements: ");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + "\t");
                }
                Console.WriteLine();
            }

            int[,] changed_array = new int[N, N];

            int y = 0;
            int x = 0;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] >= 0)
                    {
                        if (x > array.GetLength(1) - 1)
                        {
                            y++;
                            x = 0;
                            changed_array[y, x] = array[i, j];
                            x++;
                        }
                        else
                        {
                            changed_array[y, x] = array[i, j];
                            x++;
                        }
                    }
                }
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] < 0)
                    {

                        if (x > array.GetLength(1) - 1)
                        {
                            y++;
                            x = 0;
                            changed_array[y, x] = array[i, j];
                            x++;
                        }
                        else
                        {
                            if (changed_array[y, x] == 0)
                            {
                                changed_array[y, x] = array[i, j];
                                x++;
                            }
                            else
                            {
                                x++;
                                changed_array[y, x] = array[i, j];
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\nYour matrix after swaping elements: ");

                for (int z = 0; z < changed_array.GetLength(0); z++)
                {
                    for (int j = 0; j < changed_array.GetLength(1); j++)
                    {
                        Console.Write(changed_array[z, j] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.ReadLine();
            }
        }
    }

