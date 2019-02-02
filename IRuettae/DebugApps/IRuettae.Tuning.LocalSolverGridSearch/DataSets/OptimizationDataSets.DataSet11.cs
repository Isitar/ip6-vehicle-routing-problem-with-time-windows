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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet11()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3221,3167),
		(1907,1229),
		(718,1145),
		(1415,671),
		(703,1472),
		(1986,1142),
		(2000,2763),
		(1194,1256),
		(2121,2000),
		(2450,2501),
		(2502,2826),
		(1998,1238),
		(1175,1022),
		(2455,2681),
		(3320,2690),
		(2882,2638),
		(1767,1656),
		(1423,1142),
		(805,1169)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1191, 743, 1228, 117, 1536, 713, 800, 1383, 1704, 91, 760, 1551, 2032, 1713, 449, 491, 1103 },
			{ 1191, 0, 842, 327, 1268, 2064, 488, 1642, 2199, 2451, 1283, 473, 2318, 3026, 2629, 1166, 705, 90 },
			{ 743, 842, 0, 1071, 740, 2172, 625, 1504, 2102, 2413, 813, 425, 2263, 2775, 2453, 1046, 471, 787 },
			{ 1228, 327, 1071, 0, 1324, 1829, 536, 1513, 2027, 2251, 1315, 652, 2128, 2886, 2471, 1079, 792, 319 },
			{ 117, 1268, 740, 1324, 0, 1621, 800, 868, 1436, 1761, 96, 819, 1608, 2043, 1743, 558, 563, 1181 },
			{ 1536, 2064, 2172, 1829, 1621, 0, 1709, 772, 520, 505, 1525, 1926, 462, 1322, 890, 1131, 1720, 1992 },
			{ 713, 488, 625, 536, 800, 1709, 0, 1188, 1768, 2043, 804, 234, 1902, 2564, 2181, 698, 255, 398 },
			{ 800, 1642, 1504, 1513, 868, 772, 1188, 0, 599, 909, 771, 1360, 758, 1383, 993, 493, 1106, 1556 },
			{ 1383, 2199, 2102, 2027, 1436, 520, 1768, 599, 0, 329, 1341, 1952, 180, 890, 453, 1086, 1703, 2116 },
			{ 1704, 2451, 2413, 2251, 1761, 505, 2043, 909, 329, 0, 1666, 2239, 152, 829, 423, 1381, 2000, 2371 },
			{ 91, 1283, 813, 1315, 96, 1525, 804, 771, 1341, 1666, 0, 850, 1513, 1963, 1655, 477, 582, 1194 },
			{ 760, 473, 425, 652, 819, 1926, 234, 1360, 1952, 2239, 850, 0, 2095, 2717, 2350, 867, 275, 398 },
			{ 1551, 2318, 2263, 2128, 1608, 462, 1902, 758, 180, 152, 1513, 2095, 0, 865, 429, 1234, 1852, 2238 },
			{ 2032, 3026, 2775, 2886, 2043, 1322, 2564, 1383, 890, 829, 1963, 2717, 865, 0, 441, 1865, 2448, 2939 },
			{ 1713, 2629, 2453, 2471, 1743, 890, 2181, 993, 453, 423, 1655, 2350, 429, 441, 0, 1485, 2089, 2543 },
			{ 449, 1166, 1046, 1079, 558, 1131, 698, 493, 1086, 1381, 477, 867, 1234, 1865, 1485, 0, 618, 1078 },
			{ 491, 705, 471, 792, 563, 1720, 255, 1106, 1703, 2000, 582, 275, 1852, 2448, 2089, 618, 0, 618 },
			{ 1103, 90, 787, 319, 1181, 1992, 398, 1556, 2116, 2371, 1194, 398, 2238, 2939, 2543, 1078, 618, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3163, Id=0,WayCostFromHome=2341, WayCostToHome=2341,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2155, Id=1,WayCostFromHome=3217, WayCostToHome=3217,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3357, Id=2,WayCostFromHome=3080, WayCostToHome=3080,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2572, Id=3,WayCostFromHome=3035, WayCostToHome=3035,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3456, Id=4,WayCostFromHome=2371, WayCostToHome=2371,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1644, Id=5,WayCostFromHome=1286, WayCostToHome=1286,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2873, Id=6,WayCostFromHome=2785, WayCostToHome=2785,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3468, Id=7,WayCostFromHome=1603, WayCostToHome=1603,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3469, Id=8,WayCostFromHome=1018, WayCostToHome=1018,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1276, Id=9,WayCostFromHome=795, WayCostToHome=795,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1730, Id=10,WayCostFromHome=2284, WayCostToHome=2284,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2680, Id=11,WayCostFromHome=2964, WayCostToHome=2964,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3098, Id=12,WayCostFromHome=907, WayCostToHome=907,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1983, Id=13,WayCostFromHome=487, WayCostToHome=487,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2882, Id=14,WayCostFromHome=628, WayCostToHome=628,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2653, Id=15,WayCostFromHome=2096, WayCostToHome=2096,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=2708, WayCostToHome=2708,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 120, (0 * Hour + 120) + 32188),(24 * Hour + 120, (24 * Hour + 120) + 32188)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=3135, WayCostToHome=3135,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 12515, (0 * Hour + 12515) + 14289),(24 * Hour + 12515, (24 * Hour + 12515) + 14289)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
