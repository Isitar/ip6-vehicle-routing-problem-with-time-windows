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
        public int NumberOfSantas { get; }
        public int NumberOfVisits { get; }
        public int NumberOfDays { get; }
        public int[] SlicesPerDay { get; }
        public SolverVariables Variables { get; }

        public SolverData(SolverInputData solverInputData, GLS.Solver solver)
        {
            Input = solverInputData;
            Solver = solver;
            NumberOfSantas = solverInputData.Santas.Length;
            NumberOfVisits = solverInputData.VisitsLength.Length;
            NumberOfDays = solverInputData.Visits.Length;
            SlicesPerDay = solverInputData.Santas.Select(d => d.GetLength(1)).ToArray();
            Variables = new SolverVariables();
        }
    }
}
