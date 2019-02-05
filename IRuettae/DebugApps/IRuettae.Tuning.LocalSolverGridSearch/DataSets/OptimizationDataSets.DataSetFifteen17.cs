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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen17()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1741,1843),
		(1662,1549),
		(2952,3132),
		(2800,2362),
		(2765,2786),
		(2903,2762),
		(3223,2290),
		(2156,2477),
		(2567,2189),
		(1909,918),
		(2903,2627),
		(2481,2630),
		(2308,3217),
		(3327,3897)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2042, 1398, 1657, 1735, 1727, 1051, 1108, 677, 1643, 1356, 1788, 2878 },
			{ 2042, 0, 784, 393, 373, 884, 1030, 1018, 2447, 507, 688, 649, 851 },
			{ 1398, 784, 0, 425, 413, 429, 654, 290, 1696, 284, 416, 986, 1622 },
			{ 1657, 393, 425, 0, 140, 675, 682, 628, 2054, 210, 324, 628, 1245 },
			{ 1735, 373, 413, 140, 0, 570, 799, 664, 2094, 135, 442, 749, 1211 },
			{ 1727, 884, 429, 675, 570, 0, 1083, 663, 1899, 464, 816, 1302, 1610 },
			{ 1051, 1030, 654, 682, 799, 1083, 0, 501, 1578, 761, 359, 755, 1840 },
			{ 1108, 1018, 290, 628, 664, 663, 501, 0, 1431, 552, 449, 1060, 1869 },
			{ 677, 2447, 1696, 2054, 2094, 1899, 1578, 1431, 0, 1977, 1805, 2333, 3299 },
			{ 1643, 507, 284, 210, 135, 464, 761, 552, 1977, 0, 422, 837, 1338 },
			{ 1356, 688, 416, 324, 442, 816, 359, 449, 1805, 422, 0, 611, 1523 },
			{ 1788, 649, 986, 628, 749, 1302, 755, 1060, 2333, 837, 611, 0, 1225 },
			{ 2878, 851, 1622, 1245, 1211, 1610, 1840, 1869, 3299, 1338, 1523, 1225, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1763, Id=0,WayCostFromHome=304, WayCostToHome=304,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2368, Id=1,WayCostFromHome=1768, WayCostToHome=1768,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1211, Id=2,WayCostFromHome=1179, WayCostToHome=1179,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2173, Id=3,WayCostFromHome=1392, WayCostToHome=1392,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2276, Id=4,WayCostFromHome=1481, WayCostToHome=1481,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2230, Id=5,WayCostFromHome=1547, WayCostToHome=1547,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1507, Id=6,WayCostFromHome=757, WayCostToHome=757,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2888, Id=7,WayCostFromHome=895, WayCostToHome=895,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1287, Id=8,WayCostFromHome=940, WayCostToHome=940,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2860, Id=9,WayCostFromHome=1401, WayCostToHome=1401,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3518, Id=10,WayCostFromHome=1080, WayCostToHome=1080,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1486, WayCostToHome=1486,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 9206, (0 * Hour + 9206) + 7027),(24 * Hour + 9206, (24 * Hour + 9206) + 7027)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=2595, WayCostToHome=2595,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1088, (0 * Hour + 1088) + 10479),(24 * Hour + 1088, (24 * Hour + 1088) + 10479)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
