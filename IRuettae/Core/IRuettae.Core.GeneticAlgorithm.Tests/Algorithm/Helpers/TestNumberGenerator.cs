using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Helpers
{
    /// <summary>
    /// Class to produce a simple deterministic sequence of values for testing purposes.
    /// </summary>
    public class TestNumberGenerator : Random
    {
        /// <summary>
        /// Will increase with each call to GetBytes
        /// </summary>
        int counterBytes = 0;

        public override void NextBytes(byte[] data)
        {
            byte[] randomBytes = BitConverter.GetBytes(counterBytes++);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = randomBytes[i % randomBytes.Length];
            }
        }

        /// <summary>
        /// Will increase with each call to Next
        /// </summary>
        int counterInt = 0;

        public override int Next(int minValue, int maxValue)
        {
            // long is needed to avoid overflow problems
            return minValue + (int)(counterInt++ % ((long)maxValue - minValue));
        }

        public override int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        public override int Next()
        {
            return Next(int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// Will increase with each call to NextDouble
        /// </summary>
        int counterDouble = 0;
        public override double NextDouble()
        {
            var values = new double[4]
            {
                0.0, 0.25, 0.5, 0.99
            };
            return values[counterDouble++ % values.Length];
        }
    }
}
