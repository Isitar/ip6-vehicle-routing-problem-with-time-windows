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
        // GET: Visit
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVisit(VisitVM v)
        {
            v.Desired.RemoveAll(periodVM => periodVM.Start == null && periodVM.End == null);
            v.Unavailable.RemoveAll(periodVM => periodVM.Start == null && periodVM.End == null);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl);
                var response = client.PostAsJsonAsync("api/address/CheckAddress", v).Result;
                var respCode = response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(nameof(v.Street), "Der Ort konnte nicht von Google gefunden werden, bitte alternative Adresse oder Koordinaten angeben.");
                    v.AlternativeAddressNeeded = true;
                }
            }

            if (!ModelState.IsValid)
            {
                return View("Index", v);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl);
                var response = client.PostAsJsonAsync("api/visit", v).Result;
                if (response.IsSuccessStatusCode)
                {
                    return View("Thanks", v);
                }
                else
                {
                    ModelState.AddModelError("Request", "mit dem Request ist etwas schief gelaufen " + response.StatusCode);
                    return View("Index", v);
                }
            }
        }

        /// <summary>
        /// helper method for ajax calls
        /// </summary>
        /// <param name="zip"></param>
        /// <returns>json encoded string array</returns>
        [HttpPost]
        public string CityFromZip(int zip)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl);
                var response = client.PostAsync($"api/address/CityFromZip/{zip}", null).Result;
                var retVal = JArray.Parse(response.Content.ReadAsStringAsync().Result).ToObject<string[]>();
               
               return System.Web.Helpers.Json.Encode(retVal);
            }
        }
    }
}