using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using fp.lib;
using fp.lib.mysql;

namespace GetPlants
{
    class Parse
    {
        public Dictionary<string, string> content = new Dictionary<string, string>();
        public Dictionary<string, string> urls = new Dictionary<string, string>();
        public HashSet<string> soils = new HashSet<string>();

        public Parse()
        {
        }

        public void GetUrls()
        {
            string u = "https://www.bentonswcd.org/resources/native-plants-database/";
            var web = new HtmlWeb();
            var doc = web.Load(u);

            var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'box-wrap')]"); // /@id=dvResults/div
            foreach(var n in nodes)
            {
                content.Clear();
                GetPart(n, "plantType", ".//h2");
                GetSet(n, "url", ".//ul/li", ".//a/@href");
                string type = Content("plantType");
                if(type != "")
                {
                    foreach (string key in content.Keys)
                    {
                        if (key.StartsWith("url"))
                        {
                            string url = content[key];
                            if (!urls.ContainsKey(url))
                            {
                                urls.Add(url, type);
                                Console.WriteLine(type + " " + url);
                            }
                        }
                    }
                }
            }
        }

        public void ParsePlant(string url)
        {
            string type = urls[url];
            content.Clear();
            content.Add("plantType", type);

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var articles = doc.DocumentNode.SelectNodes("//div[contains(@class,'main')]"); // /@id=dvResults/div
            GetSet(articles.First(), "image", ".//ul[@id='image_tabs']/li", ".//img/@src");
            GetSet(articles.First(), "habitat", ".//dl/dt[contains(text(), 'Habitat:')]/following-sibling::dd[1]/a", ".//text()");
            GetPart(articles.First(), "habDescription", ".//dl/dt[contains(text(), 'Habitat:')]/following::dd/text()");
            GetSet(articles.First(), "shadePreference", ".//div[@class='shade-preference']/img", ".//@title");
            GetPart(articles.First(), "specialUses", ".//dl/dt[contains(text(), 'Special Uses:')]/following::dd/text()");
            GetPart(articles.First(), "soilTolerance", ".//dl/dt[contains(text(), 'Soil Tolerance:')]/following::dd/text()");
            GetPart(articles.First(), "matureHeight", ".//dl/dt[contains(text(), 'Mature Height:')]/following::dd/text()");
            GetPart(articles.First(), "description", ".//p");
            GetPart(articles.First(), "commonName", ".//h1");
            GetPart(articles.First(), "scientificName", ".//h2[@class='scientific']");

            soils.AddIfNotExists(Content("soilTolerance"));

        }

        List<string> GetContentValues(string prefix)
        {
            var r =
                from item in content
                where item.Key.StartsWith(prefix)
                select item.Value;

            return r.ToList<string>();
        }

        List<string> GetSoilPreferences(string pref)
        {
            pref = pref.ToLower();

            HashSet<string> result = new HashSet<string>();
            if (pref.Contains("moist"))
                result.AddIfNotExists("Moist");
            if (pref.Contains("dry"))
                result.AddIfNotExists("Dry");
            if (pref.Contains("rich"))
                result.AddIfNotExists("Rich");
            if (pref.Contains("acidic"))
                result.AddIfNotExists("Acidic");
            if (pref.Contains("drained"))
                result.AddIfNotExists("Well Drained");
            if (pref.Contains("good drainage"))
                result.AddIfNotExists("Well Drained");
            if (pref.Contains("rocky"))
                result.AddIfNotExists("Rocky");
            if (pref.Contains("drought"))
                result.AddIfNotExists("Drought Tolerant");

            return result.ToList() ;
        }

        List<string> GetPlantUses(string spacialUses)
        {
            spacialUses = spacialUses.ToLower();

            HashSet<string> result = new HashSet<string>();
            string insectName = "Bees, Butterflies, Pollinators";
            if (spacialUses.Contains("insects"))
                result.AddIfNotExists(insectName);
            if (spacialUses.Contains("bees"))
                result.AddIfNotExists(insectName);
            if (spacialUses.Contains("butterflies"))
                result.AddIfNotExists(insectName);
            if (spacialUses.Contains("pollinators"))
                result.AddIfNotExists(insectName);

            if (spacialUses.Contains("hummingbirds"))
                result.AddIfNotExists("Birds");
            if (spacialUses.Contains("birds"))
                result.AddIfNotExists("Birds");

            if (spacialUses.Contains("drought tolerant"))
                result.AddIfNotExists("Drought Tolerant");

            if (spacialUses.Contains("deer resistant"))
                result.AddIfNotExists("Deer Resistant");

            if (spacialUses.Contains("bank stabilization"))
                result.AddIfNotExists("Bank Stabilization");

            return result.ToList();
        }

        string Content(string key, string def = "")
        {
            if (content.ContainsKey(key))
                return content[key];

            return def;
        }

        public void SavePlant()
        {
            string habitats = "";
            string shades = "";
            string soils = "";
            string uses = "";

            using (QMySql s = new QMySql())
            {
                InsertAttributes(s, "habitats", GetContentValues("habitat"), ref habitats);
                InsertAttributes(s, "shadePreferences", GetContentValues("shadePreference"), ref shades);
                InsertAttributes(s, "soilPreferences", GetSoilPreferences(Content("soilTolerance")), ref soils);
                InsertAttributes(s, "uses", GetPlantUses(Content("specialUses")), ref uses);

                int id = s.Execute(@"
insert into plants (habitatDescription, scientificName, description, specialUses, soilTolerance, matureHeight, 
commonName,plantType) values 
(@1, @2, @3, 
@4, @5, @6, 
@7,@8)
", Content("habDescription"), Content("scientificName"), Content("description"),
Content("specialUses"), Content("soilTolerance"), Content("matureHeight"), 
Content("commonName"), Content("plantType"));

                int minHeight = 0;
                int maxHeight = 0;
                if(GetHeights(Content("matureHeight"), ref minHeight, ref maxHeight))
                {
                    s.Execute(@"
update plants 
set matureHeightMin=@1,matureHeightMax=@2 
where id=@3", minHeight, maxHeight, id);
                }

                if(habitats != "")
                {
                    s.Execute(@"
insert ignore into plantHabitat (plant_id, habitat_id)
select @1 as plant_id, id from habitats where name in (" + habitats + ")", id);
                }

                if(shades != "")
                {
                    s.Execute(@"
insert ignore into plantShadePreference (plant_id, shadePreference_id)
select @1 as plant_id, id from shadePreferences where name in (" + shades + ")", id);
                }

                if(soils != "")
                {
                    s.Execute(@"
insert ignore into plantSoilPreference (plant_id, soilPreference_id)
select @1 as plant_id, id from soilPreferences where name in (" + soils + ")", id);
                }

                if (uses != "")
                {
                    s.Execute(@"
insert ignore into plantUse (plant_id, use_id)
select @1 as plant_id, id from uses where name in (" + uses + ")", id);
                }

                int i = 0;
                List<string> images = GetContentValues("image");
                foreach(string u in images)
                {
                    i++;
                    string fileName = u.Substring(u.LastIndexOf('/') + 1);
                    string filePath = @"C:\dev\GetPlants\GetPlants\images\" + fileName;
                    try
                    {
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(u, filePath);
                        s.Execute(@"
insert ignore into plantImages (plant_id, image, seq) values (@1, @2, @3)", id, fileName, i);
                    }
                    catch (Exception e)
                    {

                    }

                }

            }
        }

        void InsertAttributes(QMySql s, string table, List<string> values, ref string sqlList)
        {
            foreach (string v in values)
            {
                sqlList = sqlList.AppendTo("'" + v + "'", ",");
                s.Execute("insert ignore into " + table + " (name) values (@1)", v);
            }
        }

        bool GetHeights(string val, ref int min, ref int max)
        {
            val = val.ToLettersAndDigits();
            val = val.Trim();
            string[] parts = val.Split(new char[] { ',', ' ' });
            if(parts.Length == 2)
            {
                min = T.ToInt(parts[0]);
                max = T.ToInt(parts[1]);
                if(min > 0 && max > 0)
                    return true;
            }

            return false;
        }

        void GetSet(HtmlNode node, string set, string containerDef, string itemDef)
        {
            int i = 0;
            var nodes = node.SelectNodes(containerDef); // 
            if(nodes != null)
            {
                foreach (var n in nodes)
                {
                    i++;
                    GetPart(n, set + i, itemDef); // 
                }
            }
        }

        public bool GetPart(HtmlNode node, string part, string def)
        {
            try
            {
                string result;

                int at = def.LastIndexOf('/');
                if (def.LastIndexOf('@') == at + 1)
                {
                    string attrib = def.Substring(at + 2);
                    result = node.SelectNodes(def).First().GetAttributeValue(attrib, "");
                }
                else
                {
                    result = node.SelectNodes(def).First().InnerText;
                }

                result = result.Replace("&amp;", "&");
                result = result.Replace("&nbsp;", " ");
                result = result.Replace("\r\n", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("&#8217;", "\'");
                result = result.Replace("&#8242;", " ft");
                result = result.Replace("&#8243;", " in");
                result = result.Replace("&#8211;", "-");
                result = result.Replace("&#8220;", "\"");
                result = result.Replace("&#8221;", "\"");




                result = result.Replace("\t", " ");
                result = ToAscii(result);
                result = result.Replace("    ", " ");
                result = result.Replace("   ", " ");
                result = result.Replace("  ", " ");
                result = result.Replace("  ", " ");
                result = result.Trim();
                result = result.RemoveFromEnd(",");// .TrimEnd(new char[',']);
                result = result.Trim();

                Console.WriteLine(part + ": " + result);
                content.Add(part, result);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("could not get part " + part + " " + e.Message);
            }

            return false;

        }

        string ToAscii(string text)
        {
            string spaces = "";
            string result = "";
            foreach (char c in text)
            {
                int unicode = c;
                if (unicode < 128)
                {
                    result += spaces;
                    spaces = "";
                    result += c;
                }
                else
                {
                    spaces = " ";
                }
            }

            return result.Trim();
        }


    }
}
