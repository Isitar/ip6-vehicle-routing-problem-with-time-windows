using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 10 Visits, 2 Days, 1 Santas
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet1()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(809,1445),
		(1501,1443),
		(1517,1040),
		(3125,3141),
		(1262,1702),
		(2215,3241),
		(1257,1293),
		(3110,2503),
		(2666,2955),
		(474,766),
		(2109,1002)
	};
	const int workingDayDuration = 10 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 403, 2349, 352, 1934, 286, 1926, 1908, 1230, 751 },
			{ 403, 0, 2645, 709, 2309, 362, 2162, 2233, 1078, 593 },
			{ 2349, 2645, 0, 2354, 915, 2627, 638, 495, 3559, 2368 },
			{ 352, 709, 2354, 0, 1810, 409, 2014, 1881, 1223, 1098 },
			{ 1934, 2309, 915, 1810, 0, 2170, 1160, 534, 3026, 2241 },
			{ 286, 362, 2627, 409, 2170, 0, 2213, 2178, 943, 900 },
			{ 1926, 2162, 638, 2014, 1160, 2213, 0, 633, 3156, 1804 },
			{ 1908, 2233, 495, 1881, 534, 2178, 633, 0, 3097, 2030 },
			{ 1230, 1078, 3559, 1223, 3026, 943, 3156, 3097, 0, 1651 },
			{ 751, 593, 2368, 1098, 2241, 900, 1804, 2030, 1651, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3103, Id=0,WayCostFromHome=692, WayCostToHome=692,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1583, Id=1,WayCostFromHome=815, WayCostToHome=815,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3118, Id=2,WayCostFromHome=2870, WayCostToHome=2870,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2936, Id=3,WayCostFromHome=520, WayCostToHome=520,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3418, Id=4,WayCostFromHome=2280, WayCostToHome=2280,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2267, Id=5,WayCostFromHome=473, WayCostToHome=473,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2302, Id=6,WayCostFromHome=2532, WayCostToHome=2532,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2199, Id=7,WayCostFromHome=2393, WayCostToHome=2393,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2875, Id=8,WayCostFromHome=757, WayCostToHome=757,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2490, Id=9,WayCostFromHome=1373, WayCostToHome=1373,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
