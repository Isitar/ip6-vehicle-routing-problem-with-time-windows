using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class GoogleRoutingStarterData : IStarterData
    {
        public int MaxNumberOfSantas { get; private set; }
        public SolvingMode Mode { get; private set; }

        /// <summary>
        /// use GetDefault
        /// </summary>
        private GoogleRoutingStarterData()
        {

        }

        /// <summary>
        /// Constructor for testing purpose.
        /// </summary>
        /// <param name="maxNumberOfSantas"></param>
        /// <param name="mode"></param>
        public GoogleRoutingStarterData(int maxNumberOfSantas, SolvingMode mode)
        {
            MaxNumberOfSantas = maxNumberOfSantas;
            Mode = mode;
        }

        public static GoogleRoutingStarterData GetDefault(OptimizationInput input)
        {
            return new GoogleRoutingStarterData
            {
                MaxNumberOfSantas = input.NumberOfSantas(),
                Mode = input.NumberOfVisits() <= 50 ? SolvingMode.Default : SolvingMode.Fast,
            };
        }

        public static GoogleRoutingStarterData GetDefaultAdditionalSantas(OptimizationInput input)
        {
            // get default and set MaxNumberOfSantas
            var ret = GetDefault(input);
            ret.MaxNumberOfSantas = input.Visits.Length;
            return ret;
        }
    }
}
