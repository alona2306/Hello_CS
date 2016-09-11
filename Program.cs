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
        public struct sNutrient
        {
            public string name;
            public string units;
            public float valuePer100gram;
        }

        public struct sFood
        {
            public string foodName;
            public string foodGroup;
            public string url;
            // more info
            public List<sNutrient> nutirentList;
        }

        static void Main(string[] args)
        {
            Dictionary<string, sFood> foodDictionary = new Dictionary<string, sFood>();
            //var client = new WebClient();
            //var downloadString = client.DownloadString("https://ndb.nal.usda.gov/ndb/foods");

            // The HtmlWeb class is a utility class to get the HTML over HTTP
            var htmlWeb = new HtmlWeb();

            // Creates an HtmlDocument object from an URL
            //var foodlist = htmlWeb.Load("https://ndb.nal.usda.gov/ndb/foods?format=&count=&max=10000&sort=&fgcd=&manu=&lfacet=&qlookup=&offset=0&order=desc");
            var foodlist = htmlWeb.Load("https://ndb.nal.usda.gov/ndb/foods?format=&count=&max=10&sort=&fgcd=&manu=&lfacet=&qlookup=&offset=0&order=desc");

            // Targets a specific node
            var foodTable = foodlist.DocumentNode.SelectNodes("//table")[1];

            char[] charsToTrim = {' ', '\t', '\n'}; 
           
            foreach (var row in foodTable.SelectNodes("//tr"))
            {
                //th - columns in header row in our case are optional? (columns=cells in regular table)
                var columns = row.SelectNodes("th|td");

                int id;
                //The TryParse method is like the Parse method, except the TryParse method does not throw an exception if the conversion fails
                //The Trim method removes from the current string all leading and trailing chars
                //out - by reference initialized before usage
                if (!int.TryParse(columns[0].InnerText.Trim(charsToTrim), out id)) continue;
              
                var detailsUrl = string.Concat("https://ndb.nal.usda.gov", columns[0].Element("a").GetAttributeValue("href", "unknown"));
                var foodName = columns[1].InnerText.Trim(charsToTrim);
                var foodgroup = columns[2].InnerText.Trim(charsToTrim);

                Console.WriteLine(string.Format("ID: {0}\tDescription: {1}\tFood group: {2}\nUrl: {3}\n\n", id, foodName, foodgroup, detailsUrl));

                var foodReport = htmlWeb.Load(detailsUrl);
                var foodReportTable = foodReport.DocumentNode.SelectNodes("//table")[0].SelectNodes("//tbody")[0];

                sFood foodInfo;
                foodInfo.foodName = foodName;
                foodInfo.foodGroup = foodgroup;
                foodInfo.url = detailsUrl;
                foodInfo.nutirentList = new List<sNutrient>();

                foreach (var nutrient in foodReportTable.SelectNodes("//tr"))
                {
                    var nutrientValue = nutrient.SelectNodes("th|td");
                    if (nutrientValue.Count == 1) continue;

                    //if (!int.TryParse(nutrientValue.)continue;

                    var nutrientName = nutrientValue[0].InnerText.Trim(charsToTrim);
                    var units = nutrientValue[1].InnerText.Trim(charsToTrim);
                    var per100gram = nutrientValue[2].InnerText.Trim(charsToTrim);

                    sNutrient nutrientInfo;
                    nutrientInfo.name = nutrientName;
                    nutrientInfo.units = units;
                    nutrientInfo.valuePer100gram = float.Parse(per100gram);
                    foodInfo.nutirentList.Add(nutrientInfo);

                }

                // add food to dictionary
                foodDictionary.Add(foodName, foodInfo);
            }

            // save dictionary to file

            Console.ReadKey(); // wait for key-press to prevent windows from closing
        }
    }
}
