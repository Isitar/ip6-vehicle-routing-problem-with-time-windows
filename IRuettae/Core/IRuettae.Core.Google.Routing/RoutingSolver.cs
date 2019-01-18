using System;
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
            //CapacitatedVehicleRoutingProblemWithTimeWindows.Main(new String[0]);

            // transform input
            var data = new RoutingData(input);
            new SantaCreator(data).Create(starterData.MaxNumberOfSantas);
            new VisitCreator(data).Create();
            new UnavailableCreator(data).Create();

            // create RoutingModel
            //new RoutingModelCreator(data).Create();

            // create solver settings


            throw new NotImplementedException();
        }
    }
}
