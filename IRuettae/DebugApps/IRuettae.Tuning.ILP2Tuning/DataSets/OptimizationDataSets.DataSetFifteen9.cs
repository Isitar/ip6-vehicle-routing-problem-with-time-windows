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
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSetFifteen9()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1485,1437),
		(1243,653),
		(1267,1700),
		(2089,1125),
		(1163,1756),
		(3279,2724),
		(1654,1083),
		(1671,1416),
		(1459,1336),
		(3465,1880),
		(1281,1578),
		(1719,2893),
		(2513,1833),
		(2613,2443)
	};
	const int workingDayDuration = 5 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1047, 968, 1105, 2904, 594, 874, 716, 2538, 925, 2290, 1733, 2254 },
			{ 1047, 0, 1003, 118, 2257, 728, 493, 411, 2205, 122, 1275, 1253, 1537 },
			{ 968, 1003, 0, 1120, 1993, 437, 509, 664, 1569, 926, 1806, 825, 1418 },
			{ 1105, 118, 1120, 0, 2326, 833, 611, 513, 2305, 213, 1265, 1352, 1604 },
			{ 2904, 2257, 1993, 2326, 0, 2309, 2072, 2288, 864, 2303, 1569, 1175, 722 },
			{ 594, 728, 437, 833, 2309, 0, 333, 319, 1978, 619, 1811, 1140, 1664 },
			{ 874, 493, 509, 611, 2072, 333, 0, 226, 1853, 422, 1477, 939, 1393 },
			{ 716, 411, 664, 513, 2288, 319, 226, 0, 2078, 300, 1578, 1165, 1599 },
			{ 2538, 2205, 1569, 2305, 864, 1978, 1853, 2078, 0, 2204, 2018, 953, 1021 },
			{ 925, 122, 926, 213, 2303, 619, 422, 300, 2204, 0, 1386, 1258, 1588 },
			{ 2290, 1275, 1806, 1265, 1569, 1811, 1477, 1578, 2018, 1386, 0, 1324, 1000 },
			{ 1733, 1253, 825, 1352, 1175, 1140, 939, 1165, 953, 1258, 1324, 0, 618 },
			{ 2254, 1537, 1418, 1604, 722, 1664, 1393, 1599, 1021, 1588, 1000, 618, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1332, Id=0,WayCostFromHome=820, WayCostToHome=820,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1784, Id=1,WayCostFromHome=341, WayCostToHome=341,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1205, Id=2,WayCostFromHome=679, WayCostToHome=679,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3547, Id=3,WayCostFromHome=453, WayCostToHome=453,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2609, Id=4,WayCostFromHome=2207, WayCostToHome=2207,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2957, Id=5,WayCostFromHome=392, WayCostToHome=392,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2102, Id=6,WayCostFromHome=187, WayCostToHome=187,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1906, Id=7,WayCostFromHome=104, WayCostToHome=104,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1392, Id=8,WayCostFromHome=2028, WayCostToHome=2028,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1489, Id=9,WayCostFromHome=247, WayCostToHome=247,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2951, Id=10,WayCostFromHome=1474, WayCostToHome=1474,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=11,WayCostFromHome=1101, WayCostToHome=1101,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1612, (0 * Hour + 1612) + 15473),(24 * Hour + 1612, (24 * Hour + 1612) + 15473)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=12,WayCostFromHome=1511, WayCostToHome=1511,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 3848, (0 * Hour + 3848) + 8380),(24 * Hour + 3848, (24 * Hour + 3848) + 8380)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
