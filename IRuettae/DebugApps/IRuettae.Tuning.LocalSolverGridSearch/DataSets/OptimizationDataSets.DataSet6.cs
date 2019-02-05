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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet6()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(223,3216),
		(2453,1989),
		(1816,1785),
		(1058,1974),
		(1090,1249),
		(793,1600),
		(2563,3247),
		(3091,2218),
		(819,2111),
		(2914,2166),
		(1261,1230),
		(3428,3385),
		(1686,930),
		(1474,1110),
		(1432,1243),
		(1811,1158),
		(1411,1433),
		(2579,2594),
		(1220,1719)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 668, 1395, 1550, 1704, 1262, 677, 1638, 493, 1413, 1702, 1307, 1315, 1264, 1050, 1181, 617, 1262 },
			{ 668, 0, 781, 902, 1039, 1641, 1346, 1048, 1162, 784, 2271, 864, 756, 664, 627, 536, 1112, 599 },
			{ 1395, 781, 0, 725, 458, 1971, 2047, 275, 1865, 771, 2758, 1218, 958, 821, 1110, 645, 1642, 302 },
			{ 1550, 902, 725, 0, 459, 2482, 2223, 903, 2041, 172, 3166, 676, 408, 342, 726, 369, 2006, 487 },
			{ 1704, 1039, 458, 459, 0, 2417, 2379, 511, 2195, 596, 3182, 1116, 838, 731, 1109, 640, 2043, 443 },
			{ 1262, 1641, 1971, 2482, 2417, 0, 1156, 2081, 1136, 2400, 875, 2477, 2398, 2301, 2220, 2148, 653, 2034 },
			{ 677, 1346, 2047, 2223, 2379, 1156, 0, 2274, 184, 2079, 1214, 1906, 1960, 1924, 1661, 1854, 635, 1936 },
			{ 1638, 1048, 275, 903, 511, 2081, 2274, 0, 2095, 985, 2903, 1465, 1196, 1062, 1375, 900, 1825, 560 },
			{ 493, 1162, 1865, 2041, 2195, 1136, 184, 2095, 0, 1899, 1322, 1742, 1785, 1745, 1494, 1672, 543, 1751 },
			{ 1413, 784, 771, 172, 596, 2400, 2079, 985, 1899, 0, 3056, 520, 244, 171, 554, 252, 1896, 490 },
			{ 1702, 2271, 2758, 3166, 3182, 875, 1214, 2903, 1322, 3056, 0, 3010, 2998, 2927, 2752, 2806, 1160, 2766 },
			{ 1307, 864, 1218, 676, 1116, 2477, 1906, 1465, 1742, 520, 3010, 0, 278, 403, 260, 573, 1888, 916 },
			{ 1315, 756, 958, 408, 838, 2398, 1960, 1196, 1785, 244, 2998, 278, 0, 139, 340, 329, 1850, 659 },
			{ 1264, 664, 821, 342, 731, 2301, 1924, 1062, 1745, 171, 2927, 403, 139, 0, 388, 191, 1772, 521 },
			{ 1050, 627, 1110, 726, 1109, 2220, 1661, 1375, 1494, 554, 2752, 260, 340, 388, 0, 485, 1628, 814 },
			{ 1181, 536, 645, 369, 640, 2148, 1854, 900, 1672, 252, 2806, 573, 329, 191, 485, 0, 1646, 343 },
			{ 617, 1112, 1642, 2006, 2043, 653, 635, 1825, 543, 1896, 1160, 1888, 1850, 1772, 1628, 1646, 0, 1616 },
			{ 1262, 599, 302, 487, 443, 2034, 1936, 560, 1751, 490, 2766, 916, 659, 521, 814, 343, 1616, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1965, Id=0,WayCostFromHome=2545, WayCostToHome=2545,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1442, Id=1,WayCostFromHome=2141, WayCostToHome=2141,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1357, Id=2,WayCostFromHome=1496, WayCostToHome=1496,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3340, Id=3,WayCostFromHome=2149, WayCostToHome=2149,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2916, Id=4,WayCostFromHome=1713, WayCostToHome=1713,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3140, Id=5,WayCostFromHome=2340, WayCostToHome=2340,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2577, Id=6,WayCostFromHome=3036, WayCostToHome=3036,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2334, Id=7,WayCostFromHome=1255, WayCostToHome=1255,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3252, Id=8,WayCostFromHome=2888, WayCostToHome=2888,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2589, Id=9,WayCostFromHome=2240, WayCostToHome=2240,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2866, Id=10,WayCostFromHome=3209, WayCostToHome=3209,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2713, Id=11,WayCostFromHome=2714, WayCostToHome=2714,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2733, Id=12,WayCostFromHome=2449, WayCostToHome=2449,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2187, Id=13,WayCostFromHome=2313, WayCostToHome=2313,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3394, Id=14,WayCostFromHome=2599, WayCostToHome=2599,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3143, Id=15,WayCostFromHome=2142, WayCostToHome=2142,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=2436, WayCostToHome=2436,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 10122, (0 * Hour + 10122) + 13752),(24 * Hour + 10122, (24 * Hour + 10122) + 13752)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1798, WayCostToHome=1798,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7357, (0 * Hour + 7357) + 15759),(24 * Hour + 7357, (24 * Hour + 7357) + 15759)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
