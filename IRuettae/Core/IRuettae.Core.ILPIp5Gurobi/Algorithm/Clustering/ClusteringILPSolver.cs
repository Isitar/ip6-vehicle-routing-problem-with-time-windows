using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gurobi;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail;


namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering
{
    public class ClusteringILPSolver : ISolver
    {
        private readonly SolverData solverData;

        private bool hasModel = false;
        private ResultState resultState = ResultState.NotSolved;
        private double MIP_GAP = 0;
        private long timelimit = 0;

        private readonly GRBModel model = new GRBModel(new GRBEnv("grb.log"));


        public ClusteringILPSolver(SolverInputData solverInputData)
        {
            solverData = new SolverData(solverInputData, model);
        }

        public ResultState Solve()
        {
            CreateModel();
            return SolveInternal();
        }

        /// <summary>
        /// Starts the solving process
        /// </summary>
        /// <param name="MIP_GAP">mip gap when to stop</param>
        /// <param name="timelimit">timelimit in ms</param>
        /// <returns></returns>
        public ResultState Solve(double MIP_GAP, long timelimit)
        {
            this.MIP_GAP = MIP_GAP;
            this.timelimit = timelimit;
            return Solve();
        }

        private void CreateModel()
        {
            InitGoogleSolver();
            AddVariables();
            AddConstraints();
            AddTargetFunction();

            hasModel = true;
            resultState = ResultState.NotSolved;
        }

        private void InitGoogleSolver()
        {
            PrintDebugRessourcesBefore("InitGoogleSolver");

            model.Reset();

            PrintDebugRessourcesAfter();
        }

        private void AddVariables()
        {
            PrintDebugRessourcesBefore("AddVariables");

            var vb = new Detail.VariableBuilder(solverData);
            vb.CreateVariables();

            PrintDebugRessourcesAfter();
        }

        private void AddConstraints()
        {
            PrintDebugRessourcesBefore("AddConstraints");

            var cb = new ConstraintBuilder(solverData);
            cb.CreateConstraints();

            PrintDebugRessourcesAfter();
        }

        private void AddTargetFunction()
        {
            PrintDebugRessourcesBefore("AddTargetFunction");
            var targetFunction = new GRBLinExpr(0);
            var hour = 3600;


            var longestDay = model.AddVar(0, int.MaxValue, 0, GRB.INTEGER, "longest day");
            var workingTimeSum = new GRBLinExpr(0);
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                var santaWorkingTime = solverData.Variables.SantaVisitTime[santa] + solverData.Variables.SantaRouteCost[santa];
                model.AddConstr(longestDay >= santaWorkingTime, null);
                workingTimeSum += santaWorkingTime;
            }

            var workingTimeFactor = (40d / hour);
            var longestDayFactor = (30d / hour);

            targetFunction = workingTimeFactor * workingTimeSum
                             + longestDayFactor * longestDay;

            solverData.Model.SetObjective(targetFunction, GRB.MINIMIZE);

            PrintDebugRessourcesAfter();
        }

        private ResultState SolveInternal()
        {
            PrintDebugRessourcesBefore("SolveInternal");

            model.Set(GRB.DoubleParam.MIPGap, MIP_GAP);
            model.Set(GRB.DoubleParam.TimeLimit, timelimit);
            model.Optimize();
            if (model.SolCount == 0)
            {
                resultState = ResultState.NotSolved;
            }
            else
            {
                resultState = FromGurobiResultState(model.Status);
            }

            PrintDebugRessourcesAfter();
#if DEBUG
            PrintDebugOutput();
#endif

            return resultState;
        }

        private void PrintDebugOutput()
        {
            Debug.WriteLine(string.Empty);
            Debug.WriteLine("DEBUG OUTPUT");
            Debug.WriteLine(string.Empty);


            PrintMeta();
            PrintVariables();
        }

        private static void DebugHR()
        {
            Debug.WriteLine(new string('-', 20));
        }

        private void PrintVariables()
        {

            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                DebugHR();
                Debug.WriteLine($"{santa} Santa Visits: ");
                foreach (var visit in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    Debug.WriteLine($"Visit {visit}: {solverData.Variables.SantaVisit[santa][visit].X}");
                }
                Debug.WriteLine(string.Empty);
            }
            DebugHR();

            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                Debug.WriteLine($"{santa} Santa Uses Way");
                foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
                {
                    foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
                    {
                        var value = solverData.Variables.SantaUsesWay[santa][solverData.SourceDestArrPos(source, destination)].X;
                        if (Math.Abs(value) > 0.0001)
                        {
                            Debug.WriteLine($"S: {source}  |  D: {destination}");
                        }
                    }

                }
                DebugHR();
            }


            //foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            //{
            //    Debug.WriteLine($"{santa} Santa Way Flow");
            //    foreach (var source in Enumerable.Range(0, solverData.NumberOfVisits))
            //    {
            //        foreach (var destination in Enumerable.Range(0, solverData.NumberOfVisits))
            //        {
            //            var value = solverData.Variables.SantaWayFlow[santa][source, destination].SolutionValue();
            //            Debug.WriteLine($"S: {source}  |  D: {destination} | {value}");
            //        }

            //    }
            //    DebugHR();
            //}

        }

        private void PrintMeta()
        {
            DebugHR();
            Debug.WriteLine("-Metadata");
            Debug.WriteLine(string.Empty);
            Debug.WriteLine($"Value of the target function: {solverData.Model.ObjVal}");
            Debug.WriteLine($"Variables: {solverData.Model.NumVars}");
            Debug.WriteLine($"Number of constraints: {solverData.Model.NumConstrs}");
            Debug.WriteLine($"Iterations: {solverData.Model.IterCount}");
            Debug.WriteLine($"Nodes: {solverData.Model.NodeCount}");
            Debug.WriteLine($"Objective Minimization: {solverData.Model.ModelSense}");
            Debug.WriteLine($"Best Bound: {solverData.Model.ObjBound}");
            Debug.WriteLine(string.Empty);
            DebugHR();
        }


        private ResultState FromGurobiResultState(int resultState)
        {
            Dictionary<int, ResultState> mapping = new Dictionary<int, ResultState>()
            {
                {GRB.Status.OPTIMAL, ResultState.Optimal },
                {GRB.Status.SUBOPTIMAL, ResultState.Feasible },
                {GRB.Status.INFEASIBLE, ResultState.Infeasible },
                {GRB.Status.LOADED, ResultState.NotSolved },
            };
            if (mapping.TryGetValue(resultState, out var value))
            {
                return value;
            }
            return ResultState.Unknown;
        }


        public string ImportMPS()
        {
            throw new NotImplementedException();
        }

        public Route GetResult()
        {
            var route = new ResultBuilder(solverData).CreateResult();
            route.SolutionValue = SolutionValue();
            return route;
        }

        public double SolutionValue()
        {
            return model.ObjVal;
        }

        private void PrintDebugRessourcesBefore(string name)
        {
            Debug.WriteLine("");
            Debug.WriteLine($"{name}");
            PrintDebugRessources("before");
        }

        private void PrintDebugRessourcesAfter()
        {
            PrintDebugRessources("after");
        }

        private void PrintDebugRessources(string description)
        {
            Debug.WriteLine($"{description}: {solverData.Model.NumConstrs} constraint, {Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024} MB used memory;");
        }
    }
}