using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimaLab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = Convert.ToInt32(textBox1.Text);
            int numberOfTasks = Convert.ToInt32(textBox5.Text);
            
            

            var watch1 = Stopwatch.StartNew();
            var graph = Class1.GenerateRandomGraph(size);
            Class1.Prima(graph, graph.GetLength(0));
            watch1.Stop();

            textBox3.Text = watch1.Elapsed.ToString();

            TaskFactory tf = new TaskFactory();

            var watch2 = Stopwatch.StartNew();
            Task t1 = tf.StartNew(() => Class1.Prima(graph, graph.GetLength(0)));

            Task[] task = new Task[numberOfTasks];
            for (int i = 0; i < task.Length - 1; i++)
            {
                task[i] = Task.Factory.StartNew(() => Class1.Prima(graph, graph.GetLength(0)));
            }

            
            watch2.Stop();

            textBox4.Text = watch2.Elapsed.ToString();

            

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
