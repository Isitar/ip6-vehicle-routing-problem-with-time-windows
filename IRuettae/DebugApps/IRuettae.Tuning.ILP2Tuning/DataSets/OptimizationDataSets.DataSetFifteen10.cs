using IRuettae.Core.Models;
namespace IRuettae.Tuning.ILP2Tuning.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 15 Visits, 2 Days, 2 Santas
/// 4 Breaks, 13 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen10()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1425,1205),
		(712,1237),
		(1775,1916),
		(1737,1169),
		(1387,1636),
		(979,2207),
		(2725,2719),
		(1506,1202),
		(2394,2783),
		(1091,644),
		(2069,1936),
		(986,1316),
		(2680,2958),
		(2594,3079)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1261, 1027, 784, 1006, 2499, 794, 2284, 703, 1526, 285, 2614, 2633 },
			{ 1261, 0, 747, 478, 847, 1243, 762, 1065, 1444, 294, 991, 1380, 1422 },
			{ 1027, 747, 0, 583, 1285, 1838, 233, 1742, 832, 835, 765, 2022, 2093 },
			{ 784, 478, 583, 0, 701, 1721, 450, 1526, 1035, 745, 513, 1849, 1881 },
			{ 1006, 847, 1285, 701, 0, 1819, 1134, 1527, 1567, 1123, 891, 1859, 1835 },
			{ 2499, 1243, 1838, 1721, 1819, 0, 1946, 337, 2641, 1021, 2234, 243, 383 },
			{ 794, 762, 233, 450, 1134, 1946, 0, 1813, 695, 925, 532, 2112, 2169 },
			{ 2284, 1065, 1742, 1526, 1527, 337, 1813, 0, 2504, 907, 2033, 335, 357 },
			{ 703, 1444, 832, 1035, 1567, 2641, 695, 2504, 0, 1620, 680, 2807, 2861 },
			{ 1526, 294, 835, 745, 1123, 1021, 925, 907, 1620, 0, 1247, 1190, 1257 },
			{ 285, 991, 765, 513, 891, 2234, 532, 2033, 680, 1247, 0, 2359, 2386 },
			{ 2614, 1380, 2022, 1849, 1859, 243, 2112, 335, 2807, 1190, 2359, 0, 148 },
			{ 2633, 1422, 2093, 1881, 1835, 383, 2169, 357, 2861, 1257, 2386, 148, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3394, Id=0,WayCostFromHome=713, WayCostToHome=713,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2537, Id=1,WayCostFromHome=792, WayCostToHome=792,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1602, Id=2,WayCostFromHome=314, WayCostToHome=314,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3547, Id=3,WayCostFromHome=432, WayCostToHome=432,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2014, Id=4,WayCostFromHome=1096, WayCostToHome=1096,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3577, Id=5,WayCostFromHome=1995, WayCostToHome=1995,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1745, Id=6,WayCostFromHome=81, WayCostToHome=81,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2337, Id=7,WayCostFromHome=1851, WayCostToHome=1851,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1519, Id=8,WayCostFromHome=652, WayCostToHome=652,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2099, Id=9,WayCostFromHome=974, WayCostToHome=974,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2149, Id=10,WayCostFromHome=452, WayCostToHome=452,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=2155, WayCostToHome=2155,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7614, (0 * Hour + 7614) + 10119),(24 * Hour + 7614, (24 * Hour + 7614) + 10119)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=2208, WayCostToHome=2208,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 48, (0 * Hour + 48) + 17952),(24 * Hour + 48, (24 * Hour + 48) + 17952)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
