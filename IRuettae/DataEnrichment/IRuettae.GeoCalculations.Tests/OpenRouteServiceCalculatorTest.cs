using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.GeoCalculations.RouteCalculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.GeoCalculations.Tests
{
    [TestClass]
    public class OpenRouteServiceCalculatorTest
    {
        [TestMethod]
        public void TestMatrix()
        {
            var apiKey = Environment.GetEnvironmentVariable("open-route-service-key");
            var calc = new OpenRouteServiceCalculator(apiKey);
            var calculations = calc.CalculateWalkingDistanceMatrix(new[]
            {
                (47.474403, 8.212535),
                (47.476079, 8.211189),
                (47.474870, 8.210372)
            });

            Assert.IsNotNull(calculations);

            // check if walking:
            for (int i = 0; i < calculations.distance.GetLength(0); i++)
            {
                for (int j = 0; j < calculations.distance.GetLength(1); j++)
                {
                    if (Math.Abs(calculations.distance[i, j]) < 0.001) // == 0 check
                    {
                        Assert.AreEqual(0, calculations.duration[i, j]);
                        continue;
                    }
                    var factor = calculations.duration[i, j] / calculations.distance[i, j];
                    Assert.IsTrue(factor >= 0.6 && factor <= 0.8 );
                }
                    
            }
        }
    }
}
