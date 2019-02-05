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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen8()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1354,1071),
		(2603,2948),
		(1356,2238),
		(2080,2772),
		(867,1295),
		(3171,2023),
		(1334,433),
		(1156,1203),
		(2923,2911),
		(2450,1688),
		(1608,537),
		(3209,2571),
		(1324,1004),
		(1364,959)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1434, 551, 2397, 1085, 2817, 2266, 322, 1269, 2608, 713, 2327, 2343 },
			{ 1434, 0, 899, 1062, 1827, 1805, 1054, 1705, 1224, 1719, 1882, 1234, 1279 },
			{ 551, 899, 0, 1911, 1323, 2455, 1820, 854, 1145, 2284, 1146, 1922, 1949 },
			{ 2397, 1062, 1911, 0, 2416, 980, 303, 2615, 1631, 1060, 2667, 541, 599 },
			{ 1085, 1827, 1323, 2416, 0, 2429, 2175, 921, 795, 2156, 549, 2109, 2096 },
			{ 2817, 1805, 2455, 980, 2429, 0, 790, 2943, 1679, 293, 2843, 571, 526 },
			{ 2266, 1054, 1820, 303, 2175, 790, 0, 2457, 1381, 804, 2467, 260, 320 },
			{ 322, 1705, 854, 2615, 921, 2943, 2457, 0, 1311, 2713, 444, 2488, 2498 },
			{ 1269, 1224, 1145, 1631, 795, 1679, 1381, 1311, 0, 1426, 1164, 1317, 1307 },
			{ 2608, 1719, 2284, 1060, 2156, 293, 804, 2713, 1426, 0, 2588, 546, 487 },
			{ 713, 1882, 1146, 2667, 549, 2843, 2467, 444, 1164, 2588, 0, 2451, 2450 },
			{ 2327, 1234, 1922, 541, 2109, 571, 260, 2488, 1317, 546, 2451, 0, 60 },
			{ 2343, 1279, 1949, 599, 2096, 526, 320, 2498, 1307, 487, 2450, 60, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2697, Id=0,WayCostFromHome=2254, WayCostToHome=2254,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3102, Id=1,WayCostFromHome=1167, WayCostToHome=1167,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1416, Id=2,WayCostFromHome=1849, WayCostToHome=1849,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2735, Id=3,WayCostFromHome=536, WayCostToHome=536,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2928, Id=4,WayCostFromHome=2051, WayCostToHome=2051,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3164, Id=5,WayCostFromHome=638, WayCostToHome=638,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2335, Id=6,WayCostFromHome=237, WayCostToHome=237,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3389, Id=7,WayCostFromHome=2418, WayCostToHome=2418,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1310, Id=8,WayCostFromHome=1257, WayCostToHome=1257,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2329, Id=9,WayCostFromHome=591, WayCostToHome=591,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2343, Id=10,WayCostFromHome=2385, WayCostToHome=2385,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=73, WayCostToHome=73,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 69, (0 * Hour + 69) + 16198),(24 * Hour + 69, (24 * Hour + 69) + 16198)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=112, WayCostToHome=112,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 285, (0 * Hour + 285) + 15929),(24 * Hour + 285, (24 * Hour + 285) + 15929)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
