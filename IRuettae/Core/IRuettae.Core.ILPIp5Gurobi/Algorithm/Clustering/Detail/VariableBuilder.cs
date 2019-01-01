using System.Linq;
using Gurobi;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail
{
    internal class VariableBuilder
    {
        private readonly SolverData solverData;


        public VariableBuilder(SolverData solverData)
        {
            this.solverData = solverData;

        }

        public void CreateVariables()
        {
            CreateSantaVisit();
            CreateSantaRouteAndVisitTime();
            CreateSantaUsesWay();
            solverData.Variables.SantaUsed = solverData.Model.AddVars(solverData.NumberOfSantas, GRB.BINARY);
        }

        private void CreateSantaUsesWay()
        {
            solverData.Variables.SantaUsesWay = new GRBVar[solverData.NumberOfSantas][];

            solverData.Variables.SantaWayFlow = new GRBVar[solverData.NumberOfSantas][];
            solverData.Variables.SantaWayHasFlow = new GRBVar[solverData.NumberOfSantas][];
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                solverData.Variables.SantaUsesWay[santa] = solverData.Model.AddVars(solverData.NumberOfVisits * solverData.NumberOfVisits, GRB.BINARY);

                solverData.Variables.SantaWayFlow[santa] = solverData.Model.AddVars(solverData.NumberOfVisits * solverData.NumberOfVisits, GRB.CONTINUOUS);
                foreach (var santaWayFlow in solverData.Variables.SantaWayFlow[santa])
                {
                    solverData.Model.AddConstr(santaWayFlow <= solverData.NumberOfVisits, null);
                }
                solverData.Variables.SantaWayHasFlow[santa] = solverData.Model.AddVars(solverData.NumberOfVisits * solverData.NumberOfVisits, GRB.BINARY);
            }
        }


        private void CreateSantaRouteAndVisitTime()
        {
            solverData.Variables.SantaRouteCost = new GRBVar[solverData.NumberOfSantas];
            solverData.Variables.SantaVisitTime = new GRBVar[solverData.NumberOfSantas];
            var numberOfSantas = solverData.SolverInputData.Santas.GetLength(1);
            foreach (var day in Enumerable.Range(0, solverData.SolverInputData.DayDuration.Length))
            {
                foreach (var santa in Enumerable.Range(0, numberOfSantas))
                {
                    var newIndex = day * numberOfSantas + santa;
                    solverData.Variables.SantaRouteCost[newIndex] = solverData.Model.AddVar(0, double.MaxValue, 0, GRB.CONTINUOUS, $"Day_{day}_Santa_{santa}_routecost");
                    solverData.Variables.SantaVisitTime[newIndex] = solverData.Model.AddVar(0, solverData.SolverInputData.DayDuration[day], 0, GRB.CONTINUOUS, $"Day_{day}_Santa_{santa}_visittime");
                }
            }
        }

        public void CreateSantaVisit()
        {
            var rows = solverData.SolverInputData.Santas.Length;
            solverData.Variables.SantaVisit = new GRBVar[rows][];
            solverData.Variables.SantaVisitBonus = new GRBVar[rows][];
            var cols = solverData.SolverInputData.Visits.GetLength(1);
            for (var i = 0; i < rows; i++)
            {
                solverData.Variables.SantaVisit[i] = solverData.Model.AddVars(cols, GRB.BINARY);
                solverData.Variables.SantaVisitBonus[i] = solverData.Model.AddVars(cols, GRB.BINARY);
            }
            
            //solverData.Variables.SantaVisit = solverData.Model.MakeBoolVarMatrix(rows, cols, "SantaVisit");
            //solverData.Variables.SantaVisitBonus = solverData.Model.MakeBoolVarMatrix(rows, cols, "SantaVisitBonus");
            //solverData.Variables.SantaVisitFlow = solverData.ClusteringILPSolver.MakeNumVarMatrix(rows, cols, 0, solverData.NumberOfVisits, "SantaVisitFlow");
        }

    }
}

