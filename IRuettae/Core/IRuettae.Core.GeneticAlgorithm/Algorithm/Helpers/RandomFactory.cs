using System;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    internal sealed class RandomFactory
    {
        /// <summary>
        /// Warning: Random is not threadsafe
        /// </summary>
        public static Random Instance { get; } = new Random();
    }
}
