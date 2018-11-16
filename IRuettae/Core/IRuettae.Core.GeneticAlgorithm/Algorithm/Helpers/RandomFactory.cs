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
        public static RandomNumberGenerator Instance { get; } = new RNGCryptoServiceProvider();
    }
}
