using System;
using System.Text;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class GenAlgStarterData
    {
        public int PopulationSize { get; set; } = 10;
        public int MaxNumberOfSantas { get; set; }
        public long MaxNumberOfGenerations { get; set; } = long.MaxValue;
        public double MutationProbability { get; set; } = 0.1;
        public double OrderBasedCrossoverProbability { get; set; } = 0.5;

        /// <summary>
        ///Create default regarding the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static GenAlgStarterData GetDefault(OptimizationInput input)
        {

            var starterData = new GenAlgStarterData()
            {
                MaxNumberOfSantas = input.Santas.Length,
            };

            return starterData;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var property in GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append(": ");
                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(this, null));
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
