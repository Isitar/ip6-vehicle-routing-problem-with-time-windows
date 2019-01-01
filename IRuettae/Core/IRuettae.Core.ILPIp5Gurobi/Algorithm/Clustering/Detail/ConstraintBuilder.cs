using System;
using System.Linq;
using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail
{
    internal class ConstraintBuilder
    {
        private readonly SolverData solverData;

        public ConstraintBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        private GRBModel Model => solverData.Model;

        private int SourceDestArrPos(int source, int destination)
        {
            return source * solverData.NumberOfVisits + destination;
        }

        public void CreateConstraints()
        {
            // Variables
            SantaVisitDuration();
            SantaWayInCluster();
            SantaRouteCost();


            // Real Constraints
            VisitUnavailable();
            VisitDesired();
            VisitUsedExactlyOnce();
            StartVisitVistedByEveryUsedSanta();

            SantaBreaks();

            SantaTimeConstraint();

            ATSPConstraints();

            //Performance
            PerformanceConstraints();
        }

        private void VisitDesired()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var day = santa / realSantaCount;
                for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    var desired = Convert.ToInt32(solverData.SolverInputData.Visits[day, visit] == VisitState.Desired);
                    Model.AddConstr(solverData.Variables.SantaVisitBonus[santa][visit] == desired * solverData.Variables.SantaVisit[santa][visit], null);
                }
            }
        }

        private void SantaBreaks()
        {
            if (solverData.SolverInputData.SantaBreaks == null)
            {
                return;
            }

            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);

            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var realSantaId = santa % realSantaCount;
                var santaBreaks = solverData.SolverInputData.SantaBreaks[realSantaId];
                if (santaBreaks.Length > 0)
                {
                    foreach (var santaBreak in santaBreaks)
                    {
                        Model.AddConstr(solverData.Variables.SantaVisit[santa][santaBreak] == 1 * solverData.Variables.SantaVisit[santa][0], null);
                    }
                }
            }
        }

        private void SantaTimeConstraint()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                Model.AddConstr(
                    solverData.Variables.SantaRouteCost[santa] +
                    solverData.Variables.SantaVisitTime[santa] <=
                    solverData.SolverInputData.DayDuration[santa / realSantaCount], null);
            }
        }

        private void ATSPConstraints()
        {
            ATSP_NumberOfEdgesConstraint();
            ATSP_NumDestinationOfOnce();
            ATSP_NumSourceOfOnce();

            ATSP_NoSelfie();
            ATSP_NoBackAndFourth();

            ATSP_Flow();
        }

        private void ATSP_Flow()
        {
            var M = solverData.NumberOfVisits;
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var santaWayFlow = solverData.Variables.SantaWayFlow[santa];
                var santaWayHasFlow = solverData.Variables.SantaWayHasFlow[santa];
                var santaUsesWay = solverData.Variables.SantaUsesWay[santa];
                var numberOfVisitsInCluster = NumberOfSantaVisit(santa);
                // flow for start location if santa is in use
                foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    Model.AddConstr(santaWayFlow[SourceDestArrPos(0, destination)] == santaUsesWay[SourceDestArrPos(0, destination)] * M, null);
                }

                //flow only possible if santa uses way
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Model.AddConstr(santaWayFlow[SourceDestArrPos(source, destination)] <= santaUsesWay[SourceDestArrPos(source, destination)] * M, null);
                        //ClusteringILPSolver.Add(santaWayFlow[source, destination] >= santaUsesWay[source, destination]);
                    }
                }

                // strengthen formulasation
                for (var source = 1; source < solverData.NumberOfVisits; source++)
                {
                    for (var destination = 1; source < solverData.NumberOfVisits; source++)
                    {
                        Model.AddConstr(santaWayFlow[SourceDestArrPos(source, destination)] <= numberOfVisitsInCluster, null);
                    }
                }

                // flow only possible if neighbours have flow
                for (var source = 1; source < solverData.NumberOfVisits; source++)
                {
                    var sumFlowNeightbours = new GRBLinExpr(0);
                    foreach (var incomingNeighbours in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        sumFlowNeightbours += santaWayFlow[SourceDestArrPos(incomingNeighbours, source)];
                    }

                    // node has value of incoming flow
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Model.AddConstr(santaWayFlow[SourceDestArrPos(source, destination)] <= sumFlowNeightbours - 1 * solverData.Variables.SantaVisit[santa][source], null);
                    }
                }

                // hasFlow Variable
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Model.AddConstr(santaWayHasFlow[SourceDestArrPos(source, destination)] <= santaWayFlow[SourceDestArrPos(source, destination)], null);
                        //ClusteringILPSolver.Add(santaWayFlow[source, destination] <= santaWayHasFlow[source, destination] * M); // this constraint makes it terrible slow
                    }
                }

                var sumOfFlow = new GRBLinExpr(0);
                var sumOfEdges = new GRBLinExpr(0);
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        sumOfFlow += santaWayHasFlow[SourceDestArrPos(source, destination)];
                        sumOfEdges += santaUsesWay[SourceDestArrPos(source, destination)];
                    }
                }

                Model.AddConstr(sumOfFlow == sumOfEdges, null);
            }
        }

        private void ATSP_NoBackAndFourth()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)] + solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(destination, source)] <= 1, null);
                    }
                }
            }
        }

        private void ATSP_NoSelfie()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, source)] == 0, null);
                }
            }
        }

        private void ATSP_NumSourceOfOnce()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    var numOfSources = new GRBLinExpr();
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        numOfSources += solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)];
                    }

                    Model.AddConstr(numOfSources == 1 * solverData.Variables.SantaVisit[santa][source], null);
                }
            }
        }

        private void ATSP_NumDestinationOfOnce()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    var numOfDestinations = new GRBLinExpr();
                    foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        numOfDestinations += solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)];
                    }

                    Model.AddConstr(numOfDestinations == 1 * solverData.Variables.SantaVisit[santa][destination], null);
                }
            }
        }

        private GRBLinExpr NumberOfSantaVisit(int santa)
        {
            return Enumerable
                .Range(0, solverData.NumberOfVisits)
                .Aggregate(new GRBLinExpr(0), (current, visit) => current + solverData.Variables.SantaVisit[santa][visit]);
        }

        private void ATSP_NumberOfEdgesConstraint()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var numberOfVertices = NumberOfSantaVisit(santa);

                var numberOfEdges = new GRBLinExpr(0);
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        numberOfEdges += solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)];
                    }
                }

                Model.AddConstr(numberOfEdges == numberOfVertices, null);
            }
        }

        private void VisitUnavailable()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var day = santa / realSantaCount;
                for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    var available = Convert.ToInt32(solverData.SolverInputData.Visits[day, visit].IsAvailable());
                    Model.AddConstr(solverData.Variables.SantaVisit[santa][visit] <= available, null);
                }
            }
        }

        private void PerformanceConstraints()
        {
            FirstVisitByFirstSanta();
            ATSP_OverallSum();
            ATSP_FixingSantaUsesWay();
            //SantaUsage();
        }

        private void SantaUsage()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            var realDayCount = solverData.SolverInputData.Santas.GetLength(0);
            for (var d = 0; d < realDayCount; d++)
            {
                for (var s = 1; s < realSantaCount; s++)
                {
                    Model.AddConstr(solverData.Variables.SantaUsed[d * realSantaCount + s] <=
                                    solverData.Variables.SantaUsed[d * realSantaCount + s - 1], null);
                }
            }

            for (var s = 0; s < solverData.NumberOfSantas; s++)
            {
                for (var v = 0; v < solverData.NumberOfVisits; v++)
                {
                    Model.AddConstr(solverData.Variables.SantaUsed[s] >= solverData.Variables.SantaVisit[s][v], null);
                }

                Model.AddConstr(solverData.Variables.SantaUsed[s] <= 1, null);
            }
        }

        private void ATSP_FixingSantaUsesWay()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)] <=
                                        solverData.Variables.SantaWayHasFlow[santa][SourceDestArrPos(source, destination)], null);
                    }
                }
            }
        }

        private void ATSP_OverallSum()
        {
            //var overallUseWay = new GRBLinExpr[solverData.NumberOfSantas];
            //var overallHasFlow = new GRBLinExpr[solverData.NumberOfSantas];
            //foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            //{
            //    overallUseWay[santa] = new GRBLinExpr(0);
            //    overallHasFlow[santa] = new GRBLinExpr(0);
            //    foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
            //    {
            //        foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
            //        {
            //            overallHasFlow[santa] += solverData.Variables.SantaWayHasFlow[santa][source, destination];
            //            overallUseWay[santa] += solverData.Variables.SantaUsesWay[santa][source, destination];
            //        }
            //    }
            //}

            var overallUseWay = new GRBLinExpr(0);
            var overallHasFlow = new GRBLinExpr(0);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        overallHasFlow += solverData.Variables.SantaWayHasFlow[santa][SourceDestArrPos(source, destination)];
                        overallUseWay += solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)];
                    }
                }
            }

            Model.AddConstr(overallUseWay == overallHasFlow, null);
        }

        private void FirstVisitByFirstSanta()
        {
            var realSantaCount = solverData.SolverInputData.Santas.GetLength(1);
            var realDayCount = solverData.SolverInputData.Santas.GetLength(0);

            if (solverData.SolverInputData.Visits[0, 1].IsAvailable())
            {
                Model.AddConstr(solverData.Variables.SantaVisit[0][1] == 1, null);
            }
            else
            {
                Model.AddConstr(solverData.Variables.SantaVisit[realSantaCount][1] == 1, null);
            }
        }

        private void StartVisitVistedByEveryUsedSanta()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var sumSantaVisit = new GRBLinExpr(0);
                for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    Model.AddConstr(solverData.Variables.SantaVisit[santa][0] >= solverData.Variables.SantaVisit[santa][visit], null);
                    sumSantaVisit += solverData.Variables.SantaVisit[santa][visit];
                }

                Model.AddConstr(solverData.Variables.SantaVisit[santa][0] <= sumSantaVisit, null);
            }
        }

        private void SantaWayInCluster()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    var sourceVisitedBySanta = solverData.Variables.SantaVisit[santa][source];
                    for (var destination = source; destination < solverData.NumberOfVisits; destination++)
                    {
                        var destinationVisitedBySanta = solverData.Variables.SantaVisit[santa][destination];

                        Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)] <= sourceVisitedBySanta, null);
                        Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)] <= destinationVisitedBySanta, null);

                        Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(destination, source)] <= sourceVisitedBySanta, null);
                        Model.AddConstr(solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(destination, source)] <= destinationVisitedBySanta, null);
                    }
                }
            }
        }

        private void SantaRouteCost()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var expr = new GRBLinExpr(0);
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        expr += solverData.Variables.SantaUsesWay[santa][SourceDestArrPos(source, destination)] * solverData.SolverInputData.Distances[source, destination];
                    }
                }

                Model.AddConstr(solverData.Variables.SantaRouteCost[santa] == expr, null);
            }
        }

        private void SantaVisitDuration()
        {
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var expr = new GRBLinExpr(0);
                foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    expr += solverData.Variables.SantaVisit[santa][visit] *
                            solverData.SolverInputData.VisitsDuration[visit];
                }

                Model.AddConstr(solverData.Variables.SantaVisitTime[santa] == expr, null);
            }
        }

        public void VisitUsedExactlyOnce()
        {
            var santaBreaks = solverData.SolverInputData.SantaBreaks;
            var realNumberOfDays = solverData.SolverInputData.Santas.GetLength(0);
            var realNumberOfSantas = solverData.SolverInputData.Santas.GetLength(1);

            // 1 because start Visit is visited by multiple santas
            for (var visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                var visitVisited = new GRBLinExpr(0);
                foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
                {
                    visitVisited += solverData.Variables.SantaVisit[santa][visit];
                }

                var isBreak = false;
                if (santaBreaks != null)
                {
                    // if it's a break, it only needs to be visited if santa is used
                    for (var breakSanta = 0; breakSanta < santaBreaks.Length; breakSanta++)
                    {
                        if (santaBreaks[breakSanta].Contains(visit))
                        {
                            isBreak = true;
                            var sumSantaUsed = new GRBLinExpr(0);
                            foreach (var day in Enumerable.Range(0, realNumberOfDays))
                            {
                                sumSantaUsed += solverData.Variables.SantaVisit[day * realNumberOfSantas + breakSanta][0];
                            }

                            Model.AddConstr(visitVisited == sumSantaUsed, null);
                        }
                    }
                }

                if (!isBreak)
                {
                    Model.AddConstr(visitVisited == 1, null);
                }
            }
        }
    }
}