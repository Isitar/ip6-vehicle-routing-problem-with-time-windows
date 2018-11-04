using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Evaluator
{
    internal class DatasetFactory
    {
        private const int Minute = 60;
        private const int Hour = 60 * Minute;
        /// <summary>
        /// 10 Visits, 2 Days, 1 Santa
        /// </summary>
        /// <returns></returns>
        public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet1()
        {

            // Coordinates of points
            var coordinates = new[]
            {
                (1383, 722),
                (3094, 542),
                (344, 2773),
                (1766, 3773),
                (3235, 912),
                (2882, 457),
                (2735, 1539),
                (1133, 2469),
                (779, 1017),
                (3455, 1316),
                (189, 3219),
            };

            const int workingDayDuration = 12 * Hour;
            var input = new OptimizationInput
            {
                Days = new[] { (0, workingDayDuration), (24 * Hour, 24 * Hour + workingDayDuration) },
                RouteCosts = new[,]
                {
                    {0, 3542, 3494, 396, 229, 1060, 2750, 2364, 855, 3951, 4426},
                    {3542, 0, 1739, 3439, 3436, 2691, 846, 1810, 3436, 473, 1155},
                    {3494, 1739, 0, 3217, 3499, 2436, 1450, 2928, 2982, 1672, 1536},
                    {396, 3439, 3217, 0, 576, 802, 2616, 2459, 461, 3822, 4249},
                    {229, 3436, 3499, 576, 0, 1092, 2666, 2177, 1033, 3858, 4360},
                    {1060, 2691, 2436, 802, 1092, 0, 1853, 2025, 754, 3051, 3453},
                    {2750, 846, 1450, 2616, 2666, 1853, 0, 1495, 2593, 1206, 1708},
                    {2364, 1810, 2928, 2459, 2177, 2025, 1495, 0, 2693, 2280, 2956},
                    {855, 3436, 2982, 461, 1033, 754, 2593, 2693, 0, 3780, 4141},
                    {3951, 473, 1672, 3822, 3858, 3051, 1206, 2280, 3780, 0, 706},
                    {4426, 1155, 1536, 4249, 4360, 3453, 1708, 2956, 4141, 706, 0},

                },
                Santas = new[]
                {
                    new Santa {Id = 0},
                },
                Visits = new[] {
                    new Visit{Duration = 1453,Id = 0,WayCostFromHome = 1721,WayCostToHome = 1721,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 1325,Id = 1,WayCostFromHome = 2300,WayCostToHome = 2760,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 3386,Id = 2,WayCostFromHome = 3075,WayCostToHome = 2768,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 2784,Id = 3,WayCostFromHome = 1862,WayCostToHome = 1862,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 2622,Id = 4,WayCostFromHome = 1523,WayCostToHome = 1676,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 3554,Id = 5,WayCostFromHome = 1580,WayCostToHome = 1580,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 2291,Id = 6,WayCostFromHome = 1765,WayCostToHome = 1765,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 3293,Id = 7,WayCostFromHome =  673,WayCostToHome =  808,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 1605,Id = 8,WayCostFromHome = 2156,WayCostToHome = 2372,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]},
                    new Visit{Duration = 3473,Id = 9,WayCostFromHome = 2768,WayCostToHome = 3045,Unavailable = new (int from, int to)[0],Desired = new (int from, int to)[0]}
                }
            };
            return (input, coordinates);
        }
        /// <summary>
        /// 10 Visits, 2 Days, 1 Santa, desired 5 - 5
        /// </summary>
        /// <returns></returns>
        public static OptimizationInput DataSet2()
        {

            const int workingDayDuration = 8 * Hour;
            return new OptimizationInput
            {
                Days = new[] { (0, workingDayDuration), (24 * Hour, 24 * Hour + workingDayDuration) },
                RouteCosts = new[,]
                {
                    {0, 5073, 718, 500, 5082, 4305, 5019, 924, 1855, 1964, 2397},
                    {5073, 0, 5696, 5571, 1132, 1689, 2158, 4241, 3419, 4974, 2823},
                    {718, 5696, 0, 429, 5766, 4796, 5450, 1459, 2564, 2527, 3091},
                    {500, 5571, 429, 0, 5561, 4790, 5489, 1405, 2314, 2107, 2873},
                    {5082, 1132, 5766, 5561, 0, 2642, 3246, 4371, 3263, 4545, 2688},
                    {4305, 1689, 4796, 4790, 2642, 0, 872, 3385, 3126, 4867, 2652},
                    {5019, 2158, 5450, 5489, 3246, 872, 0, 4096, 3972, 5715, 3516},
                    {924, 4241, 1459, 1405, 4371, 3385, 4096, 0, 1368, 2301, 1766},
                    {1855, 3419, 2564, 2314, 3263, 3126, 3972, 1368, 0, 1744, 599},
                    {1964, 4974, 2527, 2107, 4545, 4867, 5715, 2301, 1744, 0, 2248},
                    {2397, 2823, 3091, 2873, 2688, 2652, 3516, 1766, 599, 2248, 0},
                },
                Santas = new[] {
                    new Santa { Id = 0 },
                    new Santa { Id = 1 },

                },
                Visits = new[]{
                    new Visit {Duration = 1679, Id = 1, WayCostFromHome =1779,WayCostToHome = 1779},
                    new Visit {Duration = 3432, Id = 2, WayCostFromHome =5839,WayCostToHome = 5839},
                    new Visit {Duration = 2321, Id = 3, WayCostFromHome =2082,WayCostToHome = 2082},
                    new Visit {Duration = 2204, Id = 4, WayCostFromHome =1666,WayCostToHome = 1500},
                    new Visit {Duration = 2007, Id = 5, WayCostFromHome =5498,WayCostToHome = 4949},
                    new Visit {Duration = 1780, Id = 6, WayCostFromHome =5525,WayCostToHome = 6630},
                    new Visit {Duration = 2451, Id = 7, WayCostFromHome =6337,WayCostToHome = 5070},
                    new Visit {Duration = 2289, Id = 8, WayCostFromHome =2477,WayCostToHome = 2973},
                    new Visit {Duration = 2669, Id = 9, WayCostFromHome =2457,WayCostToHome = 2949},
                    new Visit {Duration = 2078, Id = 10, WayCostFromHome =1011,WayCostToHome = 1113},
                    new Visit {Duration = 3529, Id = 11, WayCostFromHome =3033,WayCostToHome = 3337},
                },
            };
        }
    }
}
