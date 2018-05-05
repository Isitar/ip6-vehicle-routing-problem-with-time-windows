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
            using (var transaction = dbSession.BeginTransaction())
            { 
                dbSession.Delete("from Period p");
                dbSession.Delete("from Way w");
                dbSession.Delete("from Visit v");
                dbSession.Delete("from Santa s");
                transaction.Commit();
            }

        }
    }
}
