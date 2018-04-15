using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class ConstraintBuilder
    {
        private VariableBuilder variables;

        public ConstraintBuilder(VariableBuilder variables)
        {
            this.variables = variables;
        }

        public void CreateConstraints()
        {
            CreateSingeVisitConstaint();
            //CreateAllConnectedConstaint();
        }


        private void CreateSingeVisitConstaint()
        {
            for (int location = 0; location < variables.NumberLocations; location++)
            {
                CreateOnlyOneWayInConstraint(location);
                CreateOnlyOneWayOutConstraint(location);
            }
        }

        private void CreateOnlyOneWayInConstraint(int location)
        {
            var expr = new LinearExpr();
            for (int i = 0; i < variables.NumberLocations; i++)
            {
                if (i != location)
                {
                    expr += variables.UsesWay[i, location];
                }
            }
            variables.Solver.Add(expr == 1);
        }

        private void CreateOnlyOneWayOutConstraint(int location)
        {
            var expr = new LinearExpr();
            for (int i = 0; i < variables.NumberLocations; i++)
            {
                if (i != location)
                {
                    expr += variables.UsesWay[location, i];
                }
            }
            variables.Solver.Add(expr == 1);
        }

        private void CreateAllConnectedConstaint()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the maximal potential
        /// Must be bigger or equal than the number of locations
        /// </summary>
        /// <returns></returns>
        private int GetMaxPotential()
        {
            return variables.NumberLocations;
        }
    }
}
