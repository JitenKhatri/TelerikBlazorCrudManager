using BlazorImplementation.Models;

namespace DataAPIs.Interfaces
{
    public interface ICountryService
    {
        List<CountryModel> GetCountries();
    }
}
