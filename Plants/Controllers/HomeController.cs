using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Plants.Models;

namespace Plants.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Plant(int id)
        {
            PlantRepository r = new PlantRepository();
            Plant p = r.GetPlant(id);

            return View(p);
        }
        
        public IActionResult PlantsByHabitat(string id = "")
        {
            ViewData["Title"] = "Plants by Habitat";
            PlantRepository r = new PlantRepository();
            PlantsByAttributeViewModel model = r.GetPlantsByHabitat(id);

            return View(model);
        }

        public IActionResult PlantsByUse(string id = "")
        {
            ViewData["Title"] = "Plants by Special Use";
            PlantRepository r = new PlantRepository();
            PlantsByAttributeViewModel model = r.GetPlantsByUse(id);

            return View(model);
        }

        public IActionResult PlantsBySoilPreference(string id = "")
        {
            ViewData["Title"] = "Plants by Soil Preference";
            PlantRepository r = new PlantRepository();
            PlantsByAttributeViewModel model = r.GetPlantsBySoilPreference(id);

            return View(model);
        }

        public IActionResult PlantsByShadePreference(string id = "")
        {
            ViewData["Title"] = "Plants by Shade Preference";
            PlantRepository r = new PlantRepository();
            PlantsByAttributeViewModel model = r.GetPlantsByShadePreference(id);

            return View(model);
        }
        public IActionResult PlantsByType()
        {
            ViewData["Title"] = "Plants by Plant Type";
            PlantRepository r = new PlantRepository();
            PlantsByAttributeViewModel model = r.GetPlantsByType();

            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
