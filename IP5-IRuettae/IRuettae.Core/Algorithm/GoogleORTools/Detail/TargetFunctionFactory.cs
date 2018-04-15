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
        private readonly VariableBuilder variables;

        public TargetFunctionFactory(VariableBuilder variables)
        {
            this.variables = variables;
        }

        public LinearExpr CreateTargetFunction(TargetType target, double weight)
        {

            switch (target)
            {
                case TargetType.ShortestRoute:
                    return AddTargetFunctionShortestRoute(weight);
                default:
                    throw new NotSupportedException($"The type {target} is not supported.");
            }
        }

        private LinearExpr AddTargetFunctionShortestRoute(double weight)
        {
            double totalDistance = 0;
            foreach (var distance in variables.Distances)
            {
                totalDistance += distance;
            }
            double factor = weight / totalDistance;

            LinearExpr expr = new LinearExpr();
            for (int row = 0; row < variables.NumberLocations; row++)
            {
                for (int col = 0; col < variables.NumberLocations; col++)
                {
                    expr += factor * variables.Distances[row, col] * variables.UsesWay[row, col];
                }
            }
            return expr;
        }
    }
}
