using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRuettae.WebApi.Persistence;
using IRuettae.Preprocessing.CSVImport;
using IRuettae.Persistence.Entities;

namespace IRuettae.WebApi.Controllers
{
    public class CSVImportController : Controller
    {
        public void Post(String content)
        {
            var csvVisits = Preprocessing.CSVImport.Import.StartImport(content);
            var visits = Converter.ToDatabase(csvVisits, );
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
            catch (Exception e)
            {
                System.IO.File.AppendAllLines("C:\\temp\\webapp_error.txt", contents: new[] {
                    "Something went wrong: " + e.Message, e.StackTrace});

                throw new HttpException("Something went wrong: " + e.Message + "<br />" + e.StackTrace);
            }
        }
    }
}