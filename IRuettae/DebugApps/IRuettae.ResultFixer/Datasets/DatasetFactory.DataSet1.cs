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
		(1128,158),
		(2374,2197),
		(2480,2893),
		(883,181),
		(1388,1545),
		(1054,1141),
		(1215,879),
		(3023,2887),
		(123,1146),
		(2441,2275),
		(1232,1800)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 704, 2507, 1182, 1690, 1755, 947, 2484, 102, 1209 },
			{ 704, 0, 3147, 1734, 2258, 2378, 543, 2933, 619, 1658 },
			{ 2507, 3147, 0, 1454, 975, 772, 3449, 1228, 2610, 1656 },
			{ 1182, 1734, 1454, 0, 524, 688, 2115, 1326, 1281, 298 },
			{ 1690, 2258, 975, 524, 0, 307, 2631, 931, 1791, 682 },
			{ 1755, 2378, 772, 688, 307, 0, 2702, 1124, 1857, 921 },
			{ 947, 543, 3449, 2115, 2631, 2702, 0, 3382, 844, 2095 },
			{ 2484, 2933, 1228, 1326, 931, 1124, 3382, 0, 2578, 1287 },
			{ 102, 619, 2610, 1281, 1791, 1857, 844, 2578, 0, 1298 },
			{ 1209, 1658, 1656, 298, 682, 921, 2095, 1287, 1298, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2493, Id=0,WayCostFromHome=2389, WayCostToHome=2389,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3329, Id=1,WayCostFromHome=3050, WayCostToHome=3050,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2468, Id=2,WayCostFromHome=246, WayCostToHome=246,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3313, Id=3,WayCostFromHome=1411, WayCostToHome=1411,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1949, Id=4,WayCostFromHome=985, WayCostToHome=985,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2081, Id=5,WayCostFromHome=726, WayCostToHome=726,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2926, Id=6,WayCostFromHome=3322, WayCostToHome=3322,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2433, Id=7,WayCostFromHome=1409, WayCostToHome=1409,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1970, Id=8,WayCostFromHome=2491, WayCostToHome=2491,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1849, Id=9,WayCostFromHome=1645, WayCostToHome=1645,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
