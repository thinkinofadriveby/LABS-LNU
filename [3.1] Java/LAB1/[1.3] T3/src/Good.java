import java.util.Comparator;

public class Good {
    public String name;
    public String producer;
    public double price;

    public Good(String name, String producer, double price) {
        this.name = name;
        this.producer = producer;
        this.price = price;
    }
    public double getPrice() {return price;}

    public static Comparator<Good> PersonNameComparator = (good1, good2) -> {
        double getGood1 = Double.parseDouble(String.valueOf(good1.price));
        double getGood2 = Double.parseDouble(String.valueOf(good2.price));
        return 0;
    };

    protected int compareTo(Good g) {
        return 0;
    }


}
