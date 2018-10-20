using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using IRuettae.WebApp.Models;
using IRuettae.WebApp.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IRuettae.WebApp.Controllers
{
    public class AlgorithmController : Controller
    {
        private static readonly HttpClient Client = new HttpClient() { BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl), Timeout = TimeSpan.FromHours(1) };
        private static readonly string ConfigPath = HostingEnvironment.MapPath($"~/App_Data/AlgorithmStarterConfig.json");
        public ActionResult Index()
        {
            var model = new AlgorithmStarterVM();
            var response = Client.GetAsync("api/visit").Result;
            var visits = JArray.Parse(response.Content.ReadAsStringAsync().Result).ToObject<VisitVM[]>();
            
            if (System.IO.File.Exists(ConfigPath))
            {
                model = JsonConvert.DeserializeObject<AlgorithmStarterVM>(System.IO.File.ReadAllText(ConfigPath));
            }
            model.StarterIds = visits.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.ToString()
            });

            return View(model);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CalculateRouteAsync(AlgorithmStarterVM asvm)
        {

            asvm.DaysPeriod = asvm.DaysPeriod.Where(d => d.Start.HasValue).ToList();
            // Write back to settings
            if (System.IO.File.Exists(ConfigPath))
            {
                System.IO.File.Delete(ConfigPath);
            }
            System.IO.File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(asvm));


            asvm.Days = asvm.DaysPeriod.Select(p => (p.Start.Value, p.End.Value)).ToList();
            asvm.Beta0 -= asvm.TimePerChild;
            //            Client.Timeout = TimeSpan.FromHours(10);
            var result = await Client.PostAsJsonAsync("api/algorithm/StartRouteCalculation", asvm);
            return RedirectToAction("Results");
        }

        public async Task<ViewResult> Results()
        {
            var result = await Client.GetAsync("api/algorithm/RouteCalculations");
            var routeCalculations = JArray.Parse(result.Content.ReadAsStringAsync().Result).ToObject<RouteCalculationVM[]>().OrderByDescending(rc => rc.StartTime);
            foreach (var routeCalculationVM in routeCalculations)
            {
                routeCalculationVM.StateTextDisplay = routeCalculationVM.StateText.LastOrDefault()?.Log;
            }
            return View(routeCalculations);
        }

        public async Task<ViewResult> Result(long id)
        {
            var result = await Client.GetAsync("api/algorithm/RouteCalculationWaypoints?id=" + id);
            var routeCalculationWaypointVms = JsonConvert.DeserializeObject<List<RouteCalculationWaypointVM[]>>(result.Content.ReadAsStringAsync().Result);
            ViewBag.apiKey = Properties.Settings.Default.GoogleMapsApiKey;
            return View(routeCalculationWaypointVms);
        }

        public async Task<ViewResult> Compare(long[] ids)
        {
            var result = await Client.GetAsync("api/algorithm/RouteCalculations");
            var routeCalculations = JArray.Parse(result.Content.ReadAsStringAsync().Result).ToObject<RouteCalculationVM[]>().Where(rcvm => ids.Contains(rcvm.Id)).OrderByDescending(rc => rc.StartTime);
            return View(routeCalculations);
        }
    }
}