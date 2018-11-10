using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.ILP.Algorithm;
using IRuettae.Core.Models;
using SolverInputData = IRuettae.Core.ILP.Algorithm.Clustering.SolverInputData;

namespace IRuettae.Preprocessing.Mapping
{
    public class ClusteringSolverVariableBuilder
    {

        private int SecondsToTimeslice(double seconds) => Convert.ToInt32(Math.Ceiling(seconds / TimeSliceDuration));

        public List<Santa> Santas { get; set; }

        public List<Visit> Visits { get; set; }

        public int TimeSliceDuration { get; set; }

        private OptimizationInput optimizationInput;

        /// <summary>
        /// List of days(StartTime, EndTime)
        /// </summary>
        public List<(int Start, int End)> Days { get; set; }

        public ClusteringSolverVariableBuilder(OptimizationInput input, int timeSliceDuration)
        {
            Santas = input.Santas.ToList();
            Visits = input.Visits.ToList();

            Days = input.Days.ToList();
            TimeSliceDuration = timeSliceDuration;

            optimizationInput = input;
        }


        /// <summary>
        /// Checks, if all variables are set & Builder is ready to build
        /// </summary>
        /// <returns></returns>
        public bool IsReadyToBuild()
        {
            return Days.Count > 0
                   && Santas.Count > 0
                   && Visits.Count > 0;
        }

        //public SolverInputData(bool[,] santas, int[] visitsDuration, VisitState[,] visits, int[,] distances, int[] dayDuration)

        public SolverInputData Build()
        {
            if (!IsReadyToBuild())
            {
                throw new InvalidOperationException();
            }


            bool[,] santasVar = new bool[Days.Count, Santas.Count];
            VisitState[,] visitsVar = new VisitState[Days.Count, Visits.Count + 1];
            int[] visitDuration = Visits.Select(v => v.Duration).Prepend(0).ToArray();

            for (int day = 0; day < Days.Count; day++)
            {
                // set all santas available all days
                for (int santa = 0; santa < Santas.Count; santa++)
                {
                    santasVar[day, santa] = true;
                }

                // start visit
                visitsVar[day, 0] = VisitState.Default;


                for (int v = 1; v <= Visits.Count; v++)
                {
                    var visit = Visits[v - 1];
                    if (visit.Unavailable != null && visit.Unavailable.Any(p => Days[day].Start < p.to && p.from < Days[day].End))
                    {
                        visitsVar[day, v] = VisitState.Unavailable;
                    }
                    else if (visit.Desired != null && visit.Desired.Any(p => Days[day].Start < p.to && p.from < Days[day].End))
                    {
                        visitsVar[day, v] = VisitState.Desired;
                    }
                    else
                    {
                        visitsVar[day, v] = VisitState.Default;
                    }
                }
            }


            int[,] distances = new int[Visits.Count + 1, Visits.Count + 1];
            for (int i = 1; i < distances.GetLength(0); i++)
            {
                for (int j = 1; j < distances.GetLength(1); j++)
                {
                    distances[i, j] = SecondsToTimeslice(optimizationInput.RouteCosts[i - 1, j - 1]) * TimeSliceDuration;
                }
            }

            for (int i = 1; i < distances.GetLength(0); i++)
            {
                distances[i, 0] = SecondsToTimeslice(optimizationInput.Visits[i - 1].WayCostToHome) * TimeSliceDuration;
                distances[0, i] = SecondsToTimeslice(optimizationInput.Visits[i - 1].WayCostFromHome) * TimeSliceDuration;
            }

            distances[0, 0] = 0;



            int[] dayDuration = Days.Select(d => d.End - d.Start).ToArray();
            int[][] santaBreaks = new int[Santas.Count][];
            for (int santaIdx = 0; santaIdx < Santas.Count; santaIdx++)
            {
                var santa = Santas[santaIdx];
                santaBreaks[santaIdx] = Visits.Where(v => v.IsBreak && v.SantaId == santaIdx).Select(v => v.Id).ToArray();
            }

            var solverInputData = new SolverInputData(santasVar, visitDuration, visitsVar, distances, dayDuration, santaBreaks);

            return solverInputData;
        }
    }
}
