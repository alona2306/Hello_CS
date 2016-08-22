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
        struct sFood
        {
            string strFoodName;
            uint uiNDB;
        }

        static void Main(string[] args)
        {
            var client = new WebClient();
            var downloadString = client.DownloadString("https://ndb.nal.usda.gov/ndb/foods");

            Console.WriteLine(downloadString);
            Console.ReadKey(); // wait for key-press to prevent windows from closing
        }
    }
}
