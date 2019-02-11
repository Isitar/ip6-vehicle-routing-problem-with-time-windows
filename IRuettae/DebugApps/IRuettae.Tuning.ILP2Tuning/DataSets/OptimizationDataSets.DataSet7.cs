using IRuettae.Core.Models;
namespace IRuettae.Tuning.ILP2Tuning.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet7()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2282,2799),
		(1505,1132),
		(3559,2685),
		(1912,1802),
		(2146,2747),
		(1364,1511),
		(936,1674),
		(1117,1887),
		(2853,3045),
		(383,1055),
		(3256,3215),
		(3289,2564),
		(1475,763),
		(2854,2110),
		(2491,2867),
		(3435,3346),
		(2365,1309),
		(1191,1884),
		(2757,2711)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2575, 783, 1737, 404, 785, 848, 2340, 1124, 2721, 2287, 370, 1666, 1995, 2937, 878, 814, 2015 },
			{ 2575, 0, 1868, 1414, 2489, 2811, 2569, 792, 3569, 610, 295, 2834, 909, 1083, 672, 1821, 2499, 802 },
			{ 783, 1868, 0, 973, 620, 984, 799, 1559, 1701, 1950, 1573, 1127, 991, 1212, 2168, 669, 725, 1241 },
			{ 1737, 1414, 973, 0, 1462, 1617, 1341, 767, 2443, 1204, 1157, 2094, 952, 365, 1421, 1454, 1287, 612 },
			{ 404, 2489, 620, 1462, 0, 457, 449, 2137, 1081, 2546, 2194, 756, 1605, 1763, 2766, 1021, 411, 1838 },
			{ 785, 2811, 984, 1617, 457, 0, 279, 2356, 830, 2785, 2515, 1058, 1966, 1959, 3006, 1474, 330, 2095 },
			{ 848, 2569, 799, 1341, 449, 279, 0, 2086, 1109, 2517, 2275, 1179, 1751, 1687, 2738, 1375, 74, 1835 },
			{ 2340, 792, 1559, 767, 2137, 2356, 2086, 0, 3171, 437, 649, 2665, 935, 403, 655, 1803, 2027, 347 },
			{ 1124, 3569, 1701, 2443, 1081, 830, 1109, 3171, 0, 3594, 3274, 1130, 2686, 2779, 3816, 1998, 1157, 2894 },
			{ 2721, 610, 1950, 1204, 2546, 2785, 2517, 437, 3594, 0, 651, 3030, 1175, 840, 221, 2103, 2456, 709 },
			{ 2287, 295, 1573, 1157, 2194, 2515, 2275, 649, 3274, 651, 0, 2556, 628, 853, 795, 1558, 2205, 551 },
			{ 370, 2834, 1127, 2094, 756, 1058, 1179, 2665, 1130, 3030, 2556, 0, 1927, 2336, 3242, 1044, 1156, 2332 },
			{ 1666, 909, 991, 952, 1605, 1966, 1751, 935, 2686, 1175, 628, 1927, 0, 839, 1365, 938, 1678, 608 },
			{ 1995, 1083, 1212, 365, 1763, 1959, 1687, 403, 2779, 840, 853, 2336, 839, 0, 1058, 1563, 1629, 308 },
			{ 2937, 672, 2168, 1421, 2766, 3006, 2738, 655, 3816, 221, 795, 3242, 1365, 1058, 0, 2300, 2678, 928 },
			{ 878, 1821, 669, 1454, 1021, 1474, 1375, 1803, 1998, 2103, 1558, 1044, 938, 1563, 2300, 0, 1307, 1455 },
			{ 814, 2499, 725, 1287, 411, 330, 74, 2027, 1157, 2456, 2205, 1156, 1678, 1629, 2678, 1307, 0, 1770 },
			{ 2015, 802, 1241, 612, 1838, 2095, 1835, 347, 2894, 709, 551, 2332, 608, 308, 928, 1455, 1770, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1606, Id=0,WayCostFromHome=1839, WayCostToHome=1839,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2699, Id=1,WayCostFromHome=1282, WayCostToHome=1282,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3425, Id=2,WayCostFromHome=1063, WayCostToHome=1063,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2825, Id=3,WayCostFromHome=145, WayCostToHome=145,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3596, Id=4,WayCostFromHome=1581, WayCostToHome=1581,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1944, Id=5,WayCostFromHome=1754, WayCostToHome=1754,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1430, Id=6,WayCostFromHome=1479, WayCostToHome=1479,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2881, Id=7,WayCostFromHome=621, WayCostToHome=621,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1645, Id=8,WayCostFromHome=2578, WayCostToHome=2578,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1407, Id=9,WayCostFromHome=1059, WayCostToHome=1059,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2046, Id=10,WayCostFromHome=1034, WayCostToHome=1034,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2820, Id=11,WayCostFromHome=2190, WayCostToHome=2190,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3258, Id=12,WayCostFromHome=895, WayCostToHome=895,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3015, Id=13,WayCostFromHome=219, WayCostToHome=219,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1582, Id=14,WayCostFromHome=1276, WayCostToHome=1276,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2511, Id=15,WayCostFromHome=1492, WayCostToHome=1492,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=1423, WayCostToHome=1423,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3629, (0 * Hour + 3629) + 25631),(24 * Hour + 3629, (24 * Hour + 3629) + 25631)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=483, WayCostToHome=483,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 20007, (0 * Hour + 20007) + 11306),(24 * Hour + 20007, (24 * Hour + 20007) + 11306)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
