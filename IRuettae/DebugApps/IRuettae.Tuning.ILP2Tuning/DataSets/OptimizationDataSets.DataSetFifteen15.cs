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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen15()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1455,2181),
		(3283,3101),
		(2808,2701),
		(2867,3201),
		(2062,3543),
		(1592,971),
		(1137,2513),
		(2728,2870),
		(2303,2829),
		(3101,3083),
		(2760,3279),
		(1612,1638),
		(3197,2788),
		(2558,1608)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 620, 427, 1298, 2719, 2225, 601, 1017, 182, 552, 2220, 324, 1659 },
			{ 620, 0, 503, 1124, 2114, 1681, 186, 520, 481, 579, 1600, 398, 1121 },
			{ 427, 503, 0, 874, 2568, 1861, 359, 675, 262, 132, 2004, 528, 1622 },
			{ 1298, 1124, 874, 0, 2614, 1384, 946, 753, 1136, 746, 1957, 1363, 1997 },
			{ 2719, 2114, 2568, 2614, 0, 1607, 2212, 1989, 2595, 2586, 667, 2424, 1157 },
			{ 2225, 1681, 1861, 1384, 1607, 0, 1630, 1208, 2045, 1794, 995, 2078, 1684 },
			{ 601, 186, 359, 946, 2212, 1630, 0, 426, 429, 410, 1662, 476, 1273 },
			{ 1017, 520, 675, 753, 1989, 1208, 426, 0, 837, 641, 1376, 894, 1247 },
			{ 182, 481, 262, 1136, 2595, 2045, 429, 837, 0, 393, 2074, 310, 1571 },
			{ 552, 579, 132, 746, 2586, 1794, 410, 641, 393, 0, 2002, 657, 1683 },
			{ 2220, 1600, 2004, 1957, 667, 995, 1662, 1376, 2074, 2002, 0, 1958, 946 },
			{ 324, 398, 528, 1363, 2424, 2078, 476, 894, 310, 657, 1958, 0, 1341 },
			{ 1659, 1121, 1622, 1997, 1157, 1684, 1273, 1247, 1571, 1683, 946, 1341, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3060, Id=0,WayCostFromHome=2046, WayCostToHome=2046,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1963, Id=1,WayCostFromHome=1449, WayCostToHome=1449,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1477, Id=2,WayCostFromHome=1741, WayCostToHome=1741,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2833, Id=3,WayCostFromHome=1491, WayCostToHome=1491,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3045, Id=4,WayCostFromHome=1217, WayCostToHome=1217,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2595, Id=5,WayCostFromHome=459, WayCostToHome=459,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1849, Id=6,WayCostFromHome=1447, WayCostToHome=1447,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1648, Id=7,WayCostFromHome=1067, WayCostToHome=1067,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1295, Id=8,WayCostFromHome=1876, WayCostToHome=1876,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2496, Id=9,WayCostFromHome=1705, WayCostToHome=1705,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1202, Id=10,WayCostFromHome=565, WayCostToHome=565,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1844, WayCostToHome=1844,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1299, (0 * Hour + 1299) + 3607),(24 * Hour + 1299, (24 * Hour + 1299) + 3607)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=1242, WayCostToHome=1242,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2911, (0 * Hour + 2911) + 14148),(24 * Hour + 2911, (24 * Hour + 2911) + 14148)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
