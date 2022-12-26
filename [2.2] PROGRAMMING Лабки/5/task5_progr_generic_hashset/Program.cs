using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task5_progr_generic_hashset
{

    class Table<TRow, TColumn, TValue>
    {
        public Dictionary<TRow, Dictionary<TColumn, TValue>> FootballTable 
        { 
            get; 
            set; 
        }

        public Table()
        {
            FootballTable = new Dictionary<TRow, Dictionary<TColumn, TValue>>();
        }

        public void Add(TRow row, TColumn column, TValue value)
        {
            Dictionary<TColumn, TValue> val = new Dictionary<TColumn, TValue>()
            {
                {column, value }
            };
           
            FootballTable.Add(row, val);
        }

        public TValue GetValue(TRow row, TColumn column)
        {
            if (FootballTable[row].ContainsKey(column))
            {
                return FootballTable[row][column];
            }
            else
            {
                return default(TValue);
            }
        }


        public TValue this[TRow row, TColumn column]
        {
            get
            {
                return GetValue(row, column);
            }
            set 
            { 
                Add(row, column, value); 
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in FootballTable)
            {
                stringBuilder.Append(item.Key.ToString() + "\n");
                foreach (var value in FootballTable[item.Key])
                {
                    stringBuilder.Append(value.Key.ToString() + "\n");
                }

            }

            return stringBuilder.ToString();
        }

    }

    class FootballTeam
    {
        private string foundationYear;
        private string city;
        private string title;


        public FootballTeam(string title, string city, string foundationYear)
        {
            Title = title;
            City = city;
            FoundationYear = foundationYear;
        }


        public string Title
        {
            get 
            { 
                return title; 
            }
            set 
            { 
                title = value; 
            }
        }

        public string City
        {
            get 
            { 
                return city;
            }
            set
            {
                city = value;
            }
        }
        public string FoundationYear
        {
            get
            { 
                return foundationYear; 
            }
            set
            {
                foundationYear = value;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is FootballTeam otherTeam
                && Title == otherTeam.Title
                && City == otherTeam.City
                && FoundationYear == otherTeam.FoundationYear;
        }

        public override string ToString()
        {
            return $"{Title} {City} {FoundationYear}";
        }
    }

    class Tournament
    {
        private string title;
        private bool international;
        private string foundationYear;


        public Tournament(string title, bool international, string foundationYear)
        {
            Title = title;
            International = international;
            FoundationYear = foundationYear;
        }


        public string Title
        {
            get
            { 
                return title;
            }
            set
            { 
                title = value; 
            }
        }

        public bool International
        {
            get 
            { 
                return international;
            }
            set 
            { 
                international = value;
            }
        }

        public string FoundationYear
        {
            get
            {
                return foundationYear;
            }
            set
            {
                foundationYear = value;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Tournament otherTournament
                && Title == otherTournament.Title
                && International == otherTournament.International
                && FoundationYear == otherTournament.FoundationYear;
        }

        public override string ToString()
        {
            return $"{Title} {International} {FoundationYear}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FootballTeam team1 = new FootballTeam("Chelsea", "London", "1905");
            FootballTeam team2 = new FootballTeam("Manchester City", "Manchester", "1880");
            Tournament tournament1 = new Tournament("Confederation Cup", false, "1996");
            Tournament tournament2 = new Tournament("FIFA", true, "2012");


            Table<FootballTeam, Tournament, HashSet<int>> table = new Table<FootballTeam, Tournament, HashSet<int>>();

            table.Add(team1, tournament1, new HashSet<int>() { 1987, 1998});
            table.Add(team2, tournament2, new HashSet<int>() { 2012 });


            foreach (var n in table[team1, tournament1])
            {
                Console.WriteLine(n);
            }

            Console.ReadLine();
        }
    }
}
