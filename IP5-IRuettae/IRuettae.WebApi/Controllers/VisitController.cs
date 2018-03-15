using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IRuettae.WebApi.Controllers
{
    public class VisitController : ApiController
    {
        // GET: api/dependingVisit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/dependingVisit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/dependingVisit
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/dependingVisit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/dependingVisit/5
        public void Delete(int id)
        {
        }
    }
}
