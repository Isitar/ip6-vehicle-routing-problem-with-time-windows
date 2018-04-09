using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.GeoCalculations.Geocoding
{
    public class DebugGeocoder : IGeocoder
    {
        private readonly Random rnd = new Random();
        public (double lat, double lng) Locate(string address)
        {
            return (rnd.NextDouble() + 47d, rnd.NextDouble() + 8d);
        }

        public string[] CityFromZip(int zip)
        {
            string[] wordslist =
            {
                "apologise", "blood", "buzz", "last", "rabid", "crown", "yard", "level", "wander", "five", "volatile",
                "bee", "cobweb", "serve", "mysterious", "cause", "laugh", "trick", "industry", "cough", "chin", "teeny",
                "aloof", "tramp", "reason", "rejoice", "analyse", "zesty", "pour", "tax", "sulky", "advice",
                "concerned", "faulty", "red", "squirrel", "girl", "guide", "fantastic", "ready", "cap", "attend",
                "quilt", "offend", "damaged", "things", "alert", "extra-small", "grip", "earn"
            };
            // seldom return 2 elements
            if (rnd.Next(10) == 9)
            {
                return new[] {wordslist[rnd.Next(wordslist.Length)], wordslist[rnd.Next(wordslist.Length)]};
            }

            return new[] { wordslist[rnd.Next(wordslist.Length)] };

        }

    }

}
