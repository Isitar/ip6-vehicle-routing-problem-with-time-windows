using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;

namespace IRuettae.WebApi.Controllers
{
    public class AlgorithmController : ApiController
    {
        [HttpPost]
        public Route CalculateRoute([FromBody] AlgorithmStarter algorithmStarter)
        {

            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                var visits = dbSession.Query<Visit>().Take(10).ToList();
                visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * algorithmStarter.TimePerChild + algorithmStarter.Beta0));
                visits.Sort((a, b) =>
                {
                    if (a.Id == algorithmStarter.StarterId)
                    {
                        return -1;
                    }
                    return a.Id.CompareTo(b.Id);
                });

                var solverVariableBuilder = new SolverVariableBuilder(algorithmStarter.TimeSliceDuration)
                {
                    Visits = visits,
                    Santas = dbSession.Query<Santa>().ToList(),
                    Days = algorithmStarter.Days
                };
                
                var solverInputData = solverVariableBuilder.Build();
                return Starter.Optimise(solverInputData);
            }
        }
    }
}
