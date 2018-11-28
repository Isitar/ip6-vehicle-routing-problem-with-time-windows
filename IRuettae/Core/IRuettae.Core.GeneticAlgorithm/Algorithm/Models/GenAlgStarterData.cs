using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class GenAlgStarterData
    {
        public int PopulationSize { get; set; } = 2;
        public int MaxNumberOfSantas { get; set; }
        public long MaxNumberOfGenerations { get; set; } = long.MaxValue;
        public double MutationProbability { get; set; } = 1;

        /// <summary>
        ///Create default regarding the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static GenAlgStarterData GetDefault(OptimizationInput input)
        {
            return new GenAlgStarterData()
            {
                MaxNumberOfSantas = input.Santas.Length,
            };
        }
    }
}
