using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public OptimizationResult Solve(long timelimit, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            var sw = Stopwatch.StartNew();
            var result = new OptimizationResult
            {
                OptimizationInput = input
            };

            using (var localsolver = new localsolver.LocalSolver())
            {
                var model = localsolver.GetModel();
                var numberOfSantas = input.Santas.Length * input.Days.Length;
                var numberOfDays = input.Days.Length;
                var santaUsed = new LSExpression[numberOfSantas];
                var visitSequences = new LSExpression[numberOfSantas];
                var santaWalkingTime = new LSExpression[numberOfSantas];
                var santaRouteTime = new LSExpression[numberOfSantas];
                var santaWaitingTime = new LSExpression[numberOfSantas];
                var santaVisitDurations = new LSExpression[numberOfSantas];
                var santaDesiredDuration = new LSExpression[numberOfSantas];
                var santaUnavailableDuration = new LSExpression[numberOfSantas];
                var santaVisitStartingTimes = new LSExpression[numberOfSantas];
                var numberOfVisits = input.Visits.Length;
                // Sequence of customers visited by each truck.
                for (int k = 0; k < numberOfSantas; k++)
                    visitSequences[k] = model.List(numberOfVisits);

                model.Constraint(model.Partition(visitSequences));


                var distanceArray = model.Array(input.RouteCosts);
                var distanceFromHomeArray = model.Array(input.Visits.Select(v => v.WayCostFromHome).ToArray());
                var distanceToHomeArray = model.Array(input.Visits.Select(v => v.WayCostToHome).ToArray());
                var visitDurationArray = model.Array(input.Visits.Select(v => v.Duration).ToArray());
                for (int day = 0; day < numberOfDays; day++)
                {
                    for (int santa = 0; santa < numberOfSantas; santa++)
                    {
                        var s = santa * day;
                        var sequence = visitSequences[s];
                        var c = model.Count(sequence);

                        santaUsed[s] = c > 0;

                        // walking
                        var distSelector = model.Function(i =>
                            distanceArray[sequence[i - 1], sequence[i]] + visitDurationArray[i]);
                        santaWalkingTime[s] = model.Sum(model.Range(1, c), distSelector)
                                              + model.If(santaUsed[s],
                                                  distanceFromHomeArray[sequence[0]] +
                                                  distanceToHomeArray[sequence[c - 1]], 0);


                        // visiting
                        var visitDurationSelector = model.Function(i => visitDurationArray[i]);
                        santaVisitDurations[s] = model.Sum(model.Range(1, c), visitDurationSelector);

                        // time slot
                        var waitingTime = model.Array();

                        var currDayIndex = day; // copy because used in lambda expression
                        var visitStartingTimeSelector = model.Function((i, prev) =>
                            model.If(i == 0, input.Days[currDayIndex].from + distanceFromHomeArray[sequence[0]] + waitingTime[i],
                                prev + distanceArray[sequence[i - 1], sequence[i]] + waitingTime[i])
                        );

                        // desired
                        var visitStartingTime = model.Array(model.Range(1, c), visitStartingTimeSelector);
                        santaVisitStartingTimes[s] = visitStartingTime;
                        var inDesiredSlotSelector = model.Function(i =>
                        {
                            var visitIndex = sequence[i].GetIntValue();
                            var visit = input.Visits[visitIndex];
                            if (visit.Desired.Length == 0) return model.Int(0, 0);
                            var x = visitStartingTime[sequence[i]].GetValue();
                            var y = x + visit.Duration;

                            foreach (var desired in visit.Desired)
                            {
                                var a = desired.from;
                                var b = desired.to;

                                // check intersection
                                if ((y < a) || (x > b))
                                {
                                    continue;
                                }

                                return model.Min(b, visitStartingTime[sequence[i]]) - model.Max(a,
                                           visitStartingTime[sequence[i]] + visitDurationArray[sequence[i]]);
                            }

                            return model.Int(0, 0);

                        });
                        var timeInDesiredSlot = model.Array(model.Range(1, c), inDesiredSlotSelector);
                        santaDesiredDuration[s] = timeInDesiredSlot;

                        // unavailable
                        var inUnavailableSlotSelector = model.Function(i =>
                        {
                            var visitIndex = sequence[i].GetIntValue();
                            var visit = input.Visits[visitIndex];
                            if (visit.Unavailable.Length == 0) return model.Int(0, 0);
                            var x = visitStartingTime[sequence[i]].GetValue();
                            var y = x + visit.Duration;

                            foreach (var unavailable in visit.Unavailable)
                            {
                                var a = unavailable.from;
                                var b = unavailable.to;

                                // check intersection
                                if ((y < a) || (x > b))
                                {
                                    continue;
                                }

                                return model.Min(b, visitStartingTime[sequence[i]]) - model.Max(a,
                                           visitStartingTime[sequence[i]] + visitDurationArray[sequence[i]]);
                            }

                            return model.Int(0, 0);

                        });
                        var timeInUnavailableSlot = model.Array(model.Range(1, c), inUnavailableSlotSelector);
                        santaUnavailableDuration[s] = timeInUnavailableSlot;

                        santaWaitingTime[s] = model.Sum(waitingTime);

                        // sum all up
                        santaRouteTime[s] = santaWalkingTime[s] + santaVisitDurations[s] + santaWaitingTime[s];

                        // constraint
                        model.Constraint(santaRouteTime[s] <= input.Days[currDayIndex].to - input.Days[currDayIndex].from);
                    }
                }

                var maxRoute = model.Max(santaRouteTime);
                const int hour = 3600;
                model.Minimize(
                    (120d / hour) * model.Sum(santaUnavailableDuration) +
                    (-20d / hour) * model.Sum(santaDesiredDuration) +
                    (40d / hour) * model.Sum(santaRouteTime) +
                    (30d / hour) * maxRoute
                    );

                model.Close();

                // Parameterizes the solver.
                var phase = localsolver.CreatePhase();
                phase.SetTimeLimit((int)((timelimit - sw.ElapsedMilliseconds) / 1000));

                localsolver.Solve();
                result.Routes = new Route[numberOfSantas];
                for (int i = 0; i < numberOfSantas; i++)
                {
                    var visitIds = visitSequences[i].GetCollectionValue()
                        .Select(v => (int)v).ToArray();
                    var visitStartingTimes = santaVisitStartingTimes[i].GetCollectionValue().Select(s => (int)s).ToArray();
                    var route = new Route
                    {
                        SantaId = i % numberOfDays,
                        Waypoints = Enumerable.Range(0, visitIds.Length).Select(j =>
                             new Waypoint
                             {
                                 StartTime = visitStartingTimes[j],
                                 VisitId = visitIds[j],
                             }).ToArray()
                    };
                    result.Routes[i] = route;
                }
            }
            result.TimeElapsed = sw.ElapsedMilliseconds;
            sw.Stop();
            return result;

        }
    }
}
