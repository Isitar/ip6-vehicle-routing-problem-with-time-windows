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
        public string AddVisit(Visit v)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19259");
                var response = client.PostAsJsonAsync("api/visit", v).Result;
                return response.IsSuccessStatusCode ? "gegangen" : "nope";
            }
        }
    }
}