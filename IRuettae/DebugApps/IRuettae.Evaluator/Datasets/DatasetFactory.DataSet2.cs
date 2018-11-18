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
		(1268,1352),
		(358,3013),
		(2855,2621),
		(1169,1121),
		(2468,2818),
		(1452,999),
		(2698,3226),
		(2946,3836),
		(1555,887),
		(1549,1125),
		(1587,570)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2527, 2058, 2118, 2291, 2349, 2715, 2439, 2232, 2734 },
			{ 2527, 0, 2256, 434, 2144, 625, 1218, 2167, 1985, 2411 },
			{ 2058, 2256, 0, 2137, 308, 2601, 3244, 451, 380, 691 },
			{ 2118, 434, 2137, 0, 2083, 468, 1124, 2135, 1926, 2414 },
			{ 2291, 2144, 308, 2083, 0, 2551, 3206, 152, 159, 449 },
			{ 2349, 625, 2601, 468, 2551, 0, 658, 2603, 2394, 2879 },
			{ 2715, 1218, 3244, 1124, 3206, 658, 0, 3260, 3049, 3537 },
			{ 2439, 2167, 451, 2135, 152, 2603, 3260, 0, 238, 318 },
			{ 2232, 1985, 380, 1926, 159, 2394, 3049, 238, 0, 556 },
			{ 2734, 2411, 691, 2414, 449, 2879, 3537, 318, 556, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3199, Id=0,WayCostFromHome=1893, WayCostToHome=1893,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1395.53446920382, (0 * Hour + 1395.53446920382) + 22543.1773491167)}},
			new Visit{Duration = 1625, Id=1,WayCostFromHome=2031, WayCostToHome=2031,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 6042.89268725687, (0 * Hour + 6042.89268725687) + 24512.4975474144)}},
			new Visit{Duration = 1841, Id=2,WayCostFromHome=251, WayCostToHome=251,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3043.14349793752, (0 * Hour + 3043.14349793752) + 6006.8365804082)}},
			new Visit{Duration = 2432, Id=3,WayCostFromHome=1894, WayCostToHome=1894,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3790.50977758164, (0 * Hour + 3790.50977758164) + 24040.8037182134)}},
			new Visit{Duration = 2676, Id=4,WayCostFromHome=398, WayCostToHome=398,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4578.40194602853, (0 * Hour + 4578.40194602853) + 20286.7140171075)}},
			new Visit{Duration = 1450, Id=5,WayCostFromHome=2357, WayCostToHome=2357,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3504.87695240256, (24 * Hour + 3504.87695240256) + 16122.5685884583)}},
			new Visit{Duration = 2408, Id=6,WayCostFromHome=2997, WayCostToHome=2997,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 12364.1446891619, (24 * Hour + 12364.1446891619) + 11325.04609648)}},
			new Visit{Duration = 2629, Id=7,WayCostFromHome=546, WayCostToHome=546,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3302.35623168923, (24 * Hour + 3302.35623168923) + 19500.7740623605)}},
			new Visit{Duration = 1556, Id=8,WayCostFromHome=361, WayCostToHome=361,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 13099.1445864608, (24 * Hour + 13099.1445864608) + 16748.3446495274)}},
			new Visit{Duration = 3199, Id=9,WayCostFromHome=844, WayCostToHome=844,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3428.0610987341, (24 * Hour + 3428.0610987341) + 25175.5882566416)}}
		}
	};
	return (input, coordinates);
}
}
}
