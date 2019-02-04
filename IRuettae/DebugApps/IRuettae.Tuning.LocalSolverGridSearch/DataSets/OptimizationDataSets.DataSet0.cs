using IRuettae.Core.Models;
namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 4 Desired, 2 Unavailable on day 0
/// 4 Desired, 4 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet0()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1054,1264),
		(1340,2156),
		(1380,2279),
		(538,656),
		(1383,1586),
		(2549,2364),
		(912,1951),
		(591,1446),
		(2443,958),
		(2677,2691),
		(1153,1126),
		(2228,3152),
		(3053,2481),
		(2047,2548),
		(2734,2501),
		(2850,2780),
		(1387,497),
		(1305,1485),
		(2922,2409)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 129, 1700, 571, 1226, 474, 1032, 1628, 1440, 1046, 1334, 1743, 808, 1436, 1633, 1659, 671, 1602 },
			{ 129, 0, 1828, 693, 1172, 571, 1147, 1695, 1360, 1175, 1217, 1685, 719, 1372, 1553, 1782, 797, 1547 },
			{ 1700, 1828, 0, 1256, 2638, 1347, 791, 1928, 2952, 774, 3014, 3107, 2420, 2868, 3139, 863, 1129, 2959 },
			{ 571, 693, 1256, 0, 1401, 595, 804, 1232, 1701, 514, 1779, 1894, 1168, 1631, 1891, 1089, 127, 1745 },
			{ 1226, 1172, 2638, 1401, 0, 1688, 2162, 1409, 351, 1865, 850, 517, 534, 230, 513, 2199, 1523, 375 },
			{ 474, 571, 1347, 595, 1688, 0, 598, 1824, 1913, 859, 1781, 2205, 1282, 1903, 2107, 1529, 609, 2061 },
			{ 1032, 1147, 791, 804, 2162, 598, 0, 1915, 2429, 646, 2364, 2670, 1826, 2388, 2623, 1238, 715, 2522 },
			{ 1628, 1695, 1928, 1232, 1409, 1824, 1915, 0, 1748, 1300, 2204, 1640, 1638, 1570, 1866, 1152, 1254, 1528 },
			{ 1440, 1360, 2952, 1701, 351, 1913, 2429, 1748, 0, 2184, 643, 430, 646, 198, 194, 2545, 1826, 373 },
			{ 1046, 1175, 774, 514, 1865, 859, 646, 1300, 2184, 0, 2293, 2333, 1679, 2095, 2369, 671, 389, 2185 },
			{ 1334, 1217, 3014, 1779, 850, 1781, 2364, 2204, 643, 2293, 0, 1063, 630, 824, 724, 2785, 1905, 1016 },
			{ 1743, 1685, 3107, 1894, 517, 2205, 2670, 1640, 430, 2333, 1063, 0, 1008, 319, 361, 2590, 2011, 149 },
			{ 808, 719, 2420, 1168, 534, 1282, 1826, 1638, 646, 1679, 630, 1008, 0, 688, 835, 2154, 1296, 885 },
			{ 1436, 1372, 2868, 1631, 230, 1903, 2388, 1570, 198, 2095, 824, 319, 688, 0, 302, 2414, 1753, 209 },
			{ 1633, 1553, 3139, 1891, 513, 2107, 2623, 1866, 194, 2369, 724, 361, 835, 302, 0, 2711, 2015, 377 },
			{ 1659, 1782, 863, 1089, 2199, 1529, 1238, 1152, 2545, 671, 2785, 2590, 2154, 2414, 2711, 0, 991, 2451 },
			{ 671, 797, 1129, 127, 1523, 609, 715, 1254, 1826, 389, 1905, 2011, 1296, 1753, 2015, 991, 0, 1862 },
			{ 1602, 1547, 2959, 1745, 375, 2061, 2522, 1528, 373, 2185, 1016, 149, 885, 209, 377, 2451, 1862, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2560, Id=0,WayCostFromHome=936, WayCostToHome=936,Unavailable =new [] {(24 * Hour + 6940, (24 * Hour + 6940) + 10123)},Desired = new [] {(0 * Hour + 11899, (0 * Hour + 11899) + 20062)}},
			new Visit{Duration = 1509, Id=1,WayCostFromHome=1066, WayCostToHome=1066,Unavailable =new [] {(24 * Hour + 2069, (24 * Hour + 2069) + 22867)},Desired = new [] {(0 * Hour + 22599, (0 * Hour + 22599) + 6473)}},
			new Visit{Duration = 2722, Id=2,WayCostFromHome=797, WayCostToHome=797,Unavailable =new [] {(24 * Hour + 11785, (24 * Hour + 11785) + 12280)},Desired = new [] {(0 * Hour + 8883, (0 * Hour + 8883) + 22924)}},
			new Visit{Duration = 2138, Id=3,WayCostFromHome=460, WayCostToHome=460,Unavailable =new [] {(24 * Hour + 917, (24 * Hour + 917) + 29454)},Desired = new [] {(0 * Hour + 19477, (0 * Hour + 19477) + 12799)}},
			new Visit{Duration = 2528, Id=4,WayCostFromHome=1856, WayCostToHome=1856,Unavailable =new [] {(0 * Hour + 158, (0 * Hour + 158) + 10849)},Desired = new [] {(24 * Hour + 15978, (24 * Hour + 15978) + 2601)}},
			new Visit{Duration = 1994, Id=5,WayCostFromHome=701, WayCostToHome=701,Unavailable =new [] {(0 * Hour + 14128, (0 * Hour + 14128) + 11162)},Desired = new [] {(24 * Hour + 4633, (24 * Hour + 4633) + 19379)}},
			new Visit{Duration = 2384, Id=6,WayCostFromHome=497, WayCostToHome=497,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 9642, (24 * Hour + 9642) + 11899)}},
			new Visit{Duration = 3213, Id=7,WayCostFromHome=1422, WayCostToHome=1422,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 464, (24 * Hour + 464) + 31762)}},
			new Visit{Duration = 3501, Id=8,WayCostFromHome=2161, WayCostToHome=2161,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2010, Id=9,WayCostFromHome=169, WayCostToHome=169,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2314, Id=10,WayCostFromHome=2223, WayCostToHome=2223,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1888, Id=11,WayCostFromHome=2340, WayCostToHome=2340,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3442, Id=12,WayCostFromHome=1623, WayCostToHome=1623,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3474, Id=13,WayCostFromHome=2086, WayCostToHome=2086,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3022, Id=14,WayCostFromHome=2350, WayCostToHome=2350,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2711, Id=15,WayCostFromHome=836, WayCostToHome=836,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=334, WayCostToHome=334,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 12250, (0 * Hour + 12250) + 8679),(24 * Hour + 12250, (24 * Hour + 12250) + 8679)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=2190, WayCostToHome=2190,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1506, (0 * Hour + 1506) + 21994),(24 * Hour + 1506, (24 * Hour + 1506) + 21994)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
