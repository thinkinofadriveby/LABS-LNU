import java.util.Arrays;
import java.util.Comparator;
import java.util.Objects;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Good[] goods = {
                new foodProduct("ice-cream", "Limo", 20.75, 200.00, 90),
                new foodProduct("chips", "Lux", 34.99, 130.00, 360),
                new foodProduct("ham", "MeatProdInc", 240.25, 1000.00, 28),
                new foodProduct("cookies", "CerealInc", 24.75, 300.00, 720),
                new Clothing("jacket", "JaInc", 2200.00, "baloon", "L"),
                new Clothing("pants", "AbobaProduction", 660.00, "flax", "L"),
                new Clothing("hoodie", "Mointain", 740.00, "cotton", "S"),
                new Clothing("coat", "Abibas", 3400.00, "leather", "XL"),
                new Clothing("t-shirt", "Abinet", 300.00, "flax", "M"),
        };
        for (var good : goods) {
            System.out.println(good);
        }

        System.out.println("\nSorted by price: ");
        Arrays.sort(goods, Comparator.comparing(Good::getPrice));
        for (var good : goods) {
            System.out.println(good);
        }


        int k = 0;
        for (var good: goods) {
            if(good.getClass().getName().equals("Clothing")){
                k++;
            }
        }

        Clothing[] cloth = new Clothing[k];
        int j = 0;
        for (int i = 0; i < goods.length; i++) {
            if(goods[i].getClass().getName().equals("Clothing")){
                cloth[j] = (Clothing) goods[i];
                j++;
            }
        }
        Scanner scanner = new Scanner(System.in);
        System.out.print("\nInput size: ");
        String input = scanner.next().toUpperCase();
        for (var cl : cloth) {
            if (Objects.equals(input, cl.size)) {
                System.out.println(cl);
            }
        }

        int m = 0;
        for (var good: goods) {
            if(good.getClass().getName().equals("foodProduct")){
                m++;
            }
        }


        foodProduct[] fd = new foodProduct[m];
        int g = 0;
        for (int i = 0; i < goods.length; i++) {
            if(goods[i].getClass().getName().equals("foodProduct")){
                fd[g] = (foodProduct) goods[i];
                g++;
            }
        }
        Arrays.sort(fd, Comparator.comparing(foodProduct::getExpirationDate).reversed());

        int p = 0;
        System.out.println("\nProducts with the maximum expiration date: ");
        for (var item : fd) {
            System.out.println(item);
            p++;
            if (p == 2){
                break;
            }
        }


    }

}