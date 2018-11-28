using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
    internal partial class DatasetFactory
    {
        /**
         * Queries
         *
         * Coords
         *
            SELECT Concat('(', round(1000*(`Long` * 78.71 -651),0), ',', round(1000*(`Lat` *111 -5257),0), '),')
            FROM visit
            WHERE Id = 100;

            SELECT Concat('(', round(1000*(`Long` * 78.71 -651),0), ',', round(1000*(`Lat` *111 -5257),0), '),')
            from visit
            WHERE Id > 100 
            order by id;
         *
         * RouteCost
         *
         *
            SELECT From_id, concat('{',group_concat(Duration, ''),'},')
            from way
            WHERE TRUE
	            AND To_id > 100
	            AND From_id > 100
            GROUP BY From_id
            ORDER BY To_id;

         * Visits
         *
           SET @RowNumber = 0;
           SELECT
              CONCAT('new Visit{Duration=',v.Duration,
                     ', Id=', (@RowNumber:=@RowNumber + 1) -1 ,
                     ',WayCostFromHome=',wHome.Duration,
                     ', WayCostToHome=',wToHome.Duration,
                     ',Unavailable =',CASE WHEN unavailablePeriod.Id IS NULL THEN 'new (int from, int to)[0]'
                                      ELSE
                                        concat('new [] {(',unix_timestamp(unavailablePeriod.Start) - unix_timestamp('2018-12-08 17:00:00'), ',',unix_timestamp(unavailablePeriod.End) - unix_timestamp('2018-12-08 17:00:00'),')}')
                                      END,
                     ',Desired =',CASE WHEN desiredPeriod.Id IS NULL THEN 'new (int from, int to)[0]'
                                  ELSE
                                    concat('new [] {(',unix_timestamp(desiredPeriod.Start) - unix_timestamp('2018-12-08 17:00:00'), ',',unix_timestamp(desiredPeriod.End) - unix_timestamp('2018-12-08 17:00:00'),')}')
                                  END,
                     '},')

             
            FROM visit v
              JOIN way wHome ON v.Id = wHome.To_id AND wHome.From_id = 100
              JOIN way wToHome ON v.Id = wToHome.From_id AND wToHome.To_id = 100
              left JOIN period desiredPeriod ON v.Id = desiredPeriod.desired_visit_id
              LEFT JOIN period unavailablePeriod ON v.Id = unavailablePeriod.unavailable_visit_id
            WHERE v.Id > 100
            GROUP BY v.Id;
         *
         */

        /// <summary>
        /// Real example 2018
        /// 31 Visits, 2 Days, 3 Santas
        /// </summary>
        public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet8()
        {
            // Coordinates of points
            var coordinates = new[]
            {
                (1809,1989),
                (2346,1953),
                (2266,1912),
                (2033,2001),
                (1906,2217),
                (1888,2170),
                (1927,2276),
                (1735,1867),
                (1973,1637),
                (1933,1557),
                (2198,1486),
                (2212,1593),
                (2207,1692),
                (1548,3017),
                (1645,3070),
                (1763,3130),
                (1548,3017),
                (1468,2965),
                (1479,2927),
                (1820,1963),
                (1670,1958),
                (1655,1914),
                (1668,1737),
                (1433,3097),
                (1535,2992),
                (1615,3801),
                (2283,1771),
                (2207,1692),
                (2042,1708),
                (2109,1647),
                (1871,1843),
                (1690,2457),
            };
            const int workingDayDuration = 5 * Hour;
            var input = new OptimizationInput
            {
                Days = new[] { (0 * Hour, 0 * Hour + workingDayDuration), (24 * Hour, 24 * Hour + workingDayDuration) },

                RouteCosts = new[,]
                {
                    {0,180,545,927,971,975,753,687,733,718,568,435,1692,1829,2100,1692,1801,1836,649,845,841,883,1857,1671,2748,420,435,545,516,578,1101},
                    {180,0,438,820,864,868,646,581,634,619,469,336,1585,1722,1993,1585,1694,1729,542,738,734,776,1751,1564,2641,321,336,438,417,471,995},
                    {545,438,0,512,556,560,338,594,721,740,592,533,1277,1414,1685,1277,1386,1421,235,430,426,566,1443,1257,2333,532,533,452,540,290,687},
                    {927,820,512,0,47,164,568,800,912,1122,974,915,1110,1247,1518,1110,1219,1254,470,618,657,796,1276,1090,2166,914,915,834,923,520,520},
                    {971,864,556,47,0,208,612,843,955,1166,1018,959,1156,1293,1565,1156,1266,1301,516,664,700,840,1322,1136,2213,958,959,878,966,564,566},
                    {975,868,560,164,208,0,616,847,959,1170,1022,963,1272,1409,1448,1272,1381,1416,513,709,705,844,1438,1252,1819,962,963,882,970,568,682},
                    {753,646,338,568,612,616,0,368,480,713,703,658,1334,1471,1742,1334,1443,1478,240,120,116,228,1482,1313,2390,636,658,448,549,222,743},
                    {687,581,594,800,843,847,368,0,113,346,336,291,1564,1701,1972,1564,1674,1708,488,487,483,413,1730,1544,2621,435,291,234,182,281,974},
                    {733,634,721,912,955,959,480,113,0,382,372,327,1676,1813,2084,1676,1786,1820,600,599,595,525,1842,1656,2733,471,327,270,218,393,1086},
                    {718,619,740,1122,1166,1170,713,346,382,0,157,312,1886,2023,2295,1886,1996,2031,761,833,829,759,2052,1866,2943,435,312,289,202,627,1296},
                    {568,469,592,974,1018,1022,703,336,372,157,0,162,1739,1876,2147,1739,1848,1883,613,823,819,749,1904,1718,2795,306,162,278,156,543,1149},
                    {435,336,533,915,959,963,658,291,327,312,162,0,1679,1816,2087,1679,1789,1824,554,729,725,703,1845,1659,2736,146,0,233,110,483,1089},
                    {1692,1585,1277,1110,1156,1272,1334,1564,1676,1886,1739,1679,0,225,409,0,110,145,1166,1295,1411,1561,186,21,1057,1679,1679,1598,1687,1285,597},
                    {1829,1722,1414,1247,1293,1409,1471,1701,1813,2023,1876,1816,225,0,300,225,334,369,1303,1432,1548,1698,371,204,989,1816,1816,1735,1824,1422,734},
                    {2100,1993,1685,1518,1565,1448,1742,1972,2084,2295,2147,2087,409,300,0,409,518,553,1575,1703,1819,1970,420,429,785,2087,2087,2006,2095,1693,1005},
                    {1692,1585,1277,1110,1156,1272,1334,1564,1676,1886,1739,1679,0,225,409,0,110,145,1166,1295,1411,1561,186,21,1057,1679,1679,1598,1687,1285,597},
                    {1801,1694,1386,1219,1266,1381,1443,1674,1786,1996,1848,1789,110,334,518,110,0,35,1276,1364,1520,1671,247,130,1167,1788,1789,1708,1797,1394,706},
                    {1836,1729,1421,1254,1301,1416,1478,1708,1820,2031,1883,1824,145,369,553,145,35,0,1311,1398,1555,1706,282,165,1201,1823,1824,1743,1831,1429,741},
                    {649,542,235,470,516,513,240,488,600,761,613,554,1166,1303,1575,1166,1276,1311,0,221,317,468,1315,1146,2223,553,554,473,561,209,576},
                    {845,738,430,618,664,709,120,487,599,833,823,729,1295,1432,1703,1295,1364,1398,221,0,197,348,1379,1275,2352,728,729,567,668,341,705},
                    {841,734,426,657,700,705,116,483,595,829,819,725,1411,1548,1819,1411,1520,1555,317,197,0,344,1559,1391,2467,724,725,564,664,337,821},
                    {883,776,566,796,840,844,228,413,525,759,749,703,1561,1698,1970,1561,1671,1706,468,348,344,0,1710,1541,2618,788,703,494,594,267,971},
                    {1857,1751,1443,1276,1322,1438,1482,1730,1842,2052,1904,1845,186,371,420,186,247,282,1315,1379,1559,1710,0,207,1034,1845,1845,1764,1853,1451,782},
                    {1671,1564,1257,1090,1136,1252,1313,1544,1656,1866,1718,1659,21,204,429,21,130,165,1146,1275,1391,1541,207,0,1077,1659,1659,1578,1667,1265,576},
                    {2748,2641,2333,2166,2213,1819,2390,2621,2733,2943,2795,2736,1057,989,785,1057,1167,1201,2223,2352,2467,2618,1034,1077,0,2735,2736,2655,2744,2341,1653},
                    {420,321,532,914,958,962,636,435,471,435,306,146,1679,1816,2087,1679,1788,1823,553,728,724,788,1845,1659,2735,0,146,377,254,483,1089},
                    {435,336,533,915,959,963,658,291,327,312,162,0,1679,1816,2087,1679,1789,1824,554,729,725,703,1845,1659,2736,146,0,233,110,483,1089},
                    {545,438,452,834,878,882,448,234,270,289,278,233,1598,1735,2006,1598,1708,1743,473,567,564,494,1764,1578,2655,377,233,0,124,361,1008},
                    {516,417,540,923,966,970,549,182,218,202,156,110,1687,1824,2095,1687,1797,1831,561,668,664,594,1853,1667,2744,254,110,124,0,462,1097},
                    {578,471,290,520,564,568,222,281,393,627,543,483,1285,1422,1693,1285,1394,1429,209,341,337,267,1451,1265,2341,483,483,361,462,0,695},
                    {1101,995,687,520,566,682,743,974,1086,1296,1149,1089,597,734,1005,597,706,741,576,705,821,971,782,576,1653,1089,1089,1008,1097,695,0},
                },

                Santas = new[]
                {
                    new Santa { Id = 0 },
                    new Santa { Id = 1 },
                    new Santa { Id = 2 }
                },

                Visits = new[]
                {
                    new Visit{Duration=1500, Id=0,WayCostFromHome=701, WayCostToHome=701,Unavailable =new [] {(0,10800)},Desired =new [] {(90000,-31433400)}},
                    new Visit{Duration=1800, Id=2,WayCostFromHome=594, WayCostToHome=594,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=3,WayCostFromHome=286, WayCostToHome=286,Unavailable =new (int from, int to)[0],Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=4,WayCostFromHome=449, WayCostToHome=449,Unavailable =new (int from, int to)[0],Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1800, Id=5,WayCostFromHome=495, WayCostToHome=495,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1200, Id=6,WayCostFromHome=564, WayCostToHome=564,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=7,WayCostFromHome=189, WayCostToHome=189,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=8,WayCostFromHome=539, WayCostToHome=539,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=9,WayCostFromHome=651, WayCostToHome=651,Unavailable =new [] {(0,10800)},Desired =new [] {(88200,102600)}},
                    new Visit{Duration=1200, Id=10,WayCostFromHome=812, WayCostToHome=812,Unavailable =new (int from, int to)[0],Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1200, Id=11,WayCostFromHome=664, WayCostToHome=664,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1200, Id=12,WayCostFromHome=605, WayCostToHome=605,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=2700, Id=13,WayCostFromHome=1145, WayCostToHome=1145,Unavailable =new [] {(0,10800)},Desired =new [] {(88200,91800)}},
                    new Visit{Duration=1500, Id=14,WayCostFromHome=1282, WayCostToHome=1282,Unavailable =new [] {(0,10800)},Desired =new [] {(88230,102600)}},
                    new Visit{Duration=1500, Id=15,WayCostFromHome=1553, WayCostToHome=1553,Unavailable =new [] {(0,10800)},Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=16,WayCostFromHome=1145, WayCostToHome=1145,Unavailable =new [] {(86400,90000)},Desired =new [] {(90000,93600)}},
                    new Visit{Duration=1800, Id=17,WayCostFromHome=1255, WayCostToHome=1255,Unavailable =new (int from, int to)[0],Desired =new [] {(86400,102600)}},
                    new Visit{Duration=1500, Id=18,WayCostFromHome=1290, WayCostToHome=1290,Unavailable =new [] {(86400,10800)},Desired =new [] {(0,3600)}},
                    new Visit{Duration=900, Id=19,WayCostFromHome=52, WayCostToHome=52,Unavailable =new [] {(86400,97200)},Desired =new [] {(3600,16200)}},
                    new Visit{Duration=1500, Id=20,WayCostFromHome=170, WayCostToHome=170,Unavailable =new [] {(86400,97200)},Desired =new [] {(3600,16200)}},
                    new Visit{Duration=1800, Id=21,WayCostFromHome=266, WayCostToHome=266,Unavailable =new [] {(86400,90000)},Desired =new [] {(3600,16200)}},
                    new Visit{Duration=1500, Id=22,WayCostFromHome=417, WayCostToHome=417,Unavailable =new (int from, int to)[0],Desired =new [] {(3600,16200)}},
                    new Visit{Duration=1500, Id=23,WayCostFromHome=1294, WayCostToHome=1294,Unavailable =new (int from, int to)[0],Desired =new [] {(3600,16200)}},
                    new Visit{Duration=900, Id=24,WayCostFromHome=1125, WayCostToHome=1125,Unavailable =new [] {(86400,97200)},Desired =new [] {(7200,10800)}},
                    new Visit{Duration=1500, Id=25,WayCostFromHome=2202, WayCostToHome=2202,Unavailable =new [] {(86400,97200)},Desired =new [] {(5400,16200)}},
                    new Visit{Duration=1200, Id=26,WayCostFromHome=604, WayCostToHome=604,Unavailable =new [] {(86400,93600)},Desired =new [] {(3600,16200)}},
                    new Visit{Duration=1500, Id=27,WayCostFromHome=605, WayCostToHome=605,Unavailable =new (int from, int to)[0],Desired =new [] {(3600,16200)}},
                    new Visit{Duration=2100, Id=28,WayCostFromHome=524, WayCostToHome=524,Unavailable =new [] {(86400,97200)},Desired =new [] {(3600,16200)}},
                    new Visit{Duration=1800, Id=29,WayCostFromHome=613, WayCostToHome=613,Unavailable =new [] {(86400,97200)},Desired =new [] {(5400,16200)}},
                    new Visit{Duration=1500, Id=30,WayCostFromHome=260, WayCostToHome=260,Unavailable =new [] {(86400,97200)},Desired =new [] {(3600,16200)}},
                    new Visit{Duration=2100, Id=32,WayCostFromHome=555, WayCostToHome=555,Unavailable =new [] {(86400,97200)},Desired =new [] {(3600,16200)}},
                }
            };
            return (input, coordinates);
        }
    }
}
