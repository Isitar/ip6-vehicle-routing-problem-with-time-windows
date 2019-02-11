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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen3()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3122,2513),
		(3164,2985),
		(1314,1416),
		(2131,2195),
		(1106,1244),
		(946,881),
		(2294,2441),
		(1264,1322),
		(2230,2678),
		(2127,2589),
		(1747,1322),
		(2922,2269),
		(561,1479),
		(2528,2642)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2425, 1300, 2695, 3057, 1026, 2524, 983, 1110, 2184, 755, 3007, 722 },
			{ 2425, 0, 1128, 269, 649, 1418, 106, 1559, 1427, 443, 1820, 755, 1725 },
			{ 1300, 1128, 0, 1398, 1769, 295, 1230, 493, 394, 953, 794, 1725, 597 },
			{ 2695, 269, 1398, 0, 396, 1686, 176, 1822, 1688, 645, 2085, 593, 1994 },
			{ 3057, 649, 1769, 396, 0, 2061, 543, 2208, 2076, 914, 2414, 711, 2367 },
			{ 1026, 1418, 295, 1686, 2061, 0, 1520, 245, 223, 1245, 651, 1982, 308 },
			{ 2524, 106, 1230, 176, 543, 1520, 0, 1664, 1532, 483, 1909, 720, 1827 },
			{ 983, 1559, 493, 1822, 2208, 245, 1664, 0, 136, 1439, 803, 2055, 300 },
			{ 1110, 1427, 394, 1688, 2076, 223, 1532, 136, 0, 1322, 856, 1919, 404 },
			{ 2184, 443, 953, 645, 914, 1245, 483, 1439, 1322, 0, 1509, 1196, 1533 },
			{ 755, 1820, 794, 2085, 2414, 651, 1909, 803, 856, 1509, 0, 2489, 542 },
			{ 3007, 755, 1725, 593, 711, 1982, 720, 2055, 1919, 1196, 2489, 0, 2285 },
			{ 722, 1725, 597, 1994, 2367, 308, 1827, 300, 404, 1533, 542, 2285, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1653, Id=0,WayCostFromHome=473, WayCostToHome=473,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3472, Id=1,WayCostFromHome=2114, WayCostToHome=2114,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1441, Id=2,WayCostFromHome=1040, WayCostToHome=1040,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1378, Id=3,WayCostFromHome=2382, WayCostToHome=2382,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1875, Id=4,WayCostFromHome=2720, WayCostToHome=2720,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1658, Id=5,WayCostFromHome=831, WayCostToHome=831,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2830, Id=6,WayCostFromHome=2206, WayCostToHome=2206,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2237, Id=7,WayCostFromHome=907, WayCostToHome=907,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2565, Id=8,WayCostFromHome=997, WayCostToHome=997,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2675, Id=9,WayCostFromHome=1819, WayCostToHome=1819,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1291, Id=10,WayCostFromHome=315, WayCostToHome=315,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=2761, WayCostToHome=2761,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 6539, (0 * Hour + 6539) + 4935),(24 * Hour + 6539, (24 * Hour + 6539) + 4935)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=607, WayCostToHome=607,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1206, (0 * Hour + 1206) + 6840),(24 * Hour + 1206, (24 * Hour + 1206) + 6840)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
