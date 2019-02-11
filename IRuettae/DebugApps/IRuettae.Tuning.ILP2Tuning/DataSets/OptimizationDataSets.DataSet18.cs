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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet18()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3252,2486),
		(1666,1357),
		(2280,2374),
		(1642,1661),
		(1605,1464),
		(1894,1317),
		(1694,1620),
		(2643,3307),
		(746,2597),
		(1918,1858),
		(2786,2673),
		(2632,3078),
		(2408,2687),
		(1469,1198),
		(1791,914),
		(3303,3426),
		(1112,1134),
		(3183,2897),
		(1035,519)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1187, 304, 123, 231, 264, 2181, 1544, 560, 1728, 1973, 1522, 253, 460, 2638, 597, 2161, 1049 },
			{ 1187, 0, 956, 1133, 1125, 954, 1001, 1550, 630, 587, 787, 338, 1428, 1539, 1467, 1703, 1043, 2234 },
			{ 304, 956, 0, 200, 426, 66, 1926, 1295, 339, 1527, 1728, 1280, 494, 761, 2423, 747, 1975, 1293 },
			{ 123, 1133, 200, 0, 324, 179, 2115, 1421, 503, 1690, 1913, 1463, 298, 580, 2594, 593, 2131, 1103 },
			{ 231, 1125, 426, 324, 0, 363, 2126, 1719, 541, 1623, 1909, 1463, 441, 415, 2536, 803, 2039, 1172 },
			{ 264, 954, 66, 179, 363, 0, 1935, 1361, 326, 1516, 1733, 1283, 478, 712, 2418, 758, 1961, 1283 },
			{ 2181, 1001, 1926, 2115, 2126, 1935, 0, 2025, 1620, 649, 229, 663, 2413, 2540, 670, 2658, 678, 3218 },
			{ 1544, 1550, 1295, 1421, 1719, 1361, 2025, 0, 1385, 2041, 1946, 1664, 1574, 1981, 2688, 1508, 2455, 2098 },
			{ 560, 630, 339, 503, 541, 326, 1620, 1385, 0, 1190, 1413, 962, 798, 952, 2092, 1083, 1636, 1603 },
			{ 1728, 587, 1527, 1690, 1623, 1516, 649, 2041, 1190, 0, 433, 378, 1977, 2020, 913, 2273, 455, 2775 },
			{ 1973, 787, 1728, 1913, 1909, 1733, 229, 1946, 1413, 433, 0, 450, 2210, 2321, 755, 2467, 579, 3016 },
			{ 1522, 338, 1280, 1463, 1463, 1283, 663, 1664, 962, 378, 450, 0, 1760, 1877, 1160, 2022, 802, 2566 },
			{ 253, 1428, 494, 298, 441, 478, 2413, 1574, 798, 1977, 2210, 1760, 0, 429, 2885, 362, 2413, 805 },
			{ 460, 1539, 761, 580, 415, 712, 2540, 1981, 952, 2020, 2321, 1877, 429, 0, 2931, 713, 2422, 852 },
			{ 2638, 1467, 2423, 2594, 2536, 2418, 670, 2688, 2092, 913, 755, 1160, 2885, 2931, 0, 3170, 542, 3687 },
			{ 597, 1703, 747, 593, 803, 758, 2658, 1508, 1083, 2273, 2467, 2022, 362, 713, 3170, 0, 2719, 619 },
			{ 2161, 1043, 1975, 2131, 2039, 1961, 678, 2455, 1636, 455, 579, 802, 2413, 2422, 542, 2719, 0, 3204 },
			{ 1049, 2234, 1293, 1103, 1172, 1283, 3218, 2098, 1603, 2775, 3016, 2566, 805, 852, 3687, 619, 3204, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2105, Id=0,WayCostFromHome=1946, WayCostToHome=1946,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3545, Id=1,WayCostFromHome=978, WayCostToHome=978,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2338, Id=2,WayCostFromHome=1809, WayCostToHome=1809,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2629, Id=3,WayCostFromHome=1938, WayCostToHome=1938,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3285, Id=4,WayCostFromHome=1791, WayCostToHome=1791,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1209, Id=5,WayCostFromHome=1782, WayCostToHome=1782,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1403, Id=6,WayCostFromHome=1022, WayCostToHome=1022,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2326, Id=7,WayCostFromHome=2508, WayCostToHome=2508,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3451, Id=8,WayCostFromHome=1474, WayCostToHome=1474,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1593, Id=9,WayCostFromHome=502, WayCostToHome=502,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1257, Id=10,WayCostFromHome=857, WayCostToHome=857,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1562, Id=11,WayCostFromHome=867, WayCostToHome=867,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2110, Id=12,WayCostFromHome=2199, WayCostToHome=2199,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2623, Id=13,WayCostFromHome=2146, WayCostToHome=2146,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1780, Id=14,WayCostFromHome=941, WayCostToHome=941,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2699, Id=15,WayCostFromHome=2531, WayCostToHome=2531,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=416, WayCostToHome=416,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7183, (0 * Hour + 7183) + 14709),(24 * Hour + 7183, (24 * Hour + 7183) + 14709)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=2963, WayCostToHome=2963,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 773, (0 * Hour + 773) + 20331),(24 * Hour + 773, (24 * Hour + 773) + 20331)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
