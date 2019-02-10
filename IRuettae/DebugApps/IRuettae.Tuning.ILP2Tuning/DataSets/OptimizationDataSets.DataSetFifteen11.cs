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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen11()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(839,1413),
		(2926,2385),
		(1068,653),
		(1582,2971),
		(2633,2705),
		(1083,674),
		(1680,1291),
		(1970,1231),
		(1192,1287),
		(1040,1691),
		(911,1568),
		(1663,913),
		(1996,1356),
		(1497,1659)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2540, 1466, 433, 2514, 1658, 1498, 2052, 2009, 2174, 1939, 1386, 1602 },
			{ 2540, 0, 2374, 2580, 25, 884, 1071, 646, 1038, 928, 649, 1164, 1093 },
			{ 1466, 2374, 0, 1084, 2350, 1682, 1782, 1728, 1390, 1555, 2059, 1667, 1314 },
			{ 433, 2580, 1084, 0, 2554, 1705, 1616, 2021, 1888, 2063, 2037, 1491, 1544 },
			{ 2514, 25, 2350, 2554, 0, 858, 1047, 622, 1017, 910, 627, 1139, 1068 },
			{ 1658, 884, 1682, 1705, 858, 0, 296, 488, 754, 817, 378, 322, 410 },
			{ 1498, 1071, 1782, 1616, 1047, 296, 0, 780, 1037, 1111, 442, 127, 637 },
			{ 2052, 646, 1728, 2021, 622, 488, 780, 0, 431, 397, 601, 806, 481 },
			{ 2009, 1038, 1390, 1888, 1017, 754, 1037, 431, 0, 178, 996, 1012, 458 },
			{ 2174, 928, 1555, 2063, 910, 817, 1111, 397, 178, 0, 997, 1105, 593 },
			{ 1939, 649, 2059, 2037, 627, 378, 442, 601, 996, 997, 0, 554, 764 },
			{ 1386, 1164, 1667, 1491, 1139, 322, 127, 806, 1012, 1105, 554, 0, 583 },
			{ 1602, 1093, 1314, 1544, 1068, 410, 637, 481, 458, 593, 764, 583, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3048, Id=0,WayCostFromHome=2302, WayCostToHome=2302,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1525, Id=1,WayCostFromHome=793, WayCostToHome=793,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2029, Id=2,WayCostFromHome=1726, WayCostToHome=1726,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2979, Id=3,WayCostFromHome=2210, WayCostToHome=2210,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2238, Id=4,WayCostFromHome=778, WayCostToHome=778,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1598, Id=5,WayCostFromHome=849, WayCostToHome=849,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2552, Id=6,WayCostFromHome=1145, WayCostToHome=1145,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2549, Id=7,WayCostFromHome=374, WayCostToHome=374,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1626, Id=8,WayCostFromHome=343, WayCostToHome=343,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1255, Id=9,WayCostFromHome=170, WayCostToHome=170,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2647, Id=10,WayCostFromHome=963, WayCostToHome=963,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1158, WayCostToHome=1158,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2666, (0 * Hour + 2666) + 11963),(24 * Hour + 2666, (24 * Hour + 2666) + 11963)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=702, WayCostToHome=702,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1899, (0 * Hour + 1899) + 14934),(24 * Hour + 1899, (24 * Hour + 1899) + 14934)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
