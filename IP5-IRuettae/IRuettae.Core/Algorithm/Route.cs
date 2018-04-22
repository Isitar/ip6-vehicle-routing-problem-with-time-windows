using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.Algorithm
{
    public class Route
    {
        /// <summary>
        /// [santa,day] list of visits
        /// </summary>
        public List<int>[,] Waypoints { get; set; }

        public Route(int numberOfSantas, int numberOfDays)
        {
            Waypoints = new List<int>[numberOfSantas, numberOfDays];
            for (int santa = 0; santa < numberOfSantas; santa++)
            {
                for (int day = 0; day < numberOfDays; day++)
                {
                    Waypoints[santa, day] = new List<int>();
                }
            }
        }

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
    }
}