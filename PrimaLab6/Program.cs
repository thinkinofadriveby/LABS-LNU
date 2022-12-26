using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimaLab6
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            /*
             *                              ВИСНОВОК
             * 
             * Для графа з малою кількістю вершин (100) та малою кількістю тасків (2) головний потік рахує ЗНАЧНО швидше
             * Для графа з малою кількістю вершин (100) та великою кількістю тасків (24) головний потік рахує ЗНАЧНО швидше
             * Для графа з середньою кількістю вершин (4000) та малою кількістю тасків (2) головний потік рахує ЗНАЧНО повільніше
             * Для графа з середньою кількістю вершин (4000) та малою кількістю тасків (24) головний потік рахує ЗНАЧНО повільніше
             * Для графа з великою кількістю вершин (10000) та малою кількістю тасків (2) головний потік рахує ЗНАЧНО повільніше
             * Для графа з великою кількістю вершин (10000) та великою кількістю тасків (24) головний потік рахує ЗНАЧНО повільніше
             * Результат підкріпив скрінами
             */
        }
    }
}
