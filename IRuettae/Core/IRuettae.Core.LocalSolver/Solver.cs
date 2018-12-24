using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


            var routeCostJagged = RouteCostJagged(visits);

            using (var localSolver = new localsolver.LocalSolver())
            {
                var model = localSolver.GetModel();

                #region variable initialisation

                var santaUsed = new LSExpression[numberOfRoutes];
                var visitSequences = new LSExpression[numberOfRoutes + 1];
                var santaWalkingTime = new LSExpression[numberOfRoutes];
                var santaRouteTime = new LSExpression[numberOfRoutes];
                var santaVisitDurations = new LSExpression[numberOfRoutes];
                var santaDesiredDuration = new LSExpression[numberOfRoutes];
                var santaUnavailableDuration = new LSExpression[numberOfRoutes];
                var santaVisitStartingTimes = new LSExpression[numberOfRoutes];

                var santaOvertime = new LSExpression[numberOfRoutes];
                var santaWaitBeforeStart = new LSExpression[numberOfRoutes];
                var santaWaitBetweenVisit = new LSExpression[numberOfRoutes][];
                for (var k = 0; k < numberOfRoutes; k++)
                {
                    visitSequences[k] = model.List(visits.Count);
                    santaOvertime[k] = model.Int(0, int.MaxValue);
                    santaWaitBeforeStart[k] = model.Int(0, int.MaxValue);
                    if (UseWaitBetweenVisits)
                    {
                        santaWaitBetweenVisit[k] = new LSExpression[visits.Count];
                        for (var i = 0; i < santaWaitBetweenVisit[k].Length; i++)
                        {
                            santaWaitBetweenVisit[k][i] = model.Int(0, int.MaxValue);
                        }
                    }
                }

                // overflow for unused santa breaks
                visitSequences[numberOfRoutes] = model.List(visits.Count);


                var distanceArray = model.Array(routeCostJagged);
                var distanceFromHomeArray = model.Array(visits.Select(v => v.WayCostFromHome).ToArray());
                var distanceToHomeArray = model.Array(visits.Select(v => v.WayCostToHome).ToArray());
                var visitDurationArray = model.Array(visits.Select(v => v.Duration).ToArray());

                // desired
                var visitsOnlyDesired = visits
                    .Select(v =>
                        // fake arr
                        v.Desired.Length == 0
                            ? new[] {new[] {-1, -1}}
                            : v.Desired.Select(d => new[] {d.from, d.to}).ToArray()
                    )
                    .ToArray();
                var visitDesiredArray = model.Array(visitsOnlyDesired);
                var visitDesiredCountArray = model.Array(visits.Select(v => v.Desired.Length).ToArray());

                // unavailable
                var visitsOnlyUnavailable = visits
                    .Select(v =>
                        // fake arr
                        v.Unavailable.Length == 0
                            ? new[] {new[] {-1, -1}}
                            : v.Unavailable.Select(d => new[] {d.from, d.to}).ToArray()
                    )
                    .ToArray();
                var visitUnavailableArray = model.Array(visitsOnlyUnavailable);
                var visitUnavailableCountArray = model.Array(visits.Select(v => v.Unavailable.Length).ToArray());

                #endregion

                consoleProgress?.Invoke(this, "Starting to model");

                #region constraints

                model.Constraint(model.Partition(visitSequences));
                for (var i = 0; i < visits.Count; i++)
                {
                    if (!visits[i].IsBreak)
                    {
                        model.AddConstraint(!model.Contains(visitSequences[numberOfRoutes], i));
                    }
                }

                for (var day = 0; day < numberOfDays; day++)
                {
                    for (var santa = 0; santa < input.Santas.Length + numberOfFakeSantas; santa++)
                    {
                        var s = (input.Santas.Length + numberOfFakeSantas) * day + santa;
                        var sequence = visitSequences[s];
                        var c = model.Count(sequence);

                        santaUsed[s] = c > 0;

                        // break
                        var breaks = visits.Where(v => v.IsBreak && (v.SantaId == santa)).ToList();
                        if (breaks.Any())
                        {
                            var breakIndex = day == 0 ? visits.IndexOf(breaks.First()) : breakDictionary[(day, santa)];
                            model.Constraint(model.If(santaUsed[s], model.Contains(visitSequences[s], breakIndex), model.Contains(visitSequences[numberOfRoutes], breakIndex)));
                            //model.Constraint(model.Contains(visitSequences[s], breakIndex));
                        }

                        // walking
                        var distSelector = model.Function(i => distanceArray[sequence[i - 1], sequence[i]]);
                        santaWalkingTime[s] = model.Sum(model.Range(1, c), distSelector)
                                              + model.If(santaUsed[s],
                                                  distanceFromHomeArray[sequence[0]] + distanceToHomeArray[sequence[c - 1]],
                                                  0);

                        // visiting
                        var visitDurationSelector = model.Function(i => visitDurationArray[sequence[i]]);
                        santaVisitDurations[s] = model.Sum(model.Range(0, c), visitDurationSelector);

                        // time slot
                        var currentDayIndex = day; // copy because used in lambda expression

                        var waitBetweenVisits =
                            UseWaitBetweenVisits ? model.Array(santaWaitBetweenVisit[s]) : model.Int(0, 0);

                        var visitStartingTimeSelector = model.Function((i, prev) =>
                            model.If(i == 0,
                                input.Days[currentDayIndex].from + santaWaitBeforeStart[s] + distanceFromHomeArray[sequence[i]],
                                prev + visitDurationArray[sequence[i - 1]] + distanceArray[sequence[i - 1], sequence[i]] +
                                (UseWaitBetweenVisits ? waitBetweenVisits[sequence[i]] : model.Int(0, 0))
                            )
                        );

                        var visitStartingTime = model.Array(model.Range(0, c), visitStartingTimeSelector);
                        santaVisitStartingTimes[s] = visitStartingTime;

                        // desired
                        var visitDesiredDurationSelector = model.Function(i =>
                        {
                            var v = sequence[i];
                            var nDesired = visitDesiredCountArray[v];

                            var visitStart = visitStartingTime[i];
                            var visitEnd = visitStart + visitDurationArray[sequence[i]];

                            var desiredIntersection = model.Function(n =>
                            {
                                // desired start
                                var x = model.If(nDesired == 0, model.Int(0, 0),
                                    model.At(visitDesiredArray, v, n, model.Int(0, 0)));
                                // desired end
                                var y = model.If(nDesired == 0, model.Int(0, 0),
                                    model.At(visitDesiredArray, v, n, model.Int(1, 1)));

                                return model.If(model.Or(y < visitStart, x > visitEnd),
                                    // if no intersection    
                                    0,
                                    //else
                                    model.Min(y, visitEnd) - model.Max(x, visitStart)
                                );
                            });
                            return model.Sum(model.Range(0, nDesired), desiredIntersection);
                        });
                        santaDesiredDuration[s] = model.Sum(model.Range(0, c), visitDesiredDurationSelector);

                        // unavailable
                        var visitUnavailableDurationSelector = model.Function(i =>
                        {
                            var v = sequence[i];
                            var nUnavailable = visitUnavailableCountArray[v];

                            var visitStart = visitStartingTime[i];
                            var visitEnd = visitStart + visitDurationArray[sequence[i]];

                            var unavailableIntersection = model.Function(n =>
                            {
                                // unavailable start
                                var x = model.If(nUnavailable == 0, model.Int(0, 0),
                                    model.At(visitUnavailableArray, v, n, model.Int(0, 0)));
                                // unavailable end
                                var y = model.If(nUnavailable == 0, model.Int(0, 0),
                                    model.At(visitUnavailableArray, v, n, model.Int(1, 1)));

                                return model.If(model.Or(y < visitStart, x > visitEnd),
                                    // if no intersection    
                                    0,
                                    //else
                                    model.Min(y, visitEnd) - model.Max(x, visitStart)
                                );
                            });
                            return model.Sum(model.Range(0, nUnavailable), unavailableIntersection);
                        });
                        santaUnavailableDuration[s] = model.Sum(model.Range(0, c), visitUnavailableDurationSelector);

                        // sum all up
                        santaRouteTime[s] = santaWalkingTime[s] + santaVisitDurations[s] + (UseWaitBetweenVisits ? model.Sum(santaWaitBetweenVisit[s]) : model.Int(0, 0));

                        // constraint
                        model.Constraint(model.If(santaUsed[s], visitStartingTime[c - 1] + visitDurationArray[sequence[c - 1]] + distanceToHomeArray[sequence[c - 1]], 0) <= input.Days[currentDayIndex].to + santaOvertime[s]);
                    }
                }

                #endregion

                #region costFunction

                var maxRoute = model.Max(santaRouteTime);
                const int hour = 3600;
                var additionalSantaCount = model.Float(0, 0);
                var additionalSantaRouteTime = model.Float(0, 0);
                for (var d = 0; d < numberOfDays; d++)
                {
                    for (var i = 0; i < numberOfFakeSantas; i++)
                    {
                        var index = d * (input.Santas.Length + numberOfFakeSantas) + input.Santas.Length + i;
                        additionalSantaCount += santaUsed[index];
                        additionalSantaRouteTime += santaRouteTime[index];
                    }
                }


                var costFunction =
                    400 * additionalSantaCount +
                    40d / hour * additionalSantaRouteTime +
                    120d / hour * model.Sum(santaUnavailableDuration) +
                    120d / hour * model.Sum(santaOvertime) +
                    -20d / hour * model.Sum(santaDesiredDuration) +
                    40d / hour * model.Sum(santaRouteTime) +
                    30d / hour * maxRoute;

                #endregion

                model.Minimize(costFunction);

                model.Close();
                consoleProgress?.Invoke(this, "Done modeling");

                InitializeSolution(numberOfRoutes, numberOfFakeSantas, visitSequences, breakDictionary);

                var phase = localSolver.CreatePhase();
                phase.SetTimeLimit((int) ((timeLimitMilliseconds - sw.ElapsedMilliseconds) / 1000));


                localSolver.Solve();
                consoleProgress?.Invoke(this, "Done solving");
                //
                result.Routes = BuildResultRoutes(numberOfRoutes, visitSequences, visits, santaVisitStartingTimes, numberOfFakeSantas);

                consoleProgress?.Invoke(this, $"unavailable: : {string.Join(",", santaUnavailableDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"desired: : {string.Join(",", santaDesiredDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"routeTime: : {string.Join(",", santaRouteTime.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"longestRoute: : {maxRoute.GetIntValue()}");
                consoleProgress?.Invoke(this, $"overtime: : {string.Join(",", santaOvertime.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaWaitBeforeStart: : {string.Join(",", santaWaitBeforeStart.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaVisitDurations: : {string.Join(",", santaWalkingTime.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaWalkingTime: : {string.Join(",", santaVisitDurations.Select(o => o.GetIntValue()))}");


                for (var i = 0; i < numberOfRoutes; i++)
                {
                    consoleProgress?.Invoke(this, $"Santa {i} visit sequence: {string.Join(",", visitSequences[i].GetCollectionValue().ToArray())} ");
                    consoleProgress?.Invoke(this, $"Santa {i} visit starting time: {string.Join(",", santaVisitStartingTimes[i].GetArrayValue())} ");
                    if (UseWaitBetweenVisits)
                    {
                        consoleProgress?.Invoke(this, $"Santa {i} wait between visits: {string.Join(",", santaWaitBetweenVisit[i].Select(x => x.GetIntValue().ToString()))} ");
                    }
                }
            }

            result.TimeElapsed = sw.ElapsedMilliseconds / 1000;
            result.ResultState = ResultState.Finished;
            sw.Stop();
            return result;
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
                var visitIds = visitSequences[i].GetCollectionValue().Select(v => (int) v).ToArray();

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
                            StartTime = (int) visitStartingTimes.GetArrayValue().GetIntValue(j),
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

            var numberOfVisitsPerSanta = Math.Floor((double) (input.Visits.Count(v => !v.IsBreak) / numberOfRoutes));
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

        /// <summary>
        /// Takes the input route costs and the visits from params to create a jagged array containing all route costs
        /// </summary>
        /// <param name="visits">The input visits including duplicated breaks</param>
        /// <returns>The route costs as a jagged array</returns>
        private int[][] RouteCostJagged(IReadOnlyList<Visit> visits)
        {
            var routeCostJagged = new int[visits.Count][];
            for (var i = 0; i < routeCostJagged.Length; i++)
            {
                routeCostJagged[i] = new int[visits.Count];
                for (var j = 0; j < routeCostJagged[i].Length; j++)
                {
                    if ((i < input.RouteCosts.GetLength(0)) && (j < input.RouteCosts.GetLength(1)))
                    {
                        routeCostJagged[i][j] = input.RouteCosts[i, j];
                    }
                    else
                    {
                        // additional breaks
                        routeCostJagged[i][j] = input.RouteCosts[visits[i].Id, visits[j].Id];
                    }
                }
            }

            return routeCostJagged;
        }
    }
}