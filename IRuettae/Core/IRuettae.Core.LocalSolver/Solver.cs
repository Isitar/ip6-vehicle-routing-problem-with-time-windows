using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using localsolver;

namespace IRuettae.Core.LocalSolver
{
    public class Solver : ISolver
    {
        private readonly OptimizationInput input;

        public Solver(OptimizationInput input)
        {
            this.input = input;
        }
        public OptimizationResult Solve(int timelimit, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            var result = new OptimizationResult
            {
                OptimizationInput = input
            };

            using (var localsolver = new localsolver.LocalSolver())
            {
                var model = localsolver.GetModel();
                var numberOfSantas = input.Santas.Length;
                var santaUsed = new LSExpression[numberOfSantas];
                var visitSequences = new LSExpression[numberOfSantas];
                var santaWalkingTime = new LSExpression[numberOfSantas];
                var santaRouteTime = new LSExpression[numberOfSantas];

                var santaVisitDurations = new LSExpression[numberOfSantas];

                var numberOfVisits = input.Visits.Length;
                // Sequence of customers visited by each truck.
                for (int k = 0; k < numberOfSantas; k++)
                    visitSequences[k] = model.List(numberOfVisits);

                model.Constraint(model.Partition(visitSequences));


                var distanceArray = model.Array(input.RouteCosts);
                var distanceFromHomeArray = model.Array(input.Visits.Select(v => v.WayCostFromHome).ToArray());
                var distanceToHomeArray = model.Array(input.Visits.Select(v => v.WayCostToHome).ToArray());
                var visitDurationArray = model.Array(input.Visits.Select(v => v.Duration).ToArray());

                for (int s = 0; s < numberOfSantas; s++)
                {
                    var sequence = visitSequences[s];
                    var c = model.Count(sequence);

                    santaUsed[s] = c > 0;

                    var distSelector = model.Function(i => distanceArray[sequence[i - 1], sequence[i]] + visitDurationArray[i]);
                    var visitSelector = model.Function(i => visitDurationArray[i]);
                    santaWalkingTime[s] = model.Sum(model.Range(1, c), distSelector)
                                        + model.If(santaUsed[s], distanceFromHomeArray[sequence[0]] + distanceToHomeArray[sequence[c - 1]], 0);
                    santaVisitDurations[s] = model.Sum(model.Range(1, c), visitSelector);
                    santaRouteTime[s] = santaWalkingTime[s] + santaVisitDurations[s];
                    model.Constraint(santaRouteTime[s] <= input.Days[0].to - input.Days[0].from); // working hours from 17:00 to 21:00
                }

                var maxRoute = model.Max(santaRouteTime);
                const int hour = 3600;
                model.Minimize((40d / hour) * model.Sum(santaRouteTime) +
                               (30d / hour) * maxRoute);

                model.Close();

                // Parameterizes the solver.
                var phase = localsolver.CreatePhase();
                phase.SetTimeLimit(50);

                localsolver.Solve();
                result.Routes = new Route[numberOfSantas];
                for (int i = 0; i < numberOfSantas; i++)
                {
                    var route = new Route
                    {
                        Waypoints = visitSequences[i].GetCollectionValue()
                            .Select(v => new Waypoint {VisitId = (int) v}).ToArray()
                    };
                    result.Routes[i] = route;
                }

            }


            return result;

        }
    }
}
