using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using IRuettae.Persistence.Entities;
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

        public string GenerateDotText(int yearFilter)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var visits = dbSession.Query<Visit>().Where(v => v.Year == yearFilter).ToList();
                var sb = new StringBuilder();
                sb.AppendLine("digraph {");
                foreach (var source in visits)
                {
                    foreach (var destination in visits)
                    {
                        var w = source.FromWays.FirstOrDefault(way => way.To.Id == destination.Id)?.Duration;
                        sb.AppendLine($"{source.Id} -> {destination.Id} [label=\"{w}\", weight=\"{w}\"]");
                    }
                }

                sb.AppendLine("}");
                return sb.ToString();
            }
        }
    }
}
