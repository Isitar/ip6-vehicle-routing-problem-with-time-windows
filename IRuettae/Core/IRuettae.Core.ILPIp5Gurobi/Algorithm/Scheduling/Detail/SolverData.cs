using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;


namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.Detail
{
    internal class SolverData
    {
        public SolverInputData Input { get; }
        public GRBModel Model { get; }
        public int NumberOfSantas { get; }
        public int NumberOfVisits { get; }
        public int NumberOfDays { get; }
        public int[] SlicesPerDay { get; }
        public int StartEndPoint { get; }
        public SolverVariables Variables { get; }

        public SolverData(SolverInputData solverInputData, GRBModel model)
        {
            Input = solverInputData;
            Model = model;
            NumberOfSantas = solverInputData.Santas[0].GetLength(0);
            NumberOfVisits = solverInputData.VisitsDuration.Length;
            NumberOfDays = solverInputData.Visits.Length;
            SlicesPerDay = solverInputData.Santas.Select(d => d.GetLength(1)).ToArray();
            StartEndPoint = 0;
            Variables = new SolverVariables();
        }
    }
}
