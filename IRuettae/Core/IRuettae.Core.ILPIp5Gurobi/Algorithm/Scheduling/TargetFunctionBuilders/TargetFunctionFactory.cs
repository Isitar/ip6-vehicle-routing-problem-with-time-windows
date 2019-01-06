using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.Detail;


namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.TargetFunctionBuilders
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
        /// <param name="weight">1 is minimal weight, use higher weight for more important targets</param>
        /// <returns>LinearExpr which should be minimised</returns>
        public GRBLinExpr CreateTargetFunction(TargetType target, int weight = 1)
        {
            switch (target)
            {
                case TargetType.MinTime:
                    return CreateTargetFunctionMinTime(weight);
                case TargetType.TryVisitEarly:
                    return CreateTargetFunctionTryVisitEarly(weight);
                case TargetType.TryVisitDesired:
                    return CreateTargetFunctionTryVisitDesired(weight);
                default:
                    throw new NotSupportedException($"The type {target} is not supported.");
            }
        }

        private GRBLinExpr CreateTargetFunctionTryVisitDesired(int weight)
        {
            var sum = new GRBLinExpr(0);

            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int timeslice = 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        var isDesired = solverData.Input.Visits[day][visit, timeslice] == VisitState.Desired;
                        sum -= solverData.Variables.Visits[day][visit][timeslice] * Convert.ToInt32(isDesired);
                    }
                }
            }

            return sum * weight;
        }

        private GRBLinExpr CreateTargetFunctionTryVisitEarly(int weight)
        {
            var sum = new GRBLinExpr(0);

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                for (int timeslice = 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        sum += solverData.Variables.SantaEnRoute[day][santa][timeslice] * timeslice;

                    }
                }
                // gauss sum formula
            }

            return sum * weight;
        }

        private GRBLinExpr CreateTargetFunctionMinTime(int weight)
        {
            var sum = new GRBLinExpr(0);
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                foreach (var sv in solverData.Variables.SantaEnRoute[day])
                {
                    foreach (var v in sv)
                    {
                        sum += v;
                    }
                }
            }
            return sum * weight;
        }
    }
}
