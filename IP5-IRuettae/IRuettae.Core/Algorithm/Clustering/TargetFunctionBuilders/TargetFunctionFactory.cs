using System;
using System.Linq;
using Google.OrTools.LinearSolver;
using IRuettae.Core.Algorithm.Clustering.Detail;

namespace IRuettae.Core.Algorithm.Clustering.TargetFunctionBuilders
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
                    return MinTimeTargetfunction(weight);
                case TargetType.MinTimePerSanta:
                    return MinTimePerSantaTargetfunction(weight);
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }

        private LinearExpr MinTimeTargetfunction(double? weight = 1)
        {
            var sum = new LinearExpr[solverData.NumberOfSantas];
            foreach (var santa in Enumerable.Range(0,solverData.NumberOfSantas))
            {
                sum[santa] = solverData.Variables.SantaVisitTime[santa] + solverData.Variables.SantaRouteCost[santa];
            }
            
            return sum.Sum();
        }

        private LinearExpr MinTimePerSantaTargetfunction(double? weight = 1)
        {
            var sum = new LinearExpr[solverData.NumberOfSantas];
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                sum[santa] = solverData.Variables.SantaVisitTime[santa] + solverData.Variables.SantaRouteCost[santa];
            }

            return sum.Sum() / solverData.NumberOfSantas;
        }


    }
}
