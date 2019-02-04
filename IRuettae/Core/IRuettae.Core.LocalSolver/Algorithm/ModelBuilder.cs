using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using localsolver;

namespace IRuettae.Core.LocalSolver.Algorithm
{
    internal class ModelBuilder
    {
        private readonly SolverVariables solverVariables;
        private readonly LSModel model;

        public ModelBuilder(SolverVariables solverVariables)
        {
            this.solverVariables = solverVariables;
            this.model = this.solverVariables.Model;
        }

        /// <summary>
        /// Adds constraint that visitsequences need to be a partition (no overlap and every id included)
        /// </summary>
        public void AddPartitionConstraint()
        {
            model.Constraint(model.Partition(solverVariables.VisitSequences));
        }

        /// <summary>
        /// normal visits cannot be in last sequence (break backup sequence)
        /// </summary>
        public void AddVisitsNotInLastSequenceConstraint()
        {
            for (var i = 0; i < solverVariables.Visits.Count; i++)
            {
                if (!solverVariables.Visits[i].IsBreak)
                {
                    model.AddConstraint(!model.Contains(solverVariables.VisitSequences[solverVariables.NumberOfRoutes], i));
                }
            }
        }

        /// <summary>
        /// Breaks need to be in normal sequences if santa is used, otherwise it needs to be in the backup sequence
        /// </summary>
        /// <param name="day">the day the route takes part</param>
        /// <param name="santa">the santa that takes the route</param>
        /// <param name="breakId">the break to add the constraint for</param>
        public void AddBreakConstraint(int day, int santa, int breakId)
        {
            var s = GetSantaId(day, santa);
            model.Constraint(model.If(solverVariables.SantaUsed[s], model.Contains(solverVariables.VisitSequences[s], breakId), model.Contains(solverVariables.VisitSequences[solverVariables.NumberOfRoutes], breakId)));
        }

        /// <summary>
        /// Adds a constraint that the visitduraiton + walking time needs to be smaller than the dayduration
        /// </summary>
        /// <param name="day">the day the route takes part</param>
        /// <param name="santa">the santa that takes the route</param>
        public void AddVisitDurationPlusWalkingTimeSmallerThanDayConstraint(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var dayDuration = solverVariables.OptimizationInput.Days[day].to - solverVariables.OptimizationInput.Days[day].from;
            model.AddConstraint(model.If(solverVariables.SantaUsed[s], solverVariables.SantaVisitDurations[s] + solverVariables.SantaWalkingTime[s], 0) <= dayDuration);
        }

        /// <summary>
        /// Adds a constraint that the santaRouteTime needs to be smaller than the day duration
        /// </summary>
        /// <param name="day">the day the route takes part</param>
        /// <param name="santa">the santa that takes the route</param>
        public void AddRouteTimeSmallerThanDayConstraint(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var dayDuration = solverVariables.OptimizationInput.Days[day].to - solverVariables.OptimizationInput.Days[day].from;
            model.AddConstraint(model.If(solverVariables.SantaUsed[s], solverVariables.SantaRouteTime[s], 0) <= dayDuration);
        }

        /// <summary>
        /// Adds a constraint that no wait between visits is possible
        /// </summary>
        /// <returns>the constraint as ls expression</returns>
        public LSExpression AddNoWaitBetweenVisitsConstraint()
        {
            var constraint = GetSumWaitBetweenVisits() == 0;
            model.AddConstraint(constraint);
            return constraint;
        }

        /// <summary>
        /// adds a constraint that time after the day-end is considered overtime
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void AddOvertimeConstraint(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);
            model.Constraint(model.If(solverVariables.SantaUsed[s], solverVariables.SantaVisitStartingTimes[s][c - 1] + solverVariables.VisitDurationArray[sequence[c - 1]] + solverVariables.DistanceToHomeArray[sequence[c - 1]], 0) <= solverVariables.OptimizationInput.Days[day].to + solverVariables.SantaOvertime[s]);
        }

        /// <summary>
        /// returns the sum of all waits between visits as lsexpression
        /// </summary>
        /// <returns>the sum of all waits between visits</returns>
        private LSExpression GetSumWaitBetweenVisits()
        {
            var sumWaitBetweenVisits = model.Int(0, 0);
            for (int s = 0; s < solverVariables.NumberOfRoutes; s++)
            {
                sumWaitBetweenVisits += model.Sum(solverVariables.SantaWaitBetweenVisit[s]);
            }
            return sumWaitBetweenVisits;
        }

        /// <summary>
        /// Sets the SantaUsed variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetSantaUsed(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            solverVariables.SantaUsed[s] = c > 0;
        }

        /// <summary>
        /// Sets the SantaWalkingTime variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetSantaWalkingTime(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            var distSelector = model.Function(i => solverVariables.DistanceArray[sequence[i - 1], sequence[i]]);
            solverVariables.SantaWalkingTime[s] = model.Sum(model.Range(1, c), distSelector)
                                  + model.If(solverVariables.SantaUsed[s],
                                      solverVariables.DistanceFromHomeArray[sequence[0]] + solverVariables.DistanceToHomeArray[sequence[c - 1]],
                                      0);
        }

        /// <summary>
        /// Sets the SantaVisitDurations variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetSantaVisitDurationTime(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            var visitDurationSelector = model.Function(i => solverVariables.VisitDurationArray[sequence[i]]);
            solverVariables.SantaVisitDurations[s] = model.Sum(model.Range(0, c), visitDurationSelector);
        }

        /// <summary>
        /// Sets the SantaRouteTime variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetSantaRouteTime(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            if (solverVariables.SantaWaitBetweenVisitArray[s] is null)
            {
                solverVariables.SantaRouteTime[s] = solverVariables.SantaWalkingTime[s] + solverVariables.SantaVisitDurations[s];
            }
            else
            {
                solverVariables.SantaRouteTime[s] = solverVariables.SantaWalkingTime[s] + solverVariables.SantaVisitDurations[s] + model.Sum(solverVariables.SantaWaitBetweenVisit[s]);
            }
        }

        /// <summary>
        /// Sets the SantaVisitStartingTimes variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetVisitStartingTimes(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            var visitStartingTimeSelector = model.Function((i, prev) =>
                            model.If(i == 0,
                                solverVariables.OptimizationInput.Days[day].from + solverVariables.SantaWaitBeforeStart[s] + solverVariables.DistanceFromHomeArray[sequence[i]],
                                prev + solverVariables.VisitDurationArray[sequence[i - 1]] + solverVariables.DistanceArray[sequence[i - 1], sequence[i]] + solverVariables.SantaWaitBetweenVisitArray[s][sequence[i]]
                            )
                        );

            var visitStartingTime = model.Array(model.Range(0, c), visitStartingTimeSelector);
            solverVariables.SantaVisitStartingTimes[s] = visitStartingTime;
        }

        /// <summary>
        /// Sets the SantaDesiredDuration variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetDesiredDuration(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            var visitDesiredDurationSelector = model.Function(i =>
                        {
                            var v = sequence[i];
                            var nDesired = solverVariables.VisitDesiredCountArray[v];

                            var visitStart = solverVariables.SantaVisitStartingTimes[s][i];
                            var visitEnd = visitStart + solverVariables.VisitDurationArray[sequence[i]];

                            var desiredIntersection = model.Function(n =>
                            {
                                // desired start
                                var x = model.If(nDesired == 0, model.Int(0, 0),
                                    model.At(solverVariables.VisitDesiredArray, v, n, model.Int(0, 0)));
                                // desired end
                                var y = model.If(nDesired == 0, model.Int(0, 0),
                                    model.At(solverVariables.VisitDesiredArray, v, n, model.Int(1, 1)));

                                return model.If(model.Or(y < visitStart, x > visitEnd),
                                    // if no intersection
                                    0,
                                    //else
                                    model.Min(y, visitEnd) - model.Max(x, visitStart)
                                );
                            });
                            return model.Sum(model.Range(0, nDesired), desiredIntersection);
                        });
            solverVariables.SantaDesiredDuration[s] = model.Sum(model.Range(0, c), visitDesiredDurationSelector);
        }

        /// <summary>
        /// Sets the SantaUnavailableDuration variable
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        public void SetUnavailableDuration(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            var visitUnavailableDurationSelector = model.Function(i =>
                        {
                            var v = sequence[i];
                            var nUnavailable = solverVariables.VisitUnavailableCountArray[v];

                            var visitStart = solverVariables.SantaVisitStartingTimes[s][i];
                            var visitEnd = visitStart + solverVariables.VisitDurationArray[sequence[i]];

                            var unavailableIntersection = model.Function(n =>
                            {
                                // unavailable start
                                var x = model.If(nUnavailable == 0, model.Int(0, 0),
                                    model.At(solverVariables.VisitUnavailableArray, v, n, model.Int(0, 0)));
                                // unavailable end
                                var y = model.If(nUnavailable == 0, model.Int(0, 0),
                                    model.At(solverVariables.VisitUnavailableArray, v, n, model.Int(1, 1)));

                                return model.If(model.Or(y < visitStart, x > visitEnd),
                                    // if no intersection
                                    0,
                                    //else
                                    model.Min(y, visitEnd) - model.Max(x, visitStart)
                                );
                            });
                            return model.Sum(model.Range(0, nUnavailable), unavailableIntersection);
                        });
            solverVariables.SantaUnavailableDuration[s] = model.Sum(model.Range(0, c), visitUnavailableDurationSelector);
        }

        /// <summary>
        /// Adds the objective function.
        /// Previously set objective functions will be removed
        /// </summary>
        /// <param name="useTimeWindows">Wheter to use time window in cost function or not</param>
        public void ReAddObjective(bool useTimeWindows)
        {
            var maxRoute = model.Max(solverVariables.SantaRouteTime);
            const int hour = 3600;
            var additionalSantaCount = model.Float(0, 0);
            var additionalSantaRouteTime = model.Float(0, 0);
            for (var d = 0; d < solverVariables.OptimizationInput.Days.Length; d++)
            {
                for (var santa = 0; santa < solverVariables.NumberOfFakeSantas; santa++)
                {
                    var index = GetSantaId(d, santa + solverVariables.NumberOfSantas);
                    additionalSantaCount += solverVariables.SantaUsed[index];
                    additionalSantaRouteTime += solverVariables.SantaRouteTime[index];
                }
            }

            LSExpression costFunction;
            if (useTimeWindows)
            {
                costFunction =
                    400 * additionalSantaCount +
                    40d / hour * additionalSantaRouteTime +
                    120d / hour * model.Sum(solverVariables.SantaUnavailableDuration) +
                    120d / hour * model.Sum(solverVariables.SantaOvertime) +
                   -20d / hour * model.Sum(solverVariables.SantaDesiredDuration) +
                    40d / hour * model.Sum(solverVariables.SantaRouteTime) +
                    30d / hour * maxRoute;
            }
            else
            {
                costFunction =
                    400 * additionalSantaCount +
                    40d / hour * additionalSantaRouteTime +

                    40d / hour * model.Sum(solverVariables.SantaRouteTime) +
                    30d / hour * maxRoute;
            }

            // remove previously set objective functions
            for (int objective = 0; objective < model.GetNbObjectives(); objective++)
            {
                model.RemoveObjective(objective);
            }

            model.Minimize(costFunction);
        }

        /// <summary>
        /// Returns the route id for a given santa and day.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="santa"></param>
        /// <returns>the route id</returns>
        private int GetSantaId(int day, int santa)
        {
            return (solverVariables.NumberOfSantas + solverVariables.NumberOfFakeSantas) * day + santa;
        }
    }
}
