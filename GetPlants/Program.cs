using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fp.lib.mysql;

namespace GetPlants
{
    class Program
    {
        static void Main(string[] args)
        {

            QMySql.Connect("192.168.0.105", "plants", "dev", "willys");
            QMySql.Exec("delete from plantImages;delete from plantShadePreference;delete from plantSoilPreference;delete from plantHabitat;delete from plants");

            Parse g = new Parse();
            g.GetUrls();
            // Console.ReadLine();

            foreach(string u in g.urls.Keys)
            {
                g.ParsePlant(u);
                g.SavePlant();
                Console.WriteLine("");                
                // Console.ReadLine();
            }

            foreach (string s in g.soils)
                Console.WriteLine(s);
            Console.ReadLine();

        }
    }
}
