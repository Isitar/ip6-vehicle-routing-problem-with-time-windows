using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        public ActionResult Index()
        {
            var model = new AlgorithmStarterVM();
            var response = Client.GetAsync("api/visit").Result;
            var visits = JArray.Parse(response.Content.ReadAsStringAsync().Result).ToObject<VisitVM[]>();


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
            asvm.Days = asvm.DaysPeriod.Select(p => (p.Start.Value, p.End.Value)).ToList();
            
            //            Client.Timeout = TimeSpan.FromHours(10);
            var result = await Client.PostAsJsonAsync("api/algorithm/StartRouteCalculation", asvm);
            var x = JsonConvert.SerializeObject(asvm);
            return RedirectToAction("Results");
        }

        public async Task<ViewResult> Results()
        {
            var result = await Client.GetAsync("api/algorithm/RouteCalculations");
            var routeCalculations = JArray.Parse(result.Content.ReadAsStringAsync().Result).ToObject<RouteCalculationVM[]>().OrderByDescending(rc => rc.StartTime);
            //foreach (var routeCalculationVM in routeCalculations)
            //{
            //    routeCalculationVM.ClusteringResult = "<p>" + routeCalculationVM.ClusteringResult?.Replace(Environment.NewLine, "</p><p>") + "</p>";
            //    routeCalculationVM.SchedulingResult ="<p>" + routeCalculationVM.SchedulingResult?.Replace(Environment.NewLine, "</p><p>") + "</p>";
            //    routeCalculationVM.Result = "<p>" + routeCalculationVM.Result?.Replace(Environment.NewLine, "</p><p>") + "</p>";
            //}
            return View(routeCalculations);
        }

        public async Task<ViewResult> Result(long id)
        {
            var result = await Client.GetAsync("api/algorithm/RouteCalculationWaypoints?id=" + id);
            var routeCalculationWaypointVms = JsonConvert.DeserializeObject<List<RouteCalculationWaypointVM[]>>(result.Content.ReadAsStringAsync().Result);
            return View(routeCalculationWaypointVms);
        }
    }
}