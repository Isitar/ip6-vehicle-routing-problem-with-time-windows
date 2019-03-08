using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm
{
    /// <summary>
    /// Implementation that starts one genetic algorithm per core.
    /// </summary>
    public class ParallelGenAlgSolver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly ParallelGenAlgStarterData starterData;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="starterData"></param>
        public ParallelGenAlgSolver(OptimizationInput input, ParallelGenAlgStarterData starterData)
        {
            if (!starterData.GenAlgStarterData.IsValid())
            {
                throw new ArgumentException("The given GenAlgStarterData is invalid.");
            }

            this.input = input;
            this.starterData = starterData;
        }

        public OptimizationResult Solve(long timeLimitMilliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            consoleProgress?.Invoke(this, "Solving started");
            progress?.Invoke(this, new ProgressReport(0.01));

            // adapt timelimit so that no overdraw is made
            if (starterData.NumberOfRuns > Environment.ProcessorCount)
            {
                timeLimitMilliseconds /= (long)Math.Ceiling((double)starterData.NumberOfRuns / Environment.ProcessorCount);
            }

            var orderedResults = Enumerable.Range(0, Environment.ProcessorCount)
                .AsParallel()
                .Select(v =>
                {
                    var log = new StringBuilder();
                    var result = new GenAlgSolver(input, starterData.GenAlgStarterData).Solve(timeLimitMilliseconds, null, (obj, msg) => log.AppendLine(msg));
                    return (result: result, log: log.ToString());
                })
                .OrderBy(run => run.result.Cost())
                .ToArray();
            var bestResult = orderedResults.First();

            consoleProgress?.Invoke(this, bestResult.log);

            consoleProgress?.Invoke(this, "Internal solving finished.");
            for (int i = 1; i < orderedResults.Length; i++)
            {
                consoleProgress?.Invoke(this, $"Run {i + 1} costs {orderedResults[i].result.Cost()}.");
            }
            progress?.Invoke(this, new ProgressReport(0.99));

            return bestResult.result;
        }
    }
}
