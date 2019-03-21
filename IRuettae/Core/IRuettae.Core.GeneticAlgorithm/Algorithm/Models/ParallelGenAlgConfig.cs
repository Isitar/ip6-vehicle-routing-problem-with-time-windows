using System;
using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class ParallelGenAlgConfig : ISolverConfig
    {
        public GenAlgConfig GenAlgConfig { get; set; }
        public int NumberOfRuns { get; set; }

        public ParallelGenAlgConfig(GenAlgConfig config, int numberOfRuns)
        {
            GenAlgConfig = config;
            NumberOfRuns = numberOfRuns;
        }

        public override string ToString() => string.Join(Environment.NewLine, GetType().GetProperties().Select(p => $"{p.Name}: {(p.GetIndexParameters().Length > 0 ? "Indexed Property cannot be used" : p.GetValue(this, null))}"));
    }
}
