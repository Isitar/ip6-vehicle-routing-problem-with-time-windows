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
using static IRuettae.WebApp.Models.RouteCalculationVM;

namespace IRuettae.WebApp.Controllers
{
    public class AlgorithmController : Controller
    {
        private static readonly HttpClient Client = new HttpClient() { BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl), Timeout = TimeSpan.FromHours(1) };
        private static readonly string ConfigPath = HostingEnvironment.MapPath($"~/App_Data/AlgorithmStarterConfig.json");
        public ActionResult Index()
        {
            var model = new AlgorithmStarterVM();
            var visitResponse = Client.GetAsync("api/visit").Result;
            var visits = JArray.Parse(visitResponse.Content.ReadAsStringAsync().Result).ToObject<VisitVM[]>();
            var santaResponse = Client.GetAsync("api/santa").Result;
            var santas = JArray.Parse(santaResponse.Content.ReadAsStringAsync().Result).ToObject<SantaVM[]>();

            
            if (System.IO.File.Exists(ConfigPath))
            {
                model = JsonConvert.DeserializeObject<AlgorithmStarterVM>(System.IO.File.ReadAllText(ConfigPath));
            }

            model.StarterIds = visits.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.ToString()
            });
            model.AlgorithmTypes = new[] {
                AlgorithmType.Hybrid,
                AlgorithmType.LocalSolver,
                AlgorithmType.GeneticAlgorithm,
                AlgorithmType.GoogleRouting,
                AlgorithmType.ILP,
            }.Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = t.ToString()
            });


            var possibleYears = visits.Select(v => v.Year).Distinct();
            model.PossibleYears = possibleYears;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CalculateRouteAsync(AlgorithmStarterVM asvm)
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