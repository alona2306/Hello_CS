using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Hello_CS
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Url(@"https://ndb.nal.usda.gov/ndb/foods");

            var client = new WebClient();
            var downloadString = client.DownloadString("https://ndb.nal.usda.gov/ndb/foods");

            Console.WriteLine();
        }
    }
}
