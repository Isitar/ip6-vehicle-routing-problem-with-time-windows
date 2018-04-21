using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    public class SolverData
    {
        public SolverInputData SolverInputData { get; }
        public GLS.Solver Solver { get; }
        public int NumberLocations { get; }
        public GLS.Variable[] Potential { get; set; }
        public GLS.Variable[,] UsesWay { get; set; }

        public SolverData(SolverInputData solverInputData, GLS.Solver solver)
        {
            SolverInputData = solverInputData;
            Solver = solver;
            NumberLocations = solverInputData.VisitsLength.Length;
        }
    }
}
