using BlazorImplementation.Models;
using Dapper;
using DataAPIs.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAPIs.Services
{
    public class CountryService : ICountryService
    {
        private readonly IConfiguration _configuration;

        public CountryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<CountryModel> GetCountries()
        {
            List<CountryModel> result = new List<CountryModel>();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Get_Country", parameters, commandType: CommandType.StoredProcedure).Read<CountryModel>().ToList();
            }
            return result;
        }
    }
}
