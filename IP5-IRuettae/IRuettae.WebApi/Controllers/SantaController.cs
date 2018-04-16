using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;

namespace IRuettae.WebApi.Controllers
{
    public class SantaController : ApiController
    {
        private readonly string santaControllerErrorFile = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/logs/SantaController.error.log");

        // GET: api/Santa
        public IEnumerable<SantaDTO> Get()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                return dbSession.Query<Santa>().ToList().Select(s => (SantaDTO)s);
            }
        }

        // GET: api/Santa/5
        public SantaDTO Get(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                return (SantaDTO)dbSession.Get<Santa>(id);
            }
        }

        // POST: api/Santa
        public void Post([FromBody]Santa value)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                dbSession.Merge(value);
                transaction.Commit();
            }
        }

        // PUT: api/Santa/5
        public void Put(long id, [FromBody]Santa putSanta)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                var mgtSanta = dbSession.Get<Santa>(id);
                mgtSanta.Name = putSanta.Name;
                var deletedBreaks = mgtSanta.Breaks.Where(b => !putSanta.Breaks.Select(pb => pb.Id).Contains(b.Id)).ToList();
                foreach (var deletedBreak in deletedBreaks)
                {
                    mgtSanta.Breaks.Remove(deletedBreak);
                }

                foreach (var existingBreak in putSanta.Breaks.Where(b => mgtSanta.Breaks.Select(mb => mb.Id).Contains(b.Id)))
                {
                    var mgtBreak = mgtSanta.Breaks.FirstOrDefault(b => b.Id == existingBreak.Id);

                    mgtBreak.City = existingBreak.City;
                    mgtBreak.DeltaWayDistance = existingBreak.DeltaWayDistance;
                    mgtBreak.DeltaWayDuration = existingBreak.DeltaWayDuration;
                    mgtBreak.Duration = existingBreak.Duration;
                    mgtBreak.OriginalStreet = existingBreak.OriginalStreet;
                    mgtBreak.Street = existingBreak.Street;
                    mgtBreak.Year = existingBreak.Year;
                    mgtBreak.Zip = existingBreak.Zip;
                }

                foreach (var newBreak in putSanta.Breaks.Where(b => !mgtSanta.Breaks.Select(mb => mb.Id).Contains(b.Id)))
                {
                    newBreak.Santa = mgtSanta;
                    newBreak.VisitType = VisitType.Break;
                    mgtSanta.Breaks.Add(newBreak);
                }

                transaction.Commit();
            }
        }

        // DELETE: api/Santa/5
        public void Delete(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                var mgtSanta = dbSession.Get<Santa>(id);
                dbSession.Delete(mgtSanta);
                transaction.Commit();
            }
        }
    }
}
