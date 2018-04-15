using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    // TODO MEYERJ split in multiple classes
    class VariableBuilder
    {
        public GLS.Solver Solver { get; private set; }
        public int NumberLocations { get; private set; }
        public GLS.Variable[] Potential { get; private set; }
        public GLS.Variable[,] UsesWay { get; private set; }
        public int[,] Distances { get; private set; }

        public VariableBuilder(GLS.Solver solver)
        {
            Solver = solver;
        }

        public void CreateVariables(int[,] distances)
        {
            Distances = distances;
            NumberLocations = distances.GetLength(0);

            CreateLocations();
            CreateWays();
        }

        private void CreateLocations()
        {
            Potential = Solver.MakeIntVarArray(NumberLocations, 0, double.MaxValue);
        }

        private void CreateWays()
        {
            UsesWay = Solver.MakeBoolVarMatrix(NumberLocations, NumberLocations);
        }
    }
}
