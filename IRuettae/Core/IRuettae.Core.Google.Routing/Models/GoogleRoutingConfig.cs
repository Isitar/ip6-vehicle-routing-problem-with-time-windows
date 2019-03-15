using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class GoogleRoutingConfig : ISolverConfig
    {
        public int MaxNumberOfAdditionalSantas { get; private set; }
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
        /// <param name="maxNumberOfAdditionalSantas"></param>
        /// <param name="mode"></param>
        public GoogleRoutingConfig(int maxNumberOfAdditionalSantas, SolvingMode mode)
        {
            MaxNumberOfAdditionalSantas = maxNumberOfAdditionalSantas;
            Mode = mode;
        }

        public static GoogleRoutingConfig GetDefault(OptimizationInput input)
        {
            return new GoogleRoutingConfig
            {
                MaxNumberOfAdditionalSantas = input.NumberOfSantas(),
                Mode = input.NumberOfVisits() <= 50 ? SolvingMode.Default : SolvingMode.Fast,
            };
        }
    }
}
