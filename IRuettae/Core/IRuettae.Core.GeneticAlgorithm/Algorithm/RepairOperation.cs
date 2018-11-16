using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    public class RepairOperation
    {
        private readonly bool needRepair;
        private readonly OptimizationInput input;
        private readonly Dictionary<int, int> alleleToVisitIdMapping;
        private readonly Dictionary<int, int[]> breakMapping;

        public RepairOperation(OptimizationInput input, Dictionary<int, int> alleleToVisitIdMapping)
        {
            this.input = input;
            this.alleleToVisitIdMapping = alleleToVisitIdMapping;
            needRepair = input.Visits.Where(IsBreak).Count() > 0;
            breakMapping = new Dictionary<int, int[]>();
            if (needRepair)
            {
                CreateBreakMapping();
            }
        }

        private void CreateBreakMapping()
        {
            foreach (var santa in input.Santas.Where(s => input.Visits.Any(v => v.SantaId == s.Id)))
            {
                var breakId = input.Visits.Where(v => v.SantaId == santa.Id).First().Id;
                var breaks = alleleToVisitIdMapping.Where(e => e.Value == breakId).Select(e => e.Key).ToArray();
                breakMapping.Add(santa.Id, breaks);
            }
        }

        public void Repair(Genotype genotype)
        {
            if (!needRepair)
            {
                return;
            }

            RepairBreaks(genotype);
        }

        private void RepairBreaks(Genotype genotype)
        {
            var split = SplitGenotyp(genotype);
            for (int day = 0; day < input.Days.Length; day++)
            {
                // Loop only to last real santa, as artificial ones do not have breaks
                for (int santa = 0; santa < input.Santas.Length; santa++)
                {
                    var santaId = input.Santas[santa].Id;
                    // Does this santa has a break?
                    if (breakMapping.ContainsKey(santaId))
                    {
                        var expectedAllele = breakMapping[santaId][day];
                        // Is the break missing in this route?
                        if (!split[day][santa].Contains(expectedAllele))
                        {
                            RemoveAllele(split, expectedAllele);
                            split[day][santa].Insert(split[day][santa].Count / 2, expectedAllele);
                        }
                    }
                }
            }
            RestoreGenotype(genotype, split);
        }

        /// <summary>
        /// Split genotype into separate routes
        /// </summary>
        /// <param name="genotype"></param>
        /// <returns></returns>
        private List<Genotype>[] SplitGenotyp(Genotype genotype)
        {
            var ret = new List<Genotype>[input.Days.Length];
            {
                var routesPerDay = genotype.CountRoutes() / input.Days.Length;
                int day = 0;
                int santa = 0;
                for (int i = 0; i < genotype.Count; i++)
                {
                    ret[day].Add(new Genotype());
                    while (!PopulationGenerator.IsSeparator(genotype[i]) && i < genotype.Count) ;
                    {
                        ret[day][santa].Add(genotype[i]);
                        i++;
                    }
                    // Add separator
                    ret[day][santa].Add(genotype[i]);

                    santa++;
                    if (santa >= routesPerDay)
                    {
                        santa = 0;
                        day++;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Removes the first occurence of the allel
        /// </summary>
        /// <param name="genotype"></param>
        /// <returns>true if it was removed</returns>
        private bool RemoveAllele(List<Genotype>[] genotypes, int allele)
        {
            foreach (var day in genotypes)
            {
                foreach (var genotype in day)
                {
                    var index = genotype.IndexOf(allele);
                    if (index >= 0)
                    {
                        genotype.RemoveAt(index);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Replaces genotype with the parts of split
        /// </summary>
        /// <param name="genotype"></param>
        /// <param name="split"></param>
        private void RestoreGenotype(Genotype genotype, List<Genotype>[] split)
        {
            genotype.Clear();
            foreach (var day in split)
            {
                foreach (var part in day)
                {
                    genotype.AddRange(part);
                }
            }
        }

    }
}
