using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.Algorithm
{
    public struct Waypoint
    {
        public int Visit;

        /// <summary>
        /// in timeslice
        /// </summary>
        public int StartTime;

        public long RealVisitId;

        public Waypoint(int visit, int startTime, long realVisitId)
        {
            Visit = visit;
            StartTime = startTime;
            RealVisitId = realVisitId;
        }

        public override string ToString()
        {
            return $"{Visit} | {StartTime}";
        }
    }

    public class Route
    {
        /// <summary>
        /// [santa,day] list of visits
        /// </summary>
        public List<Waypoint>[,] Waypoints { get; set; }

        public Route(int numberOfSantas, int numberOfDays)
        {
            Waypoints = new List<Waypoint>[numberOfSantas, numberOfDays];
            for (int santa = 0; santa < numberOfSantas; santa++)
            {
                for (int day = 0; day < numberOfDays; day++)
                {
                    Waypoints[santa, day] = new List<Waypoint>();
                }
            }
        }

        public double SolutionValue { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Route)obj;

            if (Waypoints.Length != other.Waypoints.Length || Waypoints.GetLength(0) != other.Waypoints.GetLength(0))
            {
                return false;
            }

            for (int santa = 0; santa < Waypoints.GetLength(0); santa++)
            {
                for (int day = 0; day < Waypoints.GetLength(1); day++)
                {
                    if (!Enumerable.SequenceEqual(Waypoints[santa, day], other.Waypoints[santa, day]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string str = System.Environment.NewLine;
            for (int santa = 0; santa < Waypoints.GetLength(0); santa++)
            {
                str += $"Santa {santa}{System.Environment.NewLine}";
                for (int day = 0; day < Waypoints.GetLength(1); day++)
                {
                    str += $"Day {day}{System.Environment.NewLine}";
                    str += $"Visit | StartTime{System.Environment.NewLine}";
                    if (Waypoints[santa, day].Count > 0)
                    {
                        str += Waypoints[santa, day].Select(w => $"{w.Visit,6}| {w.StartTime,6}").Aggregate((a, v) => a + System.Environment.NewLine + v);
                    }
                    str += System.Environment.NewLine;
                }
                str += System.Environment.NewLine;
            }
            return str;
        }
    }
}