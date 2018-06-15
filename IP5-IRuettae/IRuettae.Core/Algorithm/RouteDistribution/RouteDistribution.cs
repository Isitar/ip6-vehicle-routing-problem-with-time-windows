using System.Collections.Generic;
using System.Linq;

namespace IRuettae.Core.Algorithm.RouteDistribution
{
    public class RouteDistribution
    {
        /// <summary>
        /// [santa,day] route that is used
        /// </summary>
        public int?[,] Distribution { get; set; }

        public RouteDistribution(int numberOfSantas, int numberOfDays)
        {
            Distribution = new int?[numberOfSantas, numberOfDays];
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (RouteDistribution)obj;

            if (Distribution.Length != other.Distribution.Length || Distribution.GetLength(0) != other.Distribution.GetLength(0))
            {
                return false;
            }

            for (int santa = 0; santa < Distribution.GetLength(0); santa++)
            {
                for (int day = 0; day < Distribution.GetLength(1); day++)
                {
                    if (Distribution[santa, day] != other.Distribution[santa, day])
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
            for (int santa = 0; santa < Distribution.GetLength(0); santa++)
            {
                str += $"Santa {santa}{System.Environment.NewLine}";
                for (int day = 0; day < Distribution.GetLength(1); day++)
                {
                    str += $"Route on Day {day}: ";
                    if (Distribution[santa, day].HasValue)
                    {
                        str += Distribution[santa, day].Value;
                    }
                    else
                    {
                        str += "none";
                    }
                    str += System.Environment.NewLine;
                }
                str += System.Environment.NewLine;
            }
            return str;
        }
    }
}