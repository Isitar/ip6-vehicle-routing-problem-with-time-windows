using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Models;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Santa = IRuettae.Persistence.Entities.Santa;
using Visit = IRuettae.Persistence.Entities.Visit;

namespace IRuettae.WebApi.Controllers
{
    [RoutePrefix("api/algorithm")]
    public class AlgorithmController : ApiController
    {
        [HttpPost]
        public OptimizationResult CalculateRoute([FromBody] AlgorithmStarter algorithmStarter)
        {

            using (var dbSession = SessionFactory.Instance.OpenSession())
            using (var transaction = dbSession.BeginTransaction())
            {
                var visits = dbSession.Query<Visit>().Where(v => v.Year == algorithmStarter.Year && v.Id != algorithmStarter.StarterId).ToList();
                visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * algorithmStarter.TimePerChild + algorithmStarter.Beta0));
                var converter = new Converter.PersistenceToCoreConverter();

                var optimizationInput = converter.Convert(algorithmStarter.Days, dbSession.Query<Visit>().First(v => v.Id == algorithmStarter.StarterId), visits,
                    dbSession.Query<Santa>().ToList());

                var ilpSolver = new ILPSolver(optimizationInput, algorithmStarter.TimeSliceDuration);
                var progress = new Progress<ProgressReport>();
                progress.ProgressChanged += (sender, i) => { Console.WriteLine($"Progress: {i}"); };
                return ilpSolver.Solve(0, progress);
            }
        }
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
            RouteCalculation rc2;
            RouteCalculation rc3;

            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                ILPStarterData ilpData = new ILPStarterData()
                {
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    ClusteringOptimizationFunction = ClusteringOptimizationGoals.OverallMinTime,
                    ClusteringMipGap = Properties.Settings.Default.MIPGapClustering,
                    ClusteringTimeLimit = Properties.Settings.Default.TimelimitClustering,
                    SchedulingMipGap = Properties.Settings.Default.MIPGapScheduling,
                    SchedulingTimeLimit = Properties.Settings.Default.TimelimitScheduling,
                };
                rc = new RouteCalculation
                {
                    Days = algorithmStarter.Days,
                    SantaJson = "",
                    VisitsJson = "",
                    StarterVisitId = algorithmStarter.StarterId,
                    State = RouteCalculationState.Creating,
                    TimePerChild = algorithmStarter.TimePerChild,
                    TimePerChildOffset = algorithmStarter.Beta0,
                    Year = algorithmStarter.Year,
                    Algorithm = AlgorithmType.ILP,
                    AlgorithmData = JsonConvert.SerializeObject(ilpData),
                };
                rc = dbSession.Merge(rc);

                ILPStarterData ilpData2 = new ILPStarterData()
                {
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    ClusteringOptimizationFunction = ClusteringOptimizationGoals.MinTimePerSanta,
                    ClusteringMipGap = Properties.Settings.Default.MIPGapClustering,
                    ClusteringTimeLimit = Properties.Settings.Default.TimelimitClustering,
                    SchedulingMipGap = Properties.Settings.Default.MIPGapScheduling,
                    SchedulingTimeLimit = Properties.Settings.Default.TimelimitScheduling,
                };
                rc2 = new RouteCalculation
                {
                    Days = algorithmStarter.Days,
                    SantaJson = "",
                    VisitsJson = "",
                    StarterVisitId = algorithmStarter.StarterId,
                    State = RouteCalculationState.Creating,
                    TimePerChild = algorithmStarter.TimePerChild,
                    TimePerChildOffset = algorithmStarter.Beta0,
                    Year = algorithmStarter.Year,
                    Algorithm = AlgorithmType.ILP,
                    AlgorithmData = JsonConvert.SerializeObject(ilpData2),
                };
                rc2 = dbSession.Merge(rc2);


                ILPStarterData ilpData3 = new ILPStarterData()
                {
                    TimeSliceDuration = algorithmStarter.TimeSliceDuration,
                    ClusteringOptimizationFunction = ClusteringOptimizationGoals.MinAvgTimePerSanta,
                    ClusteringMipGap = Properties.Settings.Default.MIPGapClustering,
                    ClusteringTimeLimit = Properties.Settings.Default.TimelimitClustering,
                    SchedulingMipGap = Properties.Settings.Default.MIPGapScheduling,
                    SchedulingTimeLimit = Properties.Settings.Default.TimelimitScheduling,
                };
                rc3 = new RouteCalculation
                {
                    Days = algorithmStarter.Days,
                    SantaJson = "",
                    VisitsJson = "",
                    StarterVisitId = algorithmStarter.StarterId,
                    State = RouteCalculationState.Creating,
                    TimePerChild = algorithmStarter.TimePerChild,
                    TimePerChildOffset = algorithmStarter.Beta0,
                    Year = algorithmStarter.Year,
                    Algorithm = AlgorithmType.ILP,
                    AlgorithmData = JsonConvert.SerializeObject(ilpData3),
                };
                rc3 = dbSession.Merge(rc3);
            }

            Task.Run(() => new Pipeline(rc).StartWorker());
            Task.Run(() => new Pipeline(rc2).StartWorker());
            Task.Run(() => new Pipeline(rc3).StartWorker());

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
                if (Pipeline.BackgroundWorkers.IsEmpty)
                {
                    dbSession.Query<RouteCalculation>()
                        .Where(rc => new[] { RouteCalculationState.Ready, RouteCalculationState.RunningPhase1, RouteCalculationState.RunningPhase2, RouteCalculationState.RunningPhase3 }.Contains(rc.State))
                        .ToList()
                        .ForEach(rc =>
                        {
                            rc.State = RouteCalculationState.Cancelled;
                            dbSession.Update(rc);
                        });
                    dbSession.Flush();
                }

                var routeCalculations = dbSession.Query<RouteCalculation>().ToList();
                return routeCalculations;
            }
        }


        [HttpGet]
        [Route("RouteCalculationWaypoints")]
        public IEnumerable<object> RouteCalculationWaypoints(long id)
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var routeCalculation = dbSession.Get<RouteCalculation>(id);
                var schedulingResults = JsonConvert.DeserializeObject<RouteCalculationResult>(routeCalculation.Result);

                return schedulingResults.OptimizationResult.Routes.Select(r => r.Waypoints.Select(wp =>
                {
                    var v = dbSession.Get<Visit>(schedulingResults.VisitMap[wp.VisitId]);
                    return new
                    {
                        Visit = (VisitDTO)v,
                        VisitStartTime = schedulingResults.ConvertTime(wp.StartTime),
                        VisitEndtime = schedulingResults.ConvertTime(wp.StartTime).AddSeconds(v.Duration),
                        SantaName = dbSession.Get<Santa>(schedulingResults.VisitMap[r.SantaId])?.Name,
                    };
                }).ToList()).ToList();
            }
        }
    }
}
