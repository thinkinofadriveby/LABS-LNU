import java.util.*;

class Matrix {
    private int m;
    private int n;
    private int[][] matrix;
    Random random = new Random();


    public Matrix(int m, int n) {
        this.m = m;
        this.n = n;
        matrix = new int[m][n];
    }

    public String toString() {
        StringBuilder matrixToStringBuilder = new StringBuilder();
        for (int i = 0; i < matrix.length; i++) {
            for (int j = 0; j < matrix[i].length; j++) {
                matrixToStringBuilder.append(matrix[i][j] + "\t");
            }
            matrixToStringBuilder.append("\n");
        }
        return matrixToStringBuilder.toString();
    }

    public void GenerateRandomMatrix() {
        for (int i = 0; i < matrix.length; i++) {
            for (int j = 0; j < matrix[i].length; j++) {
                matrix[i][j] = random.nextInt(50);
            }
        }
    }
static boolean isPrime(int n)
{
    if (n <= 1)
        return false;

    for (int i = 2; i < n; i++)
        if (n % i == 0)
            return false;

    return true;
}
public List<Integer> PushLargestPrimeNumberInVector() {
    List<Integer> Vector = new ArrayList<>();
    for (int i = 0; i < matrix.length; i++) {
        int maxElement = Integer.MIN_VALUE;
        for (int j = 0; j < matrix[i].length; j++) {
            if (isPrime(matrix[j][i]) && matrix[j][i] > maxElement){
                maxElement = matrix[j][i];
            }
        }
        Vector.add(maxElement);
    }
    return Vector;
}
}

    public class Main {
        public static void main(String[] args) {
            Scanner scanner = new Scanner(System.in);
            System.out.print("Input the number of rows: ");
            int m = Integer.parseInt(scanner.next());
            System.out.print("Input the number of columns: ");
            int n = Integer.parseInt(scanner.next());

            Matrix startMatrix = new Matrix(m, n);
            startMatrix.GenerateRandomMatrix();
            System.out.println("\n   Random matrix: \n" + startMatrix);
            System.out.println("Vector with the highest simple numbers of columns: ");
            System.out.println(startMatrix.PushLargestPrimeNumberInVector());
        }
    }
