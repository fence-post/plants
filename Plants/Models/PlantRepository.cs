using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fp.lib;
using fp.lib.mysql;

namespace Plants.Models
{
    public class PlantRepository
    {
        public PlantAttributeList GetPlantTypes()
        {
            return new PlantAttributeList(QMySql.SqlList(@"
select distinct plantType from plants order by 1"));
        }

        public PlantAttributeList GetHabitats()
        {
            return new PlantAttributeList(QMySql.SqlList(@"
select name from habitats order by 1"));
        }

        public IEnumerable<string> GetUses()
        {
            return QMySql.SqlList(@"
select name from uses order by 1");
        }

        public IEnumerable<string> GetSoilPreferences()
        {
            return QMySql.SqlList(@"
select name from soilPreferences order by 1");
        }

        public IEnumerable<string> GetShadePreferences()
        {
            return QMySql.SqlList(@"
select name from shadePreferences order by 1");
        }

        public PlantsByAttributeViewModel GetPlantsByType()
        {
            return GetPlantsByAttribute("PlantsByType", "", GetPlantTypes(), @"
select * from plants where plantType=@1");

        }

        public PlantsByAttributeViewModel GetPlantsByUse(string query = "")
        {
            return GetPlantsByAttribute("PlantsByUse", query, GetUses(), @"
select * from plants where id in 
(select p.plant_id from plantUse p
inner join uses h on h.id=p.use_id 
where h.name=@1)");

        }

        public PlantsByAttributeViewModel GetPlantsBySoilPreference(string query = "")
        {
            return GetPlantsByAttribute("PlantsBySoilPreference", query, GetSoilPreferences(), @"
select * from plants where id in 
(select p.plant_id from plantSoilPreference p
inner join soilPreferences h on h.id=p.soilPreference_id 
where h.name=@1)");

        }

        public PlantsByAttributeViewModel GetPlantsByShadePreference(string query = "")
        {
            return GetPlantsByAttribute("PlantsByShadePreference", query, GetShadePreferences(), @"
select * from plants where id in 
(select p.plant_id from plantShadePreference p
inner join shadePreferences h on h.id=p.shadePreference_id 
where h.name=@1)");

        }
        public PlantsByAttributeViewModel GetPlantsByHabitat(string query = "")
        {
            return GetPlantsByAttribute("PlantsByHabitat", query, GetHabitats(), @"
select * from plants where id in 
(select p.plant_id from plantHabitat p
inner join habitats h on h.id=p.habitat_id 
where h.name=@1)");
        }

        public PlantsByAttributeViewModel GetPlantsByAttribute(string method, string query, IEnumerable<string> types, string sql)
        {
            PlantsByAttributeViewModel result = new PlantsByAttributeViewModel(method);

            foreach (string typ in types)
            {
                if (query == "" || query == typ)
                {
                    PlantsByAttribute p = new PlantsByAttribute();
                    p.attributeId = 0;
                    p.attributeName = typ;
                    p.plantSummaries = GetPlantSummaries(sql, typ);
                    result.attributes.Add(p);
                }
            }

            return result;
        }

        public string GetFirstImage(int plantId)
        {
            return QMySql.SqlString("select image from plantImages where plant_id=" + plantId + " order by seq");
        }

        public IEnumerable<PlantSummary> GetPlantSummaries(string sql, string typ)
        {
            List<PlantSummary> result = new List<PlantSummary>();

            using (QMySql s = new QMySql())
            {
                s.Open(sql, typ);
                while (s.GetRow())
                {
                    PlantSummary p = new PlantSummary();
                    QObject.PopulateFromRow(s, p);
                    p.image = GetFirstImage(p.id);
                    result.Add(p);
                }
            }

            return result;
        }

        public Plant GetPlant(int id)
        {
            using (QMySql s = new QMySql())
            {
                s.Open(@"
select * from plants where id=@1", id);
                if (s.GetRow())
                {
                    Plant p = new Plant();
                    QObject.PopulateFromRow(s, p);

                    p.habitats = QMySql.SqlList(@"
select h.name 
from habitats h
inner join plantHabitat p on p.habitat_id=h.id
where p.plant_id=" + id).ToList();

                    p.uses = QMySql.SqlList(@"
select h.name 
from uses h
inner join plantUse p on p.use_id=h.id
where p.plant_id=" + id).ToList();

                    p.soilPreferences = QMySql.SqlList(@"
select h.name 
from soilPreferences h
inner join plantSoilPreference p on p.soilPreference_id=h.id
where p.plant_id=" + id).ToList();

                    p.shadePreferences = QMySql.SqlList(@"
select h.name 
from shadePreferences h
inner join plantShadePreference p on p.shadePreference_id=h.id
where p.plant_id=" + id).ToList();
                    p.images = QMySql.SqlList(@"
select p.image 
from plantImages p
where p.plant_id=" + id + " order by p.seq").ToList();

                    p.heatMapData = GetHeatMapData(p.habitats);

                    return p;
                }
            }

            throw new ArgumentException();
        }

        string GetHeatMapData(List<string> habitats)
        {
            string result = "";

            foreach(string h in habitats)
            {
                switch(h)
                {
                    case "Bottomland Forests":
                    case "Mixed Hardwood-Conifer Forest or Woodland":
                    case "Shallow Marsh":
                        result = result.AppendTo(HeatMapData.Wet, ",");
                        break;
                    case "Oak woodland":
                    case "Wet Prairie":
                        result = result.AppendTo(HeatMapData.Upland, ",");
                        break;
                    case "Upland Prairie and Savanna":
                    case "Shrub Swamp":
                    case "Riparian Forests":
                        result = result.AppendTo(HeatMapData.Prarie, ",");
                        break;
                }

                result = result.Replace("\r\n", "");
                result = result.Replace("\n", "");

            }

            return result;
        }
    }

    public class PlantRepositoryObject : QObject
    {

    }

    public class PlantAttributeList : List<string>
    {
        public PlantAttributeList(IEnumerable<string> items)
        {
            foreach (string s in items)
                Add(s);
        }
    }

}
