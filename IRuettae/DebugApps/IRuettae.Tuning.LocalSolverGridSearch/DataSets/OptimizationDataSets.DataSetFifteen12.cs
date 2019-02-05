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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen12()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2449,3124),
		(457,3150),
		(721,1790),
		(1448,837),
		(1348,1168),
		(1542,986),
		(2754,3258),
		(1787,1101),
		(3440,2741),
		(2580,2222),
		(920,1809),
		(2369,2193),
		(3658,2904),
		(1411,743)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1385, 2516, 2173, 2420, 2299, 2442, 3010, 2316, 1418, 2138, 3210, 2589 },
			{ 1385, 0, 1198, 883, 1149, 2507, 1269, 2880, 1908, 199, 1696, 3141, 1253 },
			{ 2516, 1198, 0, 345, 176, 2750, 429, 2755, 1788, 1106, 1639, 3025, 101 },
			{ 2173, 883, 345, 0, 266, 2518, 444, 2617, 1621, 770, 1446, 2889, 429 },
			{ 2420, 1149, 176, 266, 0, 2575, 270, 2585, 1614, 1031, 1463, 2855, 276 },
			{ 2299, 2507, 2750, 2518, 2575, 0, 2363, 859, 1050, 2337, 1132, 970, 2851 },
			{ 2442, 1269, 429, 444, 270, 2363, 0, 2328, 1373, 1119, 1237, 2598, 519 },
			{ 3010, 2880, 2755, 2617, 2585, 859, 2328, 0, 1004, 2686, 1203, 272, 2847 },
			{ 2316, 1908, 1788, 1621, 1614, 1050, 1373, 1004, 0, 1710, 212, 1275, 1885 },
			{ 1418, 199, 1106, 770, 1031, 2337, 1119, 2686, 1710, 0, 1499, 2948, 1173 },
			{ 2138, 1696, 1639, 1446, 1463, 1132, 1237, 1203, 212, 1499, 0, 1472, 1737 },
			{ 3210, 3141, 3025, 2889, 2855, 970, 2598, 272, 1275, 2948, 1472, 0, 3117 },
			{ 2589, 1253, 101, 429, 276, 2851, 519, 2847, 1885, 1173, 1737, 3117, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1338, Id=0,WayCostFromHome=1992, WayCostToHome=1992,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2071, Id=1,WayCostFromHome=2183, WayCostToHome=2183,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1773, Id=2,WayCostFromHome=2496, WayCostToHome=2496,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1319, Id=3,WayCostFromHome=2244, WayCostToHome=2244,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2988, Id=4,WayCostFromHome=2322, WayCostToHome=2322,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2642, Id=5,WayCostFromHome=333, WayCostToHome=333,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3208, Id=6,WayCostFromHome=2128, WayCostToHome=2128,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2289, Id=7,WayCostFromHome=1062, WayCostToHome=1062,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3270, Id=8,WayCostFromHome=911, WayCostToHome=911,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2888, Id=9,WayCostFromHome=2016, WayCostToHome=2016,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1597, Id=10,WayCostFromHome=934, WayCostToHome=934,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1228, WayCostToHome=1228,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 531, (0 * Hour + 531) + 17349),(24 * Hour + 531, (24 * Hour + 531) + 17349)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=2597, WayCostToHome=2597,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 8130, (0 * Hour + 8130) + 7659),(24 * Hour + 8130, (24 * Hour + 8130) + 7659)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
