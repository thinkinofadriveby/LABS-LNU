using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3_progr
{
    interface IFigurable
    {
        double CalculateArea();
    }

    class Square : IFigurable
    {
        private readonly double side;

        public double CalculateArea()
        {
            return side * side;
        }

        public Square()
        {
            side = 5;
        }

        public Square(double side)
        {
            if (side <= 0)
            {
                throw new ArgumentException("Side of square can not be zero or negative!");
            }
            else
            {
                this.side = side;
            }
        }
        public double S
        {
            get
            {
                return side;
            }
        }
        public override string ToString()
        {
            return $"Side of square is {side}\nArea of square is {CalculateArea()}";
        }
    }


    class Triangle : IFigurable
    {
        private readonly double side1;
        private readonly double side2;
        private readonly double side3;

        public double CalculateArea()
        {
            double p = (side1 + side2 + side3) / 2;
            double S = Math.Sqrt((p * (p - side1) * (p - side2) * (p - side3)));
            return S;
        }

        public Triangle()
        {
            side1 = 3;
            side2 = 4;
            side3 = 5;
        }
        public Triangle(double side1, double side2, double side3)
        {
            double[] array = { side1, side2, side3 };
            Array.Sort(array);
            if (array[0] + array[1] >= array[2] && (array[0] > 0 && array[1] > 0 && array[2] > 0))
            {
                this.side1 = side1;
                this.side2 = side2;
                this.side3 = side3;
            }
            else
            {
                throw new ArgumentException("Triangle with these sides doesn't exist!");
            }
        }

        public override string ToString()
        {
            return $"First side of triangle: {side1}\nSecond side of triange: {side2}\nThird side of triange: {side3}\n" +
                $"Square of triangle: {CalculateArea()}";
        }
    }

    class Circle : IFigurable, IComparable, ICloneable
    {
        Point center;
        double radius;

        public double CalculateArea()
        {
            return Math.PI * radius * radius;
        }

        public int CompareTo(object obj)
        {
            Circle other = obj as Circle;
            if (other != null)
            {
                if (this.CalculateArea() > other.CalculateArea())
                {
                    return 1;
                }
                if (this.CalculateArea() < other.CalculateArea())
                {
                    return -1;
                }
                return 0;
            }
            else
            {
                throw new ArgumentException("Parameter is not a Circle!");
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Circle()
        {
            center = new Point();
            this.radius = 4;
        }

        public Circle(double x, double y, double radius)
        {
            center = new Point(x, y);
            this.radius = radius;
        }

        public Circle(Point center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public override string ToString()
        {
            return $"Center of circle: {center}\nRadius of circle: {radius}\nSquare of circle: {CalculateArea()}";
        }

    }
    class Point
    {
        private readonly double x;
        private readonly double y;

        public Point()
        {
            x = 3;
            y = 3;
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"x: {x}, y: {y}";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            IFigurable square = new Square(15);
            IFigurable triangle = new Triangle(9, 13, 17);
            IFigurable circle = new Circle();
            Console.WriteLine(square);
            Console.WriteLine();
            Console.WriteLine(triangle);
            Console.WriteLine();
            Console.WriteLine(circle);

            IFigurable squareForFigures = new Square(10);
            IFigurable triangleForFigures = new Triangle(3, 6, 8);
            IFigurable circleForFigures = new Circle(5, 5, 5);

            IFigurable[] arrayOfFigures = { squareForFigures, triangleForFigures, circleForFigures };
            double sumAreas = 0;
            foreach (IFigurable f in arrayOfFigures)
            {
                sumAreas += f.CalculateArea();
            }
            Console.WriteLine($"\nSum of areas in array: {sumAreas}");

            ;

            IFigurable circle1 = new Circle();
            IFigurable circle2 = new Circle(5, 5, 7);
            IFigurable circle3 = new Circle(1, 1, 1);
            IFigurable circle4 = new Circle(3, 3, 6);
            IFigurable circle5 = new Circle(2, 4, 10);

            IFigurable[] arrayOfCircles = { circle1, circle2, circle3, circle4, circle5 };
            Array.Sort(arrayOfCircles);
            
            for (int i = 0; i < arrayOfCircles.Length; i++)
            {
                Console.WriteLine($"\nSorted by area: \n{arrayOfCircles[i]}\n");
            }
            Circle smallestCircle = (Circle)arrayOfCircles[0];
            Console.WriteLine($"\nThe smallest circle: \n{smallestCircle}");
            Console.WriteLine();
            IFigurable cloneCircle = (Circle)smallestCircle.Clone();
            Console.WriteLine($"Info about cloned circle: \n{cloneCircle}");


            Console.ReadLine();
        }
    }
}
