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
        private readonly SolverData solverData;

        public ConstraintBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        private GLS.Solver Solver => solverData.Solver;

        public void CreateConstraints()
        {
            // Variables
            SantaVisitDuration();
            SantaGraphEdge();
            SantaRouteCost();


            // Real Constraints
            VisitUnavailable();
            VisitUsedExactlyOnce();
            StartVisitVistedByEveryUsedSanta();

            SantaTimeConstraint();

            MSTConstraints();

            //Performance
            PerformanceConstraints();
        }

        private void SantaTimeConstraint()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                Solver.Add(
                    solverData.Variables.SantaRouteCost[santa] +
                    solverData.Variables.SantaVisitTime[santa] <=
                           solverData.SolverInputData.DayDuration[santa / realSantaCount]);
            }
        }

        private void MSTConstraints()
        {
            MST_OnlyAvailableEdges();

            MST_NumberOfEdgesConstraint();
            MST_MaxDestinationOfOnce();

            MST_SourceAndDestAtLeastOnce();
            MST_NoSelfie();
            MST_NoBackAndFourth();


            MST_Flow();
        }

        private void MST_Flow()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var santaWayFlow = solverData.Variables.SantaWayFlow[santa];
                var santaWayHasFlow = solverData.Variables.SantaWayHasFlow[santa];
                var santaUsesWay = solverData.Variables.SantaUsesWay[santa];


                // add flow for start destination
                Solver.Add(solverData.Variables.SantaVisitFlow[santa, 0] == solverData.NumberOfVisits);


                // flow only possible if neighbours have flow
                for (var source = 1; source < solverData.NumberOfVisits; source++)
                {
                    var sumFlowNeightbours = new LinearExpr();
                    foreach (var incomingNeighbours in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        sumFlowNeightbours += santaWayFlow[incomingNeighbours, source];
                    }

                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Solver.Add(santaWayFlow[source, destination] <= sumFlowNeightbours - 1);
                    }
                }

                // spread flow
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Solver.Add(santaWayFlow[source, destination] <= solverData.Variables.SantaVisitFlow[santa, source] - 1);
                        Solver.Add(santaWayFlow[source, destination] <= santaUsesWay[source, destination] * solverData.NumberOfVisits);
                    }
                }

                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Solver.Add(santaWayHasFlow[source, destination] <= santaWayFlow[source, destination]);
                    }
                }


                var sumOfFlow = new LinearExpr();
                var sumOfEdges = new LinearExpr();
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        sumOfFlow += santaWayHasFlow[source, destination];
                        sumOfEdges += santaUsesWay[source, destination];
                    }
                }

                Solver.Add(sumOfFlow == sumOfEdges);
            }
        }

        private void MST_OnlyAvailableEdges()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Solver.Add(solverData.Variables.SantaUsesWay[santa][source, destination] <=
                                   solverData.Variables.SantaGraphEdge[santa][source, destination]
                                   );
                    }
                }
            }
        }
        private void MST_NoBackAndFourth()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Solver.Add(solverData.Variables.SantaUsesWay[santa][source, destination] + solverData.Variables.SantaUsesWay[santa][destination, source] <= 1);
                    }
                }
            }
        }

        private void MST_SourceAndDestAtLeastOnce()
        {

            foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
            {
                var sumUsedAsSourceOrDest = new LinearExpr();
                foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
                {

                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        sumUsedAsSourceOrDest += solverData.Variables.SantaUsesWay[santa][source, destination];
                        sumUsedAsSourceOrDest += solverData.Variables.SantaUsesWay[santa][destination, source];
                    }
                }

                Solver.Add(sumUsedAsSourceOrDest >= 1);
            }
        }

        private void MST_NoSelfie()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    Solver.Add(solverData.Variables.SantaUsesWay[santa][source, source] == 0);
                }
            }

        }

        private void MST_MaxDestinationOfOnce()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    var numOfDestinations = new LinearExpr();
                    foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        numOfDestinations += solverData.Variables.SantaUsesWay[santa][source, destination];
                    }

                    Solver.Add(numOfDestinations <= 1);
                }
            }
        }


        private void MST_NumberOfEdgesConstraint()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {

                var numberOfVertices = Enumerable
                    .Range(0, solverData.NumberOfVisits)
                    .Aggregate(new LinearExpr(), (current, visit) => current + solverData.Variables.SantaVisit[santa, visit]);

                var numberOfEdges = new LinearExpr();
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        numberOfEdges += solverData.Variables.SantaUsesWay[santa][source, destination];
                    }
                }

                Solver.Add(numberOfEdges == numberOfVertices - 1);
            }
        }

        private void VisitUnavailable()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            var realDayCount = solverData.SolverInputData.Santas.GetLength(0);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    var available = Convert.ToInt32(solverData.SolverInputData.Visits[santa / realSantaCount, visit].IsAvailable());
                    Solver.Add(solverData.Variables.SantaVisit[santa, visit] <= available);
                }
            }
        }

        private void PerformanceConstraints()
        {
            FirstVisitByFirstSanta();

        }

        private void FirstVisitByFirstSanta()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            var realDayCount = solverData.SolverInputData.Santas.GetLength(0);

            if (solverData.SolverInputData.Visits[0, 1].IsAvailable())
            {
                Solver.Add(solverData.Variables.SantaVisit[0, 1] == 1);
            }
            else
            {
                Solver.Add(solverData.Variables.SantaVisit[realSantaCount + 1, 1] == 1);
            }
        }

        private void StartVisitVistedByEveryUsedSanta()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    Solver.Add(solverData.Variables.SantaVisit[santa, 0] >= solverData.Variables.SantaVisit[santa, visit]);
                }

                Solver.Add(solverData.Variables.SantaVisit[santa, 0] <= 1);
            }
        }

        private void SantaGraphEdge()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    var sourceVisitedBySanta = solverData.Variables.SantaVisit[santa, source];
                    for (var destination = source; destination < solverData.NumberOfVisits; destination++)
                    {
                        var destinationVisitedBySanta = solverData.Variables.SantaVisit[santa, destination];

                        Solver.Add(solverData.Variables.SantaGraphEdge[santa][source, destination] <= sourceVisitedBySanta);
                        Solver.Add(solverData.Variables.SantaGraphEdge[santa][source, destination] <= destinationVisitedBySanta);
                        Solver.Add(solverData.Variables.SantaGraphEdge[santa][source, destination] >= sourceVisitedBySanta + destinationVisitedBySanta - 1);

                        Solver.Add(solverData.Variables.SantaGraphEdge[santa][destination, source] <= sourceVisitedBySanta);
                        Solver.Add(solverData.Variables.SantaGraphEdge[santa][destination, source] <= destinationVisitedBySanta);
                        Solver.Add(solverData.Variables.SantaGraphEdge[santa][destination, source] >= sourceVisitedBySanta + destinationVisitedBySanta - 1);
                    }
                }
            }
        }

        private void SantaRouteCost()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var expr = new LinearExpr();
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        // Span-tree *2 <= 2* TSP in symetric graph
                        // I think this works for us too since A->C <= A->B->C
                        expr += solverData.Variables.SantaUsesWay[santa][source, destination] * solverData.SolverInputData.Distances[source, destination];
                        expr += solverData.Variables.SantaUsesWay[santa][destination, source] * solverData.SolverInputData.Distances[destination, source];
                    }
                }
                Solver.Add(solverData.Variables.SantaRouteCost[santa] == expr);
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

                Solver.Add(solverData.Variables.SantaVisitTime[santa] == expr);
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

                Solver.Add(visitVisited == 1);
            }
        }
    }
}
