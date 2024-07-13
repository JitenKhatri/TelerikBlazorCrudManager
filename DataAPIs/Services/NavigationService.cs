using BlazorImplementation.Models;
using Dapper;
using DataAPIs.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAPIs.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IConfiguration _configuration;

        public NavigationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<NavigationItem> GetNavigationItems()
        {
            List<NavigationItem> result = new List<NavigationItem>();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("GetNavigationItems", parameters, commandType: CommandType.StoredProcedure).Read<NavigationItem>().ToList();
            }
            return result;
        }

        public List<NavigationItem> GetChildrentNavigationItems(int? parentId)
        {
            List<NavigationItem> result = new List<NavigationItem>();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            parameters.Add("@ParentId", parentId, DbType.Int64, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("GetChildrenNavigationItems", parameters, commandType: CommandType.StoredProcedure).Read<NavigationItem>().ToList();
            }

            return result;
        }
    }
}
