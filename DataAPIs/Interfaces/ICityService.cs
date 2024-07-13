using BlazorImplementation.Models;

namespace DataAPIs.Interfaces
{
    public interface ICityService
    {
        List<CityModel> GetCity(long CountryId);
    }
}
