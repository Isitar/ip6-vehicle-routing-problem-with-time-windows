using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
