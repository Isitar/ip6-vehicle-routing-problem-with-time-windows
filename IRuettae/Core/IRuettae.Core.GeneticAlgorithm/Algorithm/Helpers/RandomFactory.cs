using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    /// <summary>
    /// Threadsafe
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
