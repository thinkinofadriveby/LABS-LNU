using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domZavd
{
    class Matrix
    {
        private int m;
        private int n;
        private int[,] matrix;
        private static Random random = new Random();

        public Matrix()
        {
            m = 0;
            n = 0;
            matrix = new int[m, n];
        }
        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            matrix = new int[m, n];
        }

        public int M
        {
            get
            {
                return m;
            }
        }
        public int N
        {
            get
            {
                return n;
            }
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

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator ==(Matrix FirstMatrix, Matrix SecondMatrix)
        {
            return FirstMatrix.Equals(SecondMatrix);
        }
        public static bool operator !=(Matrix FirstMatrix, Matrix SecondMatrix)
        {
            if ((FirstMatrix.matrix.GetLength(0) != SecondMatrix.matrix.GetLength(0)) || (FirstMatrix.matrix.GetLength(1) != SecondMatrix.matrix.GetLength(1)))
            {
                throw new InvalidOperationException("Error!");
            }
            else
            {
                for (int i = 0; i < FirstMatrix.matrix.GetLength(0); ++i)
                    for (int j = 0; j < FirstMatrix.matrix.GetLength(1); ++j)
                        if (FirstMatrix.matrix[i, j] == SecondMatrix.matrix[i, j])
                            return false;
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Matrix)
            {
                Matrix matrix = (Matrix)obj;
                if (this.M != matrix.M && this.N != matrix.N)
                {
                    return false;
                }
                for (int i = 0; i < this.M; i++)
                {
                    for (int j = 0; j < this.N; j++)
                    {
                        if (this.matrix[i, j] != matrix.matrix[i, j])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
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

        public static Matrix operator +(Matrix FirstMatrix, Matrix SecondMatrix)
        {
            if (FirstMatrix.m == SecondMatrix.m && FirstMatrix.n == SecondMatrix.n)
            {
                Matrix resultMatrix = new Matrix(FirstMatrix.m, FirstMatrix.n);
                for (int i = 0; i < FirstMatrix.m; i++)
                {
                    for (int j = 0; j < FirstMatrix.n; j++)
                    {
                        resultMatrix.matrix[i, j] = FirstMatrix.matrix[i, j] + SecondMatrix.matrix[i, j];
                    }
                }
                return resultMatrix;
            }
            else throw new InvalidOperationException("Different sizes of matrix!");
        }

        public static Matrix operator -(Matrix FirstMatrix, Matrix SecondMatrix)
        {
            if (FirstMatrix.m == SecondMatrix.m && FirstMatrix.n == SecondMatrix.n)
            {
                Matrix resultMatrix = new Matrix(FirstMatrix.m, FirstMatrix.n);
                for (int i = 0; i < FirstMatrix.m; i++)
                {
                    for (int j = 0; j < FirstMatrix.n; j++)
                    {
                        resultMatrix.matrix[i, j] = FirstMatrix.matrix[i, j] - SecondMatrix.matrix[i, j];
                    }
                }
                return resultMatrix;
            }
            else throw new InvalidOperationException("Different sizes of matrix!");
        }

        public static Matrix operator *(Matrix FirstMatrix, Matrix SecondMatrix)
        {
            Matrix resultMatrix = new Matrix(FirstMatrix.m, FirstMatrix.n);
            for (int i = 0; i < FirstMatrix.m; i++)
            {
                for (int j = 0; j < FirstMatrix.n; j++)
                {
                    for (int k = 0; k < SecondMatrix.m; k++)
                        resultMatrix.matrix[i, j] += FirstMatrix.matrix[i, k] * SecondMatrix.matrix[k, j];
                }
            }
            return resultMatrix;
        }

        public static Matrix operator *(Matrix FirstMatrix, int number)
        {
            Matrix MatrixByNumber = new Matrix(FirstMatrix.m, FirstMatrix.n);
            for (int i = 0; i < FirstMatrix.m; i++)
            {
                for (int j = 0; j < FirstMatrix.n; j++)
                {
                    MatrixByNumber.matrix[i, j] = FirstMatrix.matrix[i, j] * number;
                }
            }
            return MatrixByNumber;
        }

        public static Matrix operator *(int number, Matrix FirstMatrix)
        {
            Matrix NumberByMatrix = new Matrix(FirstMatrix.m, FirstMatrix.n);
            for (int i = 0; i < FirstMatrix.m; i++)
            {
                for (int j = 0; j < FirstMatrix.n; j++)
                {
                    NumberByMatrix.matrix[i, j] = number * FirstMatrix.matrix[i, j];
                }
            }
            return NumberByMatrix;
        }

        public int this[int i, int j]
        {
            get { return matrix[i, j]; }
            set { matrix[i, j] = value; }
        }


        class Vector
        {
            private static Random random = new Random();
            private int[] vector;
            private readonly int size;

            public Vector(int size)
            {
                this.size = size;
                vector = new int[size];
            }

            public Vector(int[] newVector)
            {
                this.size = 3;
                vector = new int[size];
                for (int i = 0; i < size; i++)
                {
                    vector[i] = newVector[i];
                }
            }

            public int Size
            {
                get
                {
                    return size;
                }
            }

            public void GenerateRandomVector()
            {
                for (int i = 0; i < vector.GetLength(0); i++)
                {
                    vector[i] = random.Next(-100, 100);
                }
            }

            public override string ToString()
            {
                StringBuilder vectorToStringBuilder = new StringBuilder();
                foreach (int element in vector)
                {
                    vectorToStringBuilder.Append(Convert.ToString(element) + "\t");
                }
                return vectorToStringBuilder.ToString();
            }

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            public static Vector operator +(Vector FirstVector, Vector SecondVector)
            {
                Vector VectorPlusVector = new Vector(FirstVector.Size);
                for (int i = 0; i < FirstVector.Size; i++)
                {
                    VectorPlusVector.vector[i] = FirstVector.vector[i] + SecondVector.vector[i];
                }
                return VectorPlusVector;
            }

            public static Vector operator -(Vector FirstVector, Vector SecondVector)
            {
                Vector VectorMinusVector = new Vector(FirstVector.Size);
                for (int i = 0; i < FirstVector.Size; i++)
                {
                    VectorMinusVector.vector[i] = FirstVector.vector[i] - SecondVector.vector[i];
                }
                return VectorMinusVector;
            }

            public static Vector operator *(Vector FirstVector, Vector SecondVector)
            {
                if (FirstVector.size == SecondVector.size)
                {
                    Vector VectorByVector = new Vector(FirstVector.size);
                    for (int i = 0; i < FirstVector.size; i++)
                    {
                        VectorByVector.vector[i] = FirstVector.vector[i] * SecondVector.vector[i];
                    }
                    return VectorByVector;
                }
                else throw new InvalidOperationException("Different sizes of matrix!");
            }

            public static Vector operator *(Vector FirstVector, int number)
            {
                Vector VectorByNumber = new Vector(FirstVector.Size);
                for (int i = 0; i < FirstVector.Size; i++)
                {
                    VectorByNumber.vector[i] = FirstVector.vector[i] * number;
                }
                return VectorByNumber;
            }

            public static Vector operator *(int number, Vector FirstVector)
            {
                Vector NumberNyVector = new Vector(FirstVector.Size);
                for (int i = 0; i < FirstVector.Size; i++)
                {
                    NumberNyVector.vector[i] = number * FirstVector.vector[i];
                }
                return NumberNyVector;
            }

            public override bool Equals(object obj)
            {
                if (obj != null && obj is Vector)
                {
                    Vector otherVector = (Vector)obj;

                    if (this.Size != otherVector.Size)
                    {
                        return false;
                    }
                    for (int i = 0; i < this.Size; i++)
                    {
                        if (this.vector[i] != otherVector.vector[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }

            public static bool operator !=(Vector FirstVector, Vector SecondVector)
            {
                return !FirstVector.Equals(SecondVector);
            }

            public static bool operator ==(Vector FirstVector, Vector SecondVector)
            {
                return FirstVector.Equals(SecondVector);
            }

            public static Vector operator *(Matrix FirstMatrix, Vector FirstVector)
            {
                if (FirstMatrix.matrix.GetLength(1) == FirstVector.Size)
                {
                    Vector resVect = new Vector(FirstMatrix.matrix.GetLength(1));
                    for (int i = 0; i < FirstMatrix.matrix.GetLength(1); i++)
                        for (int j = 0; j < FirstVector.Size; j++)
                            for (int k = 0; k < FirstVector.Size; k++)
                                resVect.vector[i] += FirstMatrix.matrix[i, k] * FirstVector.vector[k];
                    return resVect;
                }
                else throw new InvalidOperationException("Error!");
            }

            public int this[int i]
            {
                get { return vector[i]; }
                set { vector[i] = value; }
            }

            class Program
            {
                static void Main(string[] args)
                {

                    Console.WriteLine("\t\t\t\t\t\tMATRIX:");
                    Matrix FirstMatrix = new Matrix(3, 3);
                    FirstMatrix.GenerateRandomMatrix();
                    Console.WriteLine($"First matrix: \n{FirstMatrix}");

                    Matrix SecondMatrix = new Matrix(3, 3);
                    SecondMatrix.GenerateRandomMatrix();
                    Console.WriteLine($"Second matrix: \n{SecondMatrix}");

                    Matrix additionOfMatrix = FirstMatrix + SecondMatrix;
                    Console.WriteLine($"Addition of matrix: \n{additionOfMatrix}");

                    Matrix differenceOfMatrix = FirstMatrix - SecondMatrix;
                    Console.WriteLine($"Difference of matrix: \n{differenceOfMatrix}");

                    Matrix substractionOfMatrix = FirstMatrix * SecondMatrix;
                    Console.WriteLine($"Substraction of matrix: \n{substractionOfMatrix}");

                    Matrix MatrixByNumber = FirstMatrix * 5;
                    Console.WriteLine($"First matrix substracted by 5: \n{MatrixByNumber}");

                    Matrix NumberByMatrix = 5 * FirstMatrix;
                    Console.WriteLine($"5 substracted by First matrix: \n{NumberByMatrix}");

                    int getHashCode1 = FirstMatrix.GetHashCode();
                    Console.WriteLine($"Hash code of first matrix: {getHashCode1}");

                    int getHashCode2 = SecondMatrix.GetHashCode();
                    Console.WriteLine($"Hash code of second matrix: {getHashCode2}");

                    bool equalsMatrix = FirstMatrix == SecondMatrix;
                    Console.WriteLine($"\nFirst matrix is identical to second matrix: {equalsMatrix}");

                    bool notEqual = FirstMatrix != SecondMatrix;
                    Console.WriteLine($"\nFirst matrix is not identical to second matrix: {notEqual}");

                    bool EqualsTo = FirstMatrix.Equals(SecondMatrix);
                    Console.WriteLine($"\nFirst matrix equals to second matrix: {EqualsTo}");

                    Matrix Matr = new Matrix(3, 3);
                    Matr.GenerateRandomMatrix();
                    Console.WriteLine($"\nRandom matrix: \n{Matr}");
                    Matr[0, 0] = 10;
                    Console.WriteLine($"Random matrix with changed [0, 0] value: \n{Matr}");

                    int a = Matr[0, 0];
                    Console.WriteLine($"Assigning a variable a to the value of an matrix element: {a}");


                    Console.WriteLine("\n\n\t\t\t\t\t\tVECTOR:");
                    Vector FirstVector = new Vector(3);
                    FirstVector.GenerateRandomVector();
                    Console.WriteLine($"\n\n\nFirst vector: \n{FirstVector}");

                    Vector SecondVector = new Vector(3);
                    SecondVector.GenerateRandomVector();
                    Console.WriteLine($"\nSecond vector: \n{SecondVector}");

                    Vector additionOfVectors = FirstVector + SecondVector;
                    Console.WriteLine($"\nAddition of vectors: \n{additionOfVectors}");

                    Vector VectorMinusVector = FirstVector - SecondVector;
                    Console.WriteLine($"\nDifference of vectors: \n{VectorMinusVector}");

                    Vector VectorByVector = FirstVector * SecondVector;
                    Console.WriteLine($"\nFirst vector substracted by second vector: \n{VectorByVector}");

                    Vector vectorByNumber = FirstVector * 5;
                    Console.WriteLine($"\nFirst vector substracted by 5:\n{vectorByNumber}");

                    Vector numberByVector = 5 * FirstVector;
                    Console.WriteLine($"\n5 substracted by first vector:\n{vectorByNumber}");

                    Vector MatrixByVector = FirstMatrix * FirstVector;
                    Console.WriteLine($"\nFirst matrix substracted by vector: {MatrixByVector}");

                    int getHashCodeFirstVector = FirstVector.GetHashCode();
                    Console.WriteLine($"\nHash code of first vector: {getHashCodeFirstVector}");

                    int getHashCodeSecondVector = SecondVector.GetHashCode();
                    Console.WriteLine($"Hash code of second vector: {getHashCodeSecondVector}");

                    bool equalsVector = FirstVector == SecondVector;
                    Console.WriteLine($"\nFirst vector is identical to second vector: {equalsVector}");

                    bool notEqualVector = FirstVector != SecondVector;
                    Console.WriteLine($"\nFirst vector is not identical to second vector: {notEqualVector}");

                    bool equalsToVector = FirstVector.Equals(SecondVector);
                    Console.WriteLine($"\nFirst vector equals to second vector: {equalsToVector}");


                    Vector Vect = new Vector(3);
                    Vect.GenerateRandomVector();
                    Console.WriteLine($"\nRandom vector: \n{Vect}");
                    Vect[0] = 10;
                    Console.WriteLine($"\nRandom vector with changed [0] value: \n{Vect}");

                    int b = Vect[0];
                    Console.WriteLine($"\nAssigning a variable b to the value of an array element: {b}");
                    

                    Console.ReadLine();
                }
            }

        }
    }
}