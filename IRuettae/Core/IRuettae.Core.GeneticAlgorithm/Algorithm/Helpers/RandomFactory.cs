using System;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    internal sealed class RandomFactory
    {
        /// <summary>
        /// Warning: Random is not thread safe
        /// </summary>
        public static Random Instance { get; } = new Random();
    }
}
