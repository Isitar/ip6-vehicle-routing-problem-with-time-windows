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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet14()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2023,2037),
		(3523,2245),
		(3465,3054),
		(1543,2213),
		(999,2226),
		(881,21),
		(2844,2391),
		(3169,2901),
		(1352,1527),
		(3582,2528),
		(3142,2492),
		(2937,2607),
		(1798,2000),
		(1303,2183),
		(2565,2832),
		(2390,2747),
		(1314,1569),
		(2741,3048),
		(3026,2157)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 811, 1980, 2524, 3453, 694, 745, 2286, 289, 454, 688, 1742, 2220, 1123, 1239, 2310, 1120, 504 },
			{ 811, 0, 2097, 2601, 3984, 908, 333, 2607, 538, 648, 691, 1972, 2330, 926, 1117, 2613, 724, 998 },
			{ 1980, 2097, 0, 544, 2289, 1313, 1765, 712, 2063, 1623, 1448, 332, 241, 1194, 1001, 683, 1460, 1484 },
			{ 2524, 2601, 544, 0, 2208, 1852, 2272, 783, 2600, 2159, 1975, 830, 307, 1679, 1485, 728, 1926, 2028 },
			{ 3453, 3984, 2289, 2208, 0, 3077, 3678, 1577, 3685, 3349, 3303, 2181, 2202, 3276, 3115, 1607, 3552, 3027 },
			{ 694, 908, 1313, 1852, 3077, 0, 604, 1724, 750, 314, 235, 1116, 1554, 521, 576, 1736, 665, 296 },
			{ 745, 333, 1765, 2272, 3678, 604, 0, 2278, 556, 409, 374, 1640, 1999, 607, 794, 2283, 452, 757 },
			{ 2286, 2607, 712, 783, 1577, 1724, 2278, 0, 2444, 2033, 1917, 650, 657, 1781, 1601, 56, 2059, 1788 },
			{ 289, 538, 2063, 2600, 3685, 750, 556, 2444, 0, 441, 649, 1860, 2304, 1061, 1211, 2462, 988, 668 },
			{ 454, 648, 1623, 2159, 3349, 314, 409, 2033, 441, 0, 235, 1431, 1864, 669, 794, 2047, 685, 354 },
			{ 688, 691, 1448, 1975, 3303, 235, 374, 1917, 649, 235, 0, 1290, 1688, 434, 564, 1926, 482, 458 },
			{ 1742, 1972, 332, 830, 2181, 1116, 1640, 650, 1860, 1431, 1290, 0, 527, 1131, 953, 648, 1409, 1237 },
			{ 2220, 2330, 241, 307, 2202, 1554, 1999, 657, 2304, 1864, 1688, 527, 0, 1419, 1224, 614, 1678, 1723 },
			{ 1123, 926, 1194, 1679, 3276, 521, 607, 1781, 1061, 669, 434, 1131, 1419, 0, 194, 1777, 278, 817 },
			{ 1239, 1117, 1001, 1485, 3115, 576, 794, 1601, 1211, 794, 564, 953, 1224, 194, 0, 1595, 462, 867 },
			{ 2310, 2613, 683, 728, 1607, 1736, 2283, 56, 2462, 2047, 1926, 648, 614, 1777, 1595, 0, 2055, 1810 },
			{ 1120, 724, 1460, 1926, 3552, 665, 452, 2059, 988, 685, 482, 1409, 1678, 278, 462, 2055, 0, 935 },
			{ 504, 998, 1484, 2028, 3027, 296, 757, 1788, 668, 354, 458, 1237, 1723, 817, 867, 1810, 935, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2272, Id=0,WayCostFromHome=1514, WayCostToHome=1514,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2216, Id=1,WayCostFromHome=1764, WayCostToHome=1764,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3411, Id=2,WayCostFromHome=511, WayCostToHome=511,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2271, Id=3,WayCostFromHome=1041, WayCostToHome=1041,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3199, Id=4,WayCostFromHome=2316, WayCostToHome=2316,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1213, Id=5,WayCostFromHome=894, WayCostToHome=894,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3313, Id=6,WayCostFromHome=1435, WayCostToHome=1435,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2961, Id=7,WayCostFromHome=842, WayCostToHome=842,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2674, Id=8,WayCostFromHome=1634, WayCostToHome=1634,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1444, Id=9,WayCostFromHome=1207, WayCostToHome=1207,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2772, Id=10,WayCostFromHome=1077, WayCostToHome=1077,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2880, Id=11,WayCostFromHome=228, WayCostToHome=228,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3230, Id=12,WayCostFromHome=734, WayCostToHome=734,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1954, Id=13,WayCostFromHome=962, WayCostToHome=962,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3040, Id=14,WayCostFromHome=799, WayCostToHome=799,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3265, Id=15,WayCostFromHome=849, WayCostToHome=849,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=1240, WayCostToHome=1240,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5105, (0 * Hour + 5105) + 24603),(24 * Hour + 5105, (24 * Hour + 5105) + 24603)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=1010, WayCostToHome=1010,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3821, (0 * Hour + 3821) + 28047),(24 * Hour + 3821, (24 * Hour + 3821) + 28047)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
