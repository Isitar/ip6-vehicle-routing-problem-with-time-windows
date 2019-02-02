using IRuettae.Core.Models;
namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
internal partial class OptimizationDataSets
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 4 Breaks, 18 unique visits
/// 0 Desired, 0 Unavailable on day 0
/// 0 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet8()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(1033,2024),
		(2716,2403),
		(1281,1096),
		(3312,3234),
		(1430,3645),
		(1809,3012),
		(1228,1977),
		(2736,2731),
		(1239,1417),
		(3416,1681),
		(2104,2457),
		(2026,1000),
		(2851,2569),
		(2344,3091),
		(1317,1983),
		(1218,1765),
		(1081,2087),
		(1346,1260),
		(3957,453)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1940, 1022, 1787, 1092, 1547, 328, 1775, 1005, 614, 1563, 213, 782, 1460, 1628, 1665, 1784, 2311 },
			{ 1940, 0, 2948, 2553, 1987, 882, 2188, 323, 2213, 1590, 751, 2152, 2260, 887, 671, 1010, 176, 2752 },
			{ 1022, 2948, 0, 1926, 1519, 2433, 764, 2756, 1556, 1436, 2577, 809, 978, 2354, 2557, 2508, 2786, 2854 },
			{ 1787, 2553, 1926, 0, 737, 1680, 1594, 2236, 2793, 1365, 2711, 1782, 1068, 1665, 1891, 1596, 2386, 4071 },
			{ 1092, 1987, 1519, 737, 0, 1186, 968, 1693, 2086, 628, 2023, 1132, 540, 1140, 1379, 1177, 1812, 3341 },
			{ 1547, 882, 2433, 1680, 1186, 0, 1685, 560, 2207, 998, 1261, 1727, 1576, 89, 212, 183, 726, 3125 },
			{ 328, 2188, 764, 1594, 968, 1685, 0, 1991, 1250, 688, 1870, 198, 532, 1604, 1799, 1775, 2023, 2584 },
			{ 1775, 323, 2756, 2236, 1693, 560, 1991, 0, 2192, 1352, 890, 1981, 2005, 571, 348, 688, 189, 2883 },
			{ 1005, 2213, 1556, 2793, 2086, 2207, 1250, 2192, 0, 1524, 1547, 1052, 1771, 2120, 2199, 2370, 2112, 1341 },
			{ 614, 1590, 1436, 1365, 628, 998, 688, 1352, 1524, 0, 1459, 755, 677, 918, 1124, 1087, 1416, 2729 },
			{ 1563, 751, 2577, 2711, 2023, 1261, 1870, 890, 1547, 1459, 0, 1772, 2115, 1212, 1112, 1440, 728, 2006 },
			{ 213, 2152, 809, 1782, 1132, 1727, 198, 1981, 1052, 755, 1772, 0, 727, 1642, 1820, 1834, 1994, 2387 },
			{ 782, 2260, 978, 1068, 540, 1576, 532, 2005, 1771, 677, 2115, 727, 0, 1510, 1739, 1613, 2085, 3092 },
			{ 1460, 887, 2354, 1665, 1140, 89, 1604, 571, 2120, 918, 1212, 1642, 1510, 0, 239, 257, 723, 3051 },
			{ 1628, 671, 2557, 1891, 1379, 212, 1799, 348, 2199, 1124, 1112, 1820, 1739, 239, 0, 349, 520, 3037 },
			{ 1665, 1010, 2508, 1596, 1177, 183, 1775, 688, 2370, 1087, 1440, 1834, 1613, 257, 349, 0, 868, 3307 },
			{ 1784, 176, 2786, 2386, 1812, 726, 2023, 189, 2112, 1416, 728, 1994, 2085, 723, 520, 868, 0, 2732 },
			{ 2311, 2752, 2854, 4071, 3341, 3125, 2584, 2883, 1341, 2729, 2006, 2387, 3092, 3051, 3037, 3307, 2732, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1635, Id=0,WayCostFromHome=1725, WayCostToHome=1725,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1833, Id=1,WayCostFromHome=960, WayCostToHome=960,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3595, Id=2,WayCostFromHome=2580, WayCostToHome=2580,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2081, Id=3,WayCostFromHome=1668, WayCostToHome=1668,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2098, Id=4,WayCostFromHome=1256, WayCostToHome=1256,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1700, Id=5,WayCostFromHome=200, WayCostToHome=200,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2534, Id=6,WayCostFromHome=1843, WayCostToHome=1843,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2326, Id=7,WayCostFromHome=641, WayCostToHome=641,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3454, Id=8,WayCostFromHome=2407, WayCostToHome=2407,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3238, Id=9,WayCostFromHome=1155, WayCostToHome=1155,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1655, Id=10,WayCostFromHome=1426, WayCostToHome=1426,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2067, Id=11,WayCostFromHome=1897, WayCostToHome=1897,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2972, Id=12,WayCostFromHome=1690, WayCostToHome=1690,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 3094, Id=13,WayCostFromHome=286, WayCostToHome=286,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 1822, Id=14,WayCostFromHome=318, WayCostToHome=318,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]},
			new Visit{Duration = 2721, Id=15,WayCostFromHome=79, WayCostToHome=79,Unavailable =new (int from, int to)[0],Desired = new (int from, int to)[0]}
,			new Visit{Duration = 1800, Id=16,WayCostFromHome=825, WayCostToHome=825,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 9328, (0 * Hour + 9328) + 14048),(24 * Hour + 9328, (24 * Hour + 9328) + 14048)},SantaId=0,IsBreak = true},
			new Visit{Duration = 1800, Id=17,WayCostFromHome=3319, WayCostToHome=3319,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 801, (0 * Hour + 801) + 31104),(24 * Hour + 801, (24 * Hour + 801) + 31104)},SantaId=1,IsBreak = true}
		}
	};
	return (input, coordinates);
}
}
}
