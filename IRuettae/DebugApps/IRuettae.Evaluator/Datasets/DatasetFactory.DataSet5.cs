using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 10 Desired, 0 Unavailable on day 0
/// 10 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet5()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1137,1753),
		(2723,2140),
		(2318,1428),
		(1230,1356),
		(2107,705),
		(2121,2769),
		(2121,2523),
		(2636,3187),
		(821,2465),
		(44,1348),
		(2029,3371),
		(1178,1693),
		(3183,2899),
		(1899,2024),
		(1605,943),
		(3427,3489),
		(2050,2405),
		(2233,2692),
		(1952,2705)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 819, 1686, 1561, 870, 713, 1050, 1929, 2793, 1413, 1608, 887, 832, 1637, 1521, 723, 738, 955 },
			{ 819, 0, 1090, 753, 1355, 1112, 1787, 1821, 2275, 1964, 1170, 1706, 728, 862, 2340, 1013, 1266, 1328 },
			{ 1686, 1090, 0, 1092, 1670, 1468, 2308, 1182, 1186, 2167, 340, 2488, 945, 557, 3062, 1331, 1670, 1530 },
			{ 1561, 753, 1092, 0, 2064, 1818, 2537, 2179, 2160, 2667, 1356, 2443, 1335, 555, 3081, 1700, 1990, 2005 },
			{ 870, 1355, 1670, 2064, 0, 246, 663, 1335, 2516, 608, 1430, 1069, 777, 1897, 1491, 370, 135, 180 },
			{ 713, 1112, 1468, 1818, 246, 0, 840, 1301, 2386, 852, 1256, 1126, 546, 1662, 1624, 137, 202, 248 },
			{ 1050, 1787, 2308, 2537, 663, 840, 0, 1953, 3178, 634, 2087, 618, 1376, 2469, 846, 977, 638, 836 },
			{ 1929, 1821, 1182, 2179, 1335, 1301, 1953, 0, 1360, 1510, 850, 2401, 1164, 1712, 2799, 1230, 1430, 1156 },
			{ 2793, 2275, 1186, 2160, 2516, 2386, 3178, 1360, 0, 2834, 1185, 3501, 1974, 1612, 4003, 2267, 2568, 2341 },
			{ 1413, 1964, 2167, 2667, 608, 852, 634, 1510, 2834, 0, 1881, 1246, 1353, 2464, 1402, 966, 708, 670 },
			{ 1608, 1170, 340, 1356, 1430, 1256, 2087, 850, 1185, 1881, 0, 2339, 793, 863, 2878, 1125, 1452, 1274 },
			{ 887, 1706, 2488, 2443, 1069, 1126, 618, 2401, 3501, 1246, 2339, 0, 1553, 2513, 638, 1236, 972, 1246 },
			{ 832, 728, 945, 1335, 777, 546, 1376, 1164, 1974, 1353, 793, 1553, 0, 1120, 2116, 409, 746, 683 },
			{ 1637, 862, 557, 555, 1897, 1662, 2469, 1712, 1612, 2464, 863, 2513, 1120, 0, 3130, 1528, 1858, 1795 },
			{ 1521, 2340, 3062, 3081, 1491, 1624, 846, 2799, 4003, 1402, 2878, 638, 2116, 3130, 0, 1752, 1435, 1670 },
			{ 723, 1013, 1331, 1700, 370, 137, 977, 1230, 2267, 966, 1125, 1236, 409, 1528, 1752, 0, 340, 315 },
			{ 738, 1266, 1670, 1990, 135, 202, 638, 1430, 2568, 708, 1452, 972, 746, 1858, 1435, 340, 0, 281 },
			{ 955, 1328, 1530, 2005, 180, 248, 836, 1156, 2341, 670, 1274, 1246, 683, 1795, 1670, 315, 281, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3215, Id=0,WayCostFromHome=1632, WayCostToHome=1632,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 15280, (0 * Hour + 15280) + 12214)}},
			new Visit{Duration = 2156, Id=1,WayCostFromHome=1224, WayCostToHome=1224,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1622, (0 * Hour + 1622) + 19839)}},
			new Visit{Duration = 2633, Id=2,WayCostFromHome=407, WayCostToHome=407,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3174, (0 * Hour + 3174) + 24541)}},
			new Visit{Duration = 1909, Id=3,WayCostFromHome=1428, WayCostToHome=1428,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1006, (0 * Hour + 1006) + 31109)}},
			new Visit{Duration = 2704, Id=4,WayCostFromHome=1414, WayCostToHome=1414,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4522, (0 * Hour + 4522) + 25765)}},
			new Visit{Duration = 2409, Id=5,WayCostFromHome=1249, WayCostToHome=1249,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3082, (0 * Hour + 3082) + 26398)}},
			new Visit{Duration = 2687, Id=6,WayCostFromHome=2074, WayCostToHome=2074,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1355, (0 * Hour + 1355) + 20526)}},
			new Visit{Duration = 3241, Id=7,WayCostFromHome=778, WayCostToHome=778,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7335, (0 * Hour + 7335) + 16850)}},
			new Visit{Duration = 1574, Id=8,WayCostFromHome=1165, WayCostToHome=1165,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1306, (0 * Hour + 1306) + 18177)}},
			new Visit{Duration = 3279, Id=9,WayCostFromHome=1847, WayCostToHome=1847,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1779, (0 * Hour + 1779) + 17333)}},
			new Visit{Duration = 2375, Id=10,WayCostFromHome=72, WayCostToHome=72,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 12510, (24 * Hour + 12510) + 17320)}},
			new Visit{Duration = 1444, Id=11,WayCostFromHome=2345, WayCostToHome=2345,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 5572, (24 * Hour + 5572) + 26220)}},
			new Visit{Duration = 1537, Id=12,WayCostFromHome=808, WayCostToHome=808,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 17311, (24 * Hour + 17311) + 13062)}},
			new Visit{Duration = 3389, Id=13,WayCostFromHome=935, WayCostToHome=935,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 13129, (24 * Hour + 13129) + 7596)}},
			new Visit{Duration = 2739, Id=14,WayCostFromHome=2873, WayCostToHome=2873,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 7193, (24 * Hour + 7193) + 21813)}},
			new Visit{Duration = 1407, Id=15,WayCostFromHome=1121, WayCostToHome=1121,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 21498, (24 * Hour + 21498) + 10755)}}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=1443, WayCostToHome=1443,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 14792, (0 * Hour + 14792) + 6898),(24 * Hour + 14792, (24 * Hour + 14792) + 6898)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1253, WayCostToHome=1253,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7901, (0 * Hour + 7901) + 11472),(24 * Hour + 7901, (24 * Hour + 7901) + 11472)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
