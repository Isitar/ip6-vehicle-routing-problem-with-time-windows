using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using IRuettae.WebApp.Models;
using IRuettae.WebApp.Properties;
using Newtonsoft.Json.Linq;

namespace IRuettae.WebApp.Controllers
{
    public class SantaController : Controller
    {
        private static readonly HttpClient Client = new HttpClient() { BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl) };

        private SantaVM FindSantaVM(long id)
        {
            var response = Client.GetAsync("api/santa/" + id).Result;
            response.EnsureSuccessStatusCode();
            return JObject.Parse(response.Content.ReadAsStringAsync().Result).ToObject<SantaVM>();
        }

        // GET: Santa
        public ActionResult Index()
        {
            var response = Client.GetAsync("api/santa").Result;
            var retVal = JArray.Parse(response.Content.ReadAsStringAsync().Result).ToObject<SantaVM[]>();


            return View(retVal);

        }

        // GET: Santa/Details/5
        public ActionResult Details(long id)
        {



            return View(FindSantaVM(id));

        }

        // GET: Santa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Santa/Create
        [HttpPost]
        public ActionResult Create(SantaVM santa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(santa);
                }

                var response = Client.PostAsJsonAsync("api/santa", santa).Result;
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(santa);
            }
        }

        // GET: Santa/Edit/5
        public ActionResult Edit(long id)
        {
            return View(FindSantaVM(id));
        }

        // POST: Santa/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, SantaVM santa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(santa);
                }

                var response = Client.PutAsJsonAsync("api/santa/" + id, santa).Result;
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(santa);
            }
        }

        // GET: Santa/Delete/5
        public ActionResult Delete(long id)
        {
            return View(FindSantaVM(id));
        }

        // POST: Santa/Delete/5
        [HttpPost]
        public ActionResult Delete(long id, SantaVM santa)
        {
            try
            {
                var response = Client.DeleteAsync("api/santa/" + id).Result;
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(santa);
            }
        }

        [HttpGet]
        public ActionResult CreateBreak(long id)
        {
            return View(new BreakVM() { SantaId = id });
        }

        [HttpPost]
        public ActionResult CreateBreak(long id, BreakVM breakVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(breakVM);
                }

                var santa = FindSantaVM(id);
                santa.Breaks.Add(breakVM);
                var response = Client.PostAsJsonAsync("api/santa", santa).Result;
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(breakVM);
            }
        }
    }
}
