using IRuettae.Core.Models;
namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet3()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(148,9),
		(1075,892),
		(2637,2652),
		(2106,2900),
		(2020,1161),
		(2849,2327),
		(1650,1551),
		(1108,1823),
		(3006,2396),
		(2636,2395),
		(3058,3008),
		(795,547),
		(1550,2933),
		(1607,1309),
		(1783,1628),
		(2681,2640),
		(2597,2171),
		(2792,2171),
		(3017,2579)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2353, 2257, 982, 2281, 874, 931, 2447, 2166, 2899, 444, 2095, 675, 1021, 2373, 1988, 2141, 2572 },
			{ 2353, 0, 586, 1613, 388, 1478, 1739, 449, 257, 551, 2797, 1122, 1692, 1333, 45, 482, 505, 386 },
			{ 2257, 586, 0, 1741, 938, 1423, 1468, 1031, 732, 958, 2693, 556, 1667, 1312, 631, 878, 1001, 965 },
			{ 982, 1613, 1741, 0, 1430, 537, 1126, 1580, 1379, 2118, 1370, 1833, 438, 523, 1619, 1163, 1271, 1733 },
			{ 2281, 388, 938, 1430, 0, 1428, 1812, 171, 223, 712, 2717, 1433, 1605, 1274, 355, 296, 166, 302 },
			{ 874, 1478, 1423, 537, 1428, 0, 606, 1597, 1297, 2026, 1318, 1385, 245, 153, 1499, 1131, 1299, 1710 },
			{ 931, 1739, 1468, 1126, 1812, 606, 0, 1982, 1631, 2281, 1313, 1194, 716, 702, 1772, 1529, 1719, 2053 },
			{ 2447, 449, 1031, 1580, 171, 1597, 1982, 0, 370, 614, 2882, 1551, 1771, 1444, 406, 466, 310, 183 },
			{ 2166, 257, 732, 1379, 223, 1297, 1631, 370, 0, 744, 2608, 1211, 1496, 1147, 249, 227, 272, 423 },
			{ 2899, 551, 958, 2118, 712, 2026, 2281, 614, 744, 0, 3343, 1509, 2234, 1878, 526, 955, 878, 430 },
			{ 444, 2797, 2693, 1370, 2717, 1318, 1313, 2882, 2608, 3343, 0, 2502, 1113, 1464, 2817, 2425, 2573, 3011 },
			{ 2095, 1122, 556, 1833, 1433, 1385, 1194, 1551, 1211, 1509, 2502, 0, 1625, 1325, 1168, 1294, 1457, 1509 },
			{ 675, 1692, 1667, 438, 1605, 245, 716, 1771, 1496, 2234, 1113, 1625, 0, 364, 1710, 1312, 1465, 1897 },
			{ 1021, 1333, 1312, 523, 1274, 153, 702, 1444, 1147, 1878, 1464, 1325, 364, 0, 1352, 978, 1145, 1557 },
			{ 2373, 45, 631, 1619, 355, 1499, 1772, 406, 249, 526, 2817, 1168, 1710, 1352, 0, 476, 481, 341 },
			{ 1988, 482, 878, 1163, 296, 1131, 1529, 466, 227, 955, 2425, 1294, 1312, 978, 476, 0, 195, 585 },
			{ 2141, 505, 1001, 1271, 166, 1299, 1719, 310, 272, 878, 2573, 1457, 1465, 1145, 481, 195, 0, 465 },
			{ 2572, 386, 965, 1733, 302, 1710, 2053, 183, 423, 430, 3011, 1509, 1897, 1557, 341, 585, 465, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1279, Id=0,WayCostFromHome=1280, WayCostToHome=1280,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2762, Id=1,WayCostFromHome=3630, WayCostToHome=3630,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1538, Id=2,WayCostFromHome=3491, WayCostToHome=3491,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1558, Id=3,WayCostFromHome=2198, WayCostToHome=2198,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2846, Id=4,WayCostFromHome=3559, WayCostToHome=3559,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3086, Id=5,WayCostFromHome=2152, WayCostToHome=2152,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1398, Id=6,WayCostFromHome=2052, WayCostToHome=2052,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2113, Id=7,WayCostFromHome=3723, WayCostToHome=3723,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2136, Id=8,WayCostFromHome=3447, WayCostToHome=3447,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1488, Id=9,WayCostFromHome=4178, WayCostToHome=4178,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2050, Id=10,WayCostFromHome=841, WayCostToHome=841,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2238, Id=11,WayCostFromHome=3242, WayCostToHome=3242,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1907, Id=12,WayCostFromHome=1954, WayCostToHome=1954,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3077, Id=13,WayCostFromHome=2300, WayCostToHome=2300,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3205, Id=14,WayCostFromHome=3652, WayCostToHome=3652,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2627, Id=15,WayCostFromHome=3266, WayCostToHome=3266,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=3415, WayCostToHome=3415,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 10172, (0 * Hour + 10172) + 4389),(24 * Hour + 10172, (24 * Hour + 10172) + 4389)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=3851, WayCostToHome=3851,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 16349, (0 * Hour + 16349) + 4126),(24 * Hour + 16349, (24 * Hour + 16349) + 4126)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
