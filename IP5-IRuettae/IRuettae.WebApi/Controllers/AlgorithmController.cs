using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Hosting;
using System.Web.Http;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json.Linq;

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


        [HttpGet]
        public IEnumerable<TimeSpan> Benchmark(int n_visits)
        {
            var algorithmStarter = new AlgorithmStarter
            {
                Year = 2017,
                Beta0 = 15,
                Days = new List<(DateTime, DateTime)>()
                {
                    (new DateTime(2017,12,8,17,0,0), new DateTime(2017,12,8,22,0,0)),
                  //  (new DateTime(2017,12,9,17,0,0), new DateTime(2017,12,9,22,0,0))
                },
                StarterId = 197,
                TimePerChild = 5,
                TimeSliceDuration = 5 * 60
            };
            SolverInputData solverInputData;

            

            var retVal = new List<TimeSpan>();
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var visits = dbSession.Query<Visit>().Take(n_visits).ToList();
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

                solverInputData = solverVariableBuilder.Build();
            }

            var path = HostingEnvironment.MapPath($"~/App_Data/SolverInput{n_visits}Visits.serial");
            using (var stream = File.Open(path, FileMode.Create))
            {
                new BinaryFormatter().Serialize(stream, solverInputData);
            }

            //for (int i = 0; i < 1; ++i)
            //{
            //    var sw = Stopwatch.StartNew();
            //    Starter.Optimise(solverInputData);
            //    sw.Stop();
            //    Console.WriteLine("Elapsed ms: " + sw.ElapsedMilliseconds);
            //    retVal.Add(sw.Elapsed);
            //}

            //return retVal;
            return null;
        }
    }
}
