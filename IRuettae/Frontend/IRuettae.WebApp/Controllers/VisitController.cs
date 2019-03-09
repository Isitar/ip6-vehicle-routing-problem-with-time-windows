using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using IRuettae.WebApp.Models;
using IRuettae.WebApp.Properties;
using Newtonsoft.Json.Linq;

namespace IRuettae.WebApp.Controllers
{
    public class VisitController : Controller
    {
        private static readonly HttpClient Client = new HttpClient() { BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl) };
        // GET: Visit
        public ActionResult Index()
        {
            var retVal = JArray.Parse(Client.GetAsync("api/visit").Result.Content.ReadAsStringAsync().Result).ToObject<VisitVM[]>();
            return View(retVal);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVisit(VisitVM v)
        {
            v.Desired.RemoveAll(periodVM => periodVM.Start == null && periodVM.End == null);
            v.Unavailable.RemoveAll(periodVM => periodVM.Start == null);

            v.Unavailable = v.Unavailable.Select(u => new PeriodVM
            {
                Start = u.Start.Value.Date,
                End = u.Start.Value.Date.AddDays(1).AddSeconds(-1)
            }).ToList();

            var response = Client.PostAsJsonAsync("api/address/CheckAddress", v).Result;
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(nameof(v.Street), "Der Ort konnte nicht von Google gefunden werden, bitte alternative Adresse oder Koordinaten angeben.");
                v.AlternativeAddressNeeded = true;
            }


            if (!ModelState.IsValid)
            {
                return View("Create", v);
            }


            response = Client.PostAsJsonAsync("api/visit", v).Result;
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("Request", "mit dem Request ist etwas schief gelaufen " + response.StatusCode);
                return View("Create", v);
            }

            return View("Thanks", v);
        }

        /// <summary>
        /// helper method for ajax calls
        /// </summary>
        /// <param name="zip"></param>
        /// <returns>json encoded string array</returns>
        [HttpPost]
        public string CityFromZip(int zip)
        {
            var response = Client.PostAsync($"api/address/CityFromZip/{zip}", null).Result;
            var retVal = JArray.Parse(response.Content.ReadAsStringAsync().Result).ToObject<string[]>();

            return System.Web.Helpers.Json.Encode(retVal);

        }

        public ActionResult Delete(long id)
        {
            Client.DeleteAsync($"api/visit/{id}");
            return RedirectToAction("Index");
        }
    }
}