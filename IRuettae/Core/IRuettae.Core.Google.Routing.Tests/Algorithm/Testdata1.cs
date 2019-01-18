using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Tests.Algorithm
{
    /// <summary>
    /// Dataset for testing.
    /// </summary>
    static class Testdata1
    {
        // santa ids
        public const int SantaId1 = 100;
        public const int SantaId2 = 200;

        // used ways
        public const int W01 = 10;
        public const int W12 = 20;
        public const int W20 = 30;
        public const int W03 = 40;
        public const int W30 = 50;
        public const int W04 = 60;
        public const int W40 = 70;
        public const int W05 = 80;
        public const int W50 = 90;
        public const int W45 = 100;

        // durations
        public const int Duration1 = 100;
        public const int Duration2 = 200;
        public const int Duration3 = 300;
        public const int Duration4 = 400;
        public const int Duration5 = 500;

        // days
        public const int StartDay1 = 1000;
        public const int EndDay1 = 2000;
        public const int StartDay2 = 5000;
        public const int EndDay2 = 6000;

        // break times (relative to day)
        public const int BreakDesiredStart = 100;
        public const int BreakDesiredEnd = 200;

        public const int Hour = 3600;

        public static RoutingData Create()
        {
            return new RoutingData(new OptimizationInput()
            {
                Santas = new Santa[]
                {
                    new Santa
                    {
                        Id = SantaId1,
                    },
                    new Santa
                    {
                        Id = SantaId2,
                    },
                },
                Visits = new Visit[]
                {
                    new Visit
                    {
                        Id = 0,
                        Duration = Duration1,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = W01,
                        WayCostToHome = 0,
                    },
                    new Visit
                    {
                        Id = 1,
                        Duration = Duration2,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = 0,
                        WayCostToHome = W20,
                    },
                    new Visit
                    {
                        Id = 2,
                        Duration = Duration3,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = W03,
                        WayCostToHome =W30,
                    },
                    new Visit
                    {
                        Id = 3,
                        Duration = Duration4,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = W04,
                        WayCostToHome = W40,
                    },
                    new Visit
                    {
                        Id = 4,
                        Duration = Duration5,
                        IsBreak = true,
                        SantaId = SantaId2,
                        Desired = new(int, int)[]
                        {
                            (StartDay1 + BreakDesiredStart, StartDay1 + BreakDesiredEnd),
                            (StartDay2 + BreakDesiredStart, StartDay2 + BreakDesiredEnd),
                        },
                        WayCostFromHome = W05,
                        WayCostToHome = W50,
                    }
                },
                Days = new(int, int)[]
                {
                    (StartDay1, EndDay1),
                    (StartDay2, EndDay2),
                },
                RouteCosts = new int[,]
                {
                    {0, W12, Hour, Hour, Hour},
                    {Hour, 0, Hour, Hour, Hour},
                    {Hour, Hour, 0,Hour, Hour},
                    {Hour, Hour, Hour, 0, W45},
                    {Hour, Hour, Hour, Hour, 0},
                },
            });
        }
    }
}
