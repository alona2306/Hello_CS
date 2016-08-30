using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Hello_CS
{
    class Program
    {
        struct sFood
        {
            string strFoodName;
            uint uiNDB;
        }

        static void Main(string[] args)
        {
            //var client = new WebClient();
            //var downloadString = client.DownloadString("https://ndb.nal.usda.gov/ndb/foods");

            // The HtmlWeb class is a utility class to get the HTML over HTTP
            var htmlWeb = new HtmlWeb();

            // Creates an HtmlDocument object from an URL
            var document = htmlWeb.Load("https://ndb.nal.usda.gov/ndb/foods?format=&count=&max=10000&sort=&fgcd=&manu=&lfacet=&qlookup=&offset=0&order=desc");
            
            // Targets a specific node
            var foodTable = document.DocumentNode.SelectNodes("//table")[1];

            char[] charsToTrim = {' ', '\t', '\n'};
           
            foreach (var row in foodTable.SelectNodes("//tr"))
            {
                var columns = row.SelectNodes("th|td");

                int id;
                if (!int.TryParse(columns[0].InnerText.Trim(charsToTrim), out id)) continue;
              
                var detailsUrl = columns[0].Element("a").GetAttributeValue("href", "unknown");
                var description = columns[1].InnerText.Trim(charsToTrim);
                var foodgroup = columns[2].InnerText.Trim(charsToTrim);

                Console.WriteLine(string.Format("ID: {0}\tDescription: {1}\tFood group: {2}\nUrl: https://ndb.nal.usda.gov{3}\n\n", id, description, foodgroup, detailsUrl));
            }



            //Console.WriteLine(downloadString);
            Console.ReadKey(); // wait for key-press to prevent windows from closing
        }
    }
}
