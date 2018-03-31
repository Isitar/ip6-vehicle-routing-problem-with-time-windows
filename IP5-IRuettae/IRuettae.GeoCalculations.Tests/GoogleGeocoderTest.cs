using System;
using System.Linq;
using IRuettae.GeoCalculations.Geocoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.GeoCalculations.Tests
{
    [TestClass]
    public class GoogleGeocoderTest
    {
        private const string apiKey = "AIzaSyAdTPEkyVKvA0ZvVNAAZK5Ot3fl8zyBsks";
        private const string region = "CH";

        [TestMethod]
        public void TestCityFromZip()
        {
            var geocoder = new GoogleGeocoder(apiKey, region);
            Assert.IsNotNull(geocoder);

            var cities = geocoder.CityFromZip(5600);
            Assert.AreEqual(2, cities.Length);
            Assert.IsNotNull(cities.Where(c => c.Equals("Lenzburg")));
            Assert.IsNotNull(cities.Where(c => c.Equals("Ammerswil")));

            var cities2 = geocoder.CityFromZip(4852);
            Assert.AreEqual(1, cities2.Length);
            Assert.AreEqual("Rothrist", cities2[0]);
        }
    }
}
