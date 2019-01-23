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

        public void AddPartitionConstraint()
        {
            model.Constraint(model.Partition(solverVariables.VisitSequences));
        }

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

        public void AddBreakConstraint(int day, int santa, int breakId)
        {
            var s = GetSantaId(day, santa);
            model.Constraint(model.If(solverVariables.SantaUsed[s], model.Contains(solverVariables.VisitSequences[s], breakId), model.Contains(solverVariables.VisitSequences[solverVariables.NumberOfRoutes], breakId)));
        }

        public void AddVisitDurationPlusWalkingTimeSmallerThanDayConstraint(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var dayDuration = solverVariables.OptimizationInput.Days[day].to - solverVariables.OptimizationInput.Days[day].from;
            model.AddConstraint(model.If(solverVariables.SantaUsed[s], solverVariables.SantaVisitDurations[s] + solverVariables.SantaWalkingTime[s], 0) <= dayDuration);
        }

        public void SetSantaUsed(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            solverVariables.SantaUsed[s] = c > 0;
        }

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

        public void SetSantaVisitDurationTime(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            var sequence = solverVariables.VisitSequences[s];
            var c = model.Count(sequence);

            var visitDurationSelector = model.Function(i => solverVariables.VisitDurationArray[sequence[i]]);
            solverVariables.SantaVisitDurations[s] = model.Sum(model.Range(0, c), visitDurationSelector);
        }

        public void SetSantaRouteTime(int day, int santa)
        {
            var s = GetSantaId(day, santa);
            solverVariables.SantaRouteTime[s] = solverVariables.SantaWalkingTime[s] + solverVariables.SantaVisitDurations[s];
        }


        public void AddObjective()
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

            var costFunction =
                400 * additionalSantaCount +
                40d / hour * additionalSantaRouteTime +
                //120d / hour * model.Sum(solverVariables.SantaUnavailableDuration) +
                //120d / hour * model.Sum(solverVariables.SantaOvertime) +
                //-20d / hour * model.Sum(solverVariables.SantaDesiredDuration) +
                40d / hour * model.Sum(solverVariables.SantaRouteTime) +
                30d / hour * maxRoute;

            model.Minimize(costFunction);

        }


        private int GetSantaId(int day, int santa)
        {
            return (solverVariables.NumberOfSantas + solverVariables.NumberOfFakeSantas) * day + santa;
        }
    }
}
