using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 0 Desired, 10 Unavailable on day 0
/// 0 Desired, 10 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet6()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(943,2106),
		(2640,3095),
		(3055,1982),
		(1115,785),
		(1158,583),
		(1106,1978),
		(2379,2629),
		(2499,2252),
		(1406,898),
		(3277,2364),
		(2789,2719),
		(1629,1801),
		(2675,2784),
		(1662,3276),
		(2216,2636),
		(1071,1498),
		(1264,1738),
		(3503,3163),
		(2027,1887),
		(508,1447),
		(1249,1107)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1187, 2767, 2916, 1897, 534, 854, 2519, 969, 404, 1642, 312, 994, 624, 2238, 1932, 865, 1354, 2694, 2426 },
			{ 1187, 0, 2279, 2357, 1949, 935, 618, 1973, 441, 783, 1437, 887, 1901, 1063, 2042, 1807, 1263, 1032, 2602, 2006 },
			{ 2767, 2279, 0, 206, 1193, 2235, 2016, 312, 2677, 2557, 1138, 2535, 2550, 2153, 714, 964, 3370, 1430, 898, 348 },
			{ 2916, 2357, 206, 0, 1395, 2382, 2140, 400, 2768, 2687, 1305, 2673, 2739, 2309, 919, 1159, 3486, 1567, 1081, 531 },
			{ 1897, 1949, 1193, 1395, 0, 1429, 1419, 1120, 2205, 1838, 552, 1763, 1412, 1290, 481, 287, 2673, 925, 799, 882 },
			{ 534, 935, 2235, 2382, 1429, 0, 395, 1985, 936, 419, 1117, 334, 965, 163, 1729, 1427, 1244, 821, 2213, 1895 },
			{ 854, 618, 2016, 2140, 1419, 395, 0, 1740, 786, 549, 979, 560, 1322, 477, 1614, 1337, 1355, 596, 2147, 1695 },
			{ 2519, 1973, 312, 400, 1120, 1985, 1740, 0, 2376, 2286, 930, 2273, 2391, 1917, 687, 851, 3086, 1167, 1052, 261 },
			{ 969, 441, 2677, 2768, 2205, 936, 786, 2376, 0, 603, 1741, 734, 1854, 1095, 2369, 2108, 830, 1337, 2916, 2385 },
			{ 404, 783, 2557, 2687, 1838, 419, 549, 2286, 603, 0, 1479, 131, 1257, 578, 2107, 1813, 840, 1128, 2611, 2229 },
			{ 1642, 1437, 1138, 1305, 552, 1117, 979, 930, 1741, 1479, 0, 1435, 1475, 1020, 634, 370, 2316, 407, 1175, 791 },
			{ 312, 887, 2535, 2673, 1763, 334, 560, 2273, 734, 131, 1435, 0, 1126, 482, 2055, 1756, 910, 1106, 2546, 2201 },
			{ 994, 1901, 2550, 2739, 1412, 965, 1322, 2391, 1854, 1257, 1475, 1126, 0, 846, 1873, 1588, 1844, 1436, 2162, 2207 },
			{ 624, 1063, 2153, 2309, 1290, 163, 477, 1917, 1095, 578, 1020, 482, 846, 0, 1614, 1308, 1390, 772, 2081, 1809 },
			{ 2238, 2042, 714, 919, 481, 1729, 1614, 687, 2369, 2107, 634, 2055, 1873, 1614, 0, 307, 2947, 1032, 565, 429 },
			{ 1932, 1807, 964, 1159, 287, 1427, 1337, 851, 2108, 1813, 370, 1756, 1588, 1308, 307, 0, 2654, 777, 810, 631 },
			{ 865, 1263, 3370, 3486, 2673, 1244, 1355, 3086, 830, 840, 2316, 910, 1844, 1390, 2947, 2654, 0, 1951, 3451, 3050 },
			{ 1354, 1032, 1430, 1567, 925, 821, 596, 1167, 1337, 1128, 407, 1106, 1436, 772, 1032, 777, 1951, 0, 1581, 1101 },
			{ 2694, 2602, 898, 1081, 799, 2213, 2147, 1052, 2916, 2611, 1175, 2546, 2162, 2081, 565, 810, 3451, 1581, 0, 815 },
			{ 2426, 2006, 348, 531, 882, 1895, 1695, 261, 2385, 2229, 791, 2201, 2207, 1809, 429, 631, 3050, 1101, 815, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1534, Id=0,WayCostFromHome=1964, WayCostToHome=1964,Unavailable =new [] {(0 * Hour + 8490.17655350009, (0 * Hour + 8490.17655350009) + 8449.56531526687)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1421, Id=1,WayCostFromHome=2115, WayCostToHome=2115,Unavailable =new [] {(0 * Hour + 2433.02692387228, (0 * Hour + 2433.02692387228) + 25819.5464782471)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1921, Id=2,WayCostFromHome=1332, WayCostToHome=1332,Unavailable =new [] {(0 * Hour + 3163.43460662986, (0 * Hour + 3163.43460662986) + 22499.2668405982)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2436, Id=3,WayCostFromHome=1538, WayCostToHome=1538,Unavailable =new [] {(0 * Hour + 81.9901213200013, (0 * Hour + 81.9901213200013) + 31365.2666775795)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1898, Id=4,WayCostFromHome=207, WayCostToHome=207,Unavailable =new [] {(0 * Hour + 4173.35139818972, (0 * Hour + 4173.35139818972) + 16778.4450977717)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1878, Id=5,WayCostFromHome=1528, WayCostToHome=1528,Unavailable =new [] {(0 * Hour + 7403.74033179165, (0 * Hour + 7403.74033179165) + 16397.9717683084)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2982, Id=6,WayCostFromHome=1562, WayCostToHome=1562,Unavailable =new [] {(0 * Hour + 2690.57239859182, (0 * Hour + 2690.57239859182) + 27971.2748426782)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2420, Id=7,WayCostFromHome=1293, WayCostToHome=1293,Unavailable =new [] {(0 * Hour + 13805.5045115409, (0 * Hour + 13805.5045115409) + 10596.7663797535)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1917, Id=8,WayCostFromHome=2348, WayCostToHome=2348,Unavailable =new [] {(0 * Hour + 13285.1557963658, (0 * Hour + 13285.1557963658) + 5532.20900026346)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2169, Id=9,WayCostFromHome=1945, WayCostToHome=1945,Unavailable =new [] {(0 * Hour + 8255.83926683819, (0 * Hour + 8255.83926683819) + 19693.4280115545)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2780, Id=10,WayCostFromHome=750, WayCostToHome=750,Unavailable =new [] {(24 * Hour + 9747.48434003927, (24 * Hour + 9747.48434003927) + 22638.6283928708)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3049, Id=11,WayCostFromHome=1859, WayCostToHome=1859,Unavailable =new [] {(24 * Hour + 56.9051078086876, (24 * Hour + 56.9051078086876) + 23149.6434152227)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1408, Id=12,WayCostFromHome=1373, WayCostToHome=1373,Unavailable =new [] {(24 * Hour + 24361.7422441908, (24 * Hour + 24361.7422441908) + 2079.16400959304)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2777, Id=13,WayCostFromHome=1378, WayCostToHome=1378,Unavailable =new [] {(24 * Hour + 8162.28831380386, (24 * Hour + 8162.28831380386) + 22663.8887935485)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3358, Id=14,WayCostFromHome=621, WayCostToHome=621,Unavailable =new [] {(24 * Hour + 20285.9197018144, (24 * Hour + 20285.9197018144) + 4915.75562287111)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3429, Id=15,WayCostFromHome=488, WayCostToHome=488,Unavailable =new [] {(24 * Hour + 1478.64636038079, (24 * Hour + 1478.64636038079) + 16058.8745744484)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2672, Id=16,WayCostFromHome=2769, WayCostToHome=2769,Unavailable =new [] {(24 * Hour + 809.246585331934, (24 * Hour + 809.246585331934) + 17166.9148361231)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3226, Id=17,WayCostFromHome=1105, WayCostToHome=1105,Unavailable =new [] {(24 * Hour + 7812.90905296282, (24 * Hour + 7812.90905296282) + 16964.490979207)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2756, Id=18,WayCostFromHome=789, WayCostToHome=789,Unavailable =new [] {(24 * Hour + 8592.11736765872, (24 * Hour + 8592.11736765872) + 6320.72891214524)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1390, Id=19,WayCostFromHome=1044, WayCostToHome=1044,Unavailable =new [] {(24 * Hour + 2295.77830653969, (24 * Hour + 2295.77830653969) + 26502.4528292159)},Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
