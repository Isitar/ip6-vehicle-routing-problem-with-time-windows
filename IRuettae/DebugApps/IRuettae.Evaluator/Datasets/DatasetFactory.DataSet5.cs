using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 10 Desired, 0 Unavailable on day 0
/// 10 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet5()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2578,2611),
		(3369,2195),
		(1008,1795),
		(2095,2958),
		(749,1483),
		(1054,1683),
		(1706,1427),
		(1700,1151),
		(2820,2722),
		(1247,1313),
		(1465,1359),
		(3106,2814),
		(3362,2426),
		(1172,1173),
		(3059,2370),
		(1086,1484),
		(2043,907),
		(1363,1619),
		(3342,2605)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2394, 1485, 2715, 2370, 1831, 1968, 761, 2298, 2079, 672, 231, 2423, 355, 2391, 1848, 2087, 410 },
			{ 2394, 0, 1591, 405, 121, 789, 945, 2035, 538, 631, 2332, 2437, 643, 2130, 320, 1363, 396, 2470 },
			{ 1485, 1591, 0, 1996, 1645, 1579, 1849, 762, 1850, 1718, 1021, 1374, 2009, 1129, 1786, 2051, 1526, 1296 },
			{ 2715, 405, 1996, 0, 364, 958, 1007, 2413, 526, 726, 2706, 2777, 524, 2474, 337, 1416, 628, 2825 },
			{ 2370, 121, 1645, 364, 0, 700, 836, 2048, 417, 523, 2343, 2424, 523, 2119, 201, 1257, 315, 2466 },
			{ 1831, 789, 1579, 958, 700, 0, 276, 1708, 472, 250, 1970, 1933, 591, 1649, 622, 619, 393, 2015 },
			{ 1968, 945, 1849, 1007, 836, 276, 0, 1929, 481, 313, 2177, 2094, 528, 1825, 698, 420, 576, 2193 },
			{ 761, 2035, 762, 2413, 2048, 1708, 1929, 0, 2111, 1921, 300, 617, 2261, 425, 2130, 1974, 1827, 534 },
			{ 2298, 538, 1850, 526, 417, 472, 481, 2111, 0, 222, 2389, 2389, 158, 2097, 234, 893, 327, 2461 },
			{ 2079, 631, 1718, 726, 523, 250, 313, 1921, 222, 0, 2193, 2176, 347, 1887, 399, 733, 279, 2252 },
			{ 672, 2332, 1021, 2706, 2343, 1970, 2177, 300, 2389, 2193, 0, 464, 2536, 446, 2418, 2183, 2113, 315 },
			{ 231, 2437, 1374, 2777, 2424, 1933, 2094, 617, 2389, 2176, 464, 0, 2523, 308, 2463, 2011, 2155, 180 },
			{ 2423, 643, 2009, 524, 523, 591, 528, 2261, 158, 347, 2536, 2523, 0, 2234, 322, 910, 485, 2599 },
			{ 355, 2130, 1129, 2474, 2119, 1649, 1825, 425, 2097, 1887, 446, 308, 2234, 0, 2162, 1781, 1854, 367 },
			{ 2391, 320, 1786, 337, 201, 622, 698, 2130, 234, 399, 2418, 2463, 322, 2162, 0, 1117, 308, 2519 },
			{ 1848, 1363, 2051, 1416, 1257, 619, 420, 1974, 893, 733, 2183, 2011, 910, 1781, 1117, 0, 984, 2137 },
			{ 2087, 396, 1526, 628, 315, 393, 576, 1827, 327, 279, 2113, 2155, 485, 1854, 308, 984, 0, 2211 },
			{ 410, 2470, 1296, 2825, 2466, 2015, 2193, 534, 2461, 2252, 315, 180, 2599, 367, 2519, 2137, 2211, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2831, Id=0,WayCostFromHome=893, WayCostToHome=893,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 16673, (0 * Hour + 16673) + 14460)}},
			new Visit{Duration = 2257, Id=1,WayCostFromHome=1769, WayCostToHome=1769,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 323, (0 * Hour + 323) + 15893)}},
			new Visit{Duration = 2868, Id=2,WayCostFromHome=594, WayCostToHome=594,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 18208, (0 * Hour + 18208) + 6273)}},
			new Visit{Duration = 2844, Id=3,WayCostFromHome=2148, WayCostToHome=2148,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2888, (0 * Hour + 2888) + 19744)}},
			new Visit{Duration = 2271, Id=4,WayCostFromHome=1784, WayCostToHome=1784,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5399, (0 * Hour + 5399) + 15919)}},
			new Visit{Duration = 3404, Id=5,WayCostFromHome=1470, WayCostToHome=1470,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 20303, (0 * Hour + 20303) + 8703)}},
			new Visit{Duration = 2961, Id=6,WayCostFromHome=1703, WayCostToHome=1703,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 8994, (0 * Hour + 8994) + 17454)}},
			new Visit{Duration = 1305, Id=7,WayCostFromHome=266, WayCostToHome=266,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 7369, (0 * Hour + 7369) + 22379)}},
			new Visit{Duration = 2043, Id=8,WayCostFromHome=1859, WayCostToHome=1859,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2978, (0 * Hour + 2978) + 21930)}},
			new Visit{Duration = 2297, Id=9,WayCostFromHome=1675, WayCostToHome=1675,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 23358, (0 * Hour + 23358) + 5230)}},
			new Visit{Duration = 3598, Id=10,WayCostFromHome=565, WayCostToHome=565,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 6119, (24 * Hour + 6119) + 10510)}},
			new Visit{Duration = 1869, Id=11,WayCostFromHome=805, WayCostToHome=805,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 13576, (24 * Hour + 13576) + 14698)}},
			new Visit{Duration = 3490, Id=12,WayCostFromHome=2011, WayCostToHome=2011,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 205, (24 * Hour + 205) + 31781)}},
			new Visit{Duration = 1855, Id=13,WayCostFromHome=537, WayCostToHome=537,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 4419, (24 * Hour + 4419) + 13959)}},
			new Visit{Duration = 3136, Id=14,WayCostFromHome=1869, WayCostToHome=1869,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 4816, (24 * Hour + 4816) + 26462)}},
			new Visit{Duration = 1949, Id=15,WayCostFromHome=1786, WayCostToHome=1786,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 7937, (24 * Hour + 7937) + 5208)}}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=1568, WayCostToHome=1568,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5368, (0 * Hour + 5368) + 25863),(24 * Hour + 5368, (24 * Hour + 5368) + 25863)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=764, WayCostToHome=764,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 12309, (0 * Hour + 12309) + 15013),(24 * Hour + 12309, (24 * Hour + 12309) + 15013)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
