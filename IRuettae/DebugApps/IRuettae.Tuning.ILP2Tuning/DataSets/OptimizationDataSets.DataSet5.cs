using IRuettae.Core.Models;
namespace IRuettae.Tuning.ILP2Tuning.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet5()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2557,2680),
		(1326,1117),
		(1260,1238),
		(2670,2399),
		(2244,2609),
		(998,903),
		(1319,990),
		(1428,1307),
		(2642,2475),
		(2250,3052),
		(3478,2642),
		(1263,1785),
		(1353,990),
		(1746,1165),
		(2555,3551),
		(478,668),
		(2571,2893),
		(2528,3024),
		(2682,2543)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 137, 1857, 1751, 391, 127, 215, 1891, 2144, 2637, 670, 129, 422, 2726, 959, 2168, 2254, 1967 },
			{ 137, 0, 1826, 1687, 425, 254, 181, 1854, 2066, 2625, 547, 264, 491, 2650, 967, 2111, 2190, 1930 },
			{ 1857, 1826, 0, 474, 2243, 1952, 1653, 80, 776, 843, 1535, 1928, 1541, 1157, 2793, 503, 640, 144 },
			{ 1751, 1687, 474, 0, 2112, 1864, 1536, 419, 443, 1234, 1281, 1847, 1527, 992, 2624, 433, 502, 442 },
			{ 391, 425, 2243, 2112, 0, 332, 590, 2274, 2487, 3028, 920, 365, 792, 3071, 570, 2536, 2615, 2350 },
			{ 127, 254, 1952, 1864, 332, 0, 335, 1988, 2262, 2718, 796, 34, 461, 2843, 900, 2277, 2366, 2066 },
			{ 215, 181, 1653, 1536, 590, 335, 0, 1684, 1928, 2446, 505, 325, 348, 2511, 1144, 1954, 2039, 1760 },
			{ 1891, 1854, 80, 419, 2274, 1988, 1684, 0, 697, 852, 1541, 1966, 1587, 1079, 2819, 423, 560, 78 },
			{ 2144, 2066, 776, 443, 2487, 2262, 1928, 697, 0, 1294, 1606, 2248, 1953, 584, 2970, 358, 279, 667 },
			{ 2637, 2625, 843, 1234, 3028, 2718, 2446, 852, 1294, 0, 2375, 2691, 2276, 1295, 3591, 941, 1023, 802 },
			{ 670, 547, 1535, 1281, 920, 796, 505, 1541, 1606, 2375, 0, 800, 785, 2188, 1365, 1714, 1770, 1608 },
			{ 129, 264, 1928, 1847, 365, 34, 325, 1966, 2248, 2691, 800, 0, 430, 2829, 932, 2259, 2348, 2044 },
			{ 422, 491, 1541, 1527, 792, 461, 348, 1587, 1953, 2276, 785, 430, 0, 2519, 1361, 1914, 2016, 1665 },
			{ 2726, 2650, 1157, 992, 3071, 2843, 2511, 1079, 584, 1295, 2188, 2829, 2519, 0, 3553, 658, 527, 1015 },
			{ 959, 967, 2793, 2624, 570, 900, 1144, 2819, 2970, 3591, 1365, 932, 1361, 3553, 0, 3054, 3123, 2893 },
			{ 2168, 2111, 503, 433, 2536, 2277, 1954, 423, 358, 941, 1714, 2259, 1914, 658, 3054, 0, 137, 367 },
			{ 2254, 2190, 640, 502, 2615, 2366, 2039, 560, 279, 1023, 1770, 2348, 2016, 527, 3123, 137, 0, 505 },
			{ 1967, 1930, 144, 442, 2350, 2066, 1760, 78, 667, 802, 1608, 2044, 1665, 1015, 2893, 367, 505, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2867, Id=0,WayCostFromHome=1989, WayCostToHome=1989,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1717, Id=1,WayCostFromHome=1939, WayCostToHome=1939,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2346, Id=2,WayCostFromHome=302, WayCostToHome=302,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3436, Id=3,WayCostFromHome=320, WayCostToHome=320,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3425, Id=4,WayCostFromHome=2363, WayCostToHome=2363,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3154, Id=5,WayCostFromHome=2094, WayCostToHome=2094,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2453, Id=6,WayCostFromHome=1777, WayCostToHome=1777,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3015, Id=7,WayCostFromHome=221, WayCostToHome=221,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1635, Id=8,WayCostFromHome=482, WayCostToHome=482,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2712, Id=9,WayCostFromHome=921, WayCostToHome=921,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1715, Id=10,WayCostFromHome=1573, WayCostToHome=1573,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2114, Id=11,WayCostFromHome=2075, WayCostToHome=2075,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2560, Id=12,WayCostFromHome=1718, WayCostToHome=1718,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2691, Id=13,WayCostFromHome=871, WayCostToHome=871,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3101, Id=14,WayCostFromHome=2893, WayCostToHome=2893,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2206, Id=15,WayCostFromHome=213, WayCostToHome=213,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=345, WayCostToHome=345,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 13625, (0 * Hour + 13625) + 12603),(24 * Hour + 13625, (24 * Hour + 13625) + 12603)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=185, WayCostToHome=185,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2488, (0 * Hour + 2488) + 29792),(24 * Hour + 2488, (24 * Hour + 2488) + 29792)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
