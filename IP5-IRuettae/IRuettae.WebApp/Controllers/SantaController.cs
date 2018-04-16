using IRuettae.WebApp.Models;
using IRuettae.WebApp.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

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
                var response = Client.DeleteAsync($"api/santa/{id}").Result;
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
            ViewBag.CurrState = ViewState.Create;
            return View("BreakForm", new BreakVM() { SantaId = id });
        }


        [HttpPost]
        public ActionResult CreateBreak(long santaId, BreakVM breakVM)
        {
            ViewBag.CurrState = ViewState.Create;
            ViewBag.SantaId = santaId;
            breakVM.Id = 0L;
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("BreakForm", breakVM);
                }

                var santa = FindSantaVM(santaId);
                santa.Breaks.Add(breakVM);
                var response = Client.PutAsJsonAsync($"api/santa/{santaId}", santa).Result;
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Details", new { id = santaId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Exception", e);
                return View("BreakForm", breakVM);
            }
        }


        [HttpGet]
        public ActionResult EditBreak(long id, long breakId)
        {
            ViewBag.CurrState = ViewState.Edit;
            ViewBag.SantaId = id;
            var santa = FindSantaVM(id);
            var santaBreak = santa.Breaks.FirstOrDefault(b => b.Id == breakId);
            santaBreak.SantaId = id;
            return View("BreakForm", santaBreak);
        }

        [HttpPost]
        public ActionResult EditBreak(long santaId, BreakVM breakVM)
        {
            ViewBag.CurrState = ViewState.Edit;
            ViewBag.SantaId = santaId;
            try
            {
                if (!ModelState.IsValid)
                {
                    var ms = ModelState;
                    return View("BreakForm", breakVM);
                }

                var santa = FindSantaVM(santaId);
                santa.Breaks = santa.Breaks.Select(b => b.Id == breakVM.Id ? breakVM : b).ToList();

                var response = Client.PutAsJsonAsync($"api/santa/{santaId}", santa).Result;
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Details", new { id = santaId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("exception", e);
                return View("BreakForm", breakVM);
            }
        }

        public ActionResult DeleteBreak(long id, long breakId)
        {

            var santa = FindSantaVM(id);
            santa.Breaks = santa.Breaks.Where(b => b.Id != breakId).ToList();
            var response = Client.PutAsJsonAsync($"api/santa/{id}", santa).Result;
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Details", new { id = id });
        }
    }
}
