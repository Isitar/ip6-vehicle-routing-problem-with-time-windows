using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.Detail
{
    internal class VariableBuilder
    {
        private SolverData solverData;

        public VariableBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public void CreateVariables()
        {
            CreateVisitsPerSanta();
            CreateVisits();
            CreateSantaVisits();
            CreateUsesSanta();
            CreateVisitStart();
            CreateSantaEnRoute();
        }

        private void CreateVisits()
        {
            solverData.Variables.Visits = new GRBVar[solverData.NumberOfDays][][];

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                var rows = solverData.NumberOfVisits;
                solverData.Variables.Visits[day] = new GRBVar[rows][];
                var cols = solverData.SlicesPerDay[day];
                for (int v = 0; v < rows; v++)
                {
                    solverData.Variables.Visits[day][v] = solverData.Model.AddVars(cols, GRB.BINARY);
                }

                for (var col = 0; col < cols; col++)
                {
                    solverData.Model.AddConstr(solverData.Variables.Visits[day][0][col] == 0, null);
                }
            }
        }

        private void CreateSantaVisits()
        {
            solverData.Variables.SantaVisits = new GRBVar[solverData.NumberOfSantas][];
            for (var s = 0; s < solverData.NumberOfSantas; s++)
            {
                solverData.Variables.SantaVisits[s] = solverData.Model.AddVars(solverData.NumberOfVisits, GRB.BINARY);
                solverData.Model.AddConstr(solverData.Variables.SantaVisits[s][0] == 0, null);
            }
        }

        private void CreateVisitsPerSanta()
        {
            solverData.Variables.VisitsPerSanta = new GRBVar[solverData.NumberOfDays][][][];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                solverData.Variables.VisitsPerSanta[i] = new GRBVar[solverData.NumberOfSantas][][];

                var slicesPerDay = solverData.SlicesPerDay[i];

                for (int s = 0; s < solverData.NumberOfSantas; s++)
                {
                    solverData.Variables.VisitsPerSanta[i][s] = new GRBVar[solverData.NumberOfVisits][];
                    for (int v = 0; v < solverData.NumberOfVisits; v++)
                    {
                        solverData.Variables.VisitsPerSanta[i][s][v] = solverData.Model.AddVars(slicesPerDay, GRB.BINARY);

                        for (var col = 0; col < slicesPerDay; col++)
                        {
                            solverData.Model.AddConstr(solverData.Variables.VisitsPerSanta[i][s][0][col] == 0, null);
                        }
                    }
                }
            }
        }

        private void CreateUsesSanta()
        {
            solverData.Variables.UsesSanta = new GRBVar[solverData.NumberOfDays][];
            for (int d = 0; d < solverData.NumberOfDays; d++)
            {
                solverData.Variables.UsesSanta[d] = solverData.Model.AddVars(solverData.NumberOfSantas, GRB.BINARY);
            }
        }

        private void CreateVisitStart()
        {
            solverData.Variables.VisitStart = new GRBVar[solverData.NumberOfDays][][];

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                solverData.Variables.VisitStart[day] = new GRBVar[solverData.NumberOfVisits][];
                for (int v = 0; v < solverData.NumberOfVisits; v++)
                {
                    solverData.Variables.VisitStart[day][v] = solverData.Model.AddVars(solverData.SlicesPerDay[day], GRB.BINARY);
                    for (var col = 0; col < solverData.SlicesPerDay[day]; col++)
                    {
                        solverData.Model.AddConstr(solverData.Variables.VisitStart[day][0][col] == 0, null);
                    }
                }
            }
        }

        private void CreateSantaEnRoute()
        {
            solverData.Variables.SantaEnRoute = new GRBVar[solverData.NumberOfDays][][];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                solverData.Variables.SantaEnRoute[i] = new GRBVar[solverData.NumberOfSantas][];
                var cols = solverData.SlicesPerDay[i];
                for (int s = 0; s < solverData.NumberOfSantas; s++)
                {
                    solverData.Variables.SantaEnRoute[i][s] = solverData.Model.AddVars(cols, GRB.BINARY);
                }
            }
        }
    }
}
