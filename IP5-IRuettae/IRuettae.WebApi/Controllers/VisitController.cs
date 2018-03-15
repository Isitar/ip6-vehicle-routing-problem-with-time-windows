using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Persistence;
using NHibernate.Linq;

namespace IRuettae.WebApi.Controllers
{
    public class VisitController : ApiController
    {
        
        public IEnumerable<Visit> Get()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var result = dbSession.Query<Visit>().ToList();
                return result;
            }

        }

        public Visit Get(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                return dbSession.Get<Visit>(id);
            }
        }

        
        public void Post([FromBody]Visit visit)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    dbSession.Merge(visit);
                    transaction.Commit();
                }
            }
        }

        
        public void Put(long id, [FromBody]Visit visit)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                using (var transaction = dbSession.BeginTransaction())
                {
                    var origVisit = dbSession.Get<Visit>(id);
                    origVisit.NumberOfChildrean = visit.NumberOfChildrean;
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
                }
            }
        }
    }
}
