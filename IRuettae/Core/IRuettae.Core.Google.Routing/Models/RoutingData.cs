using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class RoutingData
    {
        public readonly OptimizationInput Input;

        public RoutingData(OptimizationInput input)
        {
            Input = input;
        }

        public CostSettings Cost { get; } = new CostSettings();

        /// <summary>
        /// List of all santa Ids.
        /// The SantaIds.Length equals maxNumberOfSantas*numberOfDays.
        /// </summary>
        public int[] SantaIds { get; set; }

        /// <summary>
        /// List of all visits.
        /// Visit.SantaId of the elements in this list refer to an index of SantaIds.
        /// The Visits.Length equals the sum of normal visits and breaks*numberOfDays.
        /// </summary>
        public Visit[] Visits { get; set; }

        /// <summary>
        /// Visits[HomeIndex[day]] is the home the santas on the specified day.
        /// All santas on the same day have the same home.
        /// </summary>
        public int[] HomeIndex { get; set; }

        /// <summary>
        /// Intervals in which a visit start can not be.
        /// Unavailable[visitId][interval].
        /// Unavailable.Length equals Visits.Length.
        /// </summary>
        public (int startFrom, int startEnd)[][] Unavailable { get; set; }

        /// <summary>
        /// Index of the visit which is the start of the santa.
        /// SantaStartIndex[santaIndex].
        /// </summary>
        public int[] SantaStartIndex { get; set; }

        /// <summary>
        /// Index of the visit which is the end of the santa.
        /// SantaEndIndex[santaIndex].
        /// </summary>
        public int[] SantaEndIndex { get; set; }


        /// <summary>
        /// Returns the number of santas.
        /// This includes duplicated santas.
        /// </summary>
        public int NumberOfSantas => SantaIds?.Length ?? 0;

        /// <summary>
        /// Returns the number of visits.
        /// This includes duplicated breaks.
        /// </summary>
        public int NumberOfVisits => Visits?.Length ?? 0;


        /// <summary>
        /// Returns the start of the first day.
        /// </summary>
        public int OverallStart
        {
            get
            {
                return Input.Days.Min(d => d.from);
            }
        }

        /// <summary>
        /// Returns the end of the latest day.
        /// </summary>
        public int OverallEnd
        {
            get
            {
                return Input.Days.Max(d => d.to);
            }
        }

        /// <summary>
        /// Returns to which day the santa with the given index belongs to.
        /// </summary>
        /// <param name="santaIndex"></param>
        /// <returns></returns>
        public int GetDayFromSanta(int santaIndex)
        {
            return santaIndex / (NumberOfSantas / Input.Days.Length);
        }
    }
}
