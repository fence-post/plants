using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.Models
{
    public class PlantSummary
    {
        public int id { get; set; }
        public string commonName { get; set; }
        public string scientificName { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
}
