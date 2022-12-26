public class Main {
    public static void main(String[] args) {
        String someString = "Aboba, grymGavenbaG, gbkapngy";
        someString = someString.toLowerCase();
        String[] arr = someString.split(",");
        char c;
        for (var item : arr) {
            item = item.strip();
            char[] ch = item.toCharArray();
            c = ch[0];
            item = item.replace(c, ' ');
            System.out.println(item);
        }
    }
}


