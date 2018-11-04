using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using System.Security.Cryptography;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public static class PopulationGenerator
    {
        public static List<int>[] Generate(OptimizationInput input, int numberOfIndividuals, int numberOfRouteSeparators)
        {
            var elements = new List<int>();
            elements.AddRange(input.Visits.Select(v => v.Id));
            elements.AddRange(Enumerable.Range(-numberOfRouteSeparators, numberOfRouteSeparators));

            var population = new List<int>[numberOfIndividuals];
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                elements.Shuffle();
                population[i] = new List<int>(elements);
            }
            return population;
        }

        /// <summary>
        /// Source: https://stackoverflow.com/questions/273313/randomize-a-listt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
