using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimaLab6
{
    class Class1
    {
        public static int MinKey(int[] key, bool[] set, int verticesCount)
        {
            int min = int.MaxValue, minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (set[v] == false && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        public static int[,] GenerateRandomGraph(int size)
        {
            Random random = new Random();
            int[,] graph = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    graph[i, j] = random.Next(0, 10);
                }
            }
            return graph;
        }
        public static void Prima(int[,] graph, int length)
        {
            int[] parent = new int[length];
            int[] key = new int[length];
            bool[] mstSet = new bool[length];

            for (int i = 0; i < length; ++i)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < length - 1; ++count)
            {
                int u = MinKey(key, mstSet, length);
                mstSet[u] = true;

                for (int v = 0; v < length; ++v)
                {
                    if (Convert.ToBoolean(graph[u, v]) && mstSet[v] == false && graph[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
                }
            }
        }
    }
}
