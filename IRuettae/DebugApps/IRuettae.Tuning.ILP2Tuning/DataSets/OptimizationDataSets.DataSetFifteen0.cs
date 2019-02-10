using IRuettae.Core.Models;
namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 15 Visits, 2 Days, 2 Santas
/// 4 Breaks, 13 unique visits
/// 4 Desired, 2 Unavailable on day 0
/// 4 Desired, 4 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen0()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(674,723),
		(2060,2247),
		(2765,2762),
		(1454,1659),
		(1291,1209),
		(1269,1958),
		(1500,1229),
		(1439,1869),
		(1606,1852),
		(2256,1358),
		(2604,2366),
		(2176,2275),
		(1160,1653),
		(3305,2894)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 873, 844, 1291, 842, 1161, 726, 601, 910, 556, 119, 1078, 1403 },
			{ 873, 0, 1713, 2141, 1698, 1987, 1598, 1473, 1493, 427, 764, 1950, 555 },
			{ 844, 1713, 0, 478, 351, 432, 210, 245, 856, 1349, 949, 294, 2225 },
			{ 1291, 2141, 478, 0, 749, 209, 676, 716, 976, 1750, 1385, 462, 2625 },
			{ 842, 1698, 351, 749, 0, 764, 191, 353, 1155, 1395, 960, 323, 2240 },
			{ 1161, 1987, 432, 209, 764, 0, 642, 631, 766, 1584, 1245, 543, 2455 },
			{ 726, 1598, 210, 676, 191, 642, 0, 167, 963, 1266, 841, 352, 2128 },
			{ 601, 1473, 245, 716, 353, 631, 167, 0, 816, 1122, 709, 488, 1993 },
			{ 910, 1493, 856, 976, 1155, 766, 963, 816, 0, 1066, 920, 1135, 1860 },
			{ 556, 427, 1349, 1750, 1395, 1584, 1266, 1122, 1066, 0, 437, 1610, 877 },
			{ 119, 764, 949, 1385, 960, 1245, 841, 709, 920, 437, 0, 1191, 1287 },
			{ 1078, 1950, 294, 462, 323, 543, 352, 488, 1135, 1610, 1191, 0, 2478 },
			{ 1403, 555, 2225, 2625, 2240, 2455, 2128, 1993, 1860, 877, 1287, 2478, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3171, Id=0,WayCostFromHome=2059, WayCostToHome=2059,Unavailable =new [] {(24 * Hour + 8274, (24 * Hour + 8274) + 9633)},Desired = new [] {(0 * Hour + 8928, (0 * Hour + 8928) + 7214)}},
			new Visit{Duration = 2598, Id=1,WayCostFromHome=2920, WayCostToHome=2920,Unavailable =new [] {(24 * Hour + 2265, (24 * Hour + 2265) + 14519)},Desired = new [] {(0 * Hour + 226, (0 * Hour + 226) + 9405)}},
			new Visit{Duration = 1759, Id=2,WayCostFromHome=1218, WayCostToHome=1218,Unavailable =new [] {(24 * Hour + 2652, (24 * Hour + 2652) + 2745)},Desired = new [] {(0 * Hour + 1537, (0 * Hour + 1537) + 16028)}},
			new Visit{Duration = 1552, Id=3,WayCostFromHome=785, WayCostToHome=785,Unavailable =new [] {(24 * Hour + 7104, (24 * Hour + 7104) + 5728)},Desired = new [] {(0 * Hour + 5700, (0 * Hour + 5700) + 8319)}},
			new Visit{Duration = 1493, Id=4,WayCostFromHome=1370, WayCostToHome=1370,Unavailable =new [] {(0 * Hour + 2549, (0 * Hour + 2549) + 13325)},Desired = new [] {(24 * Hour + 4141, (24 * Hour + 4141) + 11216)}},
			new Visit{Duration = 2354, Id=5,WayCostFromHome=968, WayCostToHome=968,Unavailable =new [] {(0 * Hour + 1448, (0 * Hour + 1448) + 14534)},Desired = new [] {(24 * Hour + 7244, (24 * Hour + 7244) + 6089)}},
			new Visit{Duration = 2020, Id=6,WayCostFromHome=1377, WayCostToHome=1377,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3402, (24 * Hour + 3402) + 13394)}},
			new Visit{Duration = 1856, Id=7,WayCostFromHome=1463, WayCostToHome=1463,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 1081, (24 * Hour + 1081) + 15765)}},
			new Visit{Duration = 2973, Id=8,WayCostFromHome=1704, WayCostToHome=1704,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3168, Id=9,WayCostFromHome=2534, WayCostToHome=2534,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1850, Id=10,WayCostFromHome=2159, WayCostToHome=2159,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1049, WayCostToHome=1049,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3172, (0 * Hour + 3172) + 11564),(24 * Hour + 3172, (24 * Hour + 3172) + 11564)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=3411, WayCostToHome=3411,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1334, (0 * Hour + 1334) + 15585),(24 * Hour + 1334, (24 * Hour + 1334) + 15585)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
