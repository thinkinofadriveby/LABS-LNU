using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.FileName;
                richTextBox2.Text = File.ReadAllText(ofd.FileName);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mainPath = @"C:\Users\user\Desktop\MAIN.txt";
            int numberOfTasks = Convert.ToInt32(textBox1.Text);
            Task[] task = new Task[numberOfTasks];
            
            Class1 cl = new Class1();
            for (int i = 0; i < task.Length; i++)
            {
                int index = i;
                task[i] = Task.Run(() => cl.CreateFile(index));
            }

            richTextBox2.Text = File.ReadAllText(mainPath);

        }

        private void createFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;

            richTextBox3.Text = filename;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            richTextBox4.Text = filename;
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path1 = richTextBox3.Text;
            string path2 = richTextBox4.Text;

            string file1 = File.ReadAllText(path1);
            string file2 = File.ReadAllText(path2);
            if (file1.Equals(file2))
            {
                richTextBox1.Text = "Files are equal!";
            }
            else
            {
                richTextBox1.Text = "Files are not equal!";
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}