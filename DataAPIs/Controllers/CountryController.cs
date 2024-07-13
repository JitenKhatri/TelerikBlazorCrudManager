using DataAPIs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : Controller
    {
        private ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        [Route("GetCountries")]
        public IActionResult GetCountries()
        {
            var result = countryService.GetCountries();
            return Ok(result);
        }
    }
}
