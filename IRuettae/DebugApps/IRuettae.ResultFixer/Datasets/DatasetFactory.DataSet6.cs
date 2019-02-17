using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 0 Desired, 10 Unavailable on day 0
/// 0 Desired, 10 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet6()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2574,2903),
		(1719,991),
		(2845,3034),
		(1316,1388),
		(1358,30),
		(2880,2465),
		(1550,845),
		(1592,1500),
		(1488,1655),
		(2394,2974),
		(2154,2791),
		(1610,1646),
		(2324,1180),
		(2979,2337),
		(2982,2745),
		(2362,2633),
		(3743,2910),
		(1021,1300),
		(188,3364),
		(2534,2718),
		(1113,1377)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2332, 565, 1026, 1876, 223, 524, 703, 2094, 1851, 664, 633, 1843, 2161, 1763, 2789, 763, 2824, 1909, 718 },
			{ 2332, 0, 2246, 3351, 570, 2543, 1980, 1934, 454, 732, 1857, 1925, 709, 319, 627, 906, 2516, 2677, 443, 2396 },
			{ 565, 2246, 0, 1358, 1898, 591, 297, 317, 1917, 1634, 391, 1029, 1914, 2148, 1626, 2864, 307, 2275, 1803, 203 },
			{ 1026, 3351, 1358, 0, 2871, 837, 1488, 1630, 3120, 2873, 1635, 1501, 2819, 3163, 2789, 3739, 1313, 3533, 2933, 1369 },
			{ 1876, 570, 1898, 2871, 0, 2096, 1609, 1610, 703, 795, 1511, 1400, 161, 298, 544, 970, 2193, 2838, 428, 2075 },
			{ 223, 2543, 591, 837, 2096, 0, 656, 812, 2290, 2037, 803, 843, 2065, 2379, 1963, 3012, 697, 2863, 2115, 688 },
			{ 524, 1980, 297, 1488, 1609, 656, 0, 186, 1678, 1408, 147, 798, 1619, 1866, 1369, 2571, 605, 2333, 1539, 494 },
			{ 703, 1934, 317, 1630, 1610, 812, 186, 0, 1600, 1316, 122, 961, 1639, 1849, 1311, 2580, 586, 2147, 1491, 466 },
			{ 2094, 454, 1917, 3120, 703, 2290, 1678, 1600, 0, 301, 1542, 1795, 864, 631, 342, 1350, 2165, 2240, 291, 2047 },
			{ 1851, 732, 1634, 2873, 795, 2037, 1408, 1316, 301, 0, 1267, 1619, 941, 829, 261, 1593, 1872, 2047, 386, 1755 },
			{ 664, 1857, 391, 1635, 1511, 803, 147, 122, 1542, 1267, 0, 852, 1533, 1757, 1240, 2479, 683, 2230, 1415, 565 },
			{ 633, 1925, 1029, 1501, 1400, 843, 798, 961, 1795, 1619, 852, 0, 1329, 1697, 1453, 2237, 1308, 3054, 1552, 1226 },
			{ 1843, 709, 1914, 2819, 161, 2065, 1619, 1639, 864, 941, 1533, 1329, 0, 408, 684, 955, 2215, 2973, 585, 2098 },
			{ 2161, 319, 2148, 3163, 298, 2379, 1866, 1849, 631, 829, 1757, 1697, 408, 0, 630, 778, 2435, 2861, 448, 2316 },
			{ 1763, 627, 1626, 2789, 544, 1963, 1369, 1311, 342, 261, 1240, 1453, 684, 630, 0, 1408, 1890, 2293, 191, 1771 },
			{ 2789, 906, 2864, 3739, 970, 3012, 2571, 2580, 1350, 1593, 2479, 2237, 955, 778, 1408, 0, 3162, 3583, 1224, 3044 },
			{ 763, 2516, 307, 1313, 2193, 697, 605, 586, 2165, 1872, 683, 1308, 2215, 2435, 1890, 3162, 0, 2225, 2073, 119 },
			{ 2824, 2677, 2275, 3533, 2838, 2863, 2333, 2147, 2240, 2047, 2230, 3054, 2973, 2861, 2293, 3583, 2225, 0, 2433, 2191 },
			{ 1909, 443, 1803, 2933, 428, 2115, 1539, 1491, 291, 386, 1415, 1552, 585, 448, 191, 1224, 2073, 2433, 0, 1953 },
			{ 718, 2396, 203, 1369, 2075, 688, 494, 466, 2047, 1755, 565, 1226, 2098, 2316, 1771, 3044, 119, 2191, 1953, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3370, Id=0,WayCostFromHome=2094, WayCostToHome=2094,Unavailable =new [] {(0 * Hour + 3374, (0 * Hour + 3374) + 28040)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1767, Id=1,WayCostFromHome=301, WayCostToHome=301,Unavailable =new [] {(0 * Hour + 1424, (0 * Hour + 1424) + 25155)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1364, Id=2,WayCostFromHome=1969, WayCostToHome=1969,Unavailable =new [] {(0 * Hour + 11954, (0 * Hour + 11954) + 6315)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1285, Id=3,WayCostFromHome=3119, WayCostToHome=3119,Unavailable =new [] {(0 * Hour + 6800, (0 * Hour + 6800) + 24644)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2677, Id=4,WayCostFromHome=534, WayCostToHome=534,Unavailable =new [] {(0 * Hour + 11556, (0 * Hour + 11556) + 18310)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1943, Id=5,WayCostFromHome=2298, WayCostToHome=2298,Unavailable =new [] {(0 * Hour + 9077, (0 * Hour + 9077) + 10975)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2766, Id=6,WayCostFromHome=1712, WayCostToHome=1712,Unavailable =new [] {(0 * Hour + 6875, (0 * Hour + 6875) + 22676)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1543, Id=7,WayCostFromHome=1654, WayCostToHome=1654,Unavailable =new [] {(0 * Hour + 1198, (0 * Hour + 1198) + 29218)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2632, Id=8,WayCostFromHome=193, WayCostToHome=193,Unavailable =new [] {(0 * Hour + 5709, (0 * Hour + 5709) + 25979)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3331, Id=9,WayCostFromHome=434, WayCostToHome=434,Unavailable =new [] {(0 * Hour + 8919, (0 * Hour + 8919) + 16132)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2213, Id=10,WayCostFromHome=1584, WayCostToHome=1584,Unavailable =new [] {(24 * Hour + 21600, (24 * Hour + 21600) + 6206)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1688, Id=11,WayCostFromHome=1741, WayCostToHome=1741,Unavailable =new [] {(24 * Hour + 5532, (24 * Hour + 5532) + 14445)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1458, Id=12,WayCostFromHome=695, WayCostToHome=695,Unavailable =new [] {(24 * Hour + 8735, (24 * Hour + 8735) + 20354)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1713, Id=13,WayCostFromHome=437, WayCostToHome=437,Unavailable =new [] {(24 * Hour + 7181, (24 * Hour + 7181) + 6568)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2649, Id=14,WayCostFromHome=343, WayCostToHome=343,Unavailable =new [] {(24 * Hour + 1033, (24 * Hour + 1033) + 28726)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2330, Id=15,WayCostFromHome=1169, WayCostToHome=1169,Unavailable =new [] {(24 * Hour + 4062, (24 * Hour + 4062) + 16358)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2250, Id=16,WayCostFromHome=2231, WayCostToHome=2231,Unavailable =new [] {(24 * Hour + 1617, (24 * Hour + 1617) + 26362)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1920, Id=17,WayCostFromHome=2430, WayCostToHome=2430,Unavailable =new [] {(24 * Hour + 8793, (24 * Hour + 8793) + 5082)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2737, Id=18,WayCostFromHome=189, WayCostToHome=189,Unavailable =new [] {(24 * Hour + 21796, (24 * Hour + 21796) + 2738)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3273, Id=19,WayCostFromHome=2112, WayCostToHome=2112,Unavailable =new [] {(24 * Hour + 5261, (24 * Hour + 5261) + 26720)},Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
