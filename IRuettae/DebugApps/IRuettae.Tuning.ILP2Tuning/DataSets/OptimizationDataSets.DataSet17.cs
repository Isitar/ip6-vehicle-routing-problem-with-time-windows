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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet17()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3080,2480),
		(3361,2638),
		(778,2165),
		(2378,2544),
		(3003,2915),
		(1168,1548),
		(2533,2519),
		(2660,3505),
		(2653,3098),
		(2658,2400),
		(1186,1719),
		(1175,1241),
		(2236,3303),
		(1416,1084),
		(1580,1551),
		(894,1275),
		(2074,2916),
		(2599,2760),
		(3122,2760)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2625, 987, 452, 2448, 836, 1114, 844, 742, 2361, 2594, 1306, 2489, 2086, 2818, 1316, 771, 268 },
			{ 2625, 0, 1644, 2348, 729, 1790, 2310, 2094, 1894, 604, 1005, 1849, 1255, 1010, 897, 1497, 1915, 2418 },
			{ 987, 1644, 0, 726, 1567, 157, 1001, 618, 314, 1449, 1773, 772, 1748, 1273, 1952, 480, 309, 774 },
			{ 452, 2348, 726, 0, 2288, 614, 682, 394, 619, 2175, 2478, 859, 2423, 1971, 2671, 929, 432, 195 },
			{ 2448, 729, 1567, 2288, 0, 1675, 2460, 2146, 1716, 171, 307, 2054, 526, 412, 386, 1640, 1875, 2299 },
			{ 836, 1790, 157, 614, 1675, 0, 994, 591, 172, 1566, 1864, 838, 1818, 1358, 2057, 606, 249, 636 },
			{ 1114, 2310, 1001, 682, 2460, 994, 0, 407, 1105, 2315, 2707, 469, 2721, 2232, 2844, 830, 747, 876 },
			{ 844, 2094, 618, 394, 2146, 591, 407, 0, 698, 2013, 2373, 464, 2363, 1882, 2533, 606, 342, 578 },
			{ 742, 1894, 314, 619, 1716, 172, 1105, 698, 0, 1621, 1882, 996, 1809, 1372, 2092, 779, 364, 587 },
			{ 2361, 604, 1449, 2175, 171, 1566, 2315, 2013, 1621, 0, 478, 1900, 675, 428, 531, 1490, 1755, 2198 },
			{ 2594, 1005, 1773, 2478, 307, 1864, 2707, 2373, 1882, 478, 0, 2318, 287, 510, 283, 1901, 2082, 2469 },
			{ 1306, 1849, 772, 859, 2054, 838, 469, 464, 996, 1900, 2318, 0, 2365, 1870, 2431, 419, 653, 1039 },
			{ 2489, 1255, 1748, 2423, 526, 1818, 2721, 2363, 1809, 675, 287, 2365, 0, 494, 555, 1946, 2051, 2391 },
			{ 2086, 1010, 1273, 1971, 412, 1358, 2232, 1882, 1372, 428, 510, 1870, 494, 0, 739, 1451, 1581, 1959 },
			{ 2818, 897, 1952, 2671, 386, 2057, 2844, 2533, 2092, 531, 283, 2431, 555, 739, 0, 2021, 2261, 2677 },
			{ 1316, 1497, 480, 929, 1640, 606, 830, 606, 779, 1490, 1901, 419, 1946, 1451, 2021, 0, 547, 1059 },
			{ 771, 1915, 309, 432, 1875, 249, 747, 342, 364, 1755, 2082, 653, 2051, 1581, 2261, 547, 0, 523 },
			{ 268, 2418, 774, 195, 2299, 636, 876, 578, 587, 2198, 2469, 1039, 2391, 1959, 2677, 1059, 523, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1508, Id=0,WayCostFromHome=322, WayCostToHome=322,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3036, Id=1,WayCostFromHome=2323, WayCostToHome=2323,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2844, Id=2,WayCostFromHome=704, WayCostToHome=704,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3351, Id=3,WayCostFromHome=441, WayCostToHome=441,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2466, Id=4,WayCostFromHome=2127, WayCostToHome=2127,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1277, Id=5,WayCostFromHome=548, WayCostToHome=548,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2442, Id=6,WayCostFromHome=1107, WayCostToHome=1107,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1811, Id=7,WayCostFromHome=751, WayCostToHome=751,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1731, Id=8,WayCostFromHome=429, WayCostToHome=429,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3247, Id=9,WayCostFromHome=2041, WayCostToHome=2041,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1280, Id=10,WayCostFromHome=2272, WayCostToHome=2272,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3240, Id=11,WayCostFromHome=1178, WayCostToHome=1178,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3214, Id=12,WayCostFromHome=2172, WayCostToHome=2172,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3035, Id=13,WayCostFromHome=1764, WayCostToHome=1764,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3192, Id=14,WayCostFromHome=2496, WayCostToHome=2496,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1408, Id=15,WayCostFromHome=1096, WayCostToHome=1096,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=556, WayCostToHome=556,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5150, (0 * Hour + 5150) + 25677),(24 * Hour + 5150, (24 * Hour + 5150) + 25677)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=283, WayCostToHome=283,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1919, (0 * Hour + 1919) + 27196),(24 * Hour + 1919, (24 * Hour + 1919) + 27196)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
