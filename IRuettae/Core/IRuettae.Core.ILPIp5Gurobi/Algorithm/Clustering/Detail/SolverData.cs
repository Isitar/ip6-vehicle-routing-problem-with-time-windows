using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail
{
    internal class SolverData
    {
        public SolverInputData SolverInputData { get; }
        public GRBModel Model { get; }
        public SolverVariables Variables { get; }

        public int NumberOfSantas => Variables.SantaVisit.Length;
        public int NumberOfVisits => Variables.SantaVisit[0].Length;

        public SolverData(SolverInputData solverInputData, GRBModel model)
        {
            SolverInputData = solverInputData;
            Model = model;
            Variables = new SolverVariables();
        }

        public int SourceDestArrPos(int source, int destination)
        {
            return source * NumberOfVisits + destination;
        }
    }
}
