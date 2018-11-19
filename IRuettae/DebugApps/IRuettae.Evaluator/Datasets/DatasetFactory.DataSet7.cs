using IRuettae.Core.Models;
namespace IRuettae.Evaluator
{
    internal partial class DatasetFactory
    {
        /**
         * Queries
         *
         * Coords
            SELECT Concat('(',Round(100000*(lat-47.36),0),',',Round(100000*(`Long`-8.28),0),'),')
            from visit
            WHERE Id <= 35 AND id != 12
            order by id;
         *
         * RouteCost
         *  SELECT From_id, concat('{',group_concat(Duration, ''),'},')
            from way
            WHERE TRUE
              AND To_id <= 35
              AND To_id != 12
              AND From_id <= 35
              AND From_id != 12
            GROUP BY From_id
            ORDER BY To_id;

         * Visits
         *
            SELECT
              CONCAT('new Visit{Duration=',v.Duration,
                     ', Id=',ROW_NUMBER() OVER(ORDER BY Id) -1,
                     ',WayCostFromHome=',wHome.Duration,
                     ', WayCostToHome=',wToHome.Duration,
                     ',Unavailable =',CASE WHEN unavailablePeriod.Id IS NULL THEN 'new (int from, int to)[0]'
                                      ELSE
                                        concat('new [] {(',unix_timestamp(unavailablePeriod.Start) - unix_timestamp('2017-12-08 17:00:00'), ',',unix_timestamp(unavailablePeriod.End) - unix_timestamp('2017-12-08 17:00:00'),')}')
                                      END,
                     ',Desired =',CASE WHEN unavailablePeriod.Id IS NULL THEN 'new (int from, int to)[0]'
                                  ELSE
                                    concat('new [] {(',unix_timestamp(desiredPeriod.Start) - unix_timestamp('2017-12-08 17:00:00'), ',',unix_timestamp(desiredPeriod.End) - unix_timestamp('2017-12-08 17:00:00'),')}')
                                  END,
                     '},')

              ,v.*
            FROM visit v
              JOIN way wHome ON v.Id = wHome.To_id AND wHome.From_id = 12
              JOIN way wToHome ON v.Id = wToHome.From_id AND wToHome.To_id = 12
              left JOIN period desiredPeriod ON v.Id = desiredPeriod.desired_visit_id
              LEFT JOIN period unavailablePeriod ON v.Id = unavailablePeriod.unavailable_visit_id
            WHERE v.Id <= 35
            AND v.Id != 12
            GROUP BY v.Id;
         *
         */

        /// <summary>
        /// Real example 2017
        /// 35 Visits, 2 Days, 3 Santas
        /// 0 Desired, 0 Unavailable on day 0
        /// 0 Desired, 0 Unavailable on day 1
        /// </summary>
        public static (OptimizationInput input, (int x, int y)[] coordinates) DataSet7()
        {
            // Coordinates of points
            var coordinates = new[]
            {
                (1662,1472),
                (1439,1542),
                (1574,1681),
                (1561,1891),
                (1375,1879),
                (2129,1497),
                (1978,1542),
                (1939,1444),
                (1984,1489),
                (2028,1351),
                (1839,1669),
                (1519,1766),
                (1601,1206),
                (1510,1593),
                (1718,1291),
                (1477,1925),
                (1510,1593),
                (2826,907),
                (3009,1000),
                (2754,1054),
                (2856,1327),
                (1632,1987),
                (1632,1987),
                (2989,1130),
                (2840,1085),
                (2802,1177),
                (2897,1075),
                (1800,1209),
                (1795,2067),
                (2250,1233),
                (1697,1463),
                (1760,1189),
                (2826,907),
                (2826,907),
            };
            const int workingDayDuration = 5 * Hour;
            var input = new OptimizationInput
            {
                Days = new[] { (0 * Hour, 0 * Hour + workingDayDuration), (24 * Hour, 24 * Hour + workingDayDuration) },

                RouteCosts = new[,]
                {
                    {0,273,274,366,383,429,408,234,398,334,222,302,253,150,156,63,240,1064,1208,1252,1205,496,563,1165,1048,1029,1112,264,418,304,15,334,999,976},
                    {246,0,203,219,181,675,654,480,644,580,468,100,428,107,331,309,120,1310,1454,1498,1451,416,483,1411,1294,1275,1358,439,504,550,260,509,1245,1222},
                    {268,215,0,180,207,621,600,426,590,526,340,105,450,184,353,255,264,1256,1400,1444,1397,377,444,1357,1240,1221,1304,461,348,496,282,531,1191,1168},
                    {414,275,224,0,267,709,688,514,678,614,426,165,632,311,539,343,324,1344,1488,1532,1485,197,264,1445,1328,1309,1392,626,311,584,395,698,1279,1256},
                    {345,170,184,200,0,774,753,579,743,679,495,81,527,206,430,408,219,1409,1553,1597,1550,293,360,1510,1393,1374,1457,538,485,649,359,608,1344,1321},
                    {480,753,680,712,858,0,186,236,176,355,443,756,733,630,525,409,720,1085,1229,977,1008,842,909,1094,1069,1050,1041,612,743,325,461,684,1020,997},
                    {441,714,641,673,819,161,0,224,8,262,404,717,694,591,486,370,681,992,1136,1069,1100,803,870,1093,976,957,1040,573,704,232,422,645,927,904},
                    {244,517,444,476,622,195,208,0,198,119,207,520,497,394,289,173,484,849,993,1037,990,606,673,950,833,814,897,376,507,89,225,448,784,761},
                    {433,706,633,665,811,153,11,216,0,254,396,709,686,583,478,362,673,984,1128,1061,1092,795,862,1085,968,949,1032,565,696,224,414,637,919,896},
                    {329,602,529,561,707,299,224,104,214,0,292,605,582,479,374,258,569,746,874,918,871,691,758,831,714,695,778,338,592,23,310,533,674,651},
                    {253,526,379,409,557,423,402,228,392,328,0,455,506,403,298,182,493,1058,1202,1246,1199,539,606,1159,1042,1023,1106,385,393,298,234,457,993,970},
                    {285,110,103,119,102,695,674,500,664,600,414,0,467,146,370,329,159,1330,1474,1518,1471,316,383,1431,1314,1295,1378,478,404,570,299,548,1265,1242},
                    {203,405,406,569,515,632,611,437,601,537,425,434,0,282,207,266,372,1267,1411,1455,1408,699,766,1368,1251,1232,1315,315,621,507,217,385,1202,1179},
                    {139,123,179,271,233,568,547,373,537,473,361,152,321,0,224,202,90,1203,1347,1391,1344,468,535,1304,1187,1168,1251,332,421,443,153,402,1138,1115},
                    {128,330,331,465,440,448,427,253,417,353,241,359,229,207,0,162,297,1083,1227,1271,1224,595,662,1184,1067,1048,1131,108,517,323,142,178,1018,995},
                    {70,343,266,298,444,365,344,170,334,270,158,342,323,220,195,0,310,954,1098,1008,1039,711,778,1055,938,919,1002,481,612,194,330,553,935,912},
                    {217,124,256,272,234,646,625,451,615,551,439,153,399,78,302,280,0,1281,1425,1469,1422,469,536,1382,1265,1246,1329,410,499,521,231,480,1216,1193},
                    {1114,1387,1314,1346,1522,1084,1009,889,999,794,1077,1420,1367,1264,1159,970,1354,0,245,316,269,1476,1543,229,112,276,176,1052,1377,808,1095,1318,78,126},
                    {1280,1553,1480,1512,1688,1250,1175,1055,1165,951,1243,1586,1533,1430,1325,1136,1520,259,0,353,306,1642,1709,139,252,315,184,1289,1543,974,1261,1484,318,366},
                    {1355,1628,1555,1587,1763,1034,1145,1130,1135,1026,1318,1661,1608,1505,1400,1082,1595,361,391,0,149,1532,1599,235,250,282,182,1364,1540,1049,1336,1559,420,468},
                    {1311,1584,1511,1543,1719,1068,1179,1086,1169,982,1274,1617,1564,1461,1356,1116,1551,317,347,152,0,1566,1633,191,206,238,138,1320,1574,1005,1292,1515,376,424},
                    {577,505,454,230,387,872,851,677,841,777,589,395,830,541,702,758,554,1507,1651,1508,1539,0,67,1608,1491,1472,1555,789,474,747,558,861,1442,1419},
                    {646,574,523,299,456,941,920,746,910,846,658,464,899,610,771,827,623,1576,1720,1577,1608,69,0,1677,1560,1541,1624,858,543,816,627,930,1511,1488},
                    {1247,1520,1447,1479,1655,1130,1142,1022,1132,918,1210,1553,1500,1397,1292,1103,1487,253,156,214,167,1609,1676,0,142,176,45,1256,1510,941,1228,1451,312,360},
                    {1125,1398,1325,1357,1533,1095,1020,900,1010,796,1088,1431,1378,1275,1170,981,1365,131,257,224,177,1487,1554,137,0,184,84,1134,1388,819,1106,1329,190,238},
                    {1109,1382,1309,1341,1517,1079,1004,884,994,780,1072,1415,1362,1259,1154,965,1349,290,322,251,204,1471,1538,166,179,0,113,1118,1372,803,1090,1313,349,397},
                    {1202,1475,1402,1434,1610,1085,1097,977,1087,873,1165,1508,1455,1352,1247,1058,1442,208,209,169,122,1564,1631,54,97,131,0,1211,1465,896,1183,1406,267,315},
                    {241,443,444,557,553,540,519,345,509,337,333,472,342,320,113,426,410,996,1211,1255,1208,687,754,1168,1051,1032,1115,0,609,351,255,270,931,908},
                    {505,606,443,350,598,775,754,580,744,680,445,496,758,521,630,661,611,1410,1554,1525,1551,480,547,1511,1394,1375,1458,717,0,650,486,789,1345,1322},
                    {306,579,506,538,714,276,201,81,191,30,269,612,559,456,351,162,546,760,904,948,901,668,735,861,744,725,808,359,569,0,287,510,695,672},
                    {19,292,293,352,402,415,394,220,384,320,208,321,272,169,175,301,259,1050,1194,1238,1191,482,549,1151,1034,1015,1098,283,404,290,0,353,985,962},
                    {270,472,473,588,582,571,550,376,540,476,364,501,371,349,142,457,439,1206,1350,1394,1347,718,785,1307,1190,1171,1254,229,640,446,284,0,1141,1118},
                    {1036,1309,1236,1268,1414,1006,931,811,921,716,999,1312,1289,1186,1081,965,1276,65,291,362,315,1398,1465,275,158,322,222,974,1299,730,1017,1240,0,48},
                    {1000,1273,1200,1232,1378,970,895,775,885,680,963,1276,1253,1150,1045,929,1240,100,326,397,350,1362,1429,310,193,357,257,938,1263,694,981,1204,35,0},

            },

                Santas = new[]
            {
                new Santa { Id = 0 },
                new Santa { Id = 1 },
                new Santa { Id = 2 }
            },

                Visits = new[]
            {
                new Visit{Duration=2700, Id=0,WayCostFromHome=106, WayCostToHome=92,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=1,WayCostFromHome=378, WayCostToHome=338,Unavailable =new [] {(86400,100800)},Desired =new [] {(0,14400)}},
                new Visit{Duration=2100, Id=2,WayCostFromHome=305, WayCostToHome=284,Unavailable =new [] {(86400,100800)},Desired =new [] {(0,14400)}},
                new Visit{Duration=3600, Id=3,WayCostFromHome=337, WayCostToHome=372,Unavailable =new [] {(86400,100800)},Desired =new [] {(0,14400)}},
                new Visit{Duration=1500, Id=4,WayCostFromHome=483, WayCostToHome=437,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1200, Id=5,WayCostFromHome=337, WayCostToHome=375,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1800, Id=6,WayCostFromHome=316, WayCostToHome=336,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1500, Id=7,WayCostFromHome=142, WayCostToHome=139,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=8,WayCostFromHome=306, WayCostToHome=328,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1800, Id=9,WayCostFromHome=242, WayCostToHome=224,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=2100, Id=10,WayCostFromHome=130, WayCostToHome=148,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1800, Id=11,WayCostFromHome=381, WayCostToHome=358,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1200, Id=12,WayCostFromHome=358, WayCostToHome=295,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=13,WayCostFromHome=255, WayCostToHome=231,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=14,WayCostFromHome=167, WayCostToHome=128,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1800, Id=15,WayCostFromHome=34, WayCostToHome=28,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1500, Id=16,WayCostFromHome=345, WayCostToHome=309,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1500, Id=17,WayCostFromHome=972, WayCostToHome=1009,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=18,WayCostFromHome=1116, WayCostToHome=1175,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=19,WayCostFromHome=1160, WayCostToHome=1250,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1200, Id=20,WayCostFromHome=1113, WayCostToHome=1206,Unavailable =new [] {(0,14400)},Desired =new [] {(86400,100800)}},
                new Visit{Duration=1200, Id=21,WayCostFromHome=467, WayCostToHome=535,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=22,WayCostFromHome=534, WayCostToHome=604,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1800, Id=23,WayCostFromHome=1073, WayCostToHome=1142,Unavailable =new [] {(86400,100800)},Desired =new [] {(3600,14400)}},
                new Visit{Duration=1500, Id=24,WayCostFromHome=956, WayCostToHome=1020,Unavailable =new [] {(86400,100800)},Desired =new [] {(7200,14400)}},
                new Visit{Duration=1500, Id=25,WayCostFromHome=937, WayCostToHome=1004,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=26,WayCostFromHome=1020, WayCostToHome=1097,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=2400, Id=27,WayCostFromHome=254, WayCostToHome=220,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=28,WayCostFromHome=389, WayCostToHome=463,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=2400, Id=29,WayCostFromHome=212, WayCostToHome=201,Unavailable =new [] {(86400,100800)},Desired =new [] {(0,14400)}},
                new Visit{Duration=1500, Id=30,WayCostFromHome=87, WayCostToHome=78,Unavailable =new [] {(86400,100800)},Desired =new [] {(0,14400)}},
                new Visit{Duration=1500, Id=31,WayCostFromHome=326, WayCostToHome=251,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1500, Id=32,WayCostFromHome=907, WayCostToHome=931,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},
                new Visit{Duration=1800, Id=33,WayCostFromHome=884, WayCostToHome=895,Unavailable =new (int from, int to)[0],Desired =new (int from, int to)[0]},

        }
            };
            return (input, coordinates);
        }
    }
}
