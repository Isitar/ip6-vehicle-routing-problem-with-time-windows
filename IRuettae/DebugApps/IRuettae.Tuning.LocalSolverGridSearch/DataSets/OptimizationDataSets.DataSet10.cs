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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet10()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2587,330),
		(1173,1527),
		(1701,1516),
		(1350,1592),
		(2827,2592),
		(1066,1032),
		(2028,2584),
		(1854,712),
		(1064,1638),
		(2750,2900),
		(1906,1202),
		(3127,2062),
		(2745,2293),
		(2214,2942),
		(1363,1733),
		(1920,1703),
		(1207,238),
		(3198,2844),
		(1247,1500)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 528, 188, 1967, 506, 1359, 1062, 155, 2090, 801, 2025, 1748, 1756, 280, 767, 1289, 2415, 78 },
			{ 528, 0, 359, 1557, 798, 1116, 818, 648, 1736, 374, 1526, 1301, 1515, 401, 287, 1370, 2001, 454 },
			{ 188, 359, 0, 1783, 627, 1201, 1014, 289, 1915, 679, 1838, 1561, 1602, 141, 580, 1361, 2232, 138 },
			{ 1967, 1557, 1783, 0, 2352, 799, 2116, 2004, 317, 1667, 609, 310, 705, 1697, 1270, 2857, 448, 1920 },
			{ 506, 798, 627, 2352, 0, 1825, 850, 606, 2515, 857, 2304, 2099, 2228, 761, 1086, 806, 2797, 501 },
			{ 1359, 1116, 1201, 799, 1825, 0, 1880, 1350, 788, 1387, 1216, 773, 403, 1080, 887, 2485, 1198, 1336 },
			{ 1062, 818, 1014, 2116, 850, 1880, 0, 1217, 2364, 492, 1855, 1814, 2258, 1132, 993, 802, 2520, 994 },
			{ 155, 648, 289, 2004, 606, 1350, 1217, 0, 2106, 948, 2106, 1804, 1738, 313, 858, 1407, 2451, 229 },
			{ 2090, 1736, 1915, 317, 2515, 788, 2364, 2106, 0, 1896, 918, 607, 537, 1812, 1456, 3076, 451, 2054 },
			{ 801, 374, 679, 1667, 857, 1387, 492, 948, 1896, 0, 1493, 1376, 1767, 759, 501, 1190, 2089, 723 },
			{ 2025, 1526, 1838, 609, 2304, 1216, 1855, 2106, 918, 1493, 0, 446, 1268, 1794, 1259, 2648, 785, 1962 },
			{ 1748, 1301, 1561, 310, 2099, 773, 1814, 1804, 607, 1376, 446, 0, 838, 1491, 1014, 2566, 713, 1694 },
			{ 1756, 1515, 1602, 705, 2228, 403, 2258, 1738, 537, 1767, 1268, 838, 0, 1478, 1273, 2885, 988, 1736 },
			{ 280, 401, 141, 1697, 761, 1080, 1132, 313, 1812, 759, 1794, 1491, 1478, 0, 557, 1503, 2145, 260 },
			{ 767, 287, 580, 1270, 1086, 887, 993, 858, 1456, 501, 1259, 1014, 1273, 557, 0, 1629, 1713, 702 },
			{ 1289, 1370, 1361, 2857, 806, 2485, 802, 1407, 3076, 1190, 2648, 2566, 2885, 1503, 1629, 0, 3279, 1262 },
			{ 2415, 2001, 2232, 448, 2797, 1198, 2520, 2451, 451, 2089, 785, 713, 988, 2145, 1713, 3279, 0, 2369 },
			{ 78, 454, 138, 1920, 501, 1336, 994, 229, 2054, 723, 1962, 1694, 1736, 260, 702, 1262, 2369, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1979, Id=0,WayCostFromHome=1852, WayCostToHome=1852,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2678, Id=1,WayCostFromHome=1480, WayCostToHome=1480,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1263, Id=2,WayCostFromHome=1767, WayCostToHome=1767,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2045, Id=3,WayCostFromHome=2274, WayCostToHome=2274,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2056, Id=4,WayCostFromHome=1675, WayCostToHome=1675,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1778, Id=5,WayCostFromHome=2322, WayCostToHome=2322,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1678, Id=6,WayCostFromHome=826, WayCostToHome=826,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1874, Id=7,WayCostFromHome=2007, WayCostToHome=2007,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2740, Id=8,WayCostFromHome=2575, WayCostToHome=2575,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2981, Id=9,WayCostFromHome=1106, WayCostToHome=1106,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1800, Id=10,WayCostFromHome=1814, WayCostToHome=1814,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3279, Id=11,WayCostFromHome=1969, WayCostToHome=1969,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3160, Id=12,WayCostFromHome=2638, WayCostToHome=2638,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3196, Id=13,WayCostFromHome=1861, WayCostToHome=1861,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1675, Id=14,WayCostFromHome=1526, WayCostToHome=1526,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1572, Id=15,WayCostFromHome=1383, WayCostToHome=1383,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=2587, WayCostToHome=2587,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4522, (0 * Hour + 4522) + 23152),(24 * Hour + 4522, (24 * Hour + 4522) + 23152)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1778, WayCostToHome=1778,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 18999, (0 * Hour + 18999) + 9971),(24 * Hour + 18999, (24 * Hour + 18999) + 9971)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
