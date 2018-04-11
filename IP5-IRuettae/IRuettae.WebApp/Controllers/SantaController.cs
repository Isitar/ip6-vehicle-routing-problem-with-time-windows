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

        // GET: Santa
        public ActionResult Index()
        {
            var response = Client.GetAsync("api/santa").Result;
            var retVal = JArray.Parse(response.Content.ReadAsStringAsync().Result).ToObject<SantaVM[]>();


            return View(retVal);

        }

        // GET: Santa/Details/5
        public ActionResult Details(int id)
        {
            var response = Client.GetAsync("api/santa/" + id).Result;
            var retVal = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToObject<SantaVM>();


            return View(retVal);

        }

        // GET: Santa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Santa/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Santa/Edit/5
        public ActionResult Edit(int id)
        {


            var response = Client.GetAsync("api/santa/" + id).Result;
            response.EnsureSuccessStatusCode();
            var retVal = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToObject<SantaVM>();


            return View(retVal);

        }

        // POST: Santa/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SantaVM santa)
        {
            try
            {
                var response = Client.PutAsJsonAsync("api/santa/" + id, santa).Result;
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Santa/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Santa/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
