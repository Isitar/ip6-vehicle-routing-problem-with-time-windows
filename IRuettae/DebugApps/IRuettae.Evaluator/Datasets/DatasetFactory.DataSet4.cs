using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet4()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1298,1580),
		(540,1074),
		(2549,1673),
		(2415,2739),
		(2907,2921),
		(2434,2811),
		(1151,1929),
		(3260,2619),
		(1271,857),
		(982,1374),
		(2863,3165),
		(3009,2590),
		(854,852),
		(1233,1669),
		(3117,2804),
		(3369,3041),
		(3580,1517),
		(1274,959),
		(1270,1536),
		(3088,2546),
		(2306,3105)
	};
	const int workingDayDuration = 10 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2096, 2507, 3002, 2569, 1050, 3128, 762, 534, 3125, 2897, 384, 913, 3103, 3445, 3072, 742, 863, 2942, 2691 },
			{ 2096, 0, 1074, 1298, 1143, 1421, 1183, 1516, 1595, 1524, 1025, 1883, 1316, 1265, 1594, 1042, 1461, 1286, 1025, 1452 },
			{ 2507, 1074, 0, 524, 74, 1501, 853, 2202, 1979, 618, 612, 2448, 1594, 705, 1000, 1688, 2114, 1660, 700, 381 },
			{ 3002, 1298, 524, 0, 485, 2016, 464, 2633, 2469, 247, 346, 2914, 2090, 240, 477, 1556, 2552, 2144, 416, 628 },
			{ 2569, 1143, 74, 485, 0, 1556, 848, 2273, 2042, 556, 616, 2516, 1657, 683, 962, 1728, 2185, 1726, 705, 320 },
			{ 1050, 1421, 1501, 2016, 1556, 0, 2219, 1078, 580, 2111, 1972, 1117, 272, 2151, 2481, 2463, 977, 410, 2032, 1648 },
			{ 3128, 1183, 853, 464, 848, 2219, 0, 2657, 2596, 675, 252, 2985, 2238, 233, 435, 1147, 2588, 2265, 186, 1070 },
			{ 762, 1516, 2202, 2633, 2273, 1078, 2657, 0, 592, 2803, 2454, 417, 812, 2683, 3028, 2401, 102, 679, 2480, 2474 },
			{ 534, 1595, 1979, 2469, 2042, 580, 2596, 592, 0, 2597, 2363, 537, 387, 2569, 2911, 2601, 507, 330, 2410, 2179 },
			{ 3125, 1524, 618, 247, 556, 2111, 675, 2803, 2597, 0, 593, 3063, 2212, 441, 520, 1797, 2718, 2278, 658, 560 },
			{ 2897, 1025, 612, 346, 616, 1972, 252, 2454, 2363, 593, 0, 2768, 2000, 239, 577, 1215, 2381, 2033, 90, 871 },
			{ 384, 1883, 2448, 2914, 2516, 1117, 2985, 417, 537, 3063, 2768, 0, 900, 2988, 3334, 2805, 433, 800, 2803, 2680 },
			{ 913, 1316, 1594, 2090, 1657, 272, 2238, 812, 387, 2212, 2000, 900, 0, 2199, 2538, 2351, 711, 138, 2051, 1792 },
			{ 3103, 1265, 705, 240, 683, 2151, 233, 2683, 2569, 441, 239, 2988, 2199, 0, 345, 1367, 2607, 2240, 259, 865 },
			{ 3445, 1594, 1000, 477, 962, 2481, 435, 3028, 2911, 520, 577, 3334, 2538, 345, 0, 1538, 2953, 2582, 569, 1064 },
			{ 3072, 1042, 1688, 1556, 1728, 2463, 1147, 2401, 2601, 1797, 1215, 2805, 2351, 1367, 1538, 0, 2372, 2310, 1140, 2035 },
			{ 742, 1461, 2114, 2552, 2185, 977, 2588, 102, 507, 2718, 2381, 433, 711, 2607, 2953, 2372, 0, 577, 2410, 2381 },
			{ 863, 1286, 1660, 2144, 1726, 410, 2265, 679, 330, 2278, 2033, 800, 138, 2240, 2582, 2310, 577, 0, 2079, 1880 },
			{ 2942, 1025, 700, 416, 705, 2032, 186, 2480, 2410, 658, 90, 2803, 2051, 259, 569, 1140, 2410, 2079, 0, 961 },
			{ 2691, 1452, 381, 628, 320, 1648, 1070, 2474, 2179, 560, 871, 2680, 1792, 865, 1064, 2035, 2381, 1880, 961, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1276, Id=0,WayCostFromHome=911, WayCostToHome=911,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3371, Id=1,WayCostFromHome=1254, WayCostToHome=1254,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2991, Id=2,WayCostFromHome=1609, WayCostToHome=1609,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3076, Id=3,WayCostFromHome=2094, WayCostToHome=2094,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1693, Id=4,WayCostFromHome=1675, WayCostToHome=1675,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2408, Id=5,WayCostFromHome=378, WayCostToHome=378,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3032, Id=6,WayCostFromHome=2220, WayCostToHome=2220,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3184, Id=7,WayCostFromHome=723, WayCostToHome=723,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1452, Id=8,WayCostFromHome=377, WayCostToHome=377,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1827, Id=9,WayCostFromHome=2227, WayCostToHome=2227,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3585, Id=10,WayCostFromHome=1986, WayCostToHome=1986,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2556, Id=11,WayCostFromHome=852, WayCostToHome=852,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2627, Id=12,WayCostFromHome=110, WayCostToHome=110,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3269, Id=13,WayCostFromHome=2192, WayCostToHome=2192,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2050, Id=14,WayCostFromHome=2534, WayCostToHome=2534,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2736, Id=15,WayCostFromHome=2282, WayCostToHome=2282,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1277, Id=16,WayCostFromHome=621, WayCostToHome=621,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3237, Id=17,WayCostFromHome=52, WayCostToHome=52,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2912, Id=18,WayCostFromHome=2034, WayCostToHome=2034,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2437, Id=19,WayCostFromHome=1828, WayCostToHome=1828,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
