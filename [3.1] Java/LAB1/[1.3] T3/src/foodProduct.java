public class foodProduct extends Good {
    public double weight;
    public int expirationDate;

    public foodProduct(String name, String producer, double price, double weight, int expirationDate) {
        super(name, producer, price);
        this.weight = weight;
        this.expirationDate = expirationDate;
    }
    public int getExpirationDate() {
        return expirationDate;
    }

    @Override
    public String toString() {
        return "[!] " + getClass().getName() + ": " + name + " " + producer + " " + price + " " + weight + " " + expirationDate;
    }
    @Override
    public int compareTo(Good g) {
        return 0;
    }
}
