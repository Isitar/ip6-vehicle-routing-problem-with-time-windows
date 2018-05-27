using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using IRuettae.Core.Algorithm.Scheduling.Detail;

namespace IRuettae.Core.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal class TargetFunctionFactory
    {
        private readonly SolverData solverData;

        public TargetFunctionFactory(SolverData solverData)
        {
            this.solverData = solverData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="weight"></param>
        /// <returns>LinearExpr which should be minimised</returns>
        public LinearExpr CreateTargetFunction(TargetType target, double? weight = 1.0)
        {
            switch (target)
            {
                case TargetType.MinTime:
                    return CreateTargetFunctionMinTime(weight);
                case TargetType.MinSantas:
                    return CreateTargetFunctionMinSantas(weight);
                case TargetType.MinSantaShifts:
                    return CreateTargetFunctionMinSantaShifts(weight);
                case TargetType.TryVisitEarly:
                    return CreateTargetFunctionTryVisitEarly(weight);
                case TargetType.TryVisitDesired:
                    return CreateTargetFunctionTryVisitDesired(weight);
                default:
                    throw new NotSupportedException($"The type {target} is not supported.");
            }
        }

        private LinearExpr CreateTargetFunctionTryVisitDesired(double? weight)
        {
            var sum = new LinearExpr[solverData.NumberOfVisits];
            sum[0] = new LinearExpr();
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                sum[visit] = new LinearExpr();
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int timeslice = 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        var isDesired = solverData.Input.Visits[day][visit, timeslice] == VisitState.Desired;
                        sum[visit] -= solverData.Variables.Visits[day][visit, timeslice] * Convert.ToInt32(isDesired);
                    }
                }
            }

            return LinearExprArrayHelper.Sum(sum) * (weight ?? 1.0);
        }

        private LinearExpr CreateTargetFunctionTryVisitEarly(double? weight)
        {
            var maxWeight = 0.0;
            var sum = new LinearExpr[solverData.NumberOfDays];
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                sum[day] = new LinearExpr();
                for (int timeslice = 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        sum[day] += solverData.Variables.SantaEnRoute[day][santa, timeslice] * timeslice;

                    }
                }
                double slices = solverData.SlicesPerDay[day];

                // gauss sum formula
                maxWeight += (slices * slices + slices) / 2;
            }

            if (weight.HasValue)
            {
                weight /= maxWeight;
            }
            return LinearExprArrayHelper.Sum(sum) * (weight ?? 1.0);
        }

        private LinearExpr CreateTargetFunctionMinTime(double? weight)
        {
            var sum = new LinearExpr[solverData.NumberOfDays];
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                sum[day] = new LinearExpr();
                foreach (var v in solverData.Variables.SantaEnRoute[day])
                {
                    sum[day] += v;
                }
            }
            return LinearExprArrayHelper.Sum(sum);
        }

        private LinearExpr CreateTargetFunctionMinSantaShifts(double? weight)
        {
            return solverData.Variables.NumberOfSantasNeeded.Sum();
        }

        private LinearExpr CreateTargetFunctionMinSantas(double? weight)
        {
            return solverData.Variables.NumberOfSantasNeededOverall;
        }
    }
}
