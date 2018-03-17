using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using IRuettae.Persistence.Entities;

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
        public ActionResult AddVisit(Visit v)
        { 
            v.Year = DateTime.Now.Year;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19259");
                var response = client.PostAsJsonAsync("api/visit", v).Result;
                if (response.IsSuccessStatusCode)
                {
                    return View("Thanks",v);
                }
                else
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
        }
    }
}