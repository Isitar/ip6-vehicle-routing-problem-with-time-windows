using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IRuettae.WebApp.Models;
using IRuettae.WebApp.Properties;
using Newtonsoft.Json.Linq;

namespace IRuettae.WebApp.Controllers
{
    public class AlgorithmController : Controller
    {
        private static readonly HttpClient Client = new HttpClient() { BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl), Timeout = TimeSpan.FromHours(1)};
        
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
        public async System.Threading.Tasks.Task<string> CalculateRouteAsync(AlgorithmStarterVM asvm)
        {
            asvm.Days = asvm.DaysPeriod.Select(p => (p.Start.Value, p.End.Value)).ToList();
//            Client.Timeout = TimeSpan.FromHours(10);
            var result = await Client.PostAsJsonAsync("api/algorithm/StartRouteCalculation", asvm);
            return result.Content.ToString();
        }

        public async Task<ViewResult> RunningProcesses()
        {
            var result = await Client.GetAsync("api/algorithm/RouteCalculations");
            var routeCalculations = JArray.Parse(result.Content.ReadAsStringAsync().Result).ToObject<RouteCalculationVM[]>();
            return View(routeCalculations);
        }
    }
}