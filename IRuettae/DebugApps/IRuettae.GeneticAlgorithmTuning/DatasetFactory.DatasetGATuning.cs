using IRuettae.Core.Models;
namespace IRuettae.GeneticAlgorithmTuning
{
    internal class DatasetFactory
    {
        private const int Hour = 3600;

        /// <summary>
        /// 20 Visits, 2 Days, 2 Santas
        /// 4 Breaks, 18 unique visits
        /// 5 Desired, 5 Unavailable on day 0
        /// 5 Desired, 5 Unavailable on day 1
        /// </summary>
        public static (OptimizationInput input, (int x, int y)[] coordinates) DatasetGATuning()
        {
            // Coordinates of points
            var coordinates = new[]
            {
                (3322,3118),
                (1931,1447),
                (2763,2682),
                (2112,1746),
                (2661,2917),
                (3057,2257),
                (103,3201),
                (2467,2803),
                (1035,454),
                (2714,1304),
                (469,996),
                (3100,2749),
                (1987,2945),
                (1328,1279),
                (3046,2252),
                (2571,3266),
                (2178,2285),
                (1646,2006),
                (1758,3005)
            };
            const int workingDayDuration = 9 * Hour;
            var input = new OptimizationInput
            {
                Days = new[] { (0 * Hour, 0 * Hour + workingDayDuration), (24 * Hour, 24 * Hour + workingDayDuration) },

                RouteCosts = new[,]
                {
            { 0, 1489, 349, 1641, 1387, 2533, 1458, 1337, 795, 1529, 1749, 1499, 625, 1375, 1928, 873, 627, 1567 },
            { 1489, 0, 1140, 256, 516, 2710, 319, 2819, 1378, 2846, 343, 819, 2006, 514, 614, 706, 1305, 1055 },
            { 349, 1140, 0, 1293, 1074, 2480, 1115, 1682, 746, 1806, 1407, 1205, 912, 1062, 1587, 543, 533, 1307 },
            { 1641, 256, 1293, 0, 769, 2573, 225, 2951, 1613, 2914, 470, 674, 2111, 768, 360, 795, 1363, 907 },
            { 1387, 516, 1074, 769, 0, 3101, 803, 2709, 1012, 2878, 493, 1272, 1986, 12, 1119, 879, 1433, 1498 },
            { 2533, 2710, 2480, 2573, 3101, 0, 2397, 2900, 3227, 2235, 3030, 1901, 2279, 3092, 2468, 2268, 1951, 1666 },
            { 1458, 319, 1115, 225, 803, 2397, 0, 2751, 1519, 2693, 635, 500, 1902, 799, 474, 593, 1144, 737 },
            { 1337, 2819, 1682, 2951, 2709, 2900, 2751, 0, 1881, 783, 3087, 2666, 875, 2697, 3204, 2158, 1667, 2651 },
            { 795, 1378, 746, 1613, 1012, 3227, 1519, 1881, 0, 2266, 1495, 1794, 1386, 1004, 1967, 1117, 1278, 1951 },
            { 1529, 2846, 1806, 2914, 2878, 2235, 2693, 783, 2266, 0, 3161, 2470, 904, 2866, 3093, 2140, 1550, 2386 },
            { 1749, 343, 1407, 470, 493, 3030, 635, 3087, 1495, 3161, 0, 1130, 2302, 499, 739, 1032, 1632, 1366 },
            { 1499, 819, 1205, 674, 1272, 1901, 500, 2666, 1794, 2470, 1130, 0, 1791, 1265, 666, 687, 999, 236 },
            { 625, 2006, 912, 2111, 1986, 2279, 1902, 875, 1386, 904, 2302, 1791, 0, 1974, 2343, 1317, 793, 1778 },
            { 1375, 514, 1062, 768, 12, 3092, 799, 2697, 1004, 2866, 499, 1265, 1974, 0, 1119, 868, 1421, 1491 },
            { 1928, 614, 1587, 360, 1119, 2468, 474, 3204, 1967, 3093, 739, 666, 2343, 1119, 0, 1056, 1563, 853 },
            { 873, 706, 543, 795, 879, 2268, 593, 2158, 1117, 2140, 1032, 687, 1317, 868, 1056, 0, 600, 833 },
            { 627, 1305, 533, 1363, 1433, 1951, 1144, 1667, 1278, 1550, 1632, 999, 793, 1421, 1563, 600, 0, 1005 },
            { 1567, 1055, 1307, 907, 1498, 1666, 737, 2651, 1951, 2386, 1366, 236, 1778, 1491, 853, 833, 1005, 0 }
        },

                Santas = new[]
                {
            new Santa { Id = 0 },
            new Santa { Id = 1 }
        },

                Visits = new[]
                {
            new Visit{Duration = 2296, Id=0,WayCostFromHome=2174, WayCostToHome=2174,Unavailable =new [] {(24 * Hour + 2575, (24 * Hour + 2575) + 21484)},Desired = new [] {(0 * Hour + 437, (0 * Hour + 437) + 31150)}},
            new Visit{Duration = 3122, Id=1,WayCostFromHome=708, WayCostToHome=708,Unavailable =new [] {(24 * Hour + 11990, (24 * Hour + 11990) + 18866)},Desired = new [] {(0 * Hour + 848, (0 * Hour + 848) + 31505)}},
            new Visit{Duration = 3161, Id=2,WayCostFromHome=1829, WayCostToHome=1829,Unavailable =new [] {(24 * Hour + 23637, (24 * Hour + 23637) + 3404)},Desired = new [] {(0 * Hour + 8000, (0 * Hour + 8000) + 11757)}},
            new Visit{Duration = 1367, Id=3,WayCostFromHome=690, WayCostToHome=690,Unavailable =new [] {(24 * Hour + 20249, (24 * Hour + 20249) + 3532)},Desired = new [] {(0 * Hour + 702, (0 * Hour + 702) + 27645)}},
            new Visit{Duration = 3512, Id=4,WayCostFromHome=900, WayCostToHome=900,Unavailable =new [] {(24 * Hour + 4981, (24 * Hour + 4981) + 25362)},Desired = new [] {(0 * Hour + 2842, (0 * Hour + 2842) + 27453)}},
            new Visit{Duration = 2281, Id=5,WayCostFromHome=3220, WayCostToHome=3220,Unavailable =new [] {(0 * Hour + 16592, (0 * Hour + 16592) + 9437)},Desired = new [] {(24 * Hour + 21597, (24 * Hour + 21597) + 5748)}},
            new Visit{Duration = 2745, Id=6,WayCostFromHome=911, WayCostToHome=911,Unavailable =new [] {(0 * Hour + 11631, (0 * Hour + 11631) + 19923)},Desired = new [] {(24 * Hour + 21, (24 * Hour + 21) + 24409)}},
            new Visit{Duration = 1268, Id=7,WayCostFromHome=3511, WayCostToHome=3511,Unavailable =new [] {(0 * Hour + 3639, (0 * Hour + 3639) + 20346)},Desired = new [] {(24 * Hour + 4732, (24 * Hour + 4732) + 9774)}},
            new Visit{Duration = 2111, Id=8,WayCostFromHome=1913, WayCostToHome=1913,Unavailable =new [] {(0 * Hour + 11083, (0 * Hour + 11083) + 17748)},Desired = new [] {(24 * Hour + 9759, (24 * Hour + 9759) + 18880)}},
            new Visit{Duration = 2658, Id=9,WayCostFromHome=3555, WayCostToHome=3555,Unavailable =new [] {(0 * Hour + 14805, (0 * Hour + 14805) + 7600)},Desired = new [] {(24 * Hour + 2493, (24 * Hour + 2493) + 28940)}},
            new Visit{Duration = 2874, Id=10,WayCostFromHome=430, WayCostToHome=430,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
            new Visit{Duration = 2958, Id=11,WayCostFromHome=1346, WayCostToHome=1346,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
            new Visit{Duration = 2707, Id=12,WayCostFromHome=2712, WayCostToHome=2712,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
            new Visit{Duration = 3192, Id=13,WayCostFromHome=908, WayCostToHome=908,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
            new Visit{Duration = 1934, Id=14,WayCostFromHome=765, WayCostToHome=765,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
            new Visit{Duration = 1614, Id=15,WayCostFromHome=1415, WayCostToHome=1415,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,           new Visit{Duration = 1800, Id=16,WayCostFromHome=2011, WayCostToHome=2011,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3074, (0 * Hour + 3074) + 22039),(24 * Hour + 3074, (24 * Hour + 3074) + 22039)},SantaId=0,IsBreak = true},
            new Visit{Duration = 1800, Id=17,WayCostFromHome=1568, WayCostToHome=1568,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7226, (0 * Hour + 7226) + 21269),(24 * Hour + 7226, (24 * Hour + 7226) + 21269)},SantaId=1,IsBreak = true}
        }
            };
            return (input, coordinates);
        }
    }
}
