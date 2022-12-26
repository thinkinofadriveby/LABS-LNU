using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab7
{
    public class Class1
    {

        public void CreateFile(int i)
        {
            int ind = i;
            
            Random random = new Random(Guid.NewGuid().GetHashCode());
            const string mainPath = @"C:\Users\user\Desktop\MAIN.txt";

            
            var processes = Process.GetProcesses();
            var myProcess = new Process();

            
            var watch1 = Stopwatch.StartNew();
            string path = $@"C:\Users\user\Desktop\file{ind}.txt";


            // File.Create(path).Close();
            // myProcesses[i] = new Process();
            myProcess.StartInfo.FileName = path;

            foreach (var p in processes)
            {
                if (p == myProcess)
                {
                    p.Kill();
                }
            }

            int[] numbers = new int[100000];
            StreamWriter sw = File.AppendText(path);
            for (int j = 0; j < numbers.Length; j++)
            {
                int index = j;
                numbers[index] = random.Next(1, 100);
                sw.WriteLine(numbers[j].ToString());
            }
            sw.Close();
            //
            // File.AppendAllText(path, numbers.ToString());

            watch1.Stop();

            var elapsed = watch1.Elapsed;

            File.AppendAllText(mainPath, $"\nTime elapsed for task {ind}: {elapsed.ToString()}");
            
            myProcess.Close();
        }      
    }
}