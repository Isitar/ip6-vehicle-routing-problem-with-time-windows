using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class SolverData
    {
        public SolverInputData Input { get; }
        public GLS.Solver Solver { get; }
        public int NumberLocations { get; }
        public SolverVariables Variables { get; }

        public SolverData(SolverInputData solverInputData, GLS.Solver solver)
        {
            Input = solverInputData;
            Solver = solver;
            NumberLocations = solverInputData.VisitsLength.Length;
            Variables = new SolverVariables();
        }
    }
}
