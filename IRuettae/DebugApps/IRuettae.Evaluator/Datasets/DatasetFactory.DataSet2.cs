using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 10 Visits, 2 Days, 1 Santas
/// 5 Desired, 0 Unavailable on day 0
/// 5 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet2()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1058,917),
		(3131,2414),
		(2502,2728),
		(1323,1651),
		(926,1198),
		(3297,2574),
		(2437,3298),
		(347,1533),
		(2169,2277),
		(2871,2771),
		(2786,2714)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 703, 1962, 2518, 230, 1123, 2920, 971, 441, 457 },
			{ 703, 0, 1596, 2196, 809, 573, 2464, 560, 371, 284 },
			{ 1962, 1596, 0, 602, 2179, 1988, 983, 1052, 1910, 1808 },
			{ 2518, 2196, 602, 0, 2741, 2587, 668, 1645, 2501, 2399 },
			{ 230, 809, 2179, 2741, 0, 1124, 3128, 1166, 469, 529 },
			{ 1123, 573, 1988, 2587, 1124, 0, 2735, 1055, 682, 680 },
			{ 2920, 2464, 983, 668, 3128, 2735, 0, 1968, 2811, 2709 },
			{ 971, 560, 1052, 1645, 1166, 1055, 1968, 0, 858, 756 },
			{ 441, 371, 1910, 2501, 469, 682, 2811, 858, 0, 102 },
			{ 457, 284, 1808, 2399, 529, 680, 2709, 756, 102, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2972, Id=0,WayCostFromHome=2557, WayCostToHome=2557,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 18869, (0 * Hour + 18869) + 8915)}},
			new Visit{Duration = 2600, Id=1,WayCostFromHome=2316, WayCostToHome=2316,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 194, (0 * Hour + 194) + 19647)}},
			new Visit{Duration = 3281, Id=2,WayCostFromHome=780, WayCostToHome=780,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5159, (0 * Hour + 5159) + 15658)}},
			new Visit{Duration = 1257, Id=3,WayCostFromHome=310, WayCostToHome=310,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5936, (0 * Hour + 5936) + 24594)}},
			new Visit{Duration = 1484, Id=4,WayCostFromHome=2785, WayCostToHome=2785,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 121, (0 * Hour + 121) + 30239)}},
			new Visit{Duration = 3211, Id=5,WayCostFromHome=2751, WayCostToHome=2751,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 1890, (24 * Hour + 1890) + 12048)}},
			new Visit{Duration = 2961, Id=6,WayCostFromHome=940, WayCostToHome=940,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 4606, (24 * Hour + 4606) + 26031)}},
			new Visit{Duration = 2072, Id=7,WayCostFromHome=1756, WayCostToHome=1756,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 1240, (24 * Hour + 1240) + 24346)}},
			new Visit{Duration = 1329, Id=8,WayCostFromHome=2593, WayCostToHome=2593,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 16964, (24 * Hour + 16964) + 6948)}},
			new Visit{Duration = 3093, Id=9,WayCostFromHome=2493, WayCostToHome=2493,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 4276, (24 * Hour + 4276) + 5035)}}
		}
	};
	return (input, coordinates);
}
}
}
