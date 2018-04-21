using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class VariableBuilder
    {
        private SolverData solverData;

        public VariableBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public void CreateVariables()
        {
            CreateLocations();
            CreateWays();
        }

        private void CreateLocations()
        {
            //Potential = Solver.MakeIntVarArray(NumberLocations, 0, double.MaxValue);
        }

        private void CreateWays()
        {
            //UsesWay = Solver.MakeBoolVarMatrix(NumberLocations, NumberLocations);
        }
    }
}
