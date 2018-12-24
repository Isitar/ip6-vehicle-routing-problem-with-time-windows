using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IRuettae.Core.GeneticAlgorithm.Algorithm;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm
{
    public class GenAlgSolver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly GenAlgStarterData starterData;
        private EventHandler<ProgressReport> progress;
        private EventHandler<string> consoleProgress;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="starterData"></param>
        public GenAlgSolver(OptimizationInput input, GenAlgStarterData starterData)
        {
            this.input = input;
            this.starterData = starterData;
        }

        public OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            this.progress = progress;
            this.consoleProgress = consoleProgress;

            var sw = Stopwatch.StartNew();
            Log("Solving started");
            Log(new ProgressReport(0.01));

            // adjust timelimit if unlimited
            if (timelimitMiliseconds == 0)
            {
                timelimitMiliseconds = long.MaxValue;
            }

            // init population
            var (population, mapping) = new PopulationGenerator(RandomFactory.Instance).Generate(input, starterData.PopulationSize, starterData.MaxNumberOfSantas);
            var decoder = new Decoder(input, mapping);

            // calculate costs
            var result = new OptimizationResult()
            {
                OptimizationInput = input,
            };
            var costCalculator = new CostCalculator(decoder, new SimplifiedOptimizationResult(result));
            costCalculator.RecalculatateCost(population);

            // Log characteristics of inital population
            Log($"Genetic Algorithm started with following paramters:{Environment.NewLine}{starterData}");
            var bestCost = GetMinCost(population);
            Log($"Best solution cost in initial population is: {bestCost}");

            // evolution
            var evolutionOperation = new EvolutionOperation(starterData);
            var repairOperation = new RepairOperation(input, mapping);
            long generation = 0;
            for (; generation < starterData.MaxNumberOfGenerations && sw.ElapsedMilliseconds < timelimitMiliseconds; generation++)
            {
                // evolve
                evolutionOperation.Evolve(population);

                // repair
                repairOperation.Repair(population);

                // recalculate costs
                costCalculator.RecalculatateCost(population);

                // log current solution
                var currentBestCost = GetMinCost(population);
                if (currentBestCost < bestCost)
                {
                    bestCost = currentBestCost;
                    Log($"Found better solution in generation {generation} with cost={bestCost}");
                }
            }

            Log($"Finished at generation {generation} with cost={bestCost}");
            Log($"Current stdev is {StdDev(population.Select(i => (double)i.Cost))}");
            Log(new ProgressReport(0.99));

            // build result
            var bestSolution = population.OrderBy(i => i.Cost).First();
            result.Routes = decoder.Decode(bestSolution);
            result.ResultState = ResultState.Finished;

            Log(new ProgressReport(1));

            sw.Stop();
            result.TimeElapsed = sw.ElapsedMilliseconds / 1000;
            return result;
        }

        /// <summary>
        /// convenience
        /// </summary>
        /// <param name="report"></param>
        public void Log(ProgressReport report)
        {
            progress?.Invoke(this, report);
        }

        /// <summary>
        /// convenience
        /// </summary>
        /// <param name="report"></param>
        public void Log(string msg)
        {
            consoleProgress?.Invoke(this, msg);
        }

        /// <summary>
        /// Returns the smallest cost of a solution in population
        /// </summary>
        /// <param name="population">with calculated costs</param>
        /// <returns></returns>
        public int GetMinCost(IEnumerable<Genotype> population)
        {
            return population.Select(i => i.Cost).Min();
        }

        public static double StdDev(IEnumerable<double> values)
        {
            double ret = 0.0;
            var count = values.Count();
            if (count > 1)
            {
                var avg = values.Average();
                var sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}
