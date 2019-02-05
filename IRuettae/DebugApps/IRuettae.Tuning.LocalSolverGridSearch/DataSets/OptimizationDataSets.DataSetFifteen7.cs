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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen7()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3781,879),
		(1550,2361),
		(2668,2705),
		(3752,2867),
		(1714,1151),
		(3127,2837),
		(3534,3417),
		(2813,2667),
		(3381,2565),
		(2556,2482),
		(1933,669),
		(2782,1829),
		(1401,1414),
		(2320,2951)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1169, 2259, 1221, 1647, 2247, 1299, 1842, 1013, 1734, 1341, 958, 970 },
			{ 1169, 0, 1096, 1823, 477, 1121, 149, 726, 249, 2164, 883, 1808, 426 },
			{ 2259, 1096, 0, 2664, 625, 591, 960, 478, 1256, 2853, 1420, 2763, 1434 },
			{ 1221, 1823, 2664, 0, 2199, 2906, 1872, 2185, 1574, 529, 1265, 408, 1899 },
			{ 1647, 477, 625, 2199, 0, 708, 357, 372, 672, 2475, 1065, 2236, 815 },
			{ 2247, 1121, 591, 2906, 708, 0, 1040, 865, 1353, 3180, 1757, 2926, 1300 },
			{ 1299, 149, 960, 1872, 357, 1040, 0, 577, 316, 2183, 838, 1887, 568 },
			{ 1842, 726, 478, 2185, 372, 865, 577, 0, 829, 2385, 948, 2290, 1129 },
			{ 1013, 249, 1256, 1574, 672, 1353, 316, 829, 0, 1917, 691, 1573, 525 },
			{ 1734, 2164, 2853, 529, 2475, 3180, 2183, 2385, 1917, 0, 1437, 915, 2314 },
			{ 1341, 883, 1420, 1265, 1065, 1757, 838, 948, 691, 1437, 0, 1442, 1213 },
			{ 958, 1808, 2763, 408, 2236, 2926, 1887, 2290, 1573, 915, 1442, 0, 1790 },
			{ 970, 426, 1434, 1899, 815, 1300, 568, 1129, 525, 2314, 1213, 1790, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3575, Id=0,WayCostFromHome=2678, WayCostToHome=2678,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3084, Id=1,WayCostFromHome=2138, WayCostToHome=2138,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1319, Id=2,WayCostFromHome=1988, WayCostToHome=1988,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3430, Id=3,WayCostFromHome=2084, WayCostToHome=2084,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2339, Id=4,WayCostFromHome=2064, WayCostToHome=2064,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2984, Id=5,WayCostFromHome=2549, WayCostToHome=2549,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1736, Id=6,WayCostFromHome=2033, WayCostToHome=2033,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2617, Id=7,WayCostFromHome=1732, WayCostToHome=1732,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2055, Id=8,WayCostFromHome=2017, WayCostToHome=2017,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1604, Id=9,WayCostFromHome=1859, WayCostToHome=1859,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2123, Id=10,WayCostFromHome=1378, WayCostToHome=1378,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=2439, WayCostToHome=2439,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1854, (0 * Hour + 1854) + 6333),(24 * Hour + 1854, (24 * Hour + 1854) + 6333)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=2535, WayCostToHome=2535,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3294, (0 * Hour + 3294) + 9116),(24 * Hour + 3294, (24 * Hour + 3294) + 9116)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
