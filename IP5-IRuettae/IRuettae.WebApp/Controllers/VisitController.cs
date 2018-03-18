using System;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using IRuettae.WebApp.Models;
using IRuettae.WebApp.Properties;

namespace IRuettae.WebApp.Controllers
{
    public class VisitController : Controller
    {
        // GET: Visit
        public ActionResult Index(VisitVM visitVM = null)
        {
            return visitVM == null ? View() : View(visitVM);
        }

        [HttpPost]
        public ActionResult AddVisit(VisitVM v)
        {
            v.Desired.RemoveAll(periodVM => periodVM.Start == null && periodVM.End == null);
            v.Unavailable.RemoveAll(periodVM => periodVM.Start == null && periodVM.End == null);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(x => x.Count > 0).ToList();
                return View("Index", v);
            }

            
            //  v.Year = DateTime.Now.Year;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl);
                var response = client.PostAsJsonAsync("api/visit", v).Result;
                if (response.IsSuccessStatusCode)
                {
                    return View("Thanks",v);
                }
                else
                {
                    ModelState.AddModelError("Request","mit dem Request ist etwas schief gelaufen " + response.StatusCode);
                    return View("Index", v);
                }
            }
        }
    }
}