using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.CSVImport;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Persistence;

namespace IRuettae.WebApi.Controllers
{
    public class CSVImportController : ApiController
    {
        private readonly string csvImportControllerErrorFile = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/logs/CSVImportController.error.log");
        public void Post([FromBody]string content)
        {
            var csvVisits = Import.StartImport(content);
            var visits = Converter.ToDatabase(csvVisits);
            var managedVisits = new List<Visit>();

            try
            {
                using (var dbSession = SessionFactory.Instance.OpenSession())
                {
                    using (var transaction = dbSession.BeginTransaction())
                    {
                        foreach (var visit in visits)
                        {
                            managedVisits.Add(dbSession.Merge(visit));
                        }
                        transaction.Commit();
                    }
                }

                foreach (var visit in managedVisits)
                {
                    VisitWayCreator.CreateWays(visit);
                }
            }
            catch (Exception e)
            {

                System.IO.File.AppendAllLines(csvImportControllerErrorFile, contents: new[] {
                    "Something went wrong in " +nameof(Post) +": " + e.Message, e.StackTrace
                });

                throw new HttpException("Something went wrong: " + e.Message + "<br />" + e.StackTrace);
            }
        }
    }
}