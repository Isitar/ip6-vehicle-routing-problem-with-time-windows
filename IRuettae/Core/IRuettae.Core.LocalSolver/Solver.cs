﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using localsolver;

namespace IRuettae.Core.LocalSolver
{
    /// <summary>
    /// IRuettae Solver using LocalSolver
    /// </summary>
    public class Solver : ISolver
    {
        private readonly OptimizationInput input;

        /// <summary>
        /// Instanciates a new LocalSolver.Solver class with the given opzimization input
        /// </summary>
        /// <param name="input">the optimization input being used to solve the problem</param>
        public Solver(OptimizationInput input)
        {
            this.input = input;
        }

        /// <inheritdoc />
        public OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            var sw = Stopwatch.StartNew();
            var result = new OptimizationResult
            {
                OptimizationInput = input
            };

            var routeCostJagged = new int[input.RouteCosts.GetLength(0)][];
            for (int i = 0; i < routeCostJagged.Length; i++)
            {
                routeCostJagged[i] = new int[input.RouteCosts.GetLength(1)];
                for (int j = 0; j < routeCostJagged[i].Length; j++)
                {
                    routeCostJagged[i][j] = input.RouteCosts[i, j];
                }

            }
            using (var localSolver = new localsolver.LocalSolver())
            {
                var model = localSolver.GetModel();
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

                for (int k = 0; k < numberOfSantas; k++)
                    visitSequences[k] = model.List(numberOfVisits);

                model.Constraint(model.Partition(visitSequences));



                var distanceArray = model.Array(routeCostJagged);
                var distanceFromHomeArray = model.Array(input.Visits.Select(v => v.WayCostFromHome).ToArray());
                var distanceToHomeArray = model.Array(input.Visits.Select(v => v.WayCostToHome).ToArray());
                var visitDurationArray = model.Array(input.Visits.Select(v => v.Duration).ToArray());
                var visitsOnlyDesired = input
                    .Visits
                    .Select(v =>
                        v.Desired.Select(d => model.Array(new[] { d.from, d.to }))
                        )
                    .ToArray();

                var visitDesiredArray = model.Array(visitsOnlyDesired);
                var visitDesiredCountArray = model.Array(input.Visits.Select(v => v.Desired.Length).ToArray());

                //var visitDesiredCountArray = model.Array(input.Visits.Select(v => v.Desired.Length).ToArray());
                // var visitUnavailableArray = model.Array(input.Visits.Select(v => v.Unavailable).ToArray());

                for (int day = 0; day < numberOfDays; day++)
                {
                    for (int santa = 0; santa < input.Santas.Length; santa++)
                    {
                        var s = input.Santas.Length * day + santa;
                        var sequence = visitSequences[s];
                        var c = model.Count(sequence);

                        santaUsed[s] = c > 0;

                        // walking
                        var distSelector = model.Function(i => distanceArray[sequence[i - 1], sequence[i]] + visitDurationArray[i]);
                        santaWalkingTime[s] = model.Sum(model.Range(1, c), distSelector)
                                              + model.If(santaUsed[s],
                                                  distanceFromHomeArray[sequence[0]] +
                                                  distanceToHomeArray[sequence[c - 1]], 0);


                        // visiting
                        var visitDurationSelector = model.Function(i => visitDurationArray[sequence[i]]);
                        santaVisitDurations[s] = model.Sum(model.Range(1, c), visitDurationSelector);

                        // time slot
                        //var waitingTime = model.List(numberOfVisits);

                        var currDayIndex = day; // copy because used in lambda expression
                        var visitStartingTimeSelector = model.Function((i, prev) =>
                            model.If(i == 0,
                                input.Days[currDayIndex].from + distanceFromHomeArray[sequence[i]],
                                prev + distanceArray[sequence[i - 1], sequence[i]]
                                )
                        );

                        var visitStartingTime = model.Array(model.Range(0, c), visitStartingTimeSelector);
                        santaVisitStartingTimes[s] = visitStartingTime;

                        // desired
                        var visitDesiredDurationSelector = model.Function(i =>
                        {
                            var v = sequence[i];
                            var desireds = model.Array(visitDesiredArray[v]);
                            var nDesired = visitDesiredCountArray[v];

                            var visitStart = model.Call(visitStartingTimeSelector, i, i - 1);
                            var visitEnd = visitStart + model.Call(visitDurationSelector, i);

                            var desiredsIntersection = model.Function((n) =>
                            {
                                // desired start
                                var desiredsN = model.Array(desireds[n]);
                                var x = desiredsN[0];
                                // desired end
                                var y = desiredsN[1];
                                //return model.Max(model.Min(y, visitEnd) - model.Max(x, visitStart), 0);
                                return model.If(model.Or(y < visitStart, x > visitEnd),
                                    // if no intersection    
                                    0,
                                    //else
                                    model.Min(y, visitEnd) - model.Max(x, visitStart)
                                );
                            });

                            return model.If(nDesired == 0, 0, model.Sum(model.Range(0, nDesired), desiredsIntersection));
                        });



                        santaDesiredDuration[s] = model.Sum(model.Range(0, c), visitDesiredDurationSelector);

                        //// unavailable
                        //var inUnavailableSlotSelector = model.Function(i =>
                        //{
                        //    var visitIndex = sequence[i].GetIntValue();
                        //    var visit = input.Visits[visitIndex];
                        //    if (visit.Unavailable.Length == 0) return model.Int(0, 0);
                        //    var x = visitStartingTime[sequence[i]].GetValue();
                        //    var y = x + visit.Duration;

                        //    foreach (var unavailable in visit.Unavailable)
                        //    {
                        //        var a = unavailable.from;
                        //        var b = unavailable.to;

                        //        // check intersection
                        //        if ((y < a) || (x > b))
                        //        {
                        //            continue;
                        //        }

                        //        return model.Min(b, visitStartingTime[sequence[i]]) - model.Max(a,
                        //                   visitStartingTime[sequence[i]] + visitDurationArray[sequence[i]]);
                        //    }

                        //    return model.Int(0, 0);

                        //});
                        //var timeInUnavailableSlot = model.Array(model.Range(1, c), inUnavailableSlotSelector);
                        //    santaUnavailableDuration[s] = model.Int(0, 0);// timeInUnavailableSlot;

                        //santaWaitingTime[s] = model.Sum(waitingTime);

                        // sum all up
                        santaRouteTime[s] = santaWalkingTime[s] + santaVisitDurations[s] /*+ santaWaitingTime[s]*/;

                        // constraint
                        model.Constraint(santaRouteTime[s] <= input.Days[currDayIndex].to - input.Days[currDayIndex].from);
                    }
                }

                var maxRoute = model.Max(santaRouteTime);
                const int hour = 3600;
                model.Minimize(
                    //  (120d / hour) * model.Sum(santaUnavailableDuration) +
                    (-20d / hour) * model.Sum(santaDesiredDuration) +
                    (40d / hour) * model.Sum(santaRouteTime) +
                    (30d / hour) * maxRoute
                    );

                model.Close();

                // Parameterizes the solver.
                var phase = localSolver.CreatePhase();
                phase.SetTimeLimit((int)((timelimitMiliseconds - sw.ElapsedMilliseconds) / 1000));

                localSolver.Solve();
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
