using IRuettae.Core.Models;
namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 15 Visits, 2 Days, 2 Santas
/// 4 Breaks, 13 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen13()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1534,1159),
		(2984,2828),
		(1690,381),
		(1777,908),
		(2150,3384),
		(2370,1667),
		(1994,2054),
		(1191,1609),
		(3547,3148),
		(2190,2890),
		(2553,2659),
		(3764,2077),
		(2481,2926),
		(1108,1175)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2768, 2267, 1002, 1313, 1256, 2168, 647, 796, 462, 1082, 512, 2500 },
			{ 2768, 0, 534, 3038, 1454, 1700, 1325, 3332, 2558, 2435, 2679, 2665, 984 },
			{ 2267, 534, 0, 2503, 963, 1166, 913, 2854, 2024, 1915, 2305, 2137, 720 },
			{ 1002, 3038, 2503, 0, 1731, 1339, 2017, 1416, 495, 829, 2076, 565, 2442 },
			{ 1313, 1454, 963, 1731, 0, 539, 1180, 1891, 1236, 1008, 1453, 1263, 1354 },
			{ 1256, 1700, 1166, 1339, 539, 0, 918, 1899, 858, 823, 1770, 998, 1248 },
			{ 2168, 1325, 913, 2017, 1180, 918, 0, 2814, 1624, 1719, 2615, 1843, 441 },
			{ 647, 3332, 2854, 1416, 1891, 1899, 2814, 0, 1381, 1107, 1092, 1088, 3137 },
			{ 796, 2558, 2024, 495, 1236, 858, 1624, 1381, 0, 430, 1771, 293, 2027 },
			{ 462, 2435, 1915, 829, 1008, 823, 1719, 1107, 430, 0, 1343, 276, 2071 },
			{ 1082, 2679, 2305, 2076, 1453, 1770, 2615, 1092, 1771, 1343, 0, 1538, 2804 },
			{ 512, 2665, 2137, 565, 1263, 998, 1843, 1088, 293, 276, 1538, 0, 2225 },
			{ 2500, 984, 720, 2442, 1354, 1248, 441, 3137, 2027, 2071, 2804, 2225, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3203, Id=0,WayCostFromHome=2210, WayCostToHome=2210,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1842, Id=1,WayCostFromHome=793, WayCostToHome=793,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1460, Id=2,WayCostFromHome=349, WayCostToHome=349,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3304, Id=3,WayCostFromHome=2308, WayCostToHome=2308,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1910, Id=4,WayCostFromHome=978, WayCostToHome=978,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2401, Id=5,WayCostFromHome=1006, WayCostToHome=1006,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3183, Id=6,WayCostFromHome=565, WayCostToHome=565,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1540, Id=7,WayCostFromHome=2829, WayCostToHome=2829,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3037, Id=8,WayCostFromHome=1851, WayCostToHome=1851,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1669, Id=9,WayCostFromHome=1813, WayCostToHome=1813,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2432, Id=10,WayCostFromHome=2411, WayCostToHome=2411,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=2004, WayCostToHome=2004,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 5826, (0 * Hour + 5826) + 2597),(24 * Hour + 5826, (24 * Hour + 5826) + 2597)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=426, WayCostToHome=426,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1825, (0 * Hour + 1825) + 13163),(24 * Hour + 1825, (24 * Hour + 1825) + 13163)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
