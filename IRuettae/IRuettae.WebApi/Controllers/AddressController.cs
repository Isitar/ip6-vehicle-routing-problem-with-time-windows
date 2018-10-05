using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using IRuettae.GeoCalculations.Geocoding;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.ExtensionMethods;

namespace IRuettae.WebApi.Controllers
{
    public class AddressController : ApiController
    {
        [System.Web.Http.Route("api/address/CityFromZip/{zip}")]
        public string[] CityFromZip(int zip)
        {
            var geocoder = DependencyResolver.Current.GetService<IGeocoder>();
            return geocoder.CityFromZip(zip);
        }

        public IHttpActionResult CheckAddress([FromBody] Visit v)
        {
            var geocoder = DependencyResolver.Current.GetService<IGeocoder>();
            try
            {
                geocoder.Locate(v.RouteCalcAddress());
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
