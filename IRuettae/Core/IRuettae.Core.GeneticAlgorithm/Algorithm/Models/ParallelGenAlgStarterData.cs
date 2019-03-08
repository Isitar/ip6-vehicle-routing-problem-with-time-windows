using System;
using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class ParallelGenAlgStarterData : IStarterData
    {
        public GenAlgStarterData GenAlgStarterData { get; set; }
        public int NumberOfRuns { get; set; }

        public ParallelGenAlgStarterData(GenAlgStarterData starterData, int numberOfRuns)
        {
            GenAlgStarterData = starterData;
            NumberOfRuns = numberOfRuns;
        }

        public override string ToString() => string.Join(Environment.NewLine, GetType().GetProperties().Select(p => $"{p.Name}: {(p.GetIndexParameters().Length > 0 ? "Indexed Property cannot be used" : p.GetValue(this, null))}"));
    }
}
