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
        private readonly ParallelGenAlgConfig config;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="config"></param>
        public ParallelGenAlgSolver(OptimizationInput input, ParallelGenAlgConfig config)
        {
            if (!config.GenAlgConfig.IsValid())
            {
                throw new ArgumentException("the given GenAlgConfig is invalid", "config");
            }

            this.input = input;
            this.config = config;
        }

        public OptimizationResult Solve(long timeLimitMilliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            consoleProgress?.Invoke(this, "Solving started");
            progress?.Invoke(this, new ProgressReport(0.01));

            // adapt timelimit so that no overdraw is made
            if (config.NumberOfRuns > Environment.ProcessorCount)
            {
                timeLimitMilliseconds /= (long)Math.Ceiling((double)config.NumberOfRuns / Environment.ProcessorCount);
            }

            var orderedResults = Enumerable.Range(0, config.NumberOfRuns)
                .AsParallel()
                .Select(v =>
                {
                    var log = new StringBuilder();
                    var result = new GenAlgSolver(input, config.GenAlgConfig).Solve(timeLimitMilliseconds, null, (obj, msg) => log.AppendLine(msg));
                    return (result: result, log: log.ToString());
                })
                .OrderBy(run => run.result.Cost())
                .ToArray();
            var bestResult = orderedResults.First();

            consoleProgress?.Invoke(this, "Internal solving finished.");
            progress?.Invoke(this, new ProgressReport(0.99));

            for (int i = 0; i < orderedResults.Length; i++)
            {
                consoleProgress?.Invoke(this, $"Run {i + 1}:{Environment.NewLine}{orderedResults[i].log}");
            }

            progress?.Invoke(this, new ProgressReport(1));

            return bestResult.result;
        }
    }
}
