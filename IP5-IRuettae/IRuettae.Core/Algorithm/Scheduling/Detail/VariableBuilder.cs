using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Scheduling.Detail
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
            solverData.Variables.Visits = new GLS.Variable[solverData.NumberOfDays][,];

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                var rows = solverData.NumberOfVisits;
                var cols = solverData.SlicesPerDay[day];
                solverData.Variables.Visits[day] = solverData.Solver.MakeBoolVarMatrix(rows, cols, $"Visit_Day_{day}");
            }
        }

        private void CreateSantaVisits()
        {
            solverData.Variables.SantaVisits = solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfSantas, solverData.NumberOfVisits, "SantaVisits");
        }

        private void CreateVisitsPerSanta()
        {
            solverData.Variables.VisitsPerSanta = new GLS.Variable[solverData.NumberOfDays][][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                solverData.Variables.VisitsPerSanta[i] = new GLS.Variable[solverData.NumberOfSantas][,];

                var rows = solverData.NumberOfVisits;
                var cols = solverData.SlicesPerDay[i];
                for (int j = 0; j < solverData.NumberOfSantas; j++)
                {
                    solverData.Variables.VisitsPerSanta[i][j] = solverData.Solver.MakeBoolVarMatrix(rows, cols, $"VisitsPerSanta_Day_{i}_Santa_{j}");
                }
            }
        }

        private void CreateUsesSanta()
        {
            solverData.Variables.UsesSanta = solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfDays, solverData.NumberOfSantas, "UsesSanta");
        }

        private void CreateVisitStart()
        {
            solverData.Variables.VisitStart = new GLS.Variable[solverData.NumberOfDays][,];

            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                solverData.Variables.VisitStart[day] = solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfVisits, solverData.SlicesPerDay[day], $"VisitStart_Day{day}");
            }
        }

        private void CreateSantaEnRoute()
        {
            solverData.Variables.SantaEnRoute = new GLS.Variable[solverData.NumberOfDays][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                var rows = solverData.NumberOfSantas;
                var cols = solverData.SlicesPerDay[i];
                solverData.Variables.SantaEnRoute[i] = solverData.Solver.MakeBoolVarMatrix(rows, cols, $"SantaEnRoute_Day_{i}");
            }
        }
    }
}
