using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 10 Visits, 2 Days, 1 Santas
/// 0 Desired, 5 Unavailable on day 0
/// 0 Desired, 5 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet3()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1510,1270),
		(1652,735),
		(631,912),
		(2206,2763),
		(2920,1469),
		(1624,1643),
		(2145,2316),
		(1737,841),
		(2212,2681),
		(1046,944),
		(2375,2530)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1036, 2102, 1465, 908, 1656, 135, 2024, 641, 1935 },
			{ 1036, 0, 2430, 2355, 1233, 2064, 1108, 2372, 416, 2378 },
			{ 2102, 2430, 0, 1477, 1262, 451, 1978, 82, 2157, 287 },
			{ 1465, 2355, 1477, 0, 1307, 1148, 1339, 1403, 1946, 1192 },
			{ 908, 1233, 1262, 1307, 0, 851, 809, 1192, 907, 1162 },
			{ 1656, 2064, 451, 1148, 851, 0, 1530, 371, 1757, 314 },
			{ 135, 1108, 1978, 1339, 809, 1530, 0, 1900, 698, 1805 },
			{ 2024, 2372, 82, 1403, 1192, 371, 1900, 0, 2092, 222 },
			{ 641, 416, 2157, 1946, 907, 1757, 698, 2092, 0, 2069 },
			{ 1935, 2378, 287, 1192, 1162, 314, 1805, 222, 2069, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 }
		},

		Visits = new[]
		{
			new Visit{Duration = 3263, Id=0,WayCostFromHome=553, WayCostToHome=553,Unavailable =new [] {(0 * Hour + 5424, (0 * Hour + 5424) + 11454)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2998, Id=1,WayCostFromHome=949, WayCostToHome=949,Unavailable =new [] {(0 * Hour + 12770, (0 * Hour + 12770) + 18031)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1292, Id=2,WayCostFromHome=1647, WayCostToHome=1647,Unavailable =new [] {(0 * Hour + 3622, (0 * Hour + 3622) + 28157)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1851, Id=3,WayCostFromHome=1423, WayCostToHome=1423,Unavailable =new [] {(0 * Hour + 5402, (0 * Hour + 5402) + 23712)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3429, Id=4,WayCostFromHome=390, WayCostToHome=390,Unavailable =new [] {(0 * Hour + 1443, (0 * Hour + 1443) + 29550)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2374, Id=5,WayCostFromHome=1223, WayCostToHome=1223,Unavailable =new [] {(24 * Hour + 12828, (24 * Hour + 12828) + 5083)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3550, Id=6,WayCostFromHome=485, WayCostToHome=485,Unavailable =new [] {(24 * Hour + 2025, (24 * Hour + 2025) + 27509)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2383, Id=7,WayCostFromHome=1575, WayCostToHome=1575,Unavailable =new [] {(24 * Hour + 10404, (24 * Hour + 10404) + 5264)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2130, Id=8,WayCostFromHome=567, WayCostToHome=567,Unavailable =new [] {(24 * Hour + 22, (24 * Hour + 22) + 20930)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3035, Id=9,WayCostFromHome=1528, WayCostToHome=1528,Unavailable =new [] {(24 * Hour + 4811, (24 * Hour + 4811) + 20016)},Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
