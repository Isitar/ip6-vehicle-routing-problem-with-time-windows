using IRuettae.Core.Models;
namespace IRuettae.Core.LocalSolverTests
{
    internal partial class DatasetFactory
    {
        /// <summary>
        /// 27 Visits, 3 Days, 3 Santas
        /// 9 Breaks, 24 unique visits
        /// 6 Desired, 0 Unavailable on day 0
        /// 6 Desired, 0 Unavailable on day 1
        /// 6 Desired, 0 Unavailable on day 2
        /// </summary>
        public static (OptimizationInput input, (int x, int y)[] coordinates) LocalSolverBreakDataSet()
        {
            // Coordinates of points
            var coordinates = new[]
            {
                (2645,2366),
                (1761,1076),
                (331,1075),
                (1333,1097),
                (2957,3144),
                (898,1097),
                (1230,1322),
                (1116,1339),
                (1231,1804),
                (1240,2070),
                (3457,3009),
                (2044,1669),
                (2468,482),
                (2515,2530),
                (1452,1622),
                (2306,2306),
                (2544,3066),
                (2880,2867),
                (141,284),
                (1188,1282),
                (1019,130),
                (1429,1213),
                (2778,2752),
                (2294,1753),
                (1758,1527)
            };
            const int workingDayDuration = 6 * Hour;
            var input = new OptimizationInput
            {
                Days = new[] { (0 * Hour, 0 * Hour + workingDayDuration), (24 * Hour, 24 * Hour + workingDayDuration), (48 * Hour, 48 * Hour + workingDayDuration) },

                RouteCosts = new[,]
                {
                    { 0, 1430, 428, 2388, 863, 585, 696, 900, 1122, 2571, 657, 923, 1637, 627, 1345, 2138, 2111, 1803, 608, 1202, 359, 1960, 861, 451 },
                    { 1430, 0, 1002, 3343, 567, 932, 828, 1158, 1347, 3675, 1813, 2217, 2624, 1247, 2327, 2976, 3115, 813, 881, 1168, 1106, 2966, 2076, 1496 },
                    { 428, 1002, 0, 2612, 435, 247, 325, 714, 977, 2857, 912, 1290, 1857, 538, 1551, 2311, 2350, 1442, 235, 1016, 150, 2197, 1163, 604 },
                    { 2388, 3343, 2612, 0, 2903, 2510, 2578, 2185, 2025, 517, 1734, 2706, 756, 2140, 1061, 420, 287, 4013, 2568, 3583, 2462, 430, 1540, 2013 },
                    { 863, 567, 435, 2903, 0, 401, 325, 781, 1031, 3194, 1280, 1686, 2160, 763, 1855, 2566, 2657, 1110, 343, 974, 543, 2504, 1542, 961 },
                    { 585, 932, 247, 2510, 401, 0, 115, 482, 748, 2793, 884, 1496, 1763, 373, 1458, 2183, 2260, 1504, 58, 1210, 226, 2107, 1147, 566 },
                    { 696, 828, 325, 2578, 325, 115, 0, 479, 741, 2875, 984, 1600, 1837, 439, 1533, 2240, 2333, 1436, 91, 1212, 337, 2181, 1248, 668 },
                    { 900, 1158, 714, 2185, 781, 482, 479, 0, 266, 2531, 824, 1810, 1475, 286, 1186, 1821, 1961, 1870, 523, 1687, 623, 1814, 1064, 595 },
                    { 1122, 1347, 977, 2025, 1031, 748, 741, 266, 0, 2407, 898, 2007, 1355, 495, 1091, 1640, 1823, 2097, 789, 1952, 877, 1682, 1100, 750 },
                    { 2571, 3675, 2857, 517, 3194, 2793, 2875, 2531, 2407, 0, 1947, 2713, 1056, 2437, 1348, 914, 594, 4292, 2851, 3772, 2708, 726, 1711, 2254 },
                    { 657, 1813, 912, 1734, 1280, 884, 984, 824, 898, 1947, 0, 1260, 981, 593, 688, 1483, 1460, 2353, 939, 1849, 765, 1308, 263, 319 },
                    { 923, 2217, 1290, 2706, 1686, 1496, 1600, 1810, 2007, 2713, 1260, 0, 2048, 1527, 1831, 2585, 2420, 2335, 1509, 1491, 1270, 2291, 1282, 1263 },
                    { 1637, 2624, 1857, 756, 2160, 1763, 1837, 1475, 1355, 1056, 981, 2048, 0, 1398, 306, 536, 496, 3268, 1821, 2828, 1707, 344, 807, 1256 },
                    { 627, 1247, 538, 2140, 763, 373, 439, 286, 495, 2437, 593, 1527, 1398, 0, 1094, 1810, 1894, 1873, 430, 1553, 409, 1742, 852, 320 },
                    { 1345, 2327, 1551, 1061, 1855, 1458, 1533, 1186, 1091, 1348, 688, 1831, 306, 1094, 0, 796, 802, 2962, 1516, 2528, 1401, 649, 553, 952 },
                    { 2138, 2976, 2311, 420, 2566, 2183, 2240, 1821, 1640, 914, 1483, 2585, 536, 1810, 796, 0, 390, 3676, 2240, 3308, 2162, 391, 1336, 1728 },
                    { 2111, 3115, 2350, 287, 2657, 2260, 2333, 1961, 1823, 594, 1460, 2420, 496, 1894, 802, 390, 0, 3764, 2318, 3309, 2200, 153, 1258, 1747 },
                    { 1803, 813, 1442, 4013, 1110, 1504, 1436, 1870, 2097, 4292, 2353, 2335, 3268, 1873, 2962, 3676, 3764, 0, 1446, 891, 1588, 3611, 2606, 2039 },
                    { 608, 881, 235, 2568, 343, 58, 91, 523, 789, 2851, 939, 1509, 1821, 430, 1516, 2240, 2318, 1446, 0, 1164, 250, 2165, 1202, 620 },
                    { 1202, 1168, 1016, 3583, 974, 1210, 1212, 1687, 1952, 3772, 1849, 1491, 2828, 1553, 2528, 3308, 3309, 891, 1164, 0, 1158, 3157, 2063, 1580 },
                    { 359, 1106, 150, 2462, 543, 226, 337, 623, 877, 2708, 765, 1270, 1707, 409, 1401, 2162, 2200, 1588, 250, 1158, 0, 2046, 1019, 454 },
                    { 1960, 2966, 2197, 430, 2504, 2107, 2181, 1814, 1682, 726, 1308, 2291, 344, 1742, 649, 391, 153, 3611, 2165, 3157, 2046, 0, 1110, 1594 },
                    { 861, 2076, 1163, 1540, 1542, 1147, 1248, 1064, 1100, 1711, 263, 1282, 807, 852, 553, 1336, 1258, 2606, 1202, 2063, 1019, 1110, 0, 581 },
                    { 451, 1496, 604, 2013, 961, 566, 668, 595, 750, 2254, 319, 1263, 1256, 320, 952, 1728, 1747, 2039, 620, 1580, 454, 1594, 581, 0 }
                },

                Santas = new[]
                {
                    new Santa { Id = 0 },
                    new Santa { Id = 1 },
                    new Santa { Id = 2 }
                },

                Visits = new[]
                {
                    new Visit{Duration = 2149, Id=0,WayCostFromHome=1563, WayCostToHome=1563,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3041, (0 * Hour + 3041) + 17057)}},
                    new Visit{Duration = 1359, Id=1,WayCostFromHome=2649, WayCostToHome=2649,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 13345, (0 * Hour + 13345) + 6525)}},
                    new Visit{Duration = 2799, Id=2,WayCostFromHome=1825, WayCostToHome=1825,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 11819, (0 * Hour + 11819) + 7000)}},
                    new Visit{Duration = 1265, Id=3,WayCostFromHome=838, WayCostToHome=838,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1555, (0 * Hour + 1555) + 18723)}},
                    new Visit{Duration = 2170, Id=4,WayCostFromHome=2159, WayCostToHome=2159,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 14561, (0 * Hour + 14561) + 5561)}},
                    new Visit{Duration = 2906, Id=5,WayCostFromHome=1758, WayCostToHome=1758,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4487, (0 * Hour + 4487) + 11616)}},
                    new Visit{Duration = 1499, Id=6,WayCostFromHome=1841, WayCostToHome=1841,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 1953, (24 * Hour + 1953) + 12477)}},
                    new Visit{Duration = 3029, Id=7,WayCostFromHome=1521, WayCostToHome=1521,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 59, (24 * Hour + 59) + 20698)}},
                    new Visit{Duration = 1787, Id=8,WayCostFromHome=1435, WayCostToHome=1435,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 7926, (24 * Hour + 7926) + 11663)}},
                    new Visit{Duration = 1896, Id=9,WayCostFromHome=1035, WayCostToHome=1035,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 5372, (24 * Hour + 5372) + 12230)}},
                    new Visit{Duration = 3188, Id=10,WayCostFromHome=920, WayCostToHome=920,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 358, (24 * Hour + 358) + 19501)}},
                    new Visit{Duration = 1693, Id=11,WayCostFromHome=1892, WayCostToHome=1892,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 5021, (24 * Hour + 5021) + 11999)}},
                    new Visit{Duration = 2613, Id=12,WayCostFromHome=209, WayCostToHome=209,Unavailable =new (int from, int to)[0],Desired = new [] {(48 * Hour + 2247, (48 * Hour + 2247) + 18509)}},
                    new Visit{Duration = 2698, Id=13,WayCostFromHome=1405, WayCostToHome=1405,Unavailable =new (int from, int to)[0],Desired = new [] {(48 * Hour + 1694, (48 * Hour + 1694) + 15783)}},
                    new Visit{Duration = 3155, Id=14,WayCostFromHome=344, WayCostToHome=344,Unavailable =new (int from, int to)[0],Desired = new [] {(48 * Hour + 301, (48 * Hour + 301) + 19446)}},
                    new Visit{Duration = 3556, Id=15,WayCostFromHome=707, WayCostToHome=707,Unavailable =new (int from, int to)[0],Desired = new [] {(48 * Hour + 938, (48 * Hour + 938) + 10884)}},
                    new Visit{Duration = 2939, Id=16,WayCostFromHome=553, WayCostToHome=553,Unavailable =new (int from, int to)[0],Desired = new [] {(48 * Hour + 7143, (48 * Hour + 7143) + 6520)}},
                    new Visit{Duration = 1995, Id=17,WayCostFromHome=3256, WayCostToHome=3256,Unavailable =new (int from, int to)[0],Desired = new [] {(48 * Hour + 14419, (48 * Hour + 14419) + 6039)}},
                    new Visit{Duration = 2878, Id=18,WayCostFromHome=1816, WayCostToHome=1816,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 17911, (0 * Hour + 17911) + 3511),(24 * Hour + 17911, (24 * Hour + 17911) + 3511),(48 * Hour + 17911, (48 * Hour + 17911) + 3511)},SantaId=0,IsBreak = true},
                    new Visit{Duration = 3472, Id=19,WayCostFromHome=2764, WayCostToHome=2764,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 16901, (0 * Hour + 16901) + 3732),(24 * Hour + 16901, (24 * Hour + 16901) + 3732),(48 * Hour + 16901, (48 * Hour + 16901) + 3732)},SantaId=1,IsBreak = true},
                    new Visit{Duration = 2877, Id=20,WayCostFromHome=1675, WayCostToHome=1675,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1756, (0 * Hour + 1756) + 18818),(24 * Hour + 1756, (24 * Hour + 1756) + 18818),(48 * Hour + 1756, (48 * Hour + 1756) + 18818)},SantaId=2,IsBreak = true}
                }
            };
            return (input, coordinates);
        }
    }
}
