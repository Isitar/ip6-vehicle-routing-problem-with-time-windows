using System;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace IRuettae.GeoCalculations.Geocoding
{
    public class GoogleGeocoder : IGeocoder
    {
        private const string BaseUrl = @"https://maps.googleapis.com/maps/api/geocode/json?";

        private readonly string apiKey;
        private readonly string region;

        /// <summary>
        /// Implementation of Geocoding Interface using the Google API
        /// </summary>
        /// <param name="apiKey">Your Google API Key to use geocoding API</param>
        /// <param name="region">The region (2 character country code)</param>
        public GoogleGeocoder(string apiKey, string region = null)
        {
            this.apiKey = apiKey;
            this.region = region;
        }

        public (double lat, double lng) Locate(string address)
        {
            var callUrl = $"{BaseUrl}address={address}&key={apiKey}";

            if (!string.IsNullOrEmpty(region))
            {
                callUrl += $"&region={region}";
            }

            var retJson = new WebClient().DownloadString(callUrl);

            dynamic retData = JObject.Parse(retJson);
            if (retData.status != "OK")
            {
                throw new LocationNotFoundException();
            }

            try
            {
                double lat = retData.results[0].geometry.location.lat;
                double lng = retData.results[0].geometry.location.lng;

                return (lat, lng);
            }
            catch
            {
                throw new LocationNotFoundException();
            }
        }

        public string[] CityFromZip(int zip)
        {
            var callUrl = $"{BaseUrl}address={zip}&key={apiKey}";

            if (!string.IsNullOrEmpty(region))
            {
                callUrl += $"&region={region}";
            }

            var retJson = new WebClient().DownloadString(callUrl);

            dynamic retData = JObject.Parse(retJson);
            if (retData.status != "OK")
            {
                throw new LocationNotFoundException();
            }

            string[] retVal;
            try
            {
                retVal = ((JArray) retData.results[0].postcode_localities).ToObject<string[]>();
            }
            catch (NullReferenceException)
            {
                // did not find postcode_localities
                retVal = new string[] {retData.results[0].address_components[1].long_name};
            }
            catch
            {
                throw new LocationNotFoundException();
            }

            // handle special characters
            return retVal.Select(r =>
            {
                byte[] data = Encoding.Default.GetBytes(r);
                return Encoding.UTF8.GetString(data);
            }).ToArray();
        }
    }
}
