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
		(2226,2450),
		(1305,1657),
		(3838,3491),
		(2471,2755),
		(1653,1341),
		(2624,2349),
		(2906,2778),
		(1504,1561),
		(2886,3025),
		(2826,2618),
		(2614,2800)
	};
	const int workingDayDuration = 8 * Hour;
	var input = new OptimizationInput
	{
		Days = new[] {(0* Hour, 0*Hour + workingDayDuration), (24* Hour, 24*Hour + workingDayDuration) },

		RouteCosts = new[,]
		{
			{ 0, 3127, 1601, 470, 1489, 1954, 220, 2090, 1799, 1737 },
			{ 3127, 0, 1552, 3065, 1666, 1173, 3028, 1059, 1336, 1405 },
			{ 1601, 1552, 0, 1633, 433, 435, 1536, 495, 380, 149 },
			{ 470, 3065, 1633, 0, 1399, 1906, 265, 2087, 1733, 1747 },
			{ 1489, 1666, 433, 1399, 0, 513, 1369, 724, 336, 451 },
			{ 1954, 1173, 435, 1906, 513, 0, 1856, 247, 178, 292 },
			{ 220, 3028, 1536, 265, 1369, 1856, 0, 2013, 1692, 1663 },
			{ 2090, 1059, 495, 2087, 724, 247, 2013, 0, 411, 353 },
			{ 1799, 1336, 380, 1733, 336, 178, 1692, 411, 0, 279 },
			{ 1737, 1405, 149, 1747, 451, 292, 1663, 353, 279, 0 }
		},

		Santas = new[]
		{
			new Santa { Id = 0 }
		},

		Visits = new[]
		{
			new Visit{Duration = 2614, Id=0,WayCostFromHome=1215, WayCostToHome=1215,Unavailable =new [] {(0 * Hour + 11484.4763241607, (0 * Hour + 11484.4763241607) + 11356.4349943383)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2009, Id=1,WayCostFromHome=1918, WayCostToHome=1918,Unavailable =new [] {(0 * Hour + 1245.56988653628, (0 * Hour + 1245.56988653628) + 6611.11583913076)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2719, Id=2,WayCostFromHome=391, WayCostToHome=391,Unavailable =new [] {(0 * Hour + 3468.61040609851, (0 * Hour + 3468.61040609851) + 25245.9347771872)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2191, Id=3,WayCostFromHome=1248, WayCostToHome=1248,Unavailable =new [] {(0 * Hour + 3570.6295669463, (0 * Hour + 3570.6295669463) + 18718.6692261364)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 3193, Id=4,WayCostFromHome=410, WayCostToHome=410,Unavailable =new [] {(0 * Hour + 3844.36520562075, (0 * Hour + 3844.36520562075) + 4149.88461868832)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1664, Id=5,WayCostFromHome=754, WayCostToHome=754,Unavailable =new [] {(24 * Hour + 19998.1130400491, (24 * Hour + 19998.1130400491) + 7884.86283425096)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 2570, Id=6,WayCostFromHome=1145, WayCostToHome=1145,Unavailable =new [] {(24 * Hour + 12380.2887298149, (24 * Hour + 12380.2887298149) + 15318.8017937116)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1678, Id=7,WayCostFromHome=875, WayCostToHome=875,Unavailable =new [] {(24 * Hour + 8816.71469201779, (24 * Hour + 8816.71469201779) + 7489.45890154012)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1404, Id=8,WayCostFromHome=623, WayCostToHome=623,Unavailable =new [] {(24 * Hour + 15485.8631570522, (24 * Hour + 15485.8631570522) + 11092.0205993299)},Desired = new (int from, int to)[0]},
			new Visit{Duration = 1765, Id=9,WayCostFromHome=522, WayCostToHome=522,Unavailable =new [] {(24 * Hour + 4009.12801804712, (24 * Hour + 4009.12801804712) + 23105.5488301071)},Desired = new (int from, int to)[0]}
		}
	};
	return (input, coordinates);
}
}
}
