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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet2()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(372,1997),
		(1212,1484),
		(2620,2318),
		(2946,2469),
		(1153,1465),
		(2972,2560),
		(3309,3123),
		(979,1576),
		(1218,1352),
		(1242,1002),
		(776,3478),
		(2798,3723),
		(2038,1348),
		(2069,2344),
		(3092,2038),
		(2222,3658),
		(2380,2486),
		(2904,3198),
		(1700,1530)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1636, 1994, 61, 2062, 2661, 250, 132, 482, 2041, 2743, 837, 1214, 1959, 2397, 1538, 2408, 490 },
			{ 1636, 0, 359, 1696, 427, 1059, 1800, 1702, 1905, 2178, 1416, 1131, 551, 548, 1397, 292, 924, 1211 },
			{ 1994, 359, 0, 2054, 94, 747, 2160, 2057, 2248, 2393, 1262, 1442, 885, 455, 1392, 566, 730, 1560 },
			{ 61, 1696, 2054, 0, 2123, 2719, 206, 130, 471, 2047, 2793, 892, 1269, 2021, 2439, 1596, 2463, 550 },
			{ 2062, 427, 94, 2123, 0, 656, 2222, 2129, 2328, 2380, 1175, 1530, 928, 535, 1329, 596, 641, 1636 },
			{ 2661, 1059, 747, 2719, 656, 0, 2796, 2740, 2961, 2557, 788, 2183, 1464, 1106, 1211, 1126, 411, 2264 },
			{ 250, 1800, 2160, 206, 2222, 2796, 0, 327, 631, 1912, 2813, 1083, 1333, 2162, 2424, 1670, 2517, 722 },
			{ 132, 1702, 2057, 130, 2129, 2740, 327, 0, 350, 2171, 2849, 820, 1307, 1995, 2515, 1623, 2500, 513 },
			{ 482, 1905, 2248, 471, 2328, 2961, 631, 350, 0, 2519, 3134, 867, 1576, 2120, 2831, 1870, 2754, 698 },
			{ 2041, 2178, 2393, 2047, 2380, 2557, 1912, 2171, 2519, 0, 2036, 2475, 1719, 2727, 1457, 1885, 2146, 2156 },
			{ 2743, 1416, 1262, 2793, 1175, 788, 2813, 2849, 3134, 2036, 0, 2493, 1559, 1710, 579, 1305, 535, 2452 },
			{ 837, 1131, 1442, 892, 1530, 2183, 1083, 820, 867, 2475, 2493, 0, 996, 1259, 2317, 1188, 2042, 383 },
			{ 1214, 551, 885, 1269, 928, 1464, 1333, 1307, 1576, 1719, 1559, 996, 0, 1067, 1322, 341, 1194, 893 },
			{ 1959, 548, 455, 2021, 535, 1106, 2162, 1995, 2120, 2727, 1710, 1259, 1067, 0, 1838, 841, 1175, 1481 },
			{ 2397, 1397, 1392, 2439, 1329, 1211, 2424, 2515, 2831, 1457, 579, 2317, 1322, 1838, 0, 1182, 822, 2191 },
			{ 1538, 292, 566, 1596, 596, 1126, 1670, 1623, 1870, 1885, 1305, 1188, 341, 841, 1182, 0, 884, 1173 },
			{ 2408, 924, 730, 2463, 641, 411, 2517, 2500, 2754, 2146, 535, 2042, 1194, 1175, 822, 884, 0, 2057 },
			{ 490, 1211, 1560, 550, 1636, 2264, 722, 513, 698, 2156, 2452, 383, 893, 1481, 2191, 1173, 2057, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1996, Id=0,WayCostFromHome=984, WayCostToHome=984,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3083, Id=1,WayCostFromHome=2270, WayCostToHome=2270,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3224, Id=2,WayCostFromHome=2616, WayCostToHome=2616,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1766, Id=3,WayCostFromHome=944, WayCostToHome=944,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3044, Id=4,WayCostFromHome=2660, WayCostToHome=2660,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3192, Id=5,WayCostFromHome=3145, WayCostToHome=3145,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3147, Id=6,WayCostFromHome=738, WayCostToHome=738,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2286, Id=7,WayCostFromHome=1063, WayCostToHome=1063,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1903, Id=8,WayCostFromHome=1321, WayCostToHome=1321,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3599, Id=9,WayCostFromHome=1535, WayCostToHome=1535,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2036, Id=10,WayCostFromHome=2977, WayCostToHome=2977,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1327, Id=11,WayCostFromHome=1787, WayCostToHome=1787,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2928, Id=12,WayCostFromHome=1732, WayCostToHome=1732,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3463, Id=13,WayCostFromHome=2720, WayCostToHome=2720,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2625, Id=14,WayCostFromHome=2486, WayCostToHome=2486,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2699, Id=15,WayCostFromHome=2066, WayCostToHome=2066,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=2802, WayCostToHome=2802,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5246, (0 * Hour + 5246) + 26373),(24 * Hour + 5246, (24 * Hour + 5246) + 26373)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1407, WayCostToHome=1407,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 13370, (0 * Hour + 13370) + 10062),(24 * Hour + 13370, (24 * Hour + 13370) + 10062)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
