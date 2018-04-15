using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.Algorithm
{
    public class Route
    {
        public List<int> Waypoints { get; set; }

        public Route()
        {
            Waypoints = new List<int>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Route)obj;

            return Enumerable.SequenceEqual(Waypoints, other.Waypoints);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}