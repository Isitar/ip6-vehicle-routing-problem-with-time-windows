using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Clustering.Detail
{
    internal class SolverData
    {
        public SolverInputData SolverInputData { get; }
        public GLS.Solver Solver { get; }
        public SolverVariables Variables { get; }

        public int NumberOfSantas => Variables.SantaVisit.GetLength(0);
        public int NumberOfVisits => Variables.SantaVisit.GetLength(1);

        public SolverData(SolverInputData solverInputData, GLS.Solver solver)
        {
            SolverInputData = solverInputData;
            Solver = solver;
            Variables = new SolverVariables();
        }
    }
}
