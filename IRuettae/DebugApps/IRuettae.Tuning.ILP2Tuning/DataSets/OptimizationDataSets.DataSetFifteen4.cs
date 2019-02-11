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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen4()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2728,2516),
		(1599,1109),
		(1968,1213),
		(2594,1415),
		(3124,3602),
		(1558,2214),
		(3323,2105),
		(2929,1948),
		(929,885),
		(2258,2973),
		(1134,1801),
		(2869,2737),
		(1152,494),
		(1532,1313)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 383, 1040, 2922, 1105, 1991, 1572, 706, 1977, 833, 2064, 760, 214 },
			{ 383, 0, 657, 2653, 1081, 1622, 1209, 1089, 1783, 1020, 1770, 1087, 447 },
			{ 1040, 657, 0, 2250, 1308, 1003, 629, 1747, 1593, 1510, 1350, 1711, 1066 },
			{ 2922, 2653, 2250, 0, 2092, 1510, 1665, 3492, 1070, 2683, 901, 3680, 2788 },
			{ 1105, 1081, 1308, 2092, 0, 1768, 1396, 1470, 1032, 591, 1411, 1767, 901 },
			{ 1991, 1622, 1003, 1510, 1768, 0, 424, 2686, 1373, 2210, 778, 2703, 1958 },
			{ 1572, 1209, 629, 1665, 1396, 424, 0, 2264, 1225, 1801, 791, 2296, 1534 },
			{ 706, 1089, 1747, 3492, 1470, 2686, 2264, 0, 2475, 938, 2682, 450, 739 },
			{ 1977, 1783, 1593, 1070, 1032, 1373, 1225, 2475, 0, 1623, 654, 2714, 1811 },
			{ 833, 1020, 1510, 2683, 591, 2210, 1801, 938, 1623, 0, 1971, 1307, 629 },
			{ 2064, 1770, 1350, 901, 1411, 778, 791, 2682, 654, 1971, 0, 2824, 1953 },
			{ 760, 1087, 1711, 3680, 1767, 2703, 2296, 450, 2714, 1307, 2824, 0, 902 },
			{ 214, 447, 1066, 2788, 901, 1958, 1534, 739, 1811, 629, 1953, 902, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3547, Id=0,WayCostFromHome=1803, WayCostToHome=1803,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1804, Id=1,WayCostFromHome=1508, WayCostToHome=1508,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2249, Id=2,WayCostFromHome=1109, WayCostToHome=1109,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1672, Id=3,WayCostFromHome=1155, WayCostToHome=1155,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2751, Id=4,WayCostFromHome=1208, WayCostToHome=1208,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1527, Id=5,WayCostFromHome=723, WayCostToHome=723,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1775, Id=6,WayCostFromHome=602, WayCostToHome=602,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1780, Id=7,WayCostFromHome=2428, WayCostToHome=2428,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1594, Id=8,WayCostFromHome=655, WayCostToHome=655,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2502, Id=9,WayCostFromHome=1747, WayCostToHome=1747,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1475, Id=10,WayCostFromHome=262, WayCostToHome=262,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=2563, WayCostToHome=2563,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2, (0 * Hour + 2) + 17996),(24 * Hour + 2, (24 * Hour + 2) + 17996)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=1696, WayCostToHome=1696,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4357, (0 * Hour + 4357) + 9827),(24 * Hour + 4357, (24 * Hour + 4357) + 9827)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
