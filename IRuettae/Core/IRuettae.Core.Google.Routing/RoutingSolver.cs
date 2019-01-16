using System;
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
            CapacitatedVehicleRoutingProblemWithTimeWindows.Main(new String[0]);
            throw new NotImplementedException();
        }
    }
}
