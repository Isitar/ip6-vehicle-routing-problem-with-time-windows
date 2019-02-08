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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen19()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2887,2634),
		(1185,996),
		(1272,689),
		(3172,3159),
		(2051,1206),
		(1290,2354),
		(2692,2694),
		(1243,1364),
		(1505,1317),
		(2520,1495),
		(2821,2926),
		(2542,2594),
		(2075,1995),
		(1410,1391)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 319, 2937, 891, 1362, 2270, 372, 453, 1425, 2530, 2096, 1337, 454 },
			{ 319, 0, 3116, 934, 1665, 2456, 675, 669, 1485, 2720, 2289, 1533, 715 },
			{ 2937, 3116, 0, 2251, 2046, 668, 2634, 2484, 1787, 421, 846, 1599, 2496 },
			{ 891, 934, 2251, 0, 1377, 1620, 823, 557, 550, 1884, 1472, 789, 667 },
			{ 1362, 1665, 2046, 1377, 0, 1442, 991, 1059, 1500, 1634, 1274, 863, 970 },
			{ 2270, 2456, 668, 1620, 1442, 0, 1966, 1817, 1211, 265, 180, 932, 1827 },
			{ 372, 675, 2634, 823, 991, 1966, 0, 266, 1283, 2220, 1788, 1044, 169 },
			{ 453, 669, 2484, 557, 1059, 1817, 266, 0, 1030, 2078, 1645, 885, 120 },
			{ 1425, 1485, 1787, 550, 1500, 1211, 1283, 1030, 0, 1462, 1099, 669, 1114 },
			{ 2530, 2720, 421, 1884, 1634, 265, 2220, 2078, 1462, 0, 433, 1193, 2084 },
			{ 2096, 2289, 846, 1472, 1274, 180, 1788, 1645, 1099, 433, 0, 759, 1651 },
			{ 1337, 1533, 1599, 789, 863, 932, 1044, 885, 669, 1193, 759, 0, 898 },
			{ 454, 715, 2496, 667, 970, 1827, 169, 120, 1114, 2084, 1651, 898, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3437, Id=0,WayCostFromHome=2362, WayCostToHome=2362,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1272, Id=1,WayCostFromHome=2528, WayCostToHome=2528,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1598, Id=2,WayCostFromHome=597, WayCostToHome=597,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1568, Id=3,WayCostFromHome=1654, WayCostToHome=1654,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2730, Id=4,WayCostFromHome=1621, WayCostToHome=1621,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2279, Id=5,WayCostFromHome=204, WayCostToHome=204,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1356, Id=6,WayCostFromHome=2077, WayCostToHome=2077,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2656, Id=7,WayCostFromHome=1909, WayCostToHome=1909,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3203, Id=8,WayCostFromHome=1196, WayCostToHome=1196,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1597, Id=9,WayCostFromHome=299, WayCostToHome=299,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3196, Id=10,WayCostFromHome=347, WayCostToHome=347,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1033, WayCostToHome=1033,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1324, (0 * Hour + 1324) + 12204),(24 * Hour + 1324, (24 * Hour + 1324) + 12204)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=1930, WayCostToHome=1930,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 113, (0 * Hour + 113) + 17591),(24 * Hour + 113, (24 * Hour + 113) + 17591)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
