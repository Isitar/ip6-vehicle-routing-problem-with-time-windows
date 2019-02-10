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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet13()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(850,1076),
		(1613,1108),
		(528,1733),
		(901,1461),
		(3944,1044),
		(1679,984),
		(1032,2018),
		(208,3056),
		(2095,2356),
		(2001,2488),
		(2819,1956),
		(1690,770),
		(1488,1508),
		(1307,1345),
		(1693,1922),
		(1787,2197),
		(2350,2339),
		(3255,2577),
		(3224,3424)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1252, 794, 2331, 140, 1079, 2401, 1337, 1433, 1474, 346, 419, 387, 817, 1102, 1434, 2203, 2821 },
			{ 1252, 0, 461, 3484, 1373, 579, 1361, 1686, 1655, 2301, 1509, 986, 870, 1180, 1341, 1920, 2854, 3182 },
			{ 794, 461, 0, 3071, 912, 572, 1739, 1492, 1504, 1980, 1048, 588, 422, 916, 1151, 1694, 2605, 3041 },
			{ 2331, 3484, 3071, 0, 2265, 3070, 4243, 2267, 2420, 1448, 2270, 2499, 2654, 2416, 2445, 2053, 1680, 2486 },
			{ 140, 1373, 912, 2265, 0, 1219, 2541, 1433, 1538, 1498, 214, 557, 518, 938, 1217, 1512, 2240, 2888 },
			{ 1079, 579, 572, 3070, 1219, 0, 1325, 1115, 1076, 1788, 1410, 684, 727, 667, 775, 1356, 2292, 2604 },
			{ 2401, 1361, 1739, 4243, 2541, 1325, 0, 2012, 1880, 2833, 2724, 2008, 2033, 1868, 1797, 2258, 3084, 3038 },
			{ 1337, 1686, 1492, 2267, 1433, 1115, 2012, 0, 162, 827, 1636, 1042, 1281, 591, 346, 255, 1180, 1554 },
			{ 1433, 1655, 1504, 2420, 1538, 1076, 1880, 162, 0, 975, 1745, 1106, 1337, 644, 361, 379, 1257, 1540 },
			{ 1474, 2301, 1980, 1448, 1498, 1788, 2833, 827, 975, 0, 1637, 1404, 1630, 1126, 1059, 605, 758, 1522 },
			{ 346, 1509, 1048, 2270, 214, 1410, 2724, 1636, 1745, 1637, 0, 765, 690, 1152, 1430, 1702, 2390, 3065 },
			{ 419, 986, 588, 2499, 557, 684, 2008, 1042, 1106, 1404, 765, 0, 243, 461, 751, 1197, 2065, 2585 },
			{ 387, 870, 422, 2654, 518, 727, 2033, 1281, 1337, 1630, 690, 243, 0, 694, 977, 1440, 2304, 2827 },
			{ 817, 1180, 916, 2416, 938, 667, 1868, 591, 644, 1126, 1152, 461, 694, 0, 290, 778, 1693, 2144 },
			{ 1102, 1341, 1151, 2445, 1217, 775, 1797, 346, 361, 1059, 1430, 751, 977, 290, 0, 580, 1516, 1889 },
			{ 1434, 1920, 1694, 2053, 1512, 1356, 2258, 255, 379, 605, 1702, 1197, 1440, 778, 580, 0, 935, 1393 },
			{ 2203, 2854, 2605, 1680, 2240, 2292, 3084, 1180, 1257, 758, 2390, 2065, 2304, 1693, 1516, 935, 0, 847 },
			{ 2821, 3182, 3041, 2486, 2888, 2604, 3038, 1554, 1540, 1522, 3065, 2585, 2827, 2144, 1889, 1393, 847, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1975, Id=0,WayCostFromHome=763, WayCostToHome=763,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1692, Id=1,WayCostFromHome=731, WayCostToHome=731,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3578, Id=2,WayCostFromHome=388, WayCostToHome=388,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1857, Id=3,WayCostFromHome=3094, WayCostToHome=3094,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1240, Id=4,WayCostFromHome=834, WayCostToHome=834,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2201, Id=5,WayCostFromHome=959, WayCostToHome=959,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2468, Id=6,WayCostFromHome=2081, WayCostToHome=2081,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3345, Id=7,WayCostFromHome=1785, WayCostToHome=1785,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2665, Id=8,WayCostFromHome=1821, WayCostToHome=1821,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1781, Id=9,WayCostFromHome=2156, WayCostToHome=2156,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3224, Id=10,WayCostFromHome=894, WayCostToHome=894,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2535, Id=11,WayCostFromHome=770, WayCostToHome=770,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2540, Id=12,WayCostFromHome=530, WayCostToHome=530,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2281, Id=13,WayCostFromHome=1194, WayCostToHome=1194,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3434, Id=14,WayCostFromHome=1461, WayCostToHome=1461,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2375, Id=15,WayCostFromHome=1960, WayCostToHome=1960,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=2834, WayCostToHome=2834,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2391, (0 * Hour + 2391) + 28819),(24 * Hour + 2391, (24 * Hour + 2391) + 28819)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=3339, WayCostToHome=3339,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3489, (0 * Hour + 3489) + 8604),(24 * Hour + 3489, (24 * Hour + 3489) + 8604)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
