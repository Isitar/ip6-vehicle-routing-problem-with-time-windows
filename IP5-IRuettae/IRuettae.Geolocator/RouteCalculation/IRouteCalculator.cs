namespace IRuettae.GeoCalculations.RouteCalculation
{
    public interface IRouteCalculator
    {
        /// <summary>
        /// Calculates the route from one point to another using strings
        /// </summary>
        /// <param name="from">the starting location</param>
        /// <param name="to">the ending location</param>
        /// <returns>a named tuple (distance in meter, duration in seconds)</returns>
        (double distance, double duration) CalculateWalkingDistance(string from, string to);

        /// <summary>
        /// Calculates the route from one point to another using strings
        /// </summary>
        /// <param name="fromLat">From latitude</param>
        /// <param name="fromLng">From longitude</param>
        /// <param name="toLat">To latitude</param>
        /// <param name="toLong">To longitude</param>
        /// <returns>a named tuple (distance in meter, duration in seconds)</returns>
        (double distance, double duration) CalculateWalkingDistance(double fromLat, double fromLong, double toLat, double toLong);
    }
}