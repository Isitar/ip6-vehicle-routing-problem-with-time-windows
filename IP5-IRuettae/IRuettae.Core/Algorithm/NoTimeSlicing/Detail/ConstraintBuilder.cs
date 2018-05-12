using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.NoTimeSlicing.Detail
{
    internal class ConstraintBuilder
    {
        private SolverData solverData;


        public ConstraintBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public void CreateConstraints()
        {
            // Variables
            SantaVisitDuration();
            SantaUsesWay();
            SantaRouteCost();

            // Real Constraints
            VisitUsedExactlyOnce();
            StartVisitVistedByEveryUsedSanta();
        }

        private void StartVisitVistedByEveryUsedSanta()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    solverData.Solver.Add(solverData.Variables.SantaVisit[santa, 0] >= solverData.Variables.SantaVisit[santa, visit]);
                }

                solverData.Solver.Add(solverData.Variables.SantaVisit[santa, 0] <= 1);


            }
        }

        private void SantaUsesWay()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    var sourceVisitedBySanta = solverData.Variables.SantaVisit[santa, source];
                    for (var destination = source; destination < solverData.NumberOfVisits; destination++) 
                    {
                        var destinationVisitedBySanta = solverData.Variables.SantaVisit[santa, destination];

                        solverData.Solver.Add(solverData.Variables.SantaUsesWay[santa][source, destination] <= sourceVisitedBySanta);
                        solverData.Solver.Add(solverData.Variables.SantaUsesWay[santa][source, destination] <= destinationVisitedBySanta);
                        solverData.Solver.Add(solverData.Variables.SantaUsesWay[santa][source, destination] >= sourceVisitedBySanta + destinationVisitedBySanta - 1);

                        solverData.Solver.Add(solverData.Variables.SantaUsesWay[santa][destination, source] <= sourceVisitedBySanta);
                        solverData.Solver.Add(solverData.Variables.SantaUsesWay[santa][destination, source] <= destinationVisitedBySanta);
                        solverData.Solver.Add(solverData.Variables.SantaUsesWay[santa][destination, source] >= sourceVisitedBySanta + destinationVisitedBySanta - 1);
                    }
                }
            }
        }

        private void SantaRouteCost()
        {
            // idea:
            // alle möglichen Wege zusammenzählen = cost
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var expr = new LinearExpr();
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        expr += solverData.Variables.SantaUsesWay[santa][source, destination] *solverData.SolverInputData.Distances[source, destination];
                        expr += solverData.Variables.SantaUsesWay[santa][destination, source] * solverData.SolverInputData.Distances[destination, source];
                    }
                }
                solverData.Solver.Add(solverData.Variables.SantaRouteCost[santa] == expr);
            }
        }

        private void SantaVisitDuration()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var expr = new LinearExpr();
                foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    expr += solverData.Variables.SantaVisit[santa, visit] *
                            solverData.SolverInputData.VisitsDuration[visit];
                }

                solverData.Solver.Add(solverData.Variables.SantaVisitTime[santa] == expr);
            }
        }

        public void VisitUsedExactlyOnce()
        {
            // 1 because start visit is visited multiple times
            for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                var visitVisited = new LinearExpr();
                foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
                {
                    visitVisited += solverData.Variables.SantaVisit[santa, visit];
                }

                solverData.Solver.Add(visitVisited == 1);
            }
        }
    }
}
