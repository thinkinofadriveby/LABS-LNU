using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2_Option11_Bezier
{
    public class Bezier
    {
        private static int Factorial(int n)
        {
            int factorial = 1;
            for (int i = 1; i <= n; i++)
            {
                factorial *= i;
            }

            return factorial;
        }

        private static float X(float t, List<PointF> listOfPoints)
        {
            float x = 0;
            int n = listOfPoints.Count - 1;
            for (int i = 0; i <= n; i++)
            {
                x += (float)((Factorial(n) / (Factorial(n - i) * Factorial(i)))
                    * Math.Pow(t, i) * Math.Pow(1 - t, n - i) * listOfPoints[i].X);
            }

            return x;
        }

        private static float Y(float t, List<PointF> listOfPoints)
        {
            float y = 0;
            int n = listOfPoints.Count - 1;
            for (int i = 0; i <= n; i++)
            {
                y += (float)((Factorial(n) / (Factorial(n - i) * Factorial(i)))
                    * Math.Pow(t, i) * Math.Pow(1 - t, n - i) * listOfPoints[i].Y);
            }

            return y;
        }

        public static void DrawBezier(Graphics gr, Pen pen, float dt, List<PointF> listPoints)
        {
            List<PointF> points = new List<PointF>();
            var a = listPoints.Take(4).ToList();

            int count = 0;

            while (listPoints.Count - count >= 4)
            {
                for (float t = 0.0f; t <= 1.0; t += dt)
                {
                    points.Add(new PointF(
                        X(t, a),
                        Y(t, a)));
                }

                count = count + 3;
                a = listPoints.Skip(count ).Take(4).ToList();
               
            }

            gr.DrawLines(pen, points.ToArray());

            for (var i = 0; i < listPoints.Count - 1; i++)
            {
                gr.DrawLine(Pens.Green, listPoints[i], listPoints[i + 1]);
            }
        }
    }
}
