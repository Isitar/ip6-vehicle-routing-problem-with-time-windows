using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;
using IRuettae.WebApi.Properties;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;

namespace IRuettae.WebApi.Controllers
{
    public class VisitController : ApiController
    {
        public IEnumerable<VisitDTO> Get()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                return dbSession.Query<Visit>().ToList().Select(v => (VisitDTO)v);
            }
        }

        public VisitDTO Get(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                return (VisitDTO)dbSession.Get<Visit>(id);
            }
        }


        public void Post([FromBody]Visit visit)
        {
            try
            {
                using (var dbSession = SessionFactory.Instance.OpenSession())
                {
                    using (var transaction = dbSession.BeginTransaction())
                    {
                        visit = dbSession.Merge(visit);
                        transaction.Commit();
                    }
                }

                // VisitWayCreator.CreateWays(visit);

            }
            catch (Exception e)
            {
                File.AppendAllLines("C:\\temp\\webapp_error.txt", contents: new[] {
                    "Something went wrong: " + e.Message, e.StackTrace});

                throw new HttpException("Something went wrong: " + e.Message + "<br />" + e.StackTrace);
            }
        }

        public void Put(long id, [FromBody]Visit visit)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    var origVisit = dbSession.Get<Visit>(id);
                    origVisit.NumberOfChildren = visit.NumberOfChildren;
                    origVisit.Street = visit.Street;
                    origVisit.Year = visit.Year;
                    origVisit.Zip = visit.Zip;
                    origVisit.ExternalReference = visit.ExternalReference;
                    // ignore times
                    transaction.Commit();
                }
            }
        }


        public void Delete(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    var origVisit = dbSession.Get<Visit>(id);
                    dbSession.Delete(origVisit);
                    transaction.Commit();
                }
            }
        }
    }
}
