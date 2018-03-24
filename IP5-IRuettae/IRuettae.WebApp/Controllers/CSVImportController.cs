using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using IRuettae.WebApp.Properties;

namespace IRuettae.WebApp.Controllers
{
    public class CSVImportController : Controller
    {
        // GET: CSVImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadCSV(HttpPostedFileBase file)
        {
            string csvText = string.Empty;
            using (BinaryReader b = new BinaryReader(file.InputStream))
            {
                byte[] binData = b.ReadBytes(file.ContentLength);
                csvText = System.Text.Encoding.UTF8.GetString(binData);
            }

            var encodedContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", csvText)
            });

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.WebAPIBaseUrl);
                var response = client.PostAsync("api/CSVImportApi", encodedContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("Request", "mit dem Request ist etwas schief gelaufen " + response.StatusCode);
                    return View("Index");
                }
            }
        }
    }
}