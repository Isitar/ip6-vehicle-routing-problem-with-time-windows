using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IRuettae.GeoCalculations.Geocoding
{
    public class NominatimGeocoder : IGeocoder
    {
        private readonly string email;
        private readonly string region;
        private readonly string referer;

        /// <summary>
        /// Creates a geocoder instance with nominatim backend
        /// </summary>
        /// <param name="email">email used for nomatim request</param>
        /// <param name="region">country code filter</param>
        /// <param name="referer">site referer</param>
        public NominatimGeocoder(string email, string region, string referer)
        {
            this.email = email;
            this.region = region;
            this.referer = referer;
        }
        public (double lat, double lng) Locate(string address)
        {
            var wc = new WebClient { Encoding = Encoding.UTF8 };
            wc.Headers.Add("Referer", referer);
            var uri = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&email={email}";
            var retJson = wc.DownloadString(uri);
            try
            {
                dynamic retData = JArray.Parse(retJson)[0];

                double lat = retData.lat;
                double lon = retData.lon;

                return (lat, lon);

            }
            catch
            {
                throw new LocationNotFoundException();
            }
        }

        public string[] CityFromZip(int zip)
        {
            var wc = new WebClient { Encoding = Encoding.UTF8 };
            wc.Headers.Add("Referer", referer);
            var uri = $"https://nominatim.openstreetmap.org/search?postalcode={zip}&countrycodes={region}&format=json&email={email}";
            var retJson = wc.DownloadString(uri);
            try
            {
                dynamic retData = JArray.Parse(retJson)[0];

                double lat = retData.lat;
                double lon = retData.lon;
                retJson = wc.DownloadString($"https://nominatim.openstreetmap.org/reverse?lat={lat}&lon={lon}&format=json&email={email}");

                retData = JObject.Parse(retJson);
                return new string[] { retData.address.village.ToString() };
            }
            catch
            {
                throw new LocationNotFoundException();
            }
        }

    }
}
