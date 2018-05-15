using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.Models
{
    public class Plant : PlantRepositoryObject
    {
        public int id { get; set; }
        public string commonName { get; set; }
        public string scientificName { get; set; }
        public string description { get; set; }
        public string specialUses { get; set; }
        public string soilTolerance { get; set; }
        public string matureHeight { get; set; }
        public int matureHeightMin { get; set; }
        public int matureHeightMax { get; set; }
        public string habitatDescription { get; set; }
        public string heatMapData { get; set; }

        public List<string> images = new List<string>();
        public List<string> habitats = new List<string>();
        public List<string> soilPreferences = new List<string>();
        public List<string> shadePreferences = new List<string>();
        public List<string> uses = new List<string>();
    }
}
