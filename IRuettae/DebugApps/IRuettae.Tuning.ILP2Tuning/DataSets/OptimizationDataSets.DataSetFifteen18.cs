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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen18()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(650,1459),
		(2610,3081),
		(1662,1385),
		(1479,1465),
		(2725,2210),
		(2908,2117),
		(429,959),
		(3080,2764),
		(2110,2566),
		(2339,3487),
		(2012,2341),
		(2404,2514),
		(3084,25),
		(3716,2694)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1942, 1972, 878, 1009, 3042, 566, 717, 488, 951, 603, 3092, 1171 },
			{ 1942, 0, 199, 1345, 1445, 1304, 1977, 1263, 2208, 1018, 1351, 1967, 2435 },
			{ 1972, 199, 0, 1451, 1570, 1165, 2061, 1269, 2197, 1025, 1398, 2156, 2552 },
			{ 878, 1345, 1451, 0, 205, 2614, 657, 710, 1334, 724, 442, 2214, 1102 },
			{ 1009, 1445, 1570, 205, 0, 2736, 669, 915, 1483, 923, 641, 2099, 992 },
			{ 3042, 1304, 1165, 2614, 2736, 0, 3207, 2325, 3168, 2101, 2513, 2814, 3716 },
			{ 566, 1977, 2061, 657, 669, 3207, 0, 990, 1035, 1148, 720, 2739, 639 },
			{ 717, 1263, 1269, 710, 915, 2325, 990, 0, 949, 245, 298, 2721, 1611 },
			{ 488, 2208, 2197, 1334, 1483, 3168, 1035, 949, 0, 1191, 975, 3541, 1589 },
			{ 951, 1018, 1025, 724, 923, 2101, 1148, 245, 1191, 0, 428, 2552, 1740 },
			{ 603, 1351, 1398, 442, 641, 2513, 720, 298, 975, 428, 0, 2580, 1324 },
			{ 3092, 1967, 2156, 2214, 2099, 2814, 2739, 2721, 3541, 2552, 2580, 0, 2742 },
			{ 1171, 2435, 2552, 1102, 992, 3716, 639, 1611, 1589, 1740, 1324, 2742, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3150, Id=0,WayCostFromHome=2544, WayCostToHome=2544,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1294, Id=1,WayCostFromHome=1014, WayCostToHome=1014,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2907, Id=2,WayCostFromHome=829, WayCostToHome=829,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2157, Id=3,WayCostFromHome=2206, WayCostToHome=2206,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1796, Id=4,WayCostFromHome=2351, WayCostToHome=2351,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2050, Id=5,WayCostFromHome=546, WayCostToHome=546,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2081, Id=6,WayCostFromHome=2758, WayCostToHome=2758,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2172, Id=7,WayCostFromHome=1832, WayCostToHome=1832,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1244, Id=8,WayCostFromHome=2639, WayCostToHome=2639,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3419, Id=9,WayCostFromHome=1622, WayCostToHome=1622,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3081, Id=10,WayCostFromHome=2046, WayCostToHome=2046,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=2825, WayCostToHome=2825,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 9600, (0 * Hour + 9600) + 8355),(24 * Hour + 9600, (24 * Hour + 9600) + 8355)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=3305, WayCostToHome=3305,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 14178, (0 * Hour + 14178) + 2795),(24 * Hour + 14178, (24 * Hour + 14178) + 2795)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
