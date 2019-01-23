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
    internal static class Testdataset1
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
        public const int NumberOfDays = 2;
        public const int StartDay1 = 1000;
        public const int EndDay1 = 3000;
        public const int StartDay2 = 6000;
        public const int EndDay2 = 8000;

        // break times (relative to day)
        public const int BreakDesiredStart = 100;
        public const int BreakDesiredEnd = 200;

        public const int LongWay = 500;

        public static OptimizationInput Create()
        {
            // unavailable
            var unavailableDay1Before = (int.MinValue, StartDay1 - 1);
            var unavailableBetween = (EndDay1 + 1, StartDay2 - 1);
            var unavailableDay2After = (EndDay2 + 1, int.MaxValue);

            return new OptimizationInput
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
                        Desired = new(int, int)[]
                        {
                            (StartDay1, EndDay1),
                            (StartDay2, EndDay2),
                        },
                        Unavailable = new(int, int)[]
                        {
                            unavailableDay1Before,
                            unavailableBetween,
                            unavailableDay2After,
                            (StartDay1, EndDay1),
                        },
                    },
                    new Visit
                    {
                        Id = 1,
                        Duration = Duration2,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = 0,
                        WayCostToHome = W20,
                        Desired = new(int, int)[]
                        {
                            (StartDay1, EndDay1),
                        },
                        Unavailable = new(int, int)[]
                        {
                            unavailableDay1Before,
                            unavailableBetween,
                            unavailableDay2After,
                            (StartDay2, EndDay2),
                        },
                    },
                    new Visit
                    {
                        Id = 2,
                        Duration = Duration3,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = W03,
                        WayCostToHome = W30,
                        Unavailable = new(int, int)[]
                        {
                            unavailableDay1Before,
                            unavailableBetween,
                            unavailableDay2After,
                        },
                    },
                    new Visit
                    {
                        Id = 3,
                        Duration = Duration4,
                        IsBreak = false,
                        SantaId = -1,
                        WayCostFromHome = W04,
                        WayCostToHome = W40,
                        Unavailable = new(int, int)[]
                        {
                            unavailableDay1Before,
                            unavailableBetween,
                            unavailableDay2After,
                        },
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
                        Unavailable = new(int, int)[]
                        {
                            unavailableDay1Before,
                            unavailableBetween,
                            unavailableDay2After,
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
                    {0, W12, LongWay, LongWay, LongWay},
                    {LongWay, 0, LongWay, LongWay, LongWay},
                    {LongWay, LongWay, 0,LongWay, LongWay},
                    {LongWay, LongWay, LongWay, 0, W45},
                    {LongWay, LongWay, LongWay, LongWay, 0},
                },
            };
        }
    }
}
