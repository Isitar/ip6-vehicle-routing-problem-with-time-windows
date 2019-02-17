using System;

namespace IRuettae.DatasetGenerator
{
    public class GaussianRandomGenerator
    {
        public static Random Random = new Random();

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
