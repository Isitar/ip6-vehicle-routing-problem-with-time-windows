using System;
using System.Threading;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    /// <summary>
    /// Thread safe
    /// </summary>
    internal static class RandomFactory
    {
        private static readonly ThreadLocal<Random> Instances = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

        public static Random Instance => Instances.Value;
    }
}
