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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet1()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1386,1608),
		(2487,2288),
		(1882,1302),
		(2407,2300),
		(1763,1330),
		(2447,2484),
		(3651,2100),
		(806,2282),
		(2878,2465),
		(2415,3002),
		(1706,2023),
		(3291,2216),
		(3343,2975),
		(1335,1563),
		(2017,1249),
		(960,3101),
		(1918,1141),
		(2805,1648),
		(809,1778)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1156, 80, 1200, 200, 1179, 1681, 429, 717, 824, 807, 1097, 1361, 1140, 1729, 1280, 714, 1753 },
			{ 1156, 0, 1127, 122, 1310, 1940, 1455, 1531, 1781, 742, 1679, 2221, 606, 145, 2021, 164, 985, 1173 },
			{ 80, 1127, 0, 1164, 188, 1259, 1601, 499, 702, 753, 887, 1154, 1300, 1121, 1653, 1257, 763, 1681 },
			{ 1200, 122, 1164, 0, 1341, 2038, 1349, 1591, 1794, 695, 1766, 2280, 487, 266, 1944, 244, 1089, 1053 },
			{ 200, 1310, 188, 1341, 0, 1263, 1653, 431, 518, 872, 885, 1021, 1443, 1307, 1609, 1443, 909, 1783 },
			{ 1179, 1940, 1259, 2038, 1263, 0, 2850, 854, 1530, 1946, 378, 927, 2377, 1842, 2871, 1980, 959, 2860 },
			{ 1681, 1455, 1601, 1349, 1653, 2850, 0, 2080, 1762, 936, 2485, 2629, 892, 1591, 833, 1593, 2097, 504 },
			{ 429, 1531, 499, 1591, 431, 854, 2080, 0, 709, 1252, 482, 690, 1787, 1489, 2020, 1635, 820, 2180 },
			{ 717, 1781, 702, 1794, 518, 1530, 1762, 709, 0, 1208, 1176, 928, 1799, 1797, 1458, 1926, 1409, 2019 },
			{ 824, 742, 753, 695, 872, 1946, 936, 1252, 1208, 0, 1596, 1893, 590, 834, 1310, 907, 1161, 929 },
			{ 807, 1679, 887, 1766, 885, 378, 2485, 482, 1176, 1596, 0, 760, 2062, 1599, 2493, 1743, 747, 2520 },
			{ 1097, 2221, 1154, 2280, 1021, 927, 2629, 690, 928, 1893, 760, 0, 2454, 2176, 2386, 2322, 1431, 2802 },
			{ 1361, 606, 1300, 487, 1443, 2377, 892, 1787, 1799, 590, 2062, 2454, 0, 750, 1583, 719, 1472, 568 },
			{ 1140, 145, 1121, 266, 1307, 1842, 1591, 1489, 1797, 834, 1599, 2176, 750, 0, 2132, 146, 883, 1318 },
			{ 1729, 2021, 1653, 1944, 1609, 2871, 833, 2020, 1458, 1310, 2493, 2386, 1583, 2132, 0, 2181, 2348, 1331 },
			{ 1280, 164, 1257, 244, 1443, 1980, 1593, 1635, 1926, 907, 1743, 2322, 719, 146, 2181, 0, 1021, 1278 },
			{ 714, 985, 763, 1089, 909, 959, 2097, 820, 1409, 1161, 747, 1431, 1472, 883, 2348, 1021, 0, 2000 },
			{ 1753, 1173, 1681, 1053, 1783, 2860, 504, 2180, 2019, 929, 2520, 2802, 568, 1318, 1331, 1278, 2000, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3039, Id=0,WayCostFromHome=1294, WayCostToHome=1294,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1684, Id=1,WayCostFromHome=582, WayCostToHome=582,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3046, Id=2,WayCostFromHome=1233, WayCostToHome=1233,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2009, Id=3,WayCostFromHome=468, WayCostToHome=468,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2188, Id=4,WayCostFromHome=1375, WayCostToHome=1375,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1213, Id=5,WayCostFromHome=2317, WayCostToHome=2317,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1943, Id=6,WayCostFromHome=889, WayCostToHome=889,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1455, Id=7,WayCostFromHome=1720, WayCostToHome=1720,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1285, Id=8,WayCostFromHome=1732, WayCostToHome=1732,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1218, Id=9,WayCostFromHome=524, WayCostToHome=524,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3205, Id=10,WayCostFromHome=1999, WayCostToHome=1999,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2934, Id=11,WayCostFromHome=2387, WayCostToHome=2387,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3174, Id=12,WayCostFromHome=68, WayCostToHome=68,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1842, Id=13,WayCostFromHome=725, WayCostToHome=725,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2972, Id=14,WayCostFromHome=1552, WayCostToHome=1552,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2782, Id=15,WayCostFromHome=707, WayCostToHome=707,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=1419, WayCostToHome=1419,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4215, (0 * Hour + 4215) + 25046),(24 * Hour + 4215, (24 * Hour + 4215) + 25046)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=601, WayCostToHome=601,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1804, (0 * Hour + 1804) + 25170),(24 * Hour + 1804, (24 * Hour + 1804) + 25170)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
