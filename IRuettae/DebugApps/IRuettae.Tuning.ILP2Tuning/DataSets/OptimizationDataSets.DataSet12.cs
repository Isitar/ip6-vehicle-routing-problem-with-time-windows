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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet12()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1728,795),
		(2405,2959),
		(1938,3269),
		(3812,3074),
		(1018,1280),
		(1515,1545),
		(1553,992),
		(2916,2761),
		(1813,1680),
		(1570,2026),
		(3020,2477),
		(2729,2759),
		(1051,838),
		(699,1956),
		(1285,1345),
		(1874,967),
		(3488,3158),
		(1590,2007),
		(2531,3001)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 560, 1411, 2177, 1670, 2143, 548, 1409, 1252, 781, 380, 2516, 1979, 1964, 2061, 1101, 1253, 132 },
			{ 560, 0, 1884, 2191, 1775, 2309, 1102, 1593, 1296, 1340, 941, 2587, 1805, 2031, 2302, 1553, 1309, 650 },
			{ 1411, 1884, 0, 3320, 2759, 3072, 949, 2437, 2474, 991, 1127, 3552, 3307, 3061, 2862, 334, 2464, 1283 },
			{ 2177, 2191, 3320, 0, 563, 607, 2407, 889, 928, 2332, 2261, 443, 747, 274, 911, 3102, 925, 2291 },
			{ 1670, 1775, 2759, 563, 0, 554, 1855, 327, 484, 1770, 1716, 845, 913, 304, 680, 2548, 468, 1775 },
			{ 2143, 2309, 3072, 607, 554, 0, 2233, 735, 1034, 2087, 2122, 525, 1287, 443, 321, 2904, 1015, 2234 },
			{ 548, 1102, 949, 2407, 1855, 2233, 0, 1544, 1533, 302, 187, 2678, 2358, 2159, 2074, 696, 1525, 453 },
			{ 1409, 1593, 2437, 889, 327, 735, 1544, 0, 422, 1446, 1415, 1135, 1147, 625, 715, 2233, 395, 1503 },
			{ 1252, 1296, 2474, 928, 484, 1034, 1533, 422, 0, 1518, 1371, 1296, 873, 738, 1101, 2227, 27, 1368 },
			{ 781, 1340, 991, 2332, 1770, 2087, 302, 1446, 1518, 0, 405, 2561, 2378, 2071, 1895, 826, 1505, 716 },
			{ 380, 941, 1127, 2261, 1716, 2122, 187, 1415, 1371, 405, 0, 2550, 2183, 2021, 1985, 857, 1364, 312 },
			{ 2516, 2587, 3552, 443, 845, 525, 2678, 1135, 1296, 2561, 2550, 0, 1172, 558, 833, 3364, 1287, 2620 },
			{ 1979, 1805, 3307, 747, 913, 1287, 2358, 1147, 873, 2378, 2183, 1172, 0, 846, 1535, 3036, 892, 2109 },
			{ 1964, 2031, 3061, 274, 304, 443, 2159, 625, 738, 2071, 2021, 558, 846, 0, 699, 2853, 728, 2072 },
			{ 2061, 2302, 2862, 911, 680, 321, 2074, 715, 1101, 1895, 1985, 833, 1535, 699, 0, 2721, 1078, 2137 },
			{ 1101, 1553, 334, 3102, 2548, 2904, 696, 2233, 2227, 826, 857, 3364, 3036, 2853, 2721, 0, 2219, 969 },
			{ 1253, 1309, 2464, 925, 468, 1015, 1525, 395, 27, 1505, 1364, 1287, 892, 728, 1078, 2219, 0, 1368 },
			{ 132, 650, 1283, 2291, 1775, 2234, 453, 1503, 1368, 716, 312, 2620, 2109, 2072, 2137, 969, 1368, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1698, Id=0,WayCostFromHome=2267, WayCostToHome=2267,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3537, Id=1,WayCostFromHome=2482, WayCostToHome=2482,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3529, Id=2,WayCostFromHome=3088, WayCostToHome=3088,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2151, Id=3,WayCostFromHome=859, WayCostToHome=859,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3400, Id=4,WayCostFromHome=779, WayCostToHome=779,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3002, Id=5,WayCostFromHome=263, WayCostToHome=263,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1964, Id=6,WayCostFromHome=2297, WayCostToHome=2297,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2820, Id=7,WayCostFromHome=889, WayCostToHome=889,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2977, Id=8,WayCostFromHome=1241, WayCostToHome=1241,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3373, Id=9,WayCostFromHome=2120, WayCostToHome=2120,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1892, Id=10,WayCostFromHome=2204, WayCostToHome=2204,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2236, Id=11,WayCostFromHome=678, WayCostToHome=678,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2194, Id=12,WayCostFromHome=1551, WayCostToHome=1551,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3011, Id=13,WayCostFromHome=706, WayCostToHome=706,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3511, Id=14,WayCostFromHome=225, WayCostToHome=225,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1757, Id=15,WayCostFromHome=2946, WayCostToHome=2946,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=1219, WayCostToHome=1219,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 9917, (0 * Hour + 9917) + 3983),(24 * Hour + 9917, (24 * Hour + 9917) + 3983)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=2347, WayCostToHome=2347,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 10913, (0 * Hour + 10913) + 5729),(24 * Hour + 10913, (24 * Hour + 10913) + 5729)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
