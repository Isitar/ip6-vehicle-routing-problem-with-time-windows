﻿using System;
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
        public void Post([FromBody]String content)
        {
            var csvVisits = Preprocessing.CSVImport.Import.StartImport((string)content);
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
                System.IO.File.AppendAllLines("C:\\temp\\webapp_error.txt", contents: new[] {
                    "Something went wrong: " + e.Message, e.StackTrace});

                throw new HttpException("Something went wrong: " + e.Message + "<br />" + e.StackTrace);
            }
        }
    }
}