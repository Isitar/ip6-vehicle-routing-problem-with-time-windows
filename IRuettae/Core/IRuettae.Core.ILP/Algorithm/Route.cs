using System;
using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.ILP.Algorithm
{
    public class Route
    {
        /// <summary>
        /// [santa,day] list of visits
        /// </summary>
        public List<Waypoint>[,] Waypoints { get; set; }

        /// <summary>
        /// [day] starting time
        /// </summary>
        public DateTime[] StartingTime { get; set; }

        public long[] SantaIds { get; set; }

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
                    if (!Waypoints[santa, day].SequenceEqual(other.Waypoints[santa, day]))
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
            string str = Environment.NewLine;
            for (int santa = 0; santa < Waypoints.GetLength(0); santa++)
            {
                str += $"Santa {santa}{Environment.NewLine}";
                for (int day = 0; day < Waypoints.GetLength(1); day++)
                {
                    str += $"Day {day}{Environment.NewLine}";
                    str += $"Visit | StartTime{Environment.NewLine}";
                    if (Waypoints[santa, day].Count > 0)
                    {
                        str += Waypoints[santa, day].Select(w => $"{w.Visit,6}| {w.StartTime,6}").Aggregate((a, v) => a + Environment.NewLine + v);
                    }
                    str += Environment.NewLine;
                }
                str += Environment.NewLine;
            }
            return str;
        }
    }
}