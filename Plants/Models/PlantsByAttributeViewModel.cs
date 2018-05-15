using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.Models
{
    public class PlantsByAttributeViewModel
    {
        public string method;
        public List<PlantsByAttribute> attributes = new List<PlantsByAttribute>();

        public PlantsByAttributeViewModel(string t)
        {
            method = t;
        }
    }

    public class PlantsByAttribute
    {
        public string attributeName { get; set; }
        public int attributeId { get; set; }
        public IEnumerable<PlantSummary> plantSummaries;
    }
}
