using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IRuettae.WebApi.Persistence;

namespace IRuettae.WebApi.Controllers
{
    public class AdminController : ApiController
    {
        public void CleanDatabase()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                dbSession.Delete("from Object o");
                dbSession.Flush();
            }

        }
    }
}
