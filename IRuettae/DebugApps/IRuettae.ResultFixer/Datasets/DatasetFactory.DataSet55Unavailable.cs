using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 0 Breaks, 20 unique visits
/// 0 Desired, 10 Unavailable on day 0
/// 0 Desired, 10 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet55Unavailable()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1392,1946),
		(1535,1302),
		(1402,1176),
		(2897,2658),
		(3208,2214),
		(2560,2692),
		(1599,2024),
		(733,773),
		(1028,1127),
		(1141,1547),
		(2325,2758),
		(956,1510),
		(2002,1142),
		(1205,1338),
		(2814,2446),
		(1273,590),
		(2237,941),
		(1638,964),
		(3098,2820),
		(2683,2190),
		(1092,2024)
	};
	const int workingDayDuration = 8 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 183, 1921, 1905, 1727, 724, 960, 536, 463, 1656, 615, 493, 331, 1715, 758, 789, 353, 2178, 1451, 847 },
			{ 183, 0, 2105, 2083, 1907, 870, 781, 377, 453, 1831, 557, 600, 255, 1899, 600, 867, 317, 2362, 1633, 902 },
			{ 1921, 2105, 0, 542, 338, 1444, 2869, 2416, 2077, 580, 2255, 1760, 2145, 227, 2629, 1839, 2110, 258, 514, 1913 },
			{ 1905, 2083, 542, 0, 805, 1620, 2863, 2435, 2171, 1037, 2359, 1613, 2186, 457, 2526, 1601, 2006, 615, 525, 2124 },
			{ 1727, 1907, 338, 805, 0, 1170, 2649, 2190, 1823, 244, 1992, 1647, 1915, 353, 2464, 1780, 1958, 553, 516, 1612 },
			{ 724, 870, 1444, 1620, 1170, 0, 1521, 1063, 661, 1032, 823, 969, 791, 1286, 1470, 1256, 1060, 1697, 1096, 507 },
			{ 960, 781, 2869, 2863, 2649, 1521, 0, 460, 874, 2544, 769, 1321, 736, 2670, 570, 1513, 924, 3127, 2410, 1301 },
			{ 536, 377, 2416, 2435, 2190, 1063, 460, 0, 434, 2083, 389, 974, 275, 2220, 590, 1223, 631, 2674, 1966, 899 },
			{ 463, 453, 2077, 2171, 1823, 661, 874, 434, 0, 1693, 188, 951, 218, 1899, 966, 1252, 766, 2334, 1670, 479 },
			{ 1656, 1831, 580, 1037, 244, 1032, 2544, 2083, 1693, 0, 1852, 1647, 1808, 580, 2409, 1819, 1921, 775, 671, 1434 },
			{ 615, 557, 2255, 2359, 1992, 823, 769, 389, 188, 1852, 0, 1108, 302, 2080, 973, 1401, 873, 2510, 1856, 531 },
			{ 493, 600, 1760, 1613, 1647, 969, 1321, 974, 951, 1647, 1108, 0, 820, 1536, 914, 309, 405, 2004, 1249, 1267 },
			{ 331, 255, 2145, 2186, 1915, 791, 736, 275, 218, 1808, 302, 820, 0, 1953, 751, 1105, 572, 2404, 1705, 695 },
			{ 1715, 1899, 227, 457, 353, 1286, 2670, 2220, 1899, 580, 2080, 1536, 1953, 0, 2412, 1611, 1891, 469, 287, 1772 },
			{ 758, 600, 2629, 2526, 2464, 1470, 570, 590, 966, 2409, 973, 914, 751, 2412, 0, 1025, 522, 2881, 2132, 1445 },
			{ 789, 867, 1839, 1601, 1780, 1256, 1513, 1223, 1252, 1819, 1401, 309, 1105, 1611, 1025, 0, 599, 2066, 1326, 1576 },
			{ 353, 317, 2110, 2006, 1958, 1060, 924, 631, 766, 1921, 873, 405, 572, 1891, 522, 599, 0, 2361, 1610, 1192 },
			{ 2178, 2362, 258, 615, 553, 1697, 3127, 2674, 2334, 775, 2510, 2004, 2404, 469, 2881, 2066, 2361, 0, 754, 2158 },
			{ 1451, 1633, 514, 525, 516, 1096, 2410, 1966, 1670, 671, 1856, 1249, 1705, 287, 2132, 1326, 1610, 754, 0, 1599 },
			{ 847, 902, 1913, 2124, 1612, 507, 1301, 899, 479, 1434, 531, 1267, 695, 1772, 1445, 1576, 1192, 2158, 1599, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3553, Id=0,WayCostFromHome=659, WayCostToHome=659,Unavailable =new [] {(0 * Hour + 5780, (0 * Hour + 5780) + 10768)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2817, Id=1,WayCostFromHome=770, WayCostToHome=770,Unavailable =new [] {(0 * Hour + 8788, (0 * Hour + 8788) + 14396)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2481, Id=2,WayCostFromHome=1664, WayCostToHome=1664,Unavailable =new [] {(0 * Hour + 12, (0 * Hour + 12) + 27512)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1727, Id=3,WayCostFromHome=1835, WayCostToHome=1835,Unavailable =new [] {(0 * Hour + 1114, (0 * Hour + 1114) + 24215)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3569, Id=4,WayCostFromHome=1385, WayCostToHome=1385,Unavailable =new [] {(0 * Hour + 314, (0 * Hour + 314) + 8942)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1725, Id=5,WayCostFromHome=221, WayCostToHome=221,Unavailable =new [] {(0 * Hour + 540, (0 * Hour + 540) + 7561)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1794, Id=6,WayCostFromHome=1345, WayCostToHome=1345,Unavailable =new [] {(0 * Hour + 5342, (0 * Hour + 5342) + 4306)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1246, Id=7,WayCostFromHome=896, WayCostToHome=896,Unavailable =new [] {(0 * Hour + 2829, (0 * Hour + 2829) + 21784)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2136, Id=8,WayCostFromHome=471, WayCostToHome=471,Unavailable =new [] {(0 * Hour + 10588, (0 * Hour + 10588) + 12353)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3351, Id=9,WayCostFromHome=1236, WayCostToHome=1236,Unavailable =new [] {(0 * Hour + 10114, (0 * Hour + 10114) + 16902)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1864, Id=10,WayCostFromHome=616, WayCostToHome=616,Unavailable =new [] {(24 * Hour + 1201, (24 * Hour + 1201) + 12546)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1314, Id=11,WayCostFromHome=1009, WayCostToHome=1009,Unavailable =new [] {(24 * Hour + 1754, (24 * Hour + 1754) + 26259)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2830, Id=12,WayCostFromHome=636, WayCostToHome=636,Unavailable =new [] {(24 * Hour + 1795, (24 * Hour + 1795) + 19512)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1540, Id=13,WayCostFromHome=1507, WayCostToHome=1507,Unavailable =new [] {(24 * Hour + 696, (24 * Hour + 696) + 23829)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1258, Id=14,WayCostFromHome=1361, WayCostToHome=1361,Unavailable =new [] {(24 * Hour + 3379, (24 * Hour + 3379) + 13380)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3196, Id=15,WayCostFromHome=1313, WayCostToHome=1313,Unavailable =new [] {(24 * Hour + 14256, (24 * Hour + 14256) + 7780)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1636, Id=16,WayCostFromHome=1012, WayCostToHome=1012,Unavailable =new [] {(24 * Hour + 1859, (24 * Hour + 1859) + 24036)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1709, Id=17,WayCostFromHome=1916, WayCostToHome=1916,Unavailable =new [] {(24 * Hour + 1197, (24 * Hour + 1197) + 27087)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3594, Id=18,WayCostFromHome=1313, WayCostToHome=1313,Unavailable =new [] {(24 * Hour + 2583, (24 * Hour + 2583) + 25783)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2165, Id=19,WayCostFromHome=309, WayCostToHome=309,Unavailable =new [] {(24 * Hour + 17, (24 * Hour + 17) + 24254)},Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
