using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.DatasetGenerator
{
    public class GaussianRandomGenerator
    {
        private static readonly Random Random = new Random();

        public static double RandomNormal()
        {
            return Math.Cos(2d * Math.PI * (1d - Random.NextDouble())) * Math.Sqrt(-2d * Math.Log(1d - Random.NextDouble()));
        }

        public static double RandomGauss(double mean, double stdev)
        {
            return mean + stdev * RandomNormal();
        }
    }
}
