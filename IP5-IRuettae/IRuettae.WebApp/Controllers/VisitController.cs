using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using IRuettae.Persistence.Entities;
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
                    return Redirect(Request.UrlReferrer?.ToString());
                }
            }
        }
    }
}