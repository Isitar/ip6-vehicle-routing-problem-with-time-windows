using System;
using System.Collections.Generic;
using System.Linq;
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
            return "hallo vater";
        }
    }
}