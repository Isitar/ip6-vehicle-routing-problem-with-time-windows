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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet19()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2006,2961),
		(123,966),
		(2454,2319),
		(740,1781),
		(1250,1098),
		(3007,2740),
		(2603,2777),
		(2404,2730),
		(1572,949),
		(2504,2478),
		(2476,2769),
		(1476,902),
		(1232,3732),
		(1849,1543),
		(2405,2983),
		(2551,2616),
		(2669,2839),
		(2850,2934),
		(3212,2880)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2695, 1022, 1134, 3385, 3070, 2883, 1449, 2820, 2964, 1354, 2980, 1819, 3045, 2935, 3160, 3362, 3633 },
			{ 2695, 0, 1796, 1714, 695, 481, 414, 1629, 166, 450, 1721, 1868, 983, 665, 312, 562, 731, 943 },
			{ 1022, 1796, 0, 852, 2461, 2112, 1915, 1176, 1896, 1997, 1146, 2012, 1134, 2053, 1994, 2200, 2404, 2705 },
			{ 1134, 1714, 852, 0, 2404, 2156, 1998, 354, 1864, 2072, 299, 2634, 746, 2210, 1999, 2246, 2435, 2650 },
			{ 3385, 695, 2461, 2404, 0, 405, 603, 2294, 567, 531, 2392, 2033, 1665, 649, 472, 352, 249, 248 },
			{ 3070, 481, 2112, 2156, 405, 0, 204, 2098, 314, 127, 2187, 1670, 1446, 285, 169, 90, 292, 617 },
			{ 2883, 414, 1915, 1998, 603, 204, 0, 1965, 271, 81, 2050, 1541, 1310, 253, 186, 286, 490, 821 },
			{ 1449, 1629, 1176, 354, 2294, 2098, 1965, 0, 1790, 2032, 106, 2803, 655, 2197, 1933, 2185, 2360, 2533 },
			{ 2820, 166, 1896, 1864, 567, 314, 271, 1790, 0, 292, 1881, 1786, 1141, 514, 145, 396, 572, 814 },
			{ 2964, 450, 1997, 2072, 531, 127, 81, 2032, 292, 0, 2117, 1573, 1377, 225, 170, 205, 408, 744 },
			{ 1354, 1721, 1146, 299, 2392, 2187, 2050, 106, 1881, 2117, 0, 2840, 741, 2278, 2023, 2274, 2452, 2631 },
			{ 2980, 1868, 2012, 2634, 2033, 1670, 1541, 2803, 1786, 1573, 2840, 0, 2274, 1391, 1727, 1691, 1804, 2155 },
			{ 1819, 983, 1134, 746, 1665, 1446, 1310, 655, 1141, 1377, 741, 2274, 0, 1543, 1282, 1533, 1713, 1909 },
			{ 3045, 665, 2053, 2210, 649, 285, 253, 2197, 514, 225, 2278, 1391, 1543, 0, 394, 300, 447, 813 },
			{ 2935, 312, 1994, 1999, 472, 169, 186, 1933, 145, 170, 2023, 1727, 1282, 394, 0, 252, 436, 711 },
			{ 3160, 562, 2200, 2246, 352, 90, 286, 2185, 396, 205, 2274, 1691, 1533, 300, 252, 0, 204, 544 },
			{ 3362, 731, 2404, 2435, 249, 292, 490, 2360, 572, 408, 2452, 1804, 1713, 447, 436, 204, 0, 366 },
			{ 3633, 943, 2705, 2650, 248, 617, 821, 2533, 814, 744, 2631, 2155, 1909, 813, 711, 544, 366, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3431, Id=0,WayCostFromHome=2743, WayCostToHome=2743,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2781, Id=1,WayCostFromHome=782, WayCostToHome=782,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2882, Id=2,WayCostFromHome=1730, WayCostToHome=1730,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2147, Id=3,WayCostFromHome=2010, WayCostToHome=2010,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2044, Id=4,WayCostFromHome=1025, WayCostToHome=1025,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1500, Id=5,WayCostFromHome=624, WayCostToHome=624,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2819, Id=6,WayCostFromHome=460, WayCostToHome=460,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2557, Id=7,WayCostFromHome=2058, WayCostToHome=2058,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1332, Id=8,WayCostFromHome=693, WayCostToHome=693,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3485, Id=9,WayCostFromHome=507, WayCostToHome=507,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3053, Id=10,WayCostFromHome=2126, WayCostToHome=2126,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2050, Id=11,WayCostFromHome=1092, WayCostToHome=1092,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1622, Id=12,WayCostFromHome=1426, WayCostToHome=1426,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1755, Id=13,WayCostFromHome=399, WayCostToHome=399,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2141, Id=14,WayCostFromHome=645, WayCostToHome=645,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2682, Id=15,WayCostFromHome=674, WayCostToHome=674,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=844, WayCostToHome=844,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4613, (0 * Hour + 4613) + 15375),(24 * Hour + 4613, (24 * Hour + 4613) + 15375)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1208, WayCostToHome=1208,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2675, (0 * Hour + 2675) + 25732),(24 * Hour + 2675, (24 * Hour + 2675) + 25732)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
