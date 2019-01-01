using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail
{
    internal class SolverData
    {
        public SolverInputData SolverInputData { get; }
        public GRBModel Model { get; }
        public SolverVariables Variables { get; }

        public int NumberOfSantas => Variables.SantaVisit.GetLength(0);
        public int NumberOfVisits => Variables.SantaVisit.GetLength(1);

        public SolverData(SolverInputData solverInputData, GRBModel model)
        {
            SolverInputData = solverInputData;
            Model = model;
            Variables = new SolverVariables();
        }
    }
}
