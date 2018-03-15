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
        // GET: api/Visit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Visit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Visit
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Visit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Visit/5
        public void Delete(int id)
        {
        }
    }
}
