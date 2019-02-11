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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen14()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1021,1265),
		(2470,2217),
		(2408,2951),
		(1377,1118),
		(818,623),
		(1438,1581),
		(1165,1467),
		(2175,2458),
		(2512,3729),
		(1317,1417),
		(1402,841),
		(2937,3174),
		(1465,1790),
		(1287,1357)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 736, 1549, 2295, 1212, 1505, 380, 1512, 1403, 1741, 1064, 1091, 1462 },
			{ 736, 0, 2103, 2819, 1678, 1935, 545, 784, 1882, 2337, 574, 1495, 1948 },
			{ 1549, 2103, 0, 746, 467, 408, 1559, 2847, 304, 278, 2580, 677, 255 },
			{ 2295, 2819, 746, 0, 1141, 912, 2282, 3537, 937, 623, 3316, 1334, 871 },
			{ 1212, 1678, 467, 1141, 0, 295, 1145, 2401, 203, 740, 2187, 210, 270 },
			{ 1505, 1935, 408, 912, 295, 0, 1414, 2632, 160, 669, 2460, 440, 164 },
			{ 380, 545, 1559, 2282, 1145, 1414, 0, 1314, 1349, 1792, 1045, 974, 1414 },
			{ 1512, 784, 2847, 3537, 2401, 2632, 1314, 0, 2602, 3093, 699, 2203, 2669 },
			{ 1403, 1882, 304, 937, 203, 160, 1349, 2602, 0, 582, 2389, 401, 67 },
			{ 1741, 2337, 278, 623, 740, 669, 1792, 3093, 582, 0, 2792, 951, 528 },
			{ 1064, 574, 2580, 3316, 2187, 2460, 1045, 699, 2389, 2792, 0, 2020, 2454 },
			{ 1091, 1495, 677, 1334, 210, 440, 974, 2203, 401, 951, 2020, 0, 468 },
			{ 1462, 1948, 255, 871, 270, 164, 1414, 2669, 67, 528, 2454, 468, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3198, Id=0,WayCostFromHome=1733, WayCostToHome=1733,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1636, Id=1,WayCostFromHome=2183, WayCostToHome=2183,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1296, Id=2,WayCostFromHome=385, WayCostToHome=385,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1267, Id=3,WayCostFromHome=673, WayCostToHome=673,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2246, Id=4,WayCostFromHome=523, WayCostToHome=523,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1337, Id=5,WayCostFromHome=248, WayCostToHome=248,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1591, Id=6,WayCostFromHome=1659, WayCostToHome=1659,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3335, Id=7,WayCostFromHome=2879, WayCostToHome=2879,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1530, Id=8,WayCostFromHome=332, WayCostToHome=332,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2130, Id=9,WayCostFromHome=570, WayCostToHome=570,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3324, Id=10,WayCostFromHome=2704, WayCostToHome=2704,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=687, WayCostToHome=687,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 958, (0 * Hour + 958) + 4087),(24 * Hour + 958, (24 * Hour + 958) + 4087)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=281, WayCostToHome=281,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 6117, (0 * Hour + 6117) + 5094),(24 * Hour + 6117, (24 * Hour + 6117) + 5094)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
