using DataAPIs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController : Controller
    {
        private ICityService cityService;

        public CityController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        [HttpGet]
        [Route("GetCities")]
        public IActionResult GetCities([FromQuery]long CountryId)
        {
            var result = cityService.GetCity(CountryId);
            return Ok(result);
        }
    }
}
