using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.Algorithm
{
    public class Route
    {
        public List<int>[] Waypoints { get; set; }

        public Route(int numberOfSantas)
        {
            Waypoints = new List<int>[numberOfSantas];
            for (int i = 0; i < numberOfSantas; i++)
            {
                Waypoints[i] = new List<int>();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Route)obj;

            if (Waypoints.Length != other.Waypoints.Length)
            {
                return false;
            }

            for (int i = 0; i < Waypoints.Length; i++)
            {
                if (!Enumerable.SequenceEqual(Waypoints[i], other.Waypoints[i]))
                {
                    return false;
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