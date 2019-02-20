using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class RoutingSolverStarterData
    {
        public int MaxNumberOfSantas { get; private set; }
        public SolvingMode Mode { get; private set; }

        /// <summary>
        /// use GetDefault
        /// </summary>
        private RoutingSolverStarterData()
        {

        }

        /// <summary>
        /// Constructor for testing purpose.
        /// </summary>
        /// <param name="maxNumberOfSantas"></param>
        /// <param name="mode"></param>
        public RoutingSolverStarterData(int maxNumberOfSantas, SolvingMode mode)
        {
            MaxNumberOfSantas = maxNumberOfSantas;
            Mode = mode;
        }

        public static RoutingSolverStarterData GetDefault(OptimizationInput input)
        {
            return new RoutingSolverStarterData
            {
                MaxNumberOfSantas = input.NumberOfSantas(),
                Mode = input.NumberOfVisits() <= 50 ? SolvingMode.Default : SolvingMode.Fast,
            };
        }

        public static RoutingSolverStarterData GetDefaultAdditionalSantas(OptimizationInput input)
        {
            // get default and set MaxNumberOfSantas
            var ret = GetDefault(input);
            ret.MaxNumberOfSantas = input.Visits.Count(v => !v.IsBreak);
            return ret;
        }
    }
}
