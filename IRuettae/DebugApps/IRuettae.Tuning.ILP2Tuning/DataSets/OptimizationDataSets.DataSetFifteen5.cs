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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen5()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(2989,2683),
		(1068,1176),
		(2083,2048),
		(2645,2963),
		(2558,2589),
		(948,1164),
		(1362,1719),
		(2884,2486),
		(2444,2908),
		(1087,1315),
		(2639,3197),
		(1365,886),
		(1290,1684),
		(1646,1417)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1338, 2383, 2053, 120, 617, 2239, 2212, 140, 2559, 415, 554, 626 },
			{ 1338, 0, 1073, 719, 1438, 792, 912, 932, 1236, 1276, 1365, 872, 767 },
			{ 2383, 1073, 0, 383, 2473, 1787, 533, 208, 2267, 234, 2439, 1863, 1840 },
			{ 2053, 719, 383, 0, 2150, 1478, 341, 338, 1946, 613, 2079, 1557, 1485 },
			{ 120, 1438, 2473, 2150, 0, 692, 2344, 2297, 205, 2644, 501, 622, 742 },
			{ 617, 792, 1787, 1478, 692, 0, 1704, 1607, 488, 1953, 833, 80, 414 },
			{ 2239, 912, 533, 341, 2344, 1704, 0, 609, 2144, 752, 2206, 1784, 1635 },
			{ 2212, 932, 208, 338, 2297, 1607, 609, 0, 2092, 348, 2291, 1682, 1691 },
			{ 140, 1236, 2267, 1946, 205, 488, 2144, 2092, 0, 2439, 511, 421, 568 },
			{ 2559, 1276, 234, 613, 2644, 1953, 752, 348, 2439, 0, 2638, 2027, 2038 },
			{ 415, 1365, 2439, 2079, 501, 833, 2206, 2291, 511, 2638, 0, 801, 600 },
			{ 554, 872, 1863, 1557, 622, 80, 1784, 1682, 421, 2027, 801, 0, 445 },
			{ 626, 767, 1840, 1485, 742, 414, 1635, 1691, 568, 2038, 600, 445, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2530, Id=0,WayCostFromHome=2441, WayCostToHome=2441,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1349, Id=1,WayCostFromHome=1106, WayCostToHome=1106,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2380, Id=2,WayCostFromHome=443, WayCostToHome=443,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1788, Id=3,WayCostFromHome=441, WayCostToHome=441,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2318, Id=4,WayCostFromHome=2544, WayCostToHome=2544,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3426, Id=5,WayCostFromHome=1891, WayCostToHome=1891,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1561, Id=6,WayCostFromHome=223, WayCostToHome=223,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3150, Id=7,WayCostFromHome=589, WayCostToHome=589,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1454, Id=8,WayCostFromHome=2342, WayCostToHome=2342,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1773, Id=9,WayCostFromHome=621, WayCostToHome=621,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2656, Id=10,WayCostFromHome=2422, WayCostToHome=2422,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1970, WayCostToHome=1970,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 9009, (0 * Hour + 9009) + 6905),(24 * Hour + 9009, (24 * Hour + 9009) + 6905)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=1845, WayCostToHome=1845,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1281, (0 * Hour + 1281) + 15795),(24 * Hour + 1281, (24 * Hour + 1281) + 15795)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
