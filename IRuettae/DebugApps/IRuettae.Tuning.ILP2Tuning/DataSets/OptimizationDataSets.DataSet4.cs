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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet4()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1362,1481),
		(1049,1755),
		(3659,2361),
		(2517,2728),
		(1314,1098),
		(825,3564),
		(2985,2545),
		(3095,2747),
		(2356,2560),
		(1967,2979),
		(1056,1442),
		(611,1465),
		(1427,2001),
		(2425,2784),
		(2506,2890),
		(828,1976),
		(2805,1320),
		(1179,1040),
		(1763,1650)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2679, 1761, 708, 1822, 2090, 2273, 1535, 1530, 313, 525, 450, 1718, 1846, 312, 1809, 726, 721 },
			{ 2679, 0, 1199, 2663, 3078, 698, 683, 1318, 1801, 2760, 3176, 2260, 1304, 1268, 2857, 1346, 2809, 2024 },
			{ 1761, 1199, 0, 2025, 1887, 502, 578, 232, 604, 1946, 2286, 1310, 107, 162, 1848, 1437, 2153, 1315 },
			{ 708, 2663, 2025, 0, 2514, 2210, 2427, 1795, 1991, 430, 793, 910, 2019, 2152, 1003, 1507, 146, 711 },
			{ 1822, 3078, 1887, 2514, 0, 2388, 2412, 1830, 1283, 2134, 2109, 1674, 1780, 1811, 1588, 2992, 2548, 2131 },
			{ 2090, 698, 502, 2210, 2388, 0, 230, 629, 1106, 2222, 2608, 1650, 608, 590, 2230, 1238, 2350, 1514 },
			{ 2273, 683, 578, 2427, 2412, 230, 0, 762, 1151, 2420, 2795, 1827, 671, 606, 2394, 1456, 2566, 1725 },
			{ 1535, 1318, 232, 1795, 1830, 629, 762, 0, 571, 1714, 2060, 1084, 234, 362, 1635, 1318, 1922, 1086 },
			{ 1530, 1801, 604, 1991, 1283, 1106, 1151, 571, 0, 1786, 2032, 1117, 497, 546, 1517, 1858, 2093, 1344 },
			{ 313, 2760, 1946, 430, 2134, 2222, 2420, 1714, 1786, 0, 445, 670, 1917, 2049, 580, 1753, 420, 736 },
			{ 525, 3176, 2286, 793, 2109, 2608, 2795, 2060, 2032, 445, 0, 976, 2242, 2371, 555, 2198, 709, 1166 },
			{ 450, 2260, 1310, 910, 1674, 1650, 1827, 1084, 1117, 670, 976, 0, 1268, 1398, 599, 1537, 992, 485 },
			{ 1718, 1304, 107, 2019, 1780, 608, 671, 234, 497, 1917, 2242, 1268, 0, 133, 1789, 1512, 2143, 1313 },
			{ 1846, 1268, 162, 2152, 1811, 590, 606, 362, 546, 2049, 2371, 1398, 133, 0, 1910, 1598, 2276, 1445 },
			{ 312, 2857, 1848, 1003, 1588, 2230, 2394, 1635, 1517, 580, 555, 599, 1789, 1910, 0, 2082, 999, 990 },
			{ 1809, 1346, 1437, 1507, 2992, 1238, 1456, 1318, 1858, 1753, 2198, 1537, 1512, 1598, 2082, 0, 1649, 1093 },
			{ 726, 2809, 2153, 146, 2548, 2350, 2566, 1922, 2093, 420, 709, 992, 2143, 2276, 999, 1649, 0, 844 },
			{ 721, 2024, 1315, 711, 2131, 1514, 1725, 1086, 1344, 736, 1166, 485, 1313, 1445, 990, 1093, 844, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3394, Id=0,WayCostFromHome=415, WayCostToHome=415,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2774, Id=1,WayCostFromHome=2459, WayCostToHome=2459,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2652, Id=2,WayCostFromHome=1699, WayCostToHome=1699,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3355, Id=3,WayCostFromHome=385, WayCostToHome=385,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2979, Id=4,WayCostFromHome=2151, WayCostToHome=2151,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2456, Id=5,WayCostFromHome=1940, WayCostToHome=1940,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1714, Id=6,WayCostFromHome=2146, WayCostToHome=2146,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3284, Id=7,WayCostFromHome=1467, WayCostToHome=1467,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2314, Id=8,WayCostFromHome=1615, WayCostToHome=1615,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2557, Id=9,WayCostFromHome=308, WayCostToHome=308,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3008, Id=10,WayCostFromHome=751, WayCostToHome=751,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3309, Id=11,WayCostFromHome=524, WayCostToHome=524,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2564, Id=12,WayCostFromHome=1681, WayCostToHome=1681,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3063, Id=13,WayCostFromHome=1814, WayCostToHome=1814,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2864, Id=14,WayCostFromHome=728, WayCostToHome=728,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2590, Id=15,WayCostFromHome=1451, WayCostToHome=1451,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=477, WayCostToHome=477,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2817, (0 * Hour + 2817) + 26537),(24 * Hour + 2817, (24 * Hour + 2817) + 26537)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=435, WayCostToHome=435,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 16237, (0 * Hour + 16237) + 10990),(24 * Hour + 16237, (24 * Hour + 16237) + 10990)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
