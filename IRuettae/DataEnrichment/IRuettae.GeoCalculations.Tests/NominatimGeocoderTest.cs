using System;
using System.Linq;
using IRuettae.GeoCalculations.Geocoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.GeoCalculations.Tests
{
    [TestClass]
    public class NominatimGeocoderTest
    {
        
        private const string region = "CH";
        private const string referer = "ip6.isitar.ch";
        private const string email = "luescherpascal@gmail.com";

        [TestMethod]
        public void TestCityFromZip()
        {
            var geocoder = new NominatimGeocoder(email,region,referer);
            Assert.IsNotNull(geocoder);

            var cities = geocoder.CityFromZip(5600);
            Assert.AreEqual(2, cities.Length);
            Assert.IsNotNull(cities.Where(c => c.Equals("Lenzburg")));

            var cities2 = geocoder.CityFromZip(4852);
            Assert.AreEqual(1, cities2.Length);
            Assert.AreEqual("Rothrist", cities2[0]);


        }

        [TestMethod]
        public void TestCityFromZipSpecialChars()
        {
            var geocoder = new NominatimGeocoder(email, region, referer);
            var cities3 = geocoder.CityFromZip(3178);
            Assert.AreEqual("Bösingen", cities3[0]);
        }

        [TestMethod]
        public void TestCoords()
        {
            var geocoder = new NominatimGeocoder(email, region, referer);
            var coords = geocoder.Locate("Schafisheimerstrasse 12 5603 Staufen");
            Assert.AreEqual(47.38376435,coords.lat);
            Assert.AreEqual(8.1582365, coords.lng);
        }
    }
}
