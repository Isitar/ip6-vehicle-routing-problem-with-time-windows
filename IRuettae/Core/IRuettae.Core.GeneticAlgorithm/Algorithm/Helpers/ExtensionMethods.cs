using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Randomly shuffels a List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = RandomFactory.Instance;
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = provider.NextInt(0, n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Returns a random number between min and max.
        /// Note: This function is focussed on performance
        /// and therefore it may have an imbalance in the probability of distrubution.
        /// Precondition: min < max
        /// </summary>
        /// <param name="random">not null</param>
        /// <param name="min">lowest possible return value</param>
        /// <param name="max">highest possible return value</param>
        /// <returns></returns>
        public static int NextInt(this RandomNumberGenerator random, int min, int max)
        {
            var data = new byte[4];
            random.GetBytes(data);

            // scale
            var generatedValue = Math.Abs(BitConverter.ToInt32(data, 0));

            // conversion to long is neccessary,
            // because the difference between int.min and int.max is bigger than int.max
            long mod = generatedValue % ((long)max + 1 - min);
            return min + (int)mod;
        }

        /// <summary>
        /// Returns a random number between 0 and 1.
        /// Note: This function is focussed on performance
        /// and therefore it may have an imbalance in the probability of distrubution.
        /// </summary>
        /// <param name="random">not null</param>
        /// <returns></returns>
        public static double NextProbability(this RandomNumberGenerator random)
        {
            return (double)random.NextInt(0, int.MaxValue) / int.MaxValue;
        }
    }
}
