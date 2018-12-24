using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm
{
    /// <summary>
    /// Class used to repair invalid genotypes
    /// </summary>
    public class RepairOperation
    {
        /// <summary>
        /// Are repairs generally needed?
        /// This will be determined from the OptimizationInput.
        /// </summary>
        private readonly bool needRepair;
        /// <summary>
        /// List of breaks per santa.
        /// The breaks are ordered by day.
        /// [santa][day]
        /// </summary>
        private readonly Dictionary<int, int[]> breakMapping;
        private readonly OptimizationInput input;

        public RepairOperation(OptimizationInput input, Dictionary<int, int> alleleToVisitIdMapping)
        {
            if (alleleToVisitIdMapping == null)
            {
                throw new ArgumentNullException();
            }

            this.input = input;
            needRepair = input.Visits.Where(v => v.IsBreak).Count() > 0;
            breakMapping = new Dictionary<int, int[]>();
            if (needRepair)
            {
                CreateBreakMapping(alleleToVisitIdMapping);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alleleToVisitIdMapping">not null</param>
        private void CreateBreakMapping(Dictionary<int, int> alleleToVisitIdMapping)
        {
            foreach (var santa in input.Santas.Where(s => input.Visits.Any(v => v.SantaId == s.Id)))
            {
                var breakId = input.Visits.Where(v => v.IsBreak && v.SantaId == santa.Id).First().Id;
                var breaks = alleleToVisitIdMapping.Where(e => e.Value == breakId).Select(e => e.Key).ToArray();
                breakMapping.Add(santa.Id, breaks);
            }
        }

        /// <summary>
        /// Repairs the given genotype in place
        /// </summary>
        /// <param name="genotype"></param>
        public void Repair(List<Genotype> population)
        {
            if (!needRepair)
            {
                return;
            }

            foreach (var individual in population)
            {
                Repair(individual);
            }
        }

        /// <summary>
        /// Repairs the given genotype in place
        /// </summary>
        /// <param name="genotype"></param>
        public void Repair(Genotype genotype)
        {
            if (!needRepair)
            {
                return;
            }

            RepairBreaks(genotype);
        }

        /// <summary>
        /// Makes sure, the breaks of a santa are in every route.
        /// If not, the break will be moved into the middle of the route.
        /// </summary>
        /// <param name="genotype"></param>
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
                            // Remove break at wrong place
                            RemoveAllele(split, expectedAllele);
                            // Add to with place
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
                for (int i = 0; i < ret.Length; i++)
                {
                    ret[i] = new List<Genotype>();
                }

                var routesPerDay = genotype.CountRoutes() / input.Days.Length;
                int day = 0;
                int santa = 0;
                for (int i = 0; i <= genotype.Count; i++)
                {
                    // add Genotype in any case
                    ret[day].Add(new Genotype());
                    while (i < genotype.Count && !PopulationGenerator.IsSeparator(genotype[i]))
                    {
                        ret[day][santa].Add(genotype[i]);
                        i++;
                    }
                    if (i < genotype.Count)
                    {
                        // Add separator
                        ret[day][santa].Add(genotype[i]);
                    }

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
