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
            VisitState[,] visitsVar = new VisitState[Days.Count, Visits.Count];
            int[] visitDuration = Visits.Select(v => v.Duration).ToArray();

            for (int day = 0; day < Days.Count; day++)
            {
                // set all santas available all days
                for (int santa = 0; santa < Santas.Count; santa++)
                {
                    santasVar[day, santa] = true;
                }

                var dayTimeduration = 24 * 60 * 60;


                for (int v = 0; v < Visits.Count; v++)
                {
                    if (Visits[v].Unavailable.Any(p => Days[day].End < p.to && p.from < Days[day].End))
                    {
                        visitsVar[day, v] = VisitState.Unavailable;
                    }
                    else if (Visits[v].Desired.Any(p => Days[day].End < p.to && p.from< Days[day].End))
                    {
                        visitsVar[day, v] = VisitState.Desired;
                    }
                    else
                    {
                        visitsVar[day, v] = VisitState.Default;
                    }
                }
            }


            //int[,] distances = new int[Visits.Count, Visits.Count];
            //for (int v = 0; v < Visits.Count; v++)
            //{
            //    for (int d = 0; d < Visits.Count; d++)
            //    {
            //        distances[v, d] = (int)(Math.Ceiling(Visits[v].FromWays.First(w => w.To.Equals(Visits[d])).Duration / (double)TimeSliceDuration) * TimeSliceDuration);
            //    }
            //}



            int[] dayDuration = Days.Select(d => d.End - d.Start).ToArray();
            int[][] santaBreaks = new int[Santas.Count][];
            for (int santaIdx = 0; santaIdx < Santas.Count; santaIdx++)
            {
                var santa = Santas[santaIdx];
                santaBreaks[santaIdx] = Visits.Where(v => v.IsBreak && v.SantaId == santaIdx).Select(v => v.Id).ToArray();
            }

            var solverInputData = new SolverInputData(santasVar, visitDuration, visitsVar, optimizationInput.RouteCosts,
                dayDuration, santaBreaks)
            {
                //VisitNames = Visits.Select(v => $"{v.Street} {v.Zip} {v.City}").ToArray(),
                //VisitIds = Visits.Select(v => v.Id).ToArray()
            };

            return solverInputData;
        }
    }
}
