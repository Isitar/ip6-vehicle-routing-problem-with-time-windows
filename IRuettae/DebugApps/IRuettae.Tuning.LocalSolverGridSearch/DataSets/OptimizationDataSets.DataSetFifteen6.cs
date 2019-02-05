using IRuettae.Core.Models;
namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 15 Visits, 2 Days, 2 Santas
/// 4 Breaks, 13 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen6()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2459,3032),
		(1138,958),
		(1164,1833),
		(2644,2858),
		(764,1544),
		(1694,1729),
		(2074,2382),
		(243,2032),
		(2802,3004),
		(2079,1668),
		(3178,2482),
		(2419,2647),
		(1589,1303),
		(2595,3160)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 875, 2424, 695, 950, 1704, 1398, 2637, 1178, 2546, 2119, 567, 2640 },
			{ 875, 0, 1800, 493, 540, 1062, 942, 2013, 929, 2115, 1495, 679, 1951 },
			{ 2424, 1800, 0, 2293, 1475, 742, 2539, 215, 1317, 653, 308, 1879, 305 },
			{ 695, 493, 2293, 0, 948, 1555, 713, 2506, 1320, 2589, 1988, 859, 2442 },
			{ 950, 540, 1475, 948, 0, 755, 1482, 1689, 389, 1664, 1169, 438, 1691 },
			{ 1704, 1062, 742, 1555, 755, 0, 1864, 957, 714, 1108, 435, 1182, 936 },
			{ 1398, 942, 2539, 713, 1482, 1864, 0, 2737, 1871, 2969, 2261, 1530, 2608 },
			{ 2637, 2013, 215, 2506, 1689, 957, 2737, 0, 1519, 643, 523, 2089, 259 },
			{ 1178, 929, 1317, 1320, 389, 714, 1871, 1519, 0, 1367, 1036, 611, 1578 },
			{ 2546, 2115, 653, 2589, 1664, 1108, 2969, 643, 1367, 0, 776, 1978, 894 },
			{ 2119, 1495, 308, 1988, 1169, 435, 2261, 523, 1036, 776, 0, 1579, 542 },
			{ 567, 679, 1879, 859, 438, 1182, 1530, 2089, 611, 1978, 1579, 0, 2111 },
			{ 2640, 1951, 305, 2442, 1691, 936, 2608, 259, 1578, 894, 542, 2111, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1970, Id=0,WayCostFromHome=2458, WayCostToHome=2458,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1654, Id=1,WayCostFromHome=1764, WayCostToHome=1764,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2826, Id=2,WayCostFromHome=253, WayCostToHome=253,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1704, Id=3,WayCostFromHome=2255, WayCostToHome=2255,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2266, Id=4,WayCostFromHome=1510, WayCostToHome=1510,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3389, Id=5,WayCostFromHome=755, WayCostToHome=755,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3188, Id=6,WayCostFromHome=2431, WayCostToHome=2431,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2402, Id=7,WayCostFromHome=344, WayCostToHome=344,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1252, Id=8,WayCostFromHome=1415, WayCostToHome=1415,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3187, Id=9,WayCostFromHome=905, WayCostToHome=905,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3117, Id=10,WayCostFromHome=387, WayCostToHome=387,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1935, WayCostToHome=1935,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1866, (0 * Hour + 1866) + 8267),(24 * Hour + 1866, (24 * Hour + 1866) + 8267)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=186, WayCostToHome=186,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 160, (0 * Hour + 160) + 13816),(24 * Hour + 160, (24 * Hour + 160) + 13816)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
