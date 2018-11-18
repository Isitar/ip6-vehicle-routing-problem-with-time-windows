using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 10 Desired, 0 Unavailable on day 0
/// 10 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet5()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3396,2724),
		(2592,3292),
		(876,948),
		(1129,1099),
		(1489,1046),
		(1195,2042),
		(723,845),
		(1174,1383),
		(1480,1601),
		(2209,2398),
		(1601,1437),
		(1567,1104),
		(826,1178),
		(2298,3144),
		(2697,2255),
		(950,1457),
		(2377,2477),
		(1605,1316),
		(2042,2055),
		(846,1651),
		(2415,3622)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2904, 2636, 2502, 1874, 3079, 2378, 2023, 972, 2103, 2416, 2754, 329, 1042, 2462, 842, 2208, 1353, 2396, 374 },
			{ 2904, 0, 294, 620, 1139, 184, 527, 889, 1969, 874, 708, 235, 2616, 2241, 514, 2142, 816, 1607, 703, 3085 },
			{ 2636, 294, 0, 363, 945, 478, 287, 612, 1689, 580, 438, 313, 2355, 1948, 400, 1859, 523, 1321, 620, 2831 },
			{ 2502, 620, 363, 0, 1038, 791, 461, 555, 1531, 406, 97, 676, 2248, 1709, 677, 1684, 293, 1150, 882, 2737 },
			{ 1874, 1139, 945, 1038, 0, 1286, 659, 525, 1074, 728, 1009, 939, 1559, 1517, 634, 1259, 833, 847, 524, 1996 },
			{ 3079, 184, 478, 791, 1286, 0, 702, 1069, 2149, 1058, 882, 348, 2786, 2425, 652, 2323, 999, 1789, 815, 3251 },
			{ 2378, 527, 287, 461, 659, 702, 0, 375, 1449, 430, 481, 403, 2089, 1754, 235, 1626, 436, 1097, 423, 2559 },
			{ 2023, 889, 612, 555, 525, 1069, 375, 0, 1080, 203, 504, 778, 1746, 1381, 549, 1253, 311, 722, 635, 2226 },
			{ 972, 1969, 1689, 1531, 1074, 2149, 1449, 1080, 0, 1137, 1444, 1844, 751, 508, 1571, 185, 1239, 381, 1554, 1241 },
			{ 2103, 874, 580, 406, 728, 1058, 430, 203, 1137, 0, 334, 817, 1843, 1367, 651, 1297, 121, 759, 784, 2331 },
			{ 2416, 708, 438, 97, 1009, 882, 481, 504, 1444, 334, 0, 744, 2167, 1612, 710, 1594, 215, 1063, 905, 2656 },
			{ 2754, 235, 313, 676, 939, 348, 403, 778, 1844, 817, 744, 0, 2456, 2158, 305, 2023, 791, 1499, 473, 2915 },
			{ 329, 2616, 2355, 2248, 1559, 2786, 2089, 1746, 751, 1843, 2167, 2456, 0, 974, 2159, 671, 1954, 1118, 2082, 492 },
			{ 1042, 2241, 1948, 1709, 1517, 2425, 1754, 1381, 508, 1367, 1612, 2158, 974, 0, 1920, 389, 1440, 684, 1947, 1395 },
			{ 2462, 514, 400, 677, 634, 652, 235, 549, 1571, 651, 710, 305, 2159, 1920, 0, 1754, 670, 1245, 220, 2614 },
			{ 842, 2142, 1859, 1684, 1259, 2323, 1626, 1253, 185, 1297, 1594, 2023, 671, 389, 1754, 0, 1394, 538, 1739, 1145 },
			{ 2208, 816, 523, 293, 833, 999, 436, 311, 1239, 121, 215, 791, 1954, 1440, 670, 1394, 0, 858, 829, 2444 },
			{ 1353, 1607, 1321, 1150, 847, 1789, 1097, 722, 381, 759, 1063, 1499, 1118, 684, 1245, 538, 858, 0, 1262, 1610 },
			{ 2396, 703, 620, 882, 524, 815, 423, 635, 1554, 784, 905, 473, 2082, 1947, 220, 1739, 829, 1262, 0, 2519 },
			{ 374, 3085, 2831, 2737, 1996, 3251, 2559, 2226, 1241, 2331, 2656, 2915, 492, 1395, 2614, 1145, 2444, 1610, 2519, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2424, Id=0,WayCostFromHome=984, WayCostToHome=984,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1453, (0 * Hour + 1453) + 19311)}},
			new Visit{Duration = 2247, Id=1,WayCostFromHome=3082, WayCostToHome=3082,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 13582, (0 * Hour + 13582) + 10074)}},
			new Visit{Duration = 3136, Id=2,WayCostFromHome=2789, WayCostToHome=2789,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3021, (0 * Hour + 3021) + 19783)}},
			new Visit{Duration = 2090, Id=3,WayCostFromHome=2540, WayCostToHome=2540,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 6634, (0 * Hour + 6634) + 4660)}},
			new Visit{Duration = 3476, Id=4,WayCostFromHome=2304, WayCostToHome=2304,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 6900, (0 * Hour + 6900) + 25054)}},
			new Visit{Duration = 1560, Id=5,WayCostFromHome=3267, WayCostToHome=3267,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 887, (0 * Hour + 887) + 29934)}},
			new Visit{Duration = 2664, Id=6,WayCostFromHome=2595, WayCostToHome=2595,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5061, (0 * Hour + 5061) + 26651)}},
			new Visit{Duration = 1423, Id=7,WayCostFromHome=2220, WayCostToHome=2220,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 6755, (0 * Hour + 6755) + 22558)}},
			new Visit{Duration = 3571, Id=8,WayCostFromHome=1230, WayCostToHome=1230,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2053, (0 * Hour + 2053) + 19173)}},
			new Visit{Duration = 2394, Id=9,WayCostFromHome=2208, WayCostToHome=2208,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5614, (0 * Hour + 5614) + 26291)}},
			new Visit{Duration = 2794, Id=10,WayCostFromHome=2443, WayCostToHome=2443,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 15185, (24 * Hour + 15185) + 12375)}},
			new Visit{Duration = 1867, Id=11,WayCostFromHome=2999, WayCostToHome=2999,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 11868, (24 * Hour + 11868) + 18955)}},
			new Visit{Duration = 1864, Id=12,WayCostFromHome=1175, WayCostToHome=1175,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3189, (24 * Hour + 3189) + 18360)}},
			new Visit{Duration = 3372, Id=13,WayCostFromHome=841, WayCostToHome=841,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 10572, (24 * Hour + 10572) + 5345)}},
			new Visit{Duration = 3472, Id=14,WayCostFromHome=2754, WayCostToHome=2754,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 5331, (24 * Hour + 5331) + 11135)}},
			new Visit{Duration = 2310, Id=15,WayCostFromHome=1048, WayCostToHome=1048,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 4207, (24 * Hour + 4207) + 23180)}},
			new Visit{Duration = 1680, Id=16,WayCostFromHome=2278, WayCostToHome=2278,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 16701, (24 * Hour + 16701) + 3131)}},
			new Visit{Duration = 2723, Id=17,WayCostFromHome=1510, WayCostToHome=1510,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3349, (24 * Hour + 3349) + 23563)}},
			new Visit{Duration = 2617, Id=18,WayCostFromHome=2766, WayCostToHome=2766,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 12122, (24 * Hour + 12122) + 11674)}},
			new Visit{Duration = 2876, Id=19,WayCostFromHome=1329, WayCostToHome=1329,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 22532, (24 * Hour + 22532) + 4654)}}
		}
	};
	return (input, coordinates);
}
}
}
