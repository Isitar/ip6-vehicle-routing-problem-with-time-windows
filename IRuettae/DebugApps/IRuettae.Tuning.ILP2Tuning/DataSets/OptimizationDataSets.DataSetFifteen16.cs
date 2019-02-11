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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen16()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2327,2228),
		(3532,2712),
		(2396,2316),
		(1868,2684),
		(2732,2802),
		(1657,1472),
		(2274,2870),
		(3073,2919),
		(2143,1074),
		(1403,570),
		(2568,2418),
		(2595,2977),
		(1580,714),
		(2212,2141)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1203, 1664, 805, 2247, 1267, 503, 2147, 3020, 1007, 973, 2793, 1438 },
			{ 1203, 0, 643, 590, 1121, 567, 906, 1267, 2008, 199, 690, 1797, 253 },
			{ 1664, 643, 0, 872, 1230, 446, 1227, 1633, 2164, 748, 783, 1990, 642 },
			{ 805, 590, 872, 0, 1710, 463, 360, 1825, 2597, 417, 222, 2384, 841 },
			{ 2247, 1121, 1230, 1710, 0, 1528, 2024, 628, 937, 1313, 1773, 761, 869 },
			{ 1267, 567, 446, 463, 1528, 0, 800, 1800, 2459, 539, 338, 2264, 731 },
			{ 503, 906, 1227, 360, 2024, 800, 0, 2066, 2882, 711, 481, 2662, 1160 },
			{ 2147, 1267, 1633, 1825, 628, 1800, 2066, 0, 895, 1409, 1955, 668, 1069 },
			{ 3020, 2008, 2164, 2597, 937, 2459, 2882, 895, 0, 2184, 2685, 228, 1767 },
			{ 1007, 199, 748, 417, 1313, 539, 711, 1409, 2184, 0, 559, 1969, 451 },
			{ 973, 690, 783, 222, 1773, 338, 481, 1955, 2685, 559, 0, 2480, 919 },
			{ 2793, 1797, 1990, 2384, 761, 2264, 2662, 668, 228, 1969, 2480, 0, 1560 },
			{ 1438, 253, 642, 841, 869, 731, 1160, 1069, 1767, 451, 919, 1560, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2629, Id=0,WayCostFromHome=1298, WayCostToHome=1298,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2256, Id=1,WayCostFromHome=111, WayCostToHome=111,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2352, Id=2,WayCostFromHome=647, WayCostToHome=647,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1463, Id=3,WayCostFromHome=702, WayCostToHome=702,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2497, Id=4,WayCostFromHome=1010, WayCostToHome=1010,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3157, Id=5,WayCostFromHome=644, WayCostToHome=644,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1478, Id=6,WayCostFromHome=1016, WayCostToHome=1016,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3017, Id=7,WayCostFromHome=1168, WayCostToHome=1168,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3291, Id=8,WayCostFromHome=1898, WayCostToHome=1898,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2845, Id=9,WayCostFromHome=306, WayCostToHome=306,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2721, Id=10,WayCostFromHome=795, WayCostToHome=795,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1688, WayCostToHome=1688,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7636, (0 * Hour + 7636) + 6086),(24 * Hour + 7636, (24 * Hour + 7636) + 6086)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=144, WayCostToHome=144,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1834, (0 * Hour + 1834) + 5480),(24 * Hour + 1834, (24 * Hour + 1834) + 5480)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
