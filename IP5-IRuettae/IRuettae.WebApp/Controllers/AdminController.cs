using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using IRuettae.WebApp.Properties;

namespace IRuettae.WebApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public HttpStatusCodeResult CleanDatabase()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl);
                var response = client.PostAsync("api/admin/CleanDatabase",null).Result;
                if (response.IsSuccessStatusCode)
                {
                    return new HttpStatusCodeResult(204);
                }
                else
                {
                    return new HttpStatusCodeResult(500);
                }
            }
        }
    }
}