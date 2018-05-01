using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;

namespace IRuettae.WebApi.Controllers
{
    public class AlgorithmController : ApiController
    {
        [HttpPost]
        public Route CalculateRoute(int year, int timeslotLength = 5 * 60, int timePerChild = 15, int beta0Time = 5)
        {

            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                var visits = dbSession.Query<Visit>().Take(10).ToList();
                visits.ForEach(v => v.Duration = 60* (v.NumberOfChildren * timePerChild + beta0Time));

                var solverVariableBuilder = new SolverVariableBuilder(timeslotLength)
                {
                    Visits = visits,
                    Santas = dbSession.Query<Santa>().ToList(),
                    Days = new List<(DateTime, DateTime)>
                    {
                        (new DateTime(2017, 12, 6, 17, 00, 00), new DateTime(2017, 12, 6, 22, 00, 00)),
                        (new DateTime(2017, 12, 7, 17, 00, 00), new DateTime(2017, 12, 7, 22, 00, 00))
                    }
                };
                var solverInputData = solverVariableBuilder.Build();
                return Starter.Optimise(solverInputData);
            }
        }
    }
}
