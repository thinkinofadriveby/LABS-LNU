public class Clothing extends Good {
    public String fabric;
    public String size;

    public Clothing(String name, String producer, double price, String fabric, String size) {
        super(name, producer, price);
        this.fabric = fabric;
        this.size = size;
    }

    @Override
    public String toString() {
        return "[!] " + getClass().getName() + ": " + name + " " + producer + " " + price + " " + fabric + " " + size;
    }
    @Override
    public int compareTo(Good g) {
        return 0;
    }
}
