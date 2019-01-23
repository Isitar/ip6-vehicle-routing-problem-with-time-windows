using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IRuettae.Core.LocalSolver.Algorithm;
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
        /// Instantiates a new LocalSolver.Solver class with the given optimization input
        /// </summary>
        /// <param name="input">the optimization input being used to solve the problem</param>
        /// <param name="useWaitsBetweenVisits"></param>
        /// <param name="useFakeSantas"></param>
        public Solver(OptimizationInput input, bool useWaitsBetweenVisits = true, bool useFakeSantas = false)
        {
            this.input = input;
            UseWaitBetweenVisits = useWaitsBetweenVisits;
            UseFakeSantas = useFakeSantas;
        }

        public bool UseWaitBetweenVisits { get; }
        public bool UseFakeSantas { get; }

        /// <inheritdoc />
        public OptimizationResult Solve(long timeLimitMilliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            var sw = Stopwatch.StartNew();
            var result = new OptimizationResult
            {
                OptimizationInput = input
            };

            var numberOfFakeSantas = UseFakeSantas ? input.Visits.Length - input.Santas.Length : 0;
            var numberOfRoutes = (input.Santas.Length + numberOfFakeSantas) * input.Days.Length;
            var numberOfDays = input.Days.Length;
            var visits = input.Visits.ToList();
            var breakDictionary = new Dictionary<(int day, int santa), int>();

            for (var i = 1; i < numberOfDays; i++)
            {
                foreach (var breakVisit in input.Visits.Where(v => v.IsBreak))
                {
                    visits.Add(breakVisit);
                    breakDictionary.Add((day: i, santa: breakVisit.SantaId), visits.Count - 1);
                }
            }


            var vrpTimeLimitFactor = 0.2;
            var vrptwTimeLimitFactor = 0.5;
            var breaksOnWayTimeLimitFactor = 0.3;
            if (Math.Abs(vrpTimeLimitFactor + vrptwTimeLimitFactor + breaksOnWayTimeLimitFactor - 1d) > 0.00001)
            {
                throw new Exception("programmer was stupid :)");
            }


            using (var localSolver = new localsolver.LocalSolver())
            {
                var model = localSolver.GetModel();

                var solverVariables = new SolverVariables(model, numberOfRoutes, visits, input.RouteCosts, input, numberOfFakeSantas);
                var modelBuilder = new ModelBuilder(solverVariables);

                consoleProgress?.Invoke(this, "Starting to model");

                #region model building

                #region VRP

                modelBuilder.AddPartitionConstraint();
                modelBuilder.AddVisitsNotInLastSequenceConstraint();


                for (var day = 0; day < numberOfDays; day++)
                {
                    for (var santa = 0; santa < input.Santas.Length + numberOfFakeSantas; santa++)
                    {
                        modelBuilder.SetSantaUsed(day, santa);

                        // break
                        var breaks = visits.Where(v => v.IsBreak && (v.SantaId == santa)).ToList();
                        if (breaks.Any())
                        {
                            var breakIndex = day == 0 ? visits.IndexOf(breaks.First()) : breakDictionary[(day, santa)];
                            modelBuilder.AddBreakConstraint(day, santa, breakIndex);
                        }

                        // walking
                        modelBuilder.SetSantaWalkingTime(day, santa);
                        modelBuilder.SetSantaVisitDurationTime(day, santa);

                        modelBuilder.AddVisitDurationPlusWalkingTimeSmallerThanDayConstraint(day, santa);

                        modelBuilder.SetSantaRouteTime(day, santa);
                    }
                }
                modelBuilder.AddObjective();
                model.Close();
                var vrpPhase = localSolver.CreatePhase();
                vrpPhase.SetTimeLimit((int)(vrpTimeLimitFactor * timeLimitMilliseconds / 1000));



                InitializeSolution(numberOfRoutes, numberOfFakeSantas, solverVariables.VisitSequences, breakDictionary);
                localSolver.Solve();

                //var solution = new List
                for (var i = 0; i < numberOfRoutes; i++)
                {
                    var visitIds = solverVariables.VisitSequences[i].GetCollectionValue().Select(v => (int)v).ToArray();

                    // breaks
                    for (var visitIdIndex = 0; visitIdIndex < visitIds.Length; visitIdIndex++)
                    {
                        if (visitIds[visitIdIndex] >= input.Visits.Length)
                        {
                            visitIds[visitIdIndex] = visits[visitIds[visitIdIndex]].Id;
                        }
                    }

                    Console.WriteLine($"route {i}: ");
                    foreach (var visitId in visitIds)
                    {
                        Console.WriteLine($"visit {visitId}");
                    }
                }


                return null;
                #endregion
                #endregion

                //    for (var day = 0; day < numberOfDays; day++)
                //    {
                //        for (var santa = 0; santa < input.Santas.Length + numberOfFakeSantas; santa++)
                //        {
                //            // time slot
                //            var currentDayIndex = day; // copy because used in lambda expression

                //            var waitBetweenVisits =
                //                UseWaitBetweenVisits ? model.Array(santaWaitBetweenVisit[s]) : model.Int(0, 0);

                //            var visitStartingTimeSelector = model.Function((i, prev) =>
                //                model.If(i == 0,
                //                    input.Days[currentDayIndex].from + santaWaitBeforeStart[s] + distanceFromHomeArray[sequence[i]],
                //                    prev + visitDurationArray[sequence[i - 1]] + distanceArray[sequence[i - 1], sequence[i]] +
                //                    (UseWaitBetweenVisits ? waitBetweenVisits[sequence[i]] : model.Int(0, 0))
                //                )
                //            );

                //            var visitStartingTime = model.Array(model.Range(0, c), visitStartingTimeSelector);
                //            santaVisitStartingTimes[s] = visitStartingTime;

                //            // desired
                //            var visitDesiredDurationSelector = model.Function(i =>
                //            {
                //                var v = sequence[i];
                //                var nDesired = visitDesiredCountArray[v];

                //                var visitStart = visitStartingTime[i];
                //                var visitEnd = visitStart + visitDurationArray[sequence[i]];

                //                var desiredIntersection = model.Function(n =>
                //                {
                //                    // desired start
                //                    var x = model.If(nDesired == 0, model.Int(0, 0),
                //                        model.At(visitDesiredArray, v, n, model.Int(0, 0)));
                //                    // desired end
                //                    var y = model.If(nDesired == 0, model.Int(0, 0),
                //                        model.At(visitDesiredArray, v, n, model.Int(1, 1)));

                //                    return model.If(model.Or(y < visitStart, x > visitEnd),
                //                        // if no intersection    
                //                        0,
                //                        //else
                //                        model.Min(y, visitEnd) - model.Max(x, visitStart)
                //                    );
                //                });
                //                return model.Sum(model.Range(0, nDesired), desiredIntersection);
                //            });
                //            santaDesiredDuration[s] = model.Sum(model.Range(0, c), visitDesiredDurationSelector);

                //            // unavailable
                //            var visitUnavailableDurationSelector = model.Function(i =>
                //            {
                //                var v = sequence[i];
                //                var nUnavailable = visitUnavailableCountArray[v];

                //                var visitStart = visitStartingTime[i];
                //                var visitEnd = visitStart + visitDurationArray[sequence[i]];

                //                var unavailableIntersection = model.Function(n =>
                //                {
                //                    // unavailable start
                //                    var x = model.If(nUnavailable == 0, model.Int(0, 0),
                //                        model.At(visitUnavailableArray, v, n, model.Int(0, 0)));
                //                    // unavailable end
                //                    var y = model.If(nUnavailable == 0, model.Int(0, 0),
                //                        model.At(visitUnavailableArray, v, n, model.Int(1, 1)));

                //                    return model.If(model.Or(y < visitStart, x > visitEnd),
                //                        // if no intersection    
                //                        0,
                //                        //else
                //                        model.Min(y, visitEnd) - model.Max(x, visitStart)
                //                    );
                //                });
                //                return model.Sum(model.Range(0, nUnavailable), unavailableIntersection);
                //            });
                //            santaUnavailableDuration[s] = model.Sum(model.Range(0, c), visitUnavailableDurationSelector);

                //            // sum all up
                //            //santaRouteTime[s] = santaWalkingTime[s] + santaVisitDurations[s] + (UseWaitBetweenVisits ? model.Sum(santaWaitBetweenVisit[s]) : model.Int(0, 0));

                //            // constraint
                //            model.Constraint(model.If(santaUsed[s], visitStartingTime[c - 1] + visitDurationArray[sequence[c - 1]] + distanceToHomeArray[sequence[c - 1]], 0) <= input.Days[currentDayIndex].to + santaOvertime[s]);
                //        }
            }

            //    #endregion

            //    #region costFunction

            //    var maxRoute = model.Max(santaRouteTime);
            //    const int hour = 3600;
            //    var additionalSantaCount = model.Float(0, 0);
            //    var additionalSantaRouteTime = model.Float(0, 0);
            //    for (var d = 0; d < numberOfDays; d++)
            //    {
            //        for (var i = 0; i < numberOfFakeSantas; i++)
            //        {
            //            var index = d * (input.Santas.Length + numberOfFakeSantas) + input.Santas.Length + i;
            //            additionalSantaCount += santaUsed[index];
            //            additionalSantaRouteTime += santaRouteTime[index];
            //        }
            //    }


            //    var costFunction =
            //        400 * additionalSantaCount +
            //        40d / hour * additionalSantaRouteTime +
            //        120d / hour * model.Sum(santaUnavailableDuration) +
            //        120d / hour * model.Sum(santaOvertime) +
            //        -20d / hour * model.Sum(santaDesiredDuration) +
            //        40d / hour * model.Sum(santaRouteTime) +
            //        30d / hour * maxRoute;

            //    #endregion

            //    model.Minimize(costFunction);

            //    model.Close();
            //    consoleProgress?.Invoke(this, "Done modeling");



            //    var phase = localSolver.CreatePhase();
            //    phase.SetTimeLimit((int)((timeLimitMilliseconds - sw.ElapsedMilliseconds) / 1000));


            //    localSolver.Solve();
            //    consoleProgress?.Invoke(this, "Done solving");
            //    //
            //    result.Routes = BuildResultRoutes(numberOfRoutes, visitSequences, visits, santaVisitStartingTimes, numberOfFakeSantas);

            //    consoleProgress?.Invoke(this, $"unavailable: : {string.Join(",", santaUnavailableDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
            //    consoleProgress?.Invoke(this, $"desired: : {string.Join(",", santaDesiredDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
            //    consoleProgress?.Invoke(this, $"routeTime: : {string.Join(",", santaRouteTime.Select(s => s.GetIntValue().ToString()).ToArray())}");
            //    consoleProgress?.Invoke(this, $"longestRoute: : {maxRoute.GetIntValue()}");
            //    consoleProgress?.Invoke(this, $"overtime: : {string.Join(",", santaOvertime.Select(o => o.GetIntValue()))}");
            //    consoleProgress?.Invoke(this, $"santaWaitBeforeStart: : {string.Join(",", santaWaitBeforeStart.Select(o => o.GetIntValue()))}");
            //    consoleProgress?.Invoke(this, $"santaVisitDurations: : {string.Join(",", santaWalkingTime.Select(o => o.GetIntValue()))}");
            //    consoleProgress?.Invoke(this, $"santaWalkingTime: : {string.Join(",", santaVisitDurations.Select(o => o.GetIntValue()))}");


            //    for (var i = 0; i < numberOfRoutes; i++)
            //    {
            //        consoleProgress?.Invoke(this, $"Santa {i} visit sequence: {string.Join(",", visitSequences[i].GetCollectionValue().ToArray())} ");
            //        consoleProgress?.Invoke(this, $"Santa {i} visit starting time: {string.Join(",", santaVisitStartingTimes[i].GetArrayValue())} ");
            //        if (UseWaitBetweenVisits)
            //        {
            //            consoleProgress?.Invoke(this, $"Santa {i} wait between visits: {string.Join(",", santaWaitBetweenVisit[i].Select(x => x.GetIntValue().ToString()))} ");
            //        }
            //    }
            //}

            //result.TimeElapsed = sw.ElapsedMilliseconds / 1000;
            //result.ResultState = ResultState.Finished;
            //sw.Stop();
            //return result;
        }


        /// <summary>
        /// Builds and returns a route array with the given parameters in a solved state
        /// </summary>
        /// <param name="numberOfRoutes">how many routes there are</param>
        /// <param name="visitSequences">the visits per route</param>
        /// <param name="visits">input visits + duplicated breaks</param>
        /// <param name="santaVisitStartingTimes">when the santa starts its visits</param>
        /// <param name="numberOfFakeSantas">how many fake santas there are</param>
        /// <returns>an array containing the routes for an optimization result</returns>
        private Route[] BuildResultRoutes(int numberOfRoutes, LSExpression[] visitSequences, IReadOnlyList<Visit> visits, LSExpression[] santaVisitStartingTimes, int numberOfFakeSantas)
        {
            if (visitSequences == null)
            {
                throw new ArgumentNullException(nameof(visitSequences));
            }

            if (santaVisitStartingTimes == null)
            {
                throw new ArgumentNullException(nameof(santaVisitStartingTimes));
            }

            var routes = new Route[numberOfRoutes];

            for (var i = 0; i < numberOfRoutes; i++)
            {
                var visitIds = visitSequences[i].GetCollectionValue().Select(v => (int)v).ToArray();

                // breaks
                for (var visitIdIndex = 0; visitIdIndex < visitIds.Length; visitIdIndex++)
                {
                    if (visitIds[visitIdIndex] >= input.Visits.Length)
                    {
                        visitIds[visitIdIndex] = visits[visitIds[visitIdIndex]].Id;
                    }
                }

                if (visitIds.Length == 0)
                {
                    continue;
                }

                var visitStartingTimes = santaVisitStartingTimes[i];
                var route = new Route
                {
                    SantaId = i % (input.Santas.Length + numberOfFakeSantas),
                    Waypoints = Enumerable.Range(0, visitIds.Length).Select(j =>
                        new Waypoint
                        {
                            StartTime = (int)visitStartingTimes.GetArrayValue().GetIntValue(j),
                            VisitId = visitIds[j]
                        }).ToArray()
                };
                var sortedWaypoints = route.Waypoints.OrderBy(wp => wp.StartTime).ToList();
                var wayCostFromHome = input.Visits
                    .First(v => v.Id == sortedWaypoints.First().VisitId)
                    .WayCostFromHome;
                var lastVisit = input.Visits.First(v => v.Id == sortedWaypoints.Last().VisitId);

                route.Waypoints = route.Waypoints
                    .Prepend(new Waypoint
                    {
                        StartTime = sortedWaypoints.First().StartTime - wayCostFromHome,
                        VisitId = Constants.VisitIdHome
                    })
                    .Append(new Waypoint
                    {
                        StartTime = sortedWaypoints.Last().StartTime + lastVisit.Duration + lastVisit.WayCostToHome,
                        VisitId = Constants.VisitIdHome
                    }).ToArray();

                routes[i] = route;
            }

            return routes;
        }

        /// <summary>
        /// Initializes the visitSequences for every santa by adding visits to them
        /// Respects breaks
        /// </summary>
        /// <param name="numberOfRoutes">how many routes there are</param>
        /// <param name="numberOfFakeSantas">how many fake santas were added</param>
        /// <param name="visitSequences">the visitSequences to be filled by this method</param>
        /// <param name="breakDictionary">break lookup dictionary</param>
        private void InitializeSolution(int numberOfRoutes, int numberOfFakeSantas, LSExpression[] visitSequences, IReadOnlyDictionary<(int day, int santa), int> breakDictionary)
        {
            if (visitSequences == null)
            {
                throw new ArgumentNullException(nameof(visitSequences));
            }

            var numberOfVisitsPerSanta = Math.Floor((double)(input.Visits.Count(v => !v.IsBreak) / numberOfRoutes));
            var visitIndex = 0;
            for (var day = 0; day < input.Days.Length; day++)
            {
                for (var santa = 0; santa < input.Santas.Length + numberOfFakeSantas; santa++)
                {
                    var s = (input.Santas.Length + numberOfFakeSantas) * day + santa;
                    var santaVisits = visitSequences[s].GetCollectionValue();
                    santaVisits.Clear();


                    if (input.Visits.Any(v => v.IsBreak && (v.SantaId == santa)))
                    {
                        santaVisits.Add(day == 0
                            ? input.Visits.First(v => v.IsBreak && (v.SantaId == santa)).Id
                            : breakDictionary[(day, santa)]);
                    }

                    for (var i = 0; i < numberOfVisitsPerSanta; i++)
                    {
                        santaVisits.Add(visitIndex);
                        visitIndex++;
                    }
                }
            }
        }


    }
}