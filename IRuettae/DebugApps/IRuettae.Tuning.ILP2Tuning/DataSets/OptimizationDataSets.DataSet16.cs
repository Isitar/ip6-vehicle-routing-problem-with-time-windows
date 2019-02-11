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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet16()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3014,3356),
		(2772,2950),
		(933,1365),
		(1196,1385),
		(1136,1220),
		(2833,2153),
		(2479,2882),
		(2774,2695),
		(903,1709),
		(2381,2822),
		(2061,2469),
		(2068,2075),
		(3466,2766),
		(1039,2409),
		(1632,1293),
		(2846,2255),
		(1086,1582),
		(2813,2525),
		(3119,2189)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2427, 2221, 2381, 799, 300, 255, 2243, 411, 858, 1123, 717, 1815, 2011, 698, 2171, 426, 836 },
			{ 2427, 0, 263, 249, 2056, 2165, 2271, 345, 2054, 1578, 1338, 2894, 1049, 702, 2109, 265, 2209, 2336 },
			{ 2221, 263, 0, 175, 1808, 1971, 2050, 436, 1862, 1386, 1111, 2657, 1035, 445, 1865, 225, 1978, 2084 },
			{ 2381, 249, 175, 0, 1936, 2136, 2204, 541, 2028, 1554, 1264, 2796, 1192, 501, 1998, 365, 2124, 2207 },
			{ 799, 2056, 1808, 1936, 0, 810, 545, 1980, 807, 834, 768, 881, 1812, 1477, 102, 1837, 372, 288 },
			{ 300, 2165, 1971, 2136, 810, 0, 349, 1964, 114, 587, 905, 993, 1515, 1800, 726, 1905, 488, 943 },
			{ 255, 2271, 2050, 2204, 545, 349, 0, 2114, 413, 747, 939, 695, 1758, 1808, 445, 2021, 174, 612 },
			{ 2243, 345, 436, 541, 1980, 1964, 2114, 0, 1850, 1385, 1221, 2772, 713, 839, 2018, 222, 2077, 2267 },
			{ 411, 2054, 1862, 2028, 807, 114, 413, 1850, 0, 476, 809, 1086, 1404, 1702, 733, 1792, 524, 972 },
			{ 858, 1578, 1386, 1554, 834, 587, 747, 1385, 476, 0, 394, 1436, 1023, 1251, 813, 1318, 754, 1094 },
			{ 1123, 1338, 1111, 1264, 768, 905, 939, 1221, 809, 394, 0, 1559, 1081, 895, 798, 1098, 870, 1057 },
			{ 717, 2894, 2657, 2796, 881, 993, 695, 2772, 1086, 1436, 1559, 0, 2453, 2352, 803, 2658, 696, 673 },
			{ 1815, 1049, 1035, 1192, 1812, 1515, 1758, 713, 1404, 1023, 1081, 2453, 0, 1263, 1813, 828, 1777, 2091 },
			{ 2011, 702, 445, 501, 1477, 1800, 1808, 839, 1702, 1251, 895, 2352, 1263, 0, 1548, 617, 1706, 1736 },
			{ 698, 2109, 1865, 1998, 102, 726, 445, 2018, 733, 813, 798, 803, 1813, 1548, 0, 1884, 272, 280 },
			{ 2171, 265, 225, 365, 1837, 1905, 2021, 222, 1792, 1318, 1098, 2658, 828, 617, 1884, 0, 1967, 2121 },
			{ 426, 2209, 1978, 2124, 372, 488, 174, 2077, 524, 754, 870, 696, 1777, 1706, 272, 1967, 0, 454 },
			{ 836, 2336, 2084, 2207, 288, 943, 612, 2267, 972, 1094, 1057, 673, 2091, 1736, 280, 2121, 454, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2275, Id=0,WayCostFromHome=472, WayCostToHome=472,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2511, Id=1,WayCostFromHome=2880, WayCostToHome=2880,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1346, Id=2,WayCostFromHome=2681, WayCostToHome=2681,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1509, Id=3,WayCostFromHome=2844, WayCostToHome=2844,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2405, Id=4,WayCostFromHome=1216, WayCostToHome=1216,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1903, Id=5,WayCostFromHome=714, WayCostToHome=714,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2648, Id=6,WayCostFromHome=703, WayCostToHome=703,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2814, Id=7,WayCostFromHome=2677, WayCostToHome=2677,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1621, Id=8,WayCostFromHome=828, WayCostToHome=828,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2497, Id=9,WayCostFromHome=1301, WayCostToHome=1301,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2474, Id=10,WayCostFromHome=1592, WayCostToHome=1592,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1445, Id=11,WayCostFromHome=743, WayCostToHome=743,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2752, Id=12,WayCostFromHome=2190, WayCostToHome=2190,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2076, Id=13,WayCostFromHome=2483, WayCostToHome=2483,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2680, Id=14,WayCostFromHome=1113, WayCostToHome=1113,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1321, Id=15,WayCostFromHome=2619, WayCostToHome=2619,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=854, WayCostToHome=854,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1256, (0 * Hour + 1256) + 25179),(24 * Hour + 1256, (24 * Hour + 1256) + 25179)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1171, WayCostToHome=1171,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 289, (0 * Hour + 289) + 25713),(24 * Hour + 289, (24 * Hour + 289) + 25713)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
