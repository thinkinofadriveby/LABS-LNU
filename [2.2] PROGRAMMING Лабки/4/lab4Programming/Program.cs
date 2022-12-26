using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4Programming
{
    public abstract class Production
    {
        protected string title;
        protected double price;
        
        protected Production(string title, double price)
        {
            this.title = title;
            this.price = price;
        }

        public abstract void localizationEN();
        public abstract void localizationUA();
    }

    class Food: Production
    {
        private DateTime expirationDate;

        public Food(string title, double price, int day, int month, int year)
            :base(title, price)
        {
            this.expirationDate = new DateTime(year, month, day);
        }

        public override void localizationUA()
        {
            Console.WriteLine($"{title.PadRight(20)}{price.ToString().PadRight(20)}{expirationDate.ToString("dd.MM.yyyy").PadRight(25)}{"---".PadRight(20)}");
        }

        public override void localizationEN()
        {
            System.Globalization.NumberFormatInfo n = (System.Globalization.NumberFormatInfo)System.Globalization.CultureInfo.InvariantCulture.NumberFormat.Clone();
            n.NumberGroupSeparator = ",";
            Console.WriteLine($"{title.PadRight(20)}{price.ToString("#,0.00", n).PadRight(20)}{expirationDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).PadRight(25)}{"---".PadRight(20)}");
        }

        public override string ToString()
        {
            return $"{title}{price}{expirationDate}";
        }
    }

    class HouseholdAppliance: Production
    {
        private int warrantyPeriodInMonths;

        public HouseholdAppliance(string title, double price, int warrantyPeriodInMonths)
            :base(title, price)
        {
            this.warrantyPeriodInMonths = warrantyPeriodInMonths;
        }

        public override void localizationUA()
        {
            Console.WriteLine($"{title.PadRight(20)}{price.ToString().PadRight(20)}{"---".PadRight(25)}{warrantyPeriodInMonths.ToString().PadRight(20)}");
        }

        public override void localizationEN()
        {
            System.Globalization.NumberFormatInfo n = (System.Globalization.NumberFormatInfo)System.Globalization.CultureInfo.InvariantCulture.NumberFormat.Clone();
            n.NumberGroupSeparator = ",";
            Console.WriteLine($"{title.PadRight(20)}{price.ToString("#,0.00", n).PadRight(20)}{"---".PadRight(25)}{warrantyPeriodInMonths.ToString().PadRight(20)}");
        }

        public override string ToString()
        {
            return $"{title}{price}{warrantyPeriodInMonths}";
        }

    }

    class Program
    {
        public static void ShowInfo()
        {
            Console.WriteLine($"{"TITLE".PadRight(20)}{"PRICE".PadRight(20)}{"EXPIRATION DATE".PadRight(25)}{"WARRANTY PERIOD".PadRight(20)}");
        }
        static void Main(string[] args)
        {
            Production cake = new Food("Cake", 17.99, 29, 5, 2022);
            Production coconut = new Food("Coconut", 9.99, 29, 5, 2022);
            Production chicken = new Food("Chicken", 24.99, 11, 5, 2022);
            Production bread = new Food("Bread", 2.99, 2, 5, 2022);
            Production bacon = new Food("Bacon", 7.99, 29, 10, 2022);
            Production lawnMower = new HouseholdAppliance("Lawn Mower", 5449.99, 24);
            Production nailGun = new HouseholdAppliance("Nail Gun", 99.99, 6);
            Production chainsaw = new HouseholdAppliance("Chainsaw", 139.99, 12);
            Production stepladder = new HouseholdAppliance("Stepladder", 274.99, 24);
            Production monkeyWrench = new HouseholdAppliance("Monkeywrench", 49.99, 6);

            Production[] production = new Production[] {cake, coconut, chicken, bread, bacon, lawnMower, nailGun, chainsaw, stepladder, monkeyWrench};

            Console.Write("Enter localization(now available EN or UA): ");
            string locale = Console.ReadLine().ToUpper();

            if (locale == "UA")
            {
                Console.WriteLine("\n");
                ShowInfo();
                foreach (Production product in production)
                {
                    product.localizationUA();
                }
            }
            else if (locale == "EN")
            {
                Console.WriteLine("\n");
                ShowInfo();
                foreach (Production product in production)
                {
                    product.localizationEN();
                }
            }
            else throw new ArgumentException("Such localization does not exist!");

            Console.ReadLine();
        }
    }
}
