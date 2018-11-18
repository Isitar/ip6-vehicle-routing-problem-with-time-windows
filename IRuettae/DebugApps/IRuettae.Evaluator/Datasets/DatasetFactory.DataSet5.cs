using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
internal partial class DatasetFactory
{
/// <summary>
/// 20 Visits, 2 Days, 2 Santas
/// 10 Desired, 0 Unavailable on day 0
/// 10 Desired, 0 Unavailable on day 1
/// </summary>
public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet5()
{
	// Coordinates of points
	var coordinates = new[]
	{
		(3011,3165),
		(2328,2020),
		(1781,1171),
		(1987,1590),
		(2533,2146),
		(3706,3826),
		(1879,1663),
		(2424,3263),
		(1253,1328),
		(2649,2699),
		(741,921),
		(2660,2944),
		(1198,891),
		(3189,2905),
		(3048,2579),
		(2051,1839),
		(963,1113),
		(814,1258),
		(1828,1454),
		(1294,1181),
		(2917,3920)
	};
	const int workingDayDuration = 9 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 1009, 548, 240, 2271, 573, 1246, 1278, 751, 1930, 981, 1597, 1234, 911, 330, 1638, 1694, 755, 1331, 1989 },
			{ 1009, 0, 466, 1231, 3279, 501, 2188, 550, 1757, 1069, 1978, 646, 2233, 1894, 720, 820, 970, 286, 487, 2974 },
			{ 548, 466, 0, 779, 2820, 130, 1729, 779, 1291, 1414, 1512, 1054, 1781, 1450, 257, 1129, 1219, 209, 804, 2508 },
			{ 240, 1231, 779, 0, 2048, 813, 1122, 1519, 565, 2170, 808, 1832, 1003, 672, 571, 1879, 1934, 987, 1570, 1815 },
			{ 2271, 3279, 2820, 2048, 0, 2831, 1400, 3501, 1545, 4150, 1368, 3860, 1056, 1409, 2585, 3858, 3867, 3025, 3579, 794 },
			{ 573, 501, 130, 813, 2831, 0, 1690, 710, 1290, 1358, 1500, 1029, 1805, 1485, 246, 1068, 1139, 215, 757, 2484 },
			{ 1246, 2188, 1729, 1122, 1400, 1690, 0, 2261, 607, 2883, 396, 2670, 844, 925, 1472, 2599, 2571, 1904, 2368, 821 },
			{ 1278, 550, 779, 1519, 3501, 710, 2261, 0, 1956, 654, 2142, 440, 2497, 2187, 947, 361, 444, 588, 152, 3080 },
			{ 751, 1757, 1291, 565, 1545, 1290, 607, 1956, 0, 2608, 245, 2318, 577, 416, 1047, 2314, 2333, 1491, 2034, 1250 },
			{ 1930, 1069, 1414, 2170, 4150, 1358, 2883, 654, 2608, 0, 2788, 457, 3151, 2840, 1599, 293, 344, 1210, 611, 3705 },
			{ 981, 1978, 1512, 808, 1368, 1500, 396, 2142, 245, 2788, 0, 2520, 530, 532, 1261, 2496, 2500, 1706, 2230, 1009 },
			{ 1597, 646, 1054, 1832, 3860, 1029, 2670, 440, 2318, 457, 2520, 0, 2832, 2504, 1275, 323, 531, 844, 305, 3482 },
			{ 1234, 2233, 1781, 1003, 1056, 1805, 844, 2497, 577, 3151, 530, 2832, 0, 355, 1559, 2857, 2890, 1989, 2561, 1050 },
			{ 911, 1894, 1450, 672, 1409, 1485, 925, 2187, 416, 2840, 532, 2504, 355, 0, 1241, 2548, 2595, 1659, 2242, 1347 },
			{ 330, 720, 257, 571, 2585, 246, 1472, 947, 1047, 1599, 1261, 1275, 1559, 1241, 0, 1307, 1366, 444, 1003, 2254 },
			{ 1638, 820, 1129, 1879, 3858, 1068, 2599, 361, 2314, 293, 2496, 323, 2857, 2548, 1307, 0, 207, 929, 337, 3420 },
			{ 1694, 970, 1219, 1934, 3867, 1139, 2571, 444, 2333, 344, 2500, 531, 2890, 2595, 1366, 207, 0, 1032, 486, 3392 },
			{ 755, 286, 209, 987, 3025, 215, 1904, 588, 1491, 1210, 1706, 844, 1989, 1659, 444, 929, 1032, 0, 599, 2695 },
			{ 1331, 487, 804, 1570, 3579, 757, 2368, 152, 2034, 611, 2230, 305, 2561, 2242, 1003, 337, 486, 599, 0, 3183 },
			{ 1989, 2974, 2508, 1815, 794, 2484, 821, 3080, 1250, 3705, 1009, 3482, 1050, 1347, 2254, 3420, 3392, 2695, 3183, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 },
			new Santa { Id = 1 }
		},

		Visits = new[]
		{
			new Visit{Duration = 1431, Id=0,WayCostFromHome=1333, WayCostToHome=1333,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2707.40402003901, (0 * Hour + 2707.40402003901) + 27518.513464012)}},
			new Visit{Duration = 3203, Id=1,WayCostFromHome=2342, WayCostToHome=2342,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1079.24899611091, (0 * Hour + 1079.24899611091) + 25024.4399051785)}},
			new Visit{Duration = 3046, Id=2,WayCostFromHome=1878, WayCostToHome=1878,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 2635.23353159021, (0 * Hour + 2635.23353159021) + 28323.9044241886)}},
			new Visit{Duration = 3254, Id=3,WayCostFromHome=1125, WayCostToHome=1125,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 1540.10949959056, (0 * Hour + 1540.10949959056) + 16499.8299460215)}},
			new Visit{Duration = 1656, Id=4,WayCostFromHome=959, WayCostToHome=959,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4406.5984144967, (0 * Hour + 4406.5984144967) + 23307.4207776764)}},
			new Visit{Duration = 1224, Id=5,WayCostFromHome=1880, WayCostToHome=1880,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 12208.5442857639, (0 * Hour + 12208.5442857639) + 8902.04259085937)}},
			new Visit{Duration = 1944, Id=6,WayCostFromHome=595, WayCostToHome=595,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 11335.7822318883, (0 * Hour + 11335.7822318883) + 15243.0585080995)}},
			new Visit{Duration = 3337, Id=7,WayCostFromHome=2542, WayCostToHome=2542,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 9835.18358305828, (0 * Hour + 9835.18358305828) + 21437.5009907039)}},
			new Visit{Duration = 2935, Id=8,WayCostFromHome=590, WayCostToHome=590,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 4169.09898236068, (0 * Hour + 4169.09898236068) + 27765.8391024409)}},
			new Visit{Duration = 3594, Id=9,WayCostFromHome=3191, WayCostToHome=3191,Unavailable =new (int from, int to)[0],Desired = new [] {(0 * Hour + 758.86608293408, (0 * Hour + 758.86608293408) + 29622.9260339173)}},
			new Visit{Duration = 3292, Id=10,WayCostFromHome=414, WayCostToHome=414,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 73.1739746963937, (24 * Hour + 73.1739746963937) + 31259.1454404682)}},
			new Visit{Duration = 1630, Id=11,WayCostFromHome=2908, WayCostToHome=2908,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 5779.66022046838, (24 * Hour + 5779.66022046838) + 16539.806760447)}},
			new Visit{Duration = 1648, Id=12,WayCostFromHome=315, WayCostToHome=315,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3586.30496445237, (24 * Hour + 3586.30496445237) + 22760.845144805)}},
			new Visit{Duration = 1243, Id=13,WayCostFromHome=587, WayCostToHome=587,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 10728.979250466, (24 * Hour + 10728.979250466) + 20786.4740174061)}},
			new Visit{Duration = 3571, Id=14,WayCostFromHome=1637, WayCostToHome=1637,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 2879.85675911765, (24 * Hour + 2879.85675911765) + 20032.4395145799)}},
			new Visit{Duration = 2364, Id=15,WayCostFromHome=2899, WayCostToHome=2899,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 2593.8198262151, (24 * Hour + 2593.8198262151) + 25161.8538114791)}},
			new Visit{Duration = 3513, Id=16,WayCostFromHome=2909, WayCostToHome=2909,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 13532.2082224751, (24 * Hour + 13532.2082224751) + 18557.2286387837)}},
			new Visit{Duration = 2549, Id=17,WayCostFromHome=2080, WayCostToHome=2080,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 8561.60968419749, (24 * Hour + 8561.60968419749) + 7847.8345215725)}},
			new Visit{Duration = 2365, Id=18,WayCostFromHome=2623, WayCostToHome=2623,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 1476.3199769074, (24 * Hour + 1476.3199769074) + 14909.1788111018)}},
			new Visit{Duration = 2896, Id=19,WayCostFromHome=760, WayCostToHome=760,Unavailable =new (int from, int to)[0],Desired = new [] {(24 * Hour + 3458.08743390997, (24 * Hour + 3458.08743390997) + 11234.3972020235)}}
		}
	};
	return (input, coordinates);
}
}
}
