using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;

namespace IRuettae.WebApi.Controllers
{
    [RoutePrefix("api/algorithm")]
    public class AlgorithmController : ApiController
    {
        //[HttpPost]
        //public Route CalculateRoute([FromBody] AlgorithmStarter algorithmStarter)
        //{

        //    using (var dbSession = SessionFactory.Instance.OpenSession())
        //    using (var transaction = dbSession.BeginTransaction())
        //    {
        //        var visits = dbSession.Query<Visit>().Take(10).ToList();
        //        visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * algorithmStarter.TimePerChild + algorithmStarter.Beta0));
        //        visits.Sort((a, b) =>
        //        {
        //            if (a.Id == algorithmStarter.StarterId)
        //            {
        //                return -1;
        //            }
        //            return a.Id.CompareTo(b.Id);
        //        });

        //        var solverVariableBuilder = new SchedulingSolverVariableBuilder(algorithmStarter.TimeSliceDuration)
        //        {
        //            Visits = visits,
        //            Santas = dbSession.Query<Santa>().ToList(),
        //            Days = algorithmStarter.Days
        //        };

        //        var solverInputData = solverVariableBuilder.Build();
        //        return Starter.Optimise(solverInputData);
        //    }
        //}


        //[HttpGet]
        //[Route("Benchmark")]
        //public IEnumerable<TimeSpan> Benchmark(int n_visits)
        //{
        //    var algorithmStarter = new AlgorithmStarter
        //    {
        //        Year = 2017,
        //        Beta0 = 15,
        //        Days = new List<(DateTime, DateTime)>()
        //        {
        //            (new DateTime(2017,12,8,17,0,0), new DateTime(2017,12,8,22,0,0)),
        //          //  (new DateTime(2017,12,9,17,0,0), new DateTime(2017,12,9,22,0,0))
        //        },
        //        StarterId = 12,
        //        TimePerChild = 5,
        //        TimeSliceDuration = 5 * 60
        //    };
        //    SolverInputData solverInputData;



        //    var retVal = new List<TimeSpan>();
        //    using (var dbSession = SessionFactory.Instance.OpenSession())
        //    {
        //        var visits = dbSession.Query<Visit>().Take(n_visits).ToList();
        //        visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * algorithmStarter.TimePerChild + algorithmStarter.Beta0));
        //        visits.Sort((a, b) =>
        //        {
        //            if (a.Id == algorithmStarter.StarterId)
        //            {
        //                return -1;
        //            }
        //            if (b.Id == algorithmStarter.StarterId)
        //            {
        //                return 1;
        //            }
        //            return a.Id.CompareTo(b.Id);
        //        });

        //        var solverVariableBuilder = new SchedulingSolverVariableBuilder(algorithmStarter.TimeSliceDuration)
        //        {
        //            Visits = visits,
        //            Santas = dbSession.Query<Santa>().ToList(),
        //            Days = algorithmStarter.Days
        //        };

        //        solverInputData = solverVariableBuilder.Build();
        //    }

        //    var path = HostingEnvironment.MapPath($"~/App_Data/SolverInput{n_visits}Visits.serial");
        //    using (var stream = File.Open(path, FileMode.Create))
        //    {
        //        new BinaryFormatter().Serialize(stream, solverInputData);
        //    }

        //    //for (int i = 0; i < 1; ++i)
        //    //{
        //    //    var sw = Stopwatch.StartNew();
        //    //    Starter.Optimise(solverInputData);
        //    //    sw.Stop();
        //    //    Console.WriteLine("Elapsed ms: " + sw.ElapsedMilliseconds);
        //    //    retVal.Add(sw.Elapsed);
        //    //}

        //    //return retVal;
        //    return null;
        //}

        /// <summary>
        /// Starts a new route calculation job
        /// </summary>
        /// <param name="algorithmStarter"></param>
        /// <returns>the id of the route calcluation job</returns>
        [HttpPost]
        [Route("StartRouteCalculation")]
        public long StartRouteCalculation([FromBody]AlgorithmStarter algorithmStarter)
        {

            RouteCalculation rc;
            var bgWorker = new BackgroundWorker();

            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                rc = new RouteCalculation
                {
                    Days = algorithmStarter.Days,
                    SantaJson = "", //JsonConvert.SerializeObject(santas,Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    VisitsJson = "", //JsonConvert.SerializeObject(visits, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    StarterVisitId = algorithmStarter.StarterId,
                    State = RouteCalculationState.Creating,
                    TimePerChild = algorithmStarter.TimePerChild,
                    TimePerChildOffset = algorithmStarter.Beta0,
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    Year = algorithmStarter.Year,
                    ClusteringOptimisationFunction = ClusteringOptimisationGoals.OverallMinTime,
                    ClustringMipGap = 0.1,
                    SchedulingMipGap = 0.1
                };
                rc = dbSession.Merge(rc);
            }

            Task.Run(() => new Pipeline(rc).StartWorker());

            return rc.Id;




            //var serialPath = HostingEnvironment.MapPath($"~/App_Data/SolverInputNew{n_visits}Visits.serial");
            //using (var stream = File.Open(serialPath, FileMode.Create))
            //{
            //    new BinaryFormatter().Serialize(stream, solverInputData);
            //}

            //var mpsPath = HostingEnvironment.MapPath($"~/App_Data/MPS_{n_visits}Visits_new.mps");
            //Starter.SaveMps(mpsPath, solverInputData);

            //var sw = Stopwatch.StartNew();
            //var routeResult = Starter.Optimise(solverInputData, MIP_GAP: 0.5);
            //sw.Stop();
            //var routes = routeResult.Waypoints
            //    .Cast<List<Waypoint>>()
            //    .Select(wp => wp.Aggregate("",
            //        (carry, n) => carry + Environment.NewLine + solverInputData.VisitNames[n.Visit]))
            //    .ToList();


            //var ctr = 0;
            //foreach (var route in routes)
            //{
            //    File.WriteAllText(HostingEnvironment.MapPath($"~/App_Data/R{ctr}_{0.5}.csv"), $"Address{Environment.NewLine}{route}");
            //    //ConsoleExt.WriteLine(ctr.ToString(), ResultColor);
            //    //ConsoleExt.WriteLine(route, ResultColor);
            //    ctr++;
            //}


            //Debug.WriteLine("Elapsed ms: " + sw.ElapsedMilliseconds);
            //return routes;
        }

        [HttpGet]
        [Route("RouteCalculations")]
        public IEnumerable<RouteCalculation> GetRouteCalculations()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var routeCalculations = dbSession.Query<RouteCalculation>().ToList();
                return routeCalculations;
            }
        }
    }
}
