using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Gurobi;
using IRuettae.Core.ILPIp5Gurobi.Algorithm;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.Detail;
using IRuettae.Core.ILPIp5Gurobi.Algorithm.Scheduling.TargetFunctionBuilders;


namespace IRuettae.Core.ILP.Algorithm.Scheduling
{
    public class SchedulingILPSolver : ILPIp5Gurobi.Algorithm.ISolver
    {
        private const int WaytimeWeight = 40;
        private const int DesiredWeight = 20;
        private readonly SolverData solverData;
        private double MIP_GAP = 0;
        private ResultState resultState = ResultState.NotSolved;

        private readonly GRBModel model = new GRBModel(new GRBEnv($"{DateTime.Now:yy-MM-dd-HH-mm-ss}_scheduling.log"));
        private long timeLimitMilliseconds = 0;

        /// <summary>
        ///
        /// </summary>
        public SchedulingILPSolver(SolverInputData solverInputData)
        {
            this.solverData = new SolverData(solverInputData, model);
        }

        public ResultState Solve()
        {
            CreateModel();
            return SolveInternal();
        }

        public ResultState Solve(double MIP_GAP, long timeLimitMilliseconds)
        {
            this.MIP_GAP = MIP_GAP;
            this.timeLimitMilliseconds = timeLimitMilliseconds;
            return Solve();
        }

        private void CreateModel()
        {
            InitGoogleSolver();
            AddVariables();
            AddConstraints();
            AddTargetFunction();

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

            var vb = new VariableBuilder(solverData);
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

            var factory = new TargetFunctionFactory(solverData);
            var targetFunction = new GRBLinExpr(0);
            targetFunction += factory.CreateTargetFunction(TargetType.MinTime, WaytimeWeight);
            targetFunction += factory.CreateTargetFunction(TargetType.TryVisitDesired, DesiredWeight);

            solverData.Model.SetObjective(targetFunction, GRB.MINIMIZE);


            // constraint target function based on presolved solution
            if (solverData.Input.Presolved.Length > 0)
            {
                var totalTimePresolved = 0;

                for (int i = 1; i < solverData.Input.Presolved.Length; i++)
                {
                    totalTimePresolved += solverData.Input.VisitsDuration[Array.IndexOf(solverData.Input.VisitIds, solverData.Input.Presolved[i] - 1)];
                    totalTimePresolved += solverData.Input.Distances[i > 1 ? Array.IndexOf(solverData.Input.VisitIds, solverData.Input.Presolved[i - 1] - 1) : 0, Array.IndexOf(solverData.Input.VisitIds, solverData.Input.Presolved[i] - 1)];
                }

                totalTimePresolved += solverData.Input.Distances[Array.IndexOf(solverData.Input.VisitIds, solverData.Input.Presolved.Last() - 1), 0];
                model.AddConstr(targetFunction <= totalTimePresolved * WaytimeWeight, null);
            }

            var minWayTime = solverData.Input.Distances.Cast<int>().Where(i => i > 0).Min();
            model.AddConstr(targetFunction >= (minWayTime * (solverData.NumberOfVisits + 1) + solverData.Input.VisitsDuration.Sum()) * (WaytimeWeight - DesiredWeight), null);
            PrintDebugRessourcesAfter();
        }

        private ResultState SolveInternal()
        {
            PrintDebugRessourcesBefore("SolveInternal");

            model.Parameters.MIPGap = MIP_GAP;
            
            if (timeLimitMilliseconds != 0)
            {
                model.Parameters.TimeLimit =  timeLimitMilliseconds / 1000;
            }
            model.Optimize();
            if (model.SolCount == 0)
            {
                resultState = ResultState.NotSolved;
            }
            else
            {
                resultState = FromGurobiState(model.Status);
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
            if (model.SolCount == 0) return;
            
            PrintMeta();
            PrintVisits();
            PrintVisitsPerSanta();
            PrintSantaEnRoute();
        }

        private void PrintMeta()
        {
            Debug.WriteLine("====================");
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
        }

        private void PrintSantaEnRoute()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-PrintSantaEnRoute");
            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                Debug.WriteLine($"Santa: {santa}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    var str = new StringBuilder();
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        str.Append(solverData.Variables.SantaEnRoute[day][santa][timeslice].X);
                    }
                    Debug.WriteLine($"SantaEnRoute: {str}");
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            }
        }

        private void PrintVisitsPerSanta()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-PrintVisitsPerSanta");
            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                Debug.WriteLine($"Santa: {santa}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                    {
                        var str = new StringBuilder();
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            str.Append(solverData.Variables.VisitsPerSanta[day][santa][visit][timeslice].X);
                        }
                        Debug.WriteLine($"Visit {visit}: {str.ToString()}");
                    }
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            }
        }

        private void PrintVisits()
        {
            Debug.WriteLine("====================");
            Debug.WriteLine("-PrintVisits");
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                Debug.WriteLine($"Visit: {visit}");
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    Debug.WriteLine($"Day: {day}");
                    var str = new StringBuilder();
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        str.Append(solverData.Variables.VisitStart[day][visit][timeslice].X);
                    }
                    Debug.WriteLine($"VisitStart: {str}");
                    str.Clear();
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        str.Append(solverData.Variables.Visits[day][visit][timeslice].X);
                    }
                    Debug.WriteLine($"Visits: {str}");
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            }
        }

        private ResultState FromGurobiState(int resultState)
        {
            Dictionary<int, ResultState> mapping = new Dictionary<int, ResultState>()
            {
                {GRB.Status.OPTIMAL, ResultState.Optimal },
                {GRB.Status.SUBOPTIMAL, ResultState.Feasible },
                {GRB.Status.TIME_LIMIT, ResultState.Feasible},
                {GRB.Status.SOLUTION_LIMIT, ResultState.Feasible},
                {GRB.Status.ITERATION_LIMIT, ResultState.Feasible},
                {GRB.Status.NODE_LIMIT, ResultState.Feasible},
                {GRB.Status.USER_OBJ_LIMIT, ResultState.Feasible},
                {GRB.Status.INFEASIBLE, ResultState.Infeasible },
                {GRB.Status.LOADED, ResultState.NotSolved },
            };
            if (mapping.TryGetValue(resultState, out var value))
            {
                return value;
            }
            return ResultState.Unknown;
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