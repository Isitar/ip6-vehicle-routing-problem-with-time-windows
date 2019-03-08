using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Converter;
using IRuettae.Core.Manual;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;

namespace IRuettae.ConsoleApp.Programs
{
    internal class CreateManualSolution
    {
        internal static void Run(string[] args)
        {
            Run2017();
        }
        internal static void Run2018()
        {
            // Note: in case of Exceptions in RouteCalculator,
            // you may want to remove the update of the visit duration in RouteCalculator

            // Real solution of 2018
            var year = 2018;
            var starterVisitId = 100;
            var timePerChild = 5;
            var Beta0 = 15;

            var days = new List<(DateTime, DateTime)>()
            {
                (new DateTime(2017, 12, 08, 17, 0, 0), new DateTime(2017, 12, 08, 22, 0, 0)),
                (new DateTime(2017, 12, 09, 17, 0, 0), new DateTime(2017, 12, 09, 22, 0, 0)),
            };

            var routes = new(int santaId, int day, int[] visitIds)[]
                {
                (8,0,new[]{ 1,2,3,4,5 }),
                (11,0,new[]{ 34,35,25,26,27,28}),
                (12,0,new[]{ 29,30,31,32,33 }),
                (8,1,new[]{ 6,7,8,9,10,11 }),
                (11,1,new[]{ 13,14,15,16,17,18 }),
                (12,1,new[]{ 19,20,21,22,23,24 }),
            };

            CreateSolution(year, starterVisitId, timePerChild, Beta0, days, routes);
        }

        internal static void Run2017()
        {
            // Note: in case of Exceptions in RouteCalculator,
            // you may want to remove the update of the visit duration in RouteCalculator

            // Real solution of 2017
            var year = 2017;
            var starterVisitId = 12;
            var timePerChild = 5;
            var Beta0 = 15;

            var days = new List<(DateTime, DateTime)>()
            {
                (new DateTime(2017, 12, 08, 17, 15, 0), new DateTime(2017, 12, 08, 22, 0, 0)),
                (new DateTime(2017, 12, 09, 17, 0, 0), new DateTime(2017, 12, 09, 22, 0, 0)),
            };

            var routes = new(int santaId, int day, int[] visitIds)[]
                {
                (8,0,new[]{ 1,2,3,4,5 }),
                (11,0,new[]{ 34,35,25,26,27,28}),
                (12,0,new[]{ 29,30,31,32,33 }),
                (8,1,new[]{ 6,7,8,9,10,11 }),
                (11,1,new[]{ 13,14,15,16,17,18 }),
                (12,1,new[]{ 19,20,21,22,23,24 }),
            };

            CreateSolution(year, starterVisitId, timePerChild, Beta0, days, routes);
        }

        private static void CreateSolution(int year, int starterVisitId, int timePerChild, int beta0, List<(DateTime, DateTime)> days, (int santaId, int day, int[] visitIds)[] routes)
        {
            routes = TranslateRoute(year, starterVisitId, days, routes);

            RouteCalculation rc;

            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                var starterData = new ManualStarterData()
                {
                    Routes = routes,
                };
                rc = new RouteCalculation
                {
                    Days = days,
                    SantaJson = "",
                    VisitsJson = "",
                    TimeLimitMiliseconds = 10000,
                    StarterVisitId = starterVisitId,
                    State = RouteCalculationState.Creating,
                    TimePerChildMinutes = timePerChild,
                    TimePerChildOffsetMinutes = beta0,
                    Year = year,
                    Algorithm = AlgorithmType.Manual,
                    AlgorithmData = JsonConvert.SerializeObject(starterData),
                };
                rc = dbSession.Merge(rc);
            }

            Task.Run(() => new RouteCalculator(rc).StartWorker());
        }

        /// <summary>
        /// Translate database Ids in virtual Ids
        /// </summary>
        /// <param name="year"></param>
        /// <param name="starterVisitId"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        private static (int santaId, int day, int[] visitIds)[] TranslateRoute(int year, int starterVisitId, List<(DateTime, DateTime)> days, (int santaId, int day, int[] visitIds)[] routes)
        {
            var dbSession = SessionFactory.Instance.OpenSession();

            // Same order as in RouteCalculator
            var santas = dbSession.Query<Santa>().OrderBy(s => s.Id).ToList();
            var visits = dbSession.Query<Visit>()
                .Where(v => v.Year == year && v.Id != starterVisitId)
                .OrderBy(v => v.Id)
                .ToList();
            var startVisit = dbSession.Query<Visit>().First(v => v.Id == starterVisitId);

            var converter = new PersistenceToCoreConverter();
            var optimizationInput = converter.Convert(days, startVisit, visits, santas);

            var SantaMap = new Dictionary<long, int>();
            foreach (var e in converter.SantaMap)
            {
                SantaMap[e.Value] = e.Key;
            }
            var VisitMap = new Dictionary<long, int>();
            foreach (var e in converter.VisitMap)
            {
                VisitMap[e.Value] = e.Key;
            }

            return routes.Select(r => (SantaMap[r.santaId], r.day, r.visitIds.Select(v => VisitMap[v]).ToArray())).ToArray();
        }
    }
}
