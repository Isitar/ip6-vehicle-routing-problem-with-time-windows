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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen2()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1814,2991),
		(2457,286),
		(2665,2718),
		(1165,979),
		(2793,2693),
		(2630,2446),
		(2770,3664),
		(1330,3483),
		(3274,2854),
		(1060,1223),
		(2784,2746),
		(1292,1411),
		(1743,2181),
		(2514,3130)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 2440, 1466, 2430, 2166, 3392, 3389, 2694, 1682, 2481, 1619, 2025, 2844 },
			{ 2440, 0, 2296, 130, 274, 951, 1538, 624, 2193, 122, 1895, 1066, 438 },
			{ 1466, 2296, 0, 2363, 2073, 3128, 2509, 2821, 265, 2396, 450, 1333, 2539 },
			{ 2430, 130, 2363, 0, 295, 971, 1662, 507, 2272, 53, 1973, 1168, 518 },
			{ 2166, 274, 2073, 295, 0, 1226, 1662, 762, 1990, 337, 1691, 925, 693 },
			{ 3392, 951, 3128, 971, 1226, 0, 1451, 954, 2980, 918, 2694, 1803, 592 },
			{ 3389, 1538, 2509, 1662, 1662, 1451, 0, 2043, 2276, 1630, 2072, 1365, 1235 },
			{ 2694, 624, 2821, 507, 762, 954, 2043, 0, 2749, 501, 2451, 1672, 808 },
			{ 1682, 2193, 265, 2272, 1990, 2980, 2276, 2749, 0, 2300, 298, 1176, 2398 },
			{ 2481, 122, 2396, 53, 337, 918, 1630, 501, 2300, 0, 2002, 1184, 469 },
			{ 1619, 1895, 450, 1973, 1691, 2694, 2072, 2451, 298, 2002, 0, 892, 2109 },
			{ 2025, 1066, 1333, 1168, 925, 1803, 1365, 1672, 1176, 1184, 892, 0, 1222 },
			{ 2844, 438, 2539, 518, 693, 592, 1235, 808, 2398, 469, 2109, 1222, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2476, Id=0,WayCostFromHome=2780, WayCostToHome=2780,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2680, Id=1,WayCostFromHome=893, WayCostToHome=893,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2167, Id=2,WayCostFromHome=2114, WayCostToHome=2114,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1537, Id=3,WayCostFromHome=1023, WayCostToHome=1023,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2010, Id=4,WayCostFromHome=981, WayCostToHome=981,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2583, Id=5,WayCostFromHome=1169, WayCostToHome=1169,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1836, Id=6,WayCostFromHome=690, WayCostToHome=690,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3580, Id=7,WayCostFromHome=1466, WayCostToHome=1466,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2126, Id=8,WayCostFromHome=1922, WayCostToHome=1922,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2624, Id=9,WayCostFromHome=1000, WayCostToHome=1000,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1768, Id=10,WayCostFromHome=1663, WayCostToHome=1663,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=813, WayCostToHome=813,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2424, (0 * Hour + 2424) + 12915),(24 * Hour + 2424, (24 * Hour + 2424) + 12915)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=713, WayCostToHome=713,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4480, (0 * Hour + 4480) + 12241),(24 * Hour + 4480, (24 * Hour + 4480) + 12241)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
