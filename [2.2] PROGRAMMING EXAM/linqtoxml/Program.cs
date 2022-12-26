using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace linqtoxml
{

    [Serializable()]
    public class Author
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
    }
    [Serializable()]
    public class Article
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Header { get; set; }
    }
    [Serializable()]
    public class BibliographicData
    {
        public int ArticleId { get; set; }
        public int Year { get; set; }
        public int Number { get; set; }
    }

    internal class Program
    {
        static void ToXml(List<Author> objList)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(List<Author>));

            TextWriter Filestream = new StreamWriter(@"Company.xml");

            serialiser.Serialize(Filestream, objList);

            Filestream.Close();
        }

        static void Main(string[] args)
        {
            
            var xmlStr1 = File.ReadAllText("C:\\Users\\user\\Desktop\\author.xml");
            var ms1 = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr1));
            XmlSerializer serializer1 = new XmlSerializer(typeof(List<Author>));
            var authors = ((List<Author>)serializer1.Deserialize(ms1));

            var xmlStr2 = File.ReadAllText("C:\\Users\\user\\Desktop\\article.xml");
            var ms2 = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr2));
            XmlSerializer serializer2 = new XmlSerializer(typeof(List<Article>));
            var articles = ((List<Article>)serializer2.Deserialize(ms2));

            var xmlStr3 = File.ReadAllText("C:\\Users\\user\\Desktop\\bibliographicdata.xml");
            var ms3 = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr3));
            XmlSerializer serializer3 = new XmlSerializer(typeof(List<BibliographicData>));
            var bibliographicsData = ((List<BibliographicData>)serializer3.Deserialize(ms3));

            // Task 1

            var query = from author in authors
                        join article in articles on author.Id equals article.AuthorId
                        select new
                        {
                            ArticleId = article.Id,
                            AuthorSurName = author.Surname,
                            ArticleHeader = article.Header
                        };
            var query2 = from q in query
                         join b in bibliographicsData on q.ArticleId equals b.ArticleId
                         select new
                         {
                             ArticleId = q.ArticleId,
                             AuthorSurName = q.AuthorSurName,
                             ArticleHeader = q.ArticleHeader,
                             ArticleNumber = b.Number
                         };

            using (var w = new StreamWriter("task1.csv"))
            {
                foreach (var item in query2)
                {
                    var line = string.Format("{0} : <{1},{2}>", item.ArticleNumber, item.AuthorSurName, item.ArticleHeader);
                    w.WriteLine(line);
                    w.Flush();
                }
            }
            // Task 3
            string word;
            Console.Write("[TASK 3] Word in article: ");
            word = Console.ReadLine();
            var query3 = from author in authors
                         join article in articles on author.Id equals article.AuthorId
                         select new
                         {
                             AuthorCountry = author.Country,
                             AuthorID = author.Id,
                             ArticleId = article.Id,
                             AuthorSurName = author.Surname,
                             ArticleHeader = article.Header,
                         };
            var query4 = from q in query3
                         join b in bibliographicsData on q.ArticleId equals b.ArticleId
                         select new
                         {
                             AuthorID = q.AuthorID,
                             AuthorCountry = q.AuthorCountry,
                             ArticleId = q.ArticleId, 
                             AuthorSurName = q.AuthorSurName,
                             ArticleHeader = q.ArticleHeader,
                             Year = b.Year,
                             ArticleNumber = b.Number
                         };
            var query5 = query4.Where(x => x.ArticleHeader.Contains(word)).Select(x => new Author {Id = x.AuthorID, Country = x.AuthorCountry, Surname = x.AuthorSurName }).ToList();
            ToXml(query5);

        }
    }
}