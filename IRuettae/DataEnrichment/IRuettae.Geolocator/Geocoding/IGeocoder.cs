namespace IRuettae.GeoCalculations.Geocoding
{
    public interface IGeocoder
    {
        /// <summary>
        /// Locates the address and returns the coordinates
        /// </summary>
        /// <param name="address">the address to be located</param>
        /// <returns>a named tuple with (lat, lng)</returns>
        (double lat, double lng) Locate(string address);


        /// <summary>
        /// Returns a string array with possible city names for a given zip
        /// </summary>
        /// <param name="zip">the zip used for search</param>
        /// <returns>a string array with city names</returns>
        string[] CityFromZip(int zip);
    }
}
