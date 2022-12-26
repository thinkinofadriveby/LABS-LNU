using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Task2_Option11_Bezier
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<PointF> Points = new List<PointF>();

        private void picCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (Points.Count > 1 && Points.Count % 3 == 1)
            {
                Points.Add(new PointF
                {
                    X = 2 * Points[Points.Count - 1].X - Points[Points.Count - 2].X,
                    Y = 2 * Points[Points.Count - 1].Y - Points[Points.Count - 2].Y
                });
            }
            else
            {
                Points.Add(new PointF { X = e.X, Y = e.Y });
            }
            //var a = new PointF { X = e.X, Y = e.Y };
            //var b = new Point3D { X=200,Y=3,Z=100};
            picCanvas.Refresh();
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(picCanvas.BackColor);
            if (Points.Count >= 4)
            {
                if (Points.Count % 3 == 1)
                {
                    using (Pen pen = new Pen(Color.Purple, 5))
                    {
                        e.Graphics.DrawBeziers(pen, Points.ToArray());
                    }
                }

                Bezier.DrawBezier(e.Graphics, Pens.Black, 0.01f, Points);
            }
            for (int i = 0; i < Points.Count; i++)
            {
                e.Graphics.FillEllipse(Brushes.White, Points[i].X - 3, Points[i].Y - 3, 6, 6);
                e.Graphics.DrawEllipse(Pens.Black, Points[i].X - 3, Points[i].Y - 3, 6, 6);
            }
        }

   
    }
}
