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
        private SolverData solverData;

        public ConstraintBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public void CreateConstraints()
        {
            //CreateSingeVisitConstaint();
            //CreateAllConnectedConstaint();
        }


        private void CreateSingeVisitConstaint()
        {
            //for (int location = 0; location < solverData.NumberLocations; location++)
            //{
            //    CreateOnlyOneWayInConstraint(location);
            //    CreateOnlyOneWayOutConstraint(location);
            //}
        }

        private void CreateOnlyOneWayInConstraint(int location)
        {
            //var expr = new LinearExpr();
            //for (int i = 0; i < solverData.NumberLocations; i++)
            //{
            //    if (i != location)
            //    {
            //        expr += solverData.UsesWay[i, location];
            //    }
            //}
            //solverData.Solver.Add(expr == 1);
        }

        private void CreateOnlyOneWayOutConstraint(int location)
        {
            //var expr = new LinearExpr();
            //for (int i = 0; i < solverData.NumberLocations; i++)
            //{
            //    if (i != location)
            //    {
            //        expr += solverData.UsesWay[location, i];
            //    }
            //}
            //solverData.Solver.Add(expr == 1);
        }

        private void CreateAllConnectedConstaint()
        {
            throw new NotImplementedException();
        }
    }
}
