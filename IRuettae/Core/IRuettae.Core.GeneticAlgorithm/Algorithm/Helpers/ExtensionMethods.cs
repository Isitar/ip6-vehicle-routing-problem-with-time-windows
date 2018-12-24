using System;
using System.Collections.Generic;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Randomly shuffles a List.
        /// Elements may retain their position.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="random"></param>
        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var k = random.Next(0, i + 1);
                T value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
        }
    }
}
