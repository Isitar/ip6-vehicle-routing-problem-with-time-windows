using System;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    /// <summary>
    /// Thread safe
    /// </summary>
    internal sealed class RandomFactory
    {
        private static ThreadLocal<Random> instances = new ThreadLocal<Random>(() => new Random());

        private RandomFactory()
        {
        }

        public static Random Instance
        {
            get { return instances.Value; }
        }
    }
}
