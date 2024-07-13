using BlazorImplementation.Models;
using Dapper;
using DataAPIs.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAPIs.Services
{
    public class CityService : ICityService
    {
        private readonly IConfiguration _configuration;

        public CityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<CityModel> GetCity(long CountryId)
        {
            List<CityModel> result = new List<CityModel>();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            parameters.Add("@CountryId", CountryId, DbType.Int64, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Get_Cities_By_Country", parameters, commandType: CommandType.StoredProcedure).Read<CityModel>().ToList();
            }
            return result;
        }
    }
}
