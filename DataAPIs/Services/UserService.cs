using BlazorImplementation.Models;
using BlazorImplementation.Models.QueryStrings;
using Dapper;
using DataAPIs.Interfaces;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace DataAPIs.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserModel Login(TokenModel token)
        {
            UserModel result = new UserModel();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", token.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", token.Password, DbType.String, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Varify_User", parameters, commandType: CommandType.StoredProcedure).ReadFirstOrDefault<UserModel>();
            }
            return result;
        }

        public UserModel Register(UserModel userModel)
        {
            UserModel result = new UserModel();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userModel.UserName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", userModel.Password, DbType.String, ParameterDirection.Input);
            parameters.Add("@FirstName", userModel.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastName", userModel.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("@DateOfBirth", userModel.DateOfBirth, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@PhoneNumber", userModel.PhoneNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@Role",userModel.Role, DbType.String, ParameterDirection.Input);
            parameters.Add("@CountryId",userModel.CountryId,DbType.Int64, ParameterDirection.Input);
            parameters.Add("@CityId",userModel.CityId,DbType.Int64, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Register_User", parameters, commandType: CommandType.StoredProcedure).ReadFirstOrDefault<UserModel>();
            }
            return result;
        }

        public List<UserModel> GetUsers(UserQueryString userQueryString)
        {
            List<UserModel> result = new List<UserModel>();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            var filtermodel = JsonConvert.DeserializeObject<List<FilterModel>>(userQueryString.Filters);
            var filterTable = new DataTable();
            filterTable.Columns.Add("Field", typeof(string));
            filterTable.Columns.Add("Operator", typeof(string));
            filterTable.Columns.Add("Value", typeof(string));
            filterTable.Columns.Add("LogicalOperator",typeof(string));
            foreach (var filter in filtermodel)
            {
                filterTable.Rows.Add(filter.Field, filter.Operator, filter.Value, filter.LogicalOperator);
            }
            parameters.Add("@Filters", filterTable, DbType.Object, ParameterDirection.Input);
            parameters.Add("@PageIndex", userQueryString.PageIndex, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@PageSize", userQueryString.PageSize, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@SortBy", userQueryString.SortBy, DbType.String, ParameterDirection.Input);
            parameters.Add("@SortOrder", userQueryString.SortOrder, DbType.String, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Get_Users", parameters, commandType: CommandType.StoredProcedure).Read<UserModel>().ToList();
            }
            return result;
        }

        public bool DeleteUser(long id)
        {
            bool result = false;
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", id, DbType.Int64, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Delete_User", parameters, commandType: CommandType.StoredProcedure).Read<bool>().FirstOrDefault();
            }
            return result;
        }

        public UserModel UpdateUser(UserModel userModel)
        {
            UserModel result = new UserModel();
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userModel.UserId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@UserName", userModel.UserName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", userModel.Password, DbType.String, ParameterDirection.Input);
            parameters.Add("@FirstName", userModel.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastName", userModel.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("@DateOfBirth", userModel.DateOfBirth, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@PhoneNumber", userModel.PhoneNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@CountryId", userModel.CountryId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@CityId", userModel.CityId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Role", userModel.Role, DbType.String, ParameterDirection.Input);
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.QueryMultiple("Update_User", parameters, commandType: CommandType.StoredProcedure).ReadFirstOrDefault<UserModel>();
            }
            return result;
        }
    }
}
