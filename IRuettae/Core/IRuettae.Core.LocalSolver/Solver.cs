using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
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
        private readonly double vrpTimeLimitFactor;
        private readonly double vrptwTimeLimitFactor;

        /// <summary>
        /// Instantiates a new LocalSolver.Solver class with the given optimization input
        /// </summary>
        /// <param name="input">the optimization input being used to solve the problem</param>
        /// <param name="useFakeSantas"></param>
        /// <param name="vrpTimeLimitFactor"></param>
        /// <param name="vrptwTimeLimitFactor"></param>
        public Solver(OptimizationInput input, double vrpTimeLimitFactor, double vrptwTimeLimitFactor, bool useFakeSantas = false)
        {
            this.input = input;
            this.vrpTimeLimitFactor = vrpTimeLimitFactor;
            this.vrptwTimeLimitFactor = vrptwTimeLimitFactor;
            UseFakeSantas = useFakeSantas;
        }

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

            var vrptwBreaksOnWayTimeLimitFactor = 1 - vrpTimeLimitFactor - vrptwTimeLimitFactor;
            if (Math.Abs(vrpTimeLimitFactor + vrptwTimeLimitFactor + vrptwBreaksOnWayTimeLimitFactor - 1d) > 0.00001)
            {
                throw new Exception("programmer was stupid :). Time limit factors don't add up to 1.");
            }

            using (var localSolver = new localsolver.LocalSolver())
            {
                var model = localSolver.GetModel();

                var solverVariables = new SolverVariables(model, numberOfRoutes, visits, input.RouteCosts, input, numberOfFakeSantas);
                var modelBuilder = new ModelBuilder(solverVariables);

                consoleProgress?.Invoke(this, "Starting to model");
                
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

                modelBuilder.ReAddObjective(false);
                model.Close();
                var vrpPhase = localSolver.CreatePhase();

                vrpPhase.SetTimeLimit((int)(vrpTimeLimitFactor * timeLimitMilliseconds / 1000));

                InitializeSolution(numberOfRoutes, numberOfFakeSantas, solverVariables.VisitSequences, breakDictionary);
                localSolver.Solve();
                vrpPhase.SetTimeLimit(0);
                vrpPhase.SetEnabled(false);
                consoleProgress?.Invoke(this, $"Solved vrp, cost: {model.GetObjective(0).GetDoubleValue()}");

#if DEBUG
                PrintStatistics(consoleProgress, vrpPhase.GetStatistics());
#endif

                #endregion VRP

                // save solution for next phase
                var vrpSolution = SavePhaseSolution(solverVariables);

                #region VRPTW

                model.Open();

                for (var day = 0; day < numberOfDays; day++)
                {
                    for (var santa = 0; santa < input.Santas.Length + numberOfFakeSantas; santa++)
                    {
                        modelBuilder.SetVisitStartingTimes(day, santa);
                        modelBuilder.SetDesiredDuration(day, santa);
                        modelBuilder.SetUnavailableDuration(day, santa);
                        modelBuilder.SetSantaRouteTime(day, santa);
                        modelBuilder.AddOvertimeConstraint(day, santa);
                        modelBuilder.AddRouteTimeSmallerThanDayConstraint(day, santa);
                    }
                }

                var noWaitBetweenVisitConstraint = modelBuilder.AddNoWaitBetweenVisitsConstraint();
                modelBuilder.ReAddObjective(true);
                model.Close();
                var vrptwPhase = localSolver.CreatePhase();
                vrptwPhase.SetTimeLimit((int)(vrptwTimeLimitFactor * timeLimitMilliseconds / 1000));

                // initialize with vrp solution solution
                InitializeSolutionWithPrecalculatedSolution(vrpSolution, solverVariables);

                localSolver.Solve();
                vrptwPhase.SetTimeLimit(0);
                vrptwPhase.SetEnabled(false);

                var vrptwSolution = SavePhaseSolution(solverVariables);

                consoleProgress?.Invoke(this, $"Solved vrptw, cost: {model.GetObjective(0).GetDoubleValue()}");
#if DEBUG
                result.Routes = BuildResultRoutes(numberOfRoutes, solverVariables.VisitSequences, solverVariables.Visits, solverVariables.SantaVisitStartingTimes, numberOfFakeSantas);
                consoleProgress?.Invoke(this, $"cost via resultcalc: {result.Cost()}");
                consoleProgress?.Invoke(this, $"unavailable: : {string.Join(",", solverVariables.SantaUnavailableDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"desired: : {string.Join(",", solverVariables.SantaDesiredDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"routeTime: : {string.Join(",", solverVariables.SantaRouteTime.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"overtime: : {string.Join(",", solverVariables.SantaOvertime.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaWaitBeforeStart: : {string.Join(",", solverVariables.SantaWaitBeforeStart.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaVisitDurations: : {string.Join(",", solverVariables.SantaWalkingTime.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaWalkingTime: : {string.Join(",", solverVariables.SantaVisitDurations.Select(o => o.GetIntValue()))}");

                for (var i = 0; i < numberOfRoutes; i++)
                {
                    consoleProgress?.Invoke(this, $"Santa {i} visit sequence: {string.Join(",", solverVariables.VisitSequences[i].GetCollectionValue().ToArray())} ");
                    consoleProgress?.Invoke(this, $"Santa {i} visit starting time: {string.Join(",", solverVariables.SantaVisitStartingTimes[i].GetArrayValue())} ");
                }
                PrintStatistics(consoleProgress, vrptwPhase.GetStatistics());

#endif

                #endregion VRPTW

                model.Open();
                model.RemoveConstraint(noWaitBetweenVisitConstraint);
                model.Close();

                InitializeSolutionWithPrecalculatedSolution(vrptwSolution, solverVariables);

                var vrptwBreaksOnWayPhase = localSolver.CreatePhase();
                vrptwBreaksOnWayPhase.SetTimeLimit((int)(vrptwBreaksOnWayTimeLimitFactor * timeLimitMilliseconds / 1000));
                localSolver.Solve();

                result.Routes = BuildResultRoutes(numberOfRoutes, solverVariables.VisitSequences, solverVariables.Visits, solverVariables.SantaVisitStartingTimes, numberOfFakeSantas);

#if DEBUG
                result.Routes = BuildResultRoutes(numberOfRoutes, solverVariables.VisitSequences, solverVariables.Visits, solverVariables.SantaVisitStartingTimes, numberOfFakeSantas);
                consoleProgress?.Invoke(this, $"cost via resultcalc: {result.Cost()}");
                consoleProgress?.Invoke(this, $"unavailable: : {string.Join(",", solverVariables.SantaUnavailableDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"desired: : {string.Join(",", solverVariables.SantaDesiredDuration.Select(s => s.GetIntValue().ToString()).ToArray())}");
                consoleProgress?.Invoke(this, $"routeTime: : {string.Join(",", solverVariables.SantaRouteTime.Select(s => s.GetIntValue().ToString()).ToArray())}");
                //consoleProgress?.Invoke(this, $"longestRoute: : {maxRoute.GetIntValue()}");
                consoleProgress?.Invoke(this, $"overtime: : {string.Join(",", solverVariables.SantaOvertime.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaWaitBeforeStart: : {string.Join(",", solverVariables.SantaWaitBeforeStart.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaVisitDurations: : {string.Join(",", solverVariables.SantaWalkingTime.Select(o => o.GetIntValue()))}");
                consoleProgress?.Invoke(this, $"santaWalkingTime: : {string.Join(",", solverVariables.SantaVisitDurations.Select(o => o.GetIntValue()))}");


                for (var i = 0; i < numberOfRoutes; i++)
                {
                    consoleProgress?.Invoke(this, $"Santa {i} visit sequence: {string.Join(",", solverVariables.VisitSequences[i].GetCollectionValue().ToArray())} ");
                    consoleProgress?.Invoke(this, $"Santa {i} visit starting time: {string.Join(",", solverVariables.SantaVisitStartingTimes[i].GetArrayValue())} ");
                    consoleProgress?.Invoke(this, $"Santa {i} wait between visits: {string.Join(",", solverVariables.SantaWaitBetweenVisit[i].Select(x => x.GetIntValue().ToString()))} ");
                }
                PrintStatistics(consoleProgress, vrptwBreaksOnWayPhase.GetStatistics());
#endif

                result.TimeElapsed = sw.ElapsedMilliseconds / 1000;
                result.ResultState = ResultState.Finished;
                return result;
            }
        }

        /// <summary>
        /// initializes solver variables with precalculated phase solution
        /// </summary>
        /// <param name="phaseSolution">the precalculated phase solution</param>
        /// <param name="solverVariables">the solver variables to be filled</param>
        private static void InitializeSolutionWithPrecalculatedSolution(PhaseSolution phaseSolution, SolverVariables solverVariables)
        {
            var numberOfRoutes = solverVariables.NumberOfRoutes;

            for (int s = 0; s <= numberOfRoutes; s++)
            {
                var sequence = solverVariables.VisitSequences[s].GetCollectionValue();
                sequence.Clear();
                foreach (var visit in phaseSolution.SantaVisitSequence[s])
                {
                    sequence.Add(visit);
                }
            }

            for (int s = 0; s < numberOfRoutes; s++)
            {
                if (solverVariables.SantaWaitBetweenVisit != null)
                {
                    // initialize wait on way = 0
                    foreach (var waitBetweenVisit in solverVariables.SantaWaitBetweenVisit[s])
                    {
                        waitBetweenVisit.SetIntValue(0);
                    }
                }

                if (phaseSolution.SantaWaitBeforeStart != null)
                {
                    solverVariables.SantaWaitBeforeStart[s].SetIntValue(phaseSolution.SantaWaitBeforeStart[s]);
                }
            }
        }

        /// <summary>
        /// saves the calculated phase solution
        /// </summary>
        /// <param name="solverVariables">the solver variables used to calculate the vrp/vrptw</param>
        /// <returns>a phase solution object containing the routes</returns>
        private static PhaseSolution SavePhaseSolution(SolverVariables solverVariables)
        {
            var output = new PhaseSolution();
            var numberOfRoutes = solverVariables.NumberOfRoutes;
            output.SantaVisitSequence = new int[numberOfRoutes + 1][];
            output.SantaVisitStartTime = new int[numberOfRoutes][];
            output.SantaWaitBeforeStart = new int[numberOfRoutes];

            for (int s = 0; s <= numberOfRoutes; s++)
            {
                output.SantaVisitSequence[s] = solverVariables.VisitSequences[s].GetCollectionValue().Select(st => (int)st).ToArray();
            }

            if ((solverVariables.SantaVisitStartingTimes != null )
                && (solverVariables.SantaVisitStartingTimes.Length > 0 )
                && !(solverVariables.SantaVisitStartingTimes[0] is null))
            {
                for (int s = 0; s < numberOfRoutes; s++)
                {
                    var startingTimeArr = solverVariables.SantaVisitStartingTimes[s].GetArrayValue();
                    output.SantaVisitStartTime[s] = new int[startingTimeArr.Count()];
                    for (int i = 0; i < startingTimeArr.Count(); i++)
                    {
                        output.SantaVisitStartTime[s][i] = (int) startingTimeArr.GetIntValue(i);
                    }

                    output.SantaWaitBeforeStart[s] = (int) solverVariables.SantaWaitBeforeStart[s].GetIntValue();
                }
            }

            return output;
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

        private void PrintStatistics(EventHandler<string> consoleProgress, LSStatistics stats)
        {
            consoleProgress?.Invoke(this, $"{stats.GetInfo()}");
            consoleProgress?.Invoke(this, $"NbIterations: {stats.GetNbIterations()}");
            consoleProgress?.Invoke(this, $"NbAcceptedMoves: {stats.GetNbAcceptedMoves()}, ({stats.GetPercentAcceptedMoves():F2}%)");
            consoleProgress?.Invoke(this, $"NbInfeasibleMoves: {stats.GetNbInfeasibleMoves()}, ({stats.GetPercentInfeasibleMoves():F2}%)");
            consoleProgress?.Invoke(this, $"NbImprovingMoves: {stats.GetNbImprovingMoves()}, ({stats.GetPercentImprovingMoves():F2}%)");
            consoleProgress?.Invoke(this, $"NbRejectedMoves: {stats.GetNbRejectedMoves()}, ({stats.GetPercentRejectedMoves():F2}%)");
            consoleProgress?.Invoke(this, $"NbMoves: {stats.GetNbMoves()}");
            consoleProgress?.Invoke(this, $"RunningTime: {stats.GetRunningTime()}");
        }
    }
}