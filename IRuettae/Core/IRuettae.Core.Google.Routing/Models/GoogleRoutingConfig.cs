using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class GoogleRoutingConfig : ISolverConfig
    {
        public int MaxNumberOfSantas { get; private set; }
        public SolvingMode Mode { get; private set; }

        /// <summary>
        /// use GetDefault
        /// </summary>
        private GoogleRoutingConfig()
        {

        }

        /// <summary>
        /// Constructor for testing purpose.
        /// </summary>
        /// <param name="maxNumberOfSantas"></param>
        /// <param name="mode"></param>
        public GoogleRoutingConfig(int maxNumberOfSantas, SolvingMode mode)
        {
            MaxNumberOfSantas = maxNumberOfSantas;
            Mode = mode;
        }

        public static GoogleRoutingConfig GetDefault(OptimizationInput input)
        {
            return new GoogleRoutingConfig
            {
                MaxNumberOfSantas = input.NumberOfSantas(),
                Mode = input.NumberOfVisits() <= 50 ? SolvingMode.Default : SolvingMode.Fast,
            };
        }

        public static GoogleRoutingConfig GetDefaultAdditionalSantas(OptimizationInput input)
        {
            // get default and set MaxNumberOfSantas
            var ret = GetDefault(input);
            ret.MaxNumberOfSantas = input.Visits.Length;
            return ret;
        }
    }
}
