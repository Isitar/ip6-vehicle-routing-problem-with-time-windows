using System;
using System.Diagnostics;
using IRuettae.Core.Google.Routing.Algorithm;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing
{
    public class RoutingSolver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly RoutingSolverStarterData starterData;

        public RoutingSolver(OptimizationInput input, RoutingSolverStarterData starterData)
        {
            this.input = input;
            this.starterData = starterData;
        }

        public OptimizationResult Solve(long timeLimitMilliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            // convenience
            void LogPercentage(double percentage)
            {
                progress?.Invoke(this, new ProgressReport(percentage));
            }
            void LogMessage(string message)
            {
                consoleProgress?.Invoke(this, message);
            }

            var sw = Stopwatch.StartNew();

            LogMessage("Solving started.");
            LogPercentage(0.01);

            var data = Converter.Convert(input, starterData.MaxNumberOfSantas);

            LogMessage("Conversion of input finished.");
            LogPercentage(0.02);

            // solve
            var ret = InternalSolver.Solve(data, timeLimitMilliseconds);
            ret.TimeElapsed = (int)sw.Elapsed.TotalSeconds;

            LogMessage("Internal solve finished.");
            LogPercentage(1);

            return ret;
        }
    }
}
