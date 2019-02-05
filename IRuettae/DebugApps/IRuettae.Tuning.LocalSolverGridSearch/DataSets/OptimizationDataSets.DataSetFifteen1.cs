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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen1()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2480,2095),
		(1736,1463),
		(2558,3192),
		(1557,921),
		(2932,2868),
		(648,1320),
		(2842,3599),
		(1434,1163),
		(1919,1631),
		(949,1710),
		(1718,1529),
		(578,1194),
		(2647,2798),
		(3194,3213)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1914, 570, 1845, 1097, 2405, 425, 248, 824, 68, 1188, 1616, 2277 },
			{ 1914, 0, 2481, 494, 2674, 496, 2319, 1686, 2187, 1863, 2812, 403, 636 },
			{ 570, 2481, 0, 2383, 992, 2970, 271, 796, 996, 628, 1016, 2170, 2816 },
			{ 1845, 494, 2383, 0, 2759, 736, 2269, 1598, 2296, 1807, 2888, 293, 433 },
			{ 1097, 2674, 992, 2759, 0, 3163, 801, 1308, 492, 1090, 144, 2486, 3172 },
			{ 2405, 496, 2970, 736, 3163, 0, 2813, 2173, 2674, 2355, 3302, 824, 522 },
			{ 425, 2319, 271, 2269, 801, 2813, 0, 673, 731, 463, 856, 2035, 2701 },
			{ 248, 1686, 796, 1598, 1308, 2173, 673, 0, 973, 225, 1410, 1375, 2031 },
			{ 824, 2187, 996, 2296, 492, 2674, 731, 973, 0, 790, 635, 2016, 2701 },
			{ 68, 1863, 628, 1807, 1090, 2355, 463, 225, 790, 0, 1188, 1572, 2239 },
			{ 1188, 2812, 1016, 2888, 144, 3302, 856, 1410, 635, 1188, 0, 2617, 3304 },
			{ 1616, 403, 2170, 293, 2486, 824, 2035, 1375, 2016, 1572, 2617, 0, 686 },
			{ 2277, 636, 2816, 433, 3172, 522, 2701, 2031, 2701, 2239, 3304, 686, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2747, Id=0,WayCostFromHome=976, WayCostToHome=976,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2964, Id=1,WayCostFromHome=1099, WayCostToHome=1099,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2102, Id=2,WayCostFromHome=1493, WayCostToHome=1493,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1343, Id=3,WayCostFromHome=895, WayCostToHome=895,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3434, Id=4,WayCostFromHome=1989, WayCostToHome=1989,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3321, Id=5,WayCostFromHome=1546, WayCostToHome=1546,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2293, Id=6,WayCostFromHome=1400, WayCostToHome=1400,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2076, Id=7,WayCostFromHome=728, WayCostToHome=728,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2423, Id=8,WayCostFromHome=1578, WayCostToHome=1578,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2005, Id=9,WayCostFromHome=949, WayCostToHome=949,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3481, Id=10,WayCostFromHome=2104, WayCostToHome=2104,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=722, WayCostToHome=722,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4408, (0 * Hour + 4408) + 2094),(24 * Hour + 4408, (24 * Hour + 4408) + 2094)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=1326, WayCostToHome=1326,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 791, (0 * Hour + 791) + 13198),(24 * Hour + 791, (24 * Hour + 791) + 13198)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
