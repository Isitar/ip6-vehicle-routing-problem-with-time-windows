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
        public static OptimizationInput DataSet1()
        {

            const int workingDayDuration = 5 * Hour;
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
                    new Santa { Id = 2 },
                    new Santa { Id = 3 },
                    new Santa { Id = 4 },
                    new Santa { Id = 5 },
                    new Santa { Id = 6 },
                    new Santa { Id = 7 },
                    new Santa { Id = 8 },
                    new Santa { Id = 9 },
                    new Santa { Id = 10 },
                    new Santa { Id = 11 },

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
