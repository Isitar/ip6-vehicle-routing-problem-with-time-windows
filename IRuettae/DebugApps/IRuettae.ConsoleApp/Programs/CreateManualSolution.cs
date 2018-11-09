using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Manual;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Persistence;
using Newtonsoft.Json;

namespace IRuettae.ConsoleApp.Programs
{
    internal class CreateManualSolution
    {
        const int minute = 60;

        internal static void Run(string[] args)
        {
            // Real solution of 2017
            var year = 2017;
            var starterId = 12;
            var timePerChild = 5 * minute;
            var Beta0 = 15 * minute;

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

            CreateSolution(year, starterId, timePerChild, Beta0, days, routes);
        }

        private static void CreateSolution(int year, int starterId, int timePerChild, int beta0, List<(DateTime, DateTime)> days, (int santaId, int day, int[] visitIds)[] routes)
        {
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
                    StarterVisitId = starterId,
                    State = RouteCalculationState.Creating,
                    TimePerChild = timePerChild,
                    TimePerChildOffset = beta0,
                    Year = year,
                    Algorithm = AlgorithmType.Manual,
                    AlgorithmData = JsonConvert.SerializeObject(starterData),
                };
                rc = dbSession.Merge(rc);
            }

            Task.Run(() => new RouteCalculator(rc).StartWorker());
        }
    }
}
