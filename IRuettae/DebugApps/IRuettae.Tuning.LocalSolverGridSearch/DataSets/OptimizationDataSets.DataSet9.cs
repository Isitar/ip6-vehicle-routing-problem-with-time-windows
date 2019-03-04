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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet9()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(459,1283),
		(2746,1787),
		(2806,2585),
		(2612,2585),
		(2746,2684),
		(1302,1249),
		(2083,3179),
		(2977,2237),
		(2567,2216),
		(3148,2794),
		(2852,2916),
		(2593,3121),
		(976,2919),
		(3109,2426),
		(1183,1514),
		(1473,1513),
		(2386,2975),
		(2083,2629),
		(2124,2314)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 800, 809, 897, 1540, 1541, 505, 464, 1084, 1133, 1342, 2101, 734, 1586, 1302, 1241, 1071, 815 },
			{ 800, 0, 194, 115, 2011, 935, 387, 439, 400, 334, 576, 1860, 342, 1944, 1710, 573, 724, 733 },
			{ 809, 194, 0, 166, 1871, 795, 504, 371, 575, 408, 536, 1669, 521, 1785, 1564, 450, 530, 558 },
			{ 897, 115, 166, 0, 2035, 827, 503, 501, 416, 255, 463, 1785, 445, 1952, 1729, 462, 665, 723 },
			{ 1540, 2011, 1871, 2035, 0, 2082, 1944, 1592, 2407, 2276, 2273, 1701, 2156, 290, 314, 2038, 1585, 1345 },
			{ 1541, 935, 795, 827, 2082, 0, 1298, 1077, 1132, 812, 513, 1137, 1272, 1892, 1774, 365, 550, 865 },
			{ 505, 387, 504, 503, 1944, 1298, 0, 410, 582, 690, 963, 2114, 230, 1934, 1669, 945, 976, 856 },
			{ 464, 439, 371, 501, 1592, 1077, 410, 0, 819, 755, 905, 1739, 581, 1551, 1300, 780, 636, 453 },
			{ 1084, 400, 575, 416, 2407, 1132, 582, 819, 0, 320, 644, 2175, 370, 2345, 2108, 783, 1077, 1130 },
			{ 1133, 334, 408, 255, 2276, 812, 690, 755, 320, 0, 330, 1876, 553, 2179, 1967, 469, 820, 944 },
			{ 1342, 576, 536, 463, 2273, 513, 963, 905, 644, 330, 0, 1629, 865, 2137, 1959, 253, 708, 933 },
			{ 2101, 1860, 1669, 1785, 1701, 1137, 2114, 1739, 2175, 1876, 1629, 0, 2189, 1420, 1491, 1411, 1144, 1297 },
			{ 734, 342, 521, 445, 2156, 1272, 230, 581, 370, 553, 865, 2189, 0, 2131, 1873, 907, 1045, 991 },
			{ 1586, 1944, 1785, 1952, 290, 1892, 1934, 1551, 2345, 2179, 2137, 1420, 2131, 0, 290, 1892, 1432, 1235 },
			{ 1302, 1710, 1564, 1729, 314, 1774, 1669, 1300, 2108, 1967, 1959, 1491, 1873, 290, 0, 1723, 1271, 1032 },
			{ 1241, 573, 450, 462, 2038, 365, 945, 780, 783, 469, 253, 1411, 907, 1892, 1723, 0, 459, 711 },
			{ 1071, 724, 530, 665, 1585, 550, 976, 636, 1077, 820, 708, 1144, 1045, 1432, 1271, 459, 0, 317 },
			{ 815, 733, 558, 723, 1345, 865, 856, 453, 1130, 944, 933, 1297, 991, 1235, 1032, 711, 317, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2313, Id=0,WayCostFromHome=2341, WayCostToHome=2341,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3395, Id=1,WayCostFromHome=2683, WayCostToHome=2683,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2570, Id=2,WayCostFromHome=2516, WayCostToHome=2516,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2277, Id=3,WayCostFromHome=2682, WayCostToHome=2682,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2890, Id=4,WayCostFromHome=843, WayCostToHome=843,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1213, Id=5,WayCostFromHome=2496, WayCostToHome=2496,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3101, Id=6,WayCostFromHome=2692, WayCostToHome=2692,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3000, Id=7,WayCostFromHome=2305, WayCostToHome=2305,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2876, Id=8,WayCostFromHome=3084, WayCostToHome=3084,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2476, Id=9,WayCostFromHome=2897, WayCostToHome=2897,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3264, Id=10,WayCostFromHome=2816, WayCostToHome=2816,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3287, Id=11,WayCostFromHome=1715, WayCostToHome=1715,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2657, Id=12,WayCostFromHome=2885, WayCostToHome=2885,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3329, Id=13,WayCostFromHome=759, WayCostToHome=759,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2373, Id=14,WayCostFromHome=1039, WayCostToHome=1039,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2311, Id=15,WayCostFromHome=2564, WayCostToHome=2564,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=2109, WayCostToHome=2109,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1356, (0 * Hour + 1356) + 27108),(24 * Hour + 1356, (24 * Hour + 1356) + 27108)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1958, WayCostToHome=1958,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1317, (0 * Hour + 1317) + 30354),(24 * Hour + 1317, (24 * Hour + 1317) + 30354)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
