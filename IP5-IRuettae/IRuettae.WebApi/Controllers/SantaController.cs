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
        }

        // PUT: api/Santa/5
        public void Put(int id, [FromBody]Santa value)
        {
        }

        // DELETE: api/Santa/5
        public void Delete(int id)
        {
        }
    }
}
