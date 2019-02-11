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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet15()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2427,3092),
		(3188,2025),
		(546,1639),
		(1639,1686),
		(665,1923),
		(713,217),
		(1343,1377),
		(1633,1669),
		(2606,2954),
		(2751,2403),
		(2454,2456),
		(1672,517),
		(3276,2639),
		(2545,2744),
		(3029,2432),
		(2141,2814),
		(2856,3104),
		(2838,2297),
		(1455,922)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2670, 1585, 2525, 3065, 1955, 1595, 1096, 577, 851, 2138, 620, 964, 436, 1311, 1128, 443, 2054 },
			{ 2670, 0, 1094, 307, 1431, 838, 1087, 2443, 2333, 2075, 1589, 2907, 2284, 2606, 1981, 2735, 2384, 1157 },
			{ 1585, 1094, 0, 1002, 1736, 427, 18, 1594, 1323, 1121, 1169, 1894, 1392, 1577, 1234, 1868, 1345, 785 },
			{ 2525, 307, 1002, 0, 1706, 870, 1000, 2197, 2140, 1866, 1729, 2707, 2051, 2418, 1724, 2489, 2204, 1275 },
			{ 3065, 1431, 1736, 1706, 0, 1320, 1718, 3327, 2988, 2836, 1004, 3526, 3121, 3204, 2963, 3595, 2973, 1023 },
			{ 1955, 838, 427, 870, 1320, 0, 411, 2020, 1742, 1548, 920, 2308, 1820, 1988, 1643, 2296, 1755, 468 },
			{ 1595, 1087, 18, 1000, 1718, 411, 0, 1611, 1337, 1137, 1152, 1907, 1409, 1590, 1252, 1885, 1358, 767 },
			{ 1096, 2443, 1594, 2197, 3327, 2020, 1611, 0, 569, 520, 2609, 740, 218, 671, 485, 291, 696, 2335 },
			{ 577, 2333, 1323, 2140, 2988, 1742, 1337, 569, 0, 301, 2172, 575, 398, 279, 735, 708, 137, 1967 },
			{ 851, 2075, 1121, 1866, 2836, 1548, 1137, 520, 301, 0, 2090, 842, 302, 575, 475, 762, 415, 1830 },
			{ 2138, 1589, 1169, 1729, 1004, 920, 1152, 2609, 2172, 2090, 0, 2660, 2391, 2347, 2344, 2845, 2127, 459 },
			{ 620, 2907, 1894, 2707, 3526, 2308, 1907, 740, 575, 842, 2660, 0, 738, 322, 1148, 626, 555, 2502 },
			{ 964, 2284, 1392, 2051, 3121, 1820, 1409, 218, 398, 302, 2391, 738, 0, 575, 410, 475, 534, 2123 },
			{ 436, 2606, 1577, 2418, 3204, 1988, 1590, 671, 279, 575, 2347, 322, 575, 0, 966, 693, 233, 2181 },
			{ 1311, 1981, 1234, 1724, 2963, 1643, 1252, 485, 735, 475, 2344, 1148, 410, 966, 0, 771, 867, 2012 },
			{ 1128, 2735, 1868, 2489, 3595, 2296, 1885, 291, 708, 762, 2845, 626, 475, 693, 771, 0, 807, 2593 },
			{ 443, 2384, 1345, 2204, 2973, 1755, 1358, 696, 137, 415, 2127, 555, 534, 233, 867, 807, 0, 1950 },
			{ 2054, 1157, 785, 1275, 1023, 468, 767, 2335, 1967, 1830, 459, 2502, 2123, 2181, 2012, 2593, 1950, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1485, Id=0,WayCostFromHome=1310, WayCostToHome=1310,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2004, Id=1,WayCostFromHome=2376, WayCostToHome=2376,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2841, Id=2,WayCostFromHome=1611, WayCostToHome=1611,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3522, Id=3,WayCostFromHome=2114, WayCostToHome=2114,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1394, Id=4,WayCostFromHome=3347, WayCostToHome=3347,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2217, Id=5,WayCostFromHome=2028, WayCostToHome=2028,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2671, Id=6,WayCostFromHome=1629, WayCostToHome=1629,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2540, Id=7,WayCostFromHome=226, WayCostToHome=226,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2844, Id=8,WayCostFromHome=761, WayCostToHome=761,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1844, Id=9,WayCostFromHome=636, WayCostToHome=636,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2459, Id=10,WayCostFromHome=2683, WayCostToHome=2683,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1227, Id=11,WayCostFromHome=962, WayCostToHome=962,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2507, Id=12,WayCostFromHome=367, WayCostToHome=367,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3061, Id=13,WayCostFromHome=893, WayCostToHome=893,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3215, Id=14,WayCostFromHome=398, WayCostToHome=398,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2761, Id=15,WayCostFromHome=429, WayCostToHome=429,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=894, WayCostToHome=894,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2031, (0 * Hour + 2031) + 29514),(24 * Hour + 2031, (24 * Hour + 2031) + 29514)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=2377, WayCostToHome=2377,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3455, (0 * Hour + 3455) + 13641),(24 * Hour + 3455, (24 * Hour + 3455) + 13641)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
