using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class TargetFunctionFactory
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
        /// <returns>LinearExpr which should be maximised</returns>
        public LinearExpr CreateTargetFunction(TargetType target, double? weight = 1.0)
        {
            switch (target)
            {
                case TargetType.ShortestRoute:
                    return CreateTargetFunctionShortestRoute(weight);
                case TargetType.MinSantas:
                    return CreateTargetFunctionMinSantas(weight);
                case TargetType.MinSantaShifts:
                    return CreateTargetFunctionMinSantaShifts(weight);
                default:
                    throw new NotSupportedException($"The type {target} is not supported.");
            }
        }

        private LinearExpr CreateTargetFunctionMinSantaShifts(double? weight)
        {
            return -solverData.Variables.NumberOfSantasNeeded.Sum();
        }

        private LinearExpr CreateTargetFunctionMinSantas(double? weight)
        {
            return -solverData.Variables.NumberOfSantasNeededOverall;
        }

        private LinearExpr CreateTargetFunctionShortestRoute(double? weight)
        {
            // TODO MEYERJ may lead to incorrect results if factor is around 0
            // -> investigate further
            //double factor = -1.0;
            //if (weight.HasValue)
            //{
            //    double totalDistance = 0;
            //    foreach (var distance in variables.Distances)
            //    {
            //        totalDistance += distance;
            //    }
            //    factor *= weight.Value / totalDistance;
            //}

            LinearExpr expr = new LinearExpr();
            //for (int row = 0; row < variables.NumberLocations; row++)
            //{
            //    for (int col = 0; col < variables.NumberLocations; col++)
            //    {
            //        expr += (factor * variables.Distances[row, col]) * variables.UsesWay[row, col];
            //    }
            //}
            return expr;
        }
    }
}
