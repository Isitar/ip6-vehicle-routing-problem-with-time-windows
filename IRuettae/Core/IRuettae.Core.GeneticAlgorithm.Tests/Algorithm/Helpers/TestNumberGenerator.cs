using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.GeneticAlgorithm.Tests.Algorithm.Helpers
{
    public class TestNumberGenerator : RandomNumberGenerator
    {
        /// <summary>
        /// Will increase with each call to GetBytes
        /// </summary>
        int counter = 0;

        public override void GetBytes(byte[] data)
        {
            byte[] randomBytes = BitConverter.GetBytes(counter++);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = randomBytes[i % randomBytes.Length];
            }
        }
    }
}
