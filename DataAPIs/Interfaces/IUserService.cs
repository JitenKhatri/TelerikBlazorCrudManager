using BlazorImplementation.Models;
using BlazorImplementation.Models.QueryStrings;

namespace DataAPIs.Interfaces
{
    public interface IUserService
    {
        public UserModel Login(TokenModel token);

        public UserModel Register(UserModel user);

        public List<UserModel> GetUsers(UserQueryString userQueryString);

        public bool DeleteUser(long id);

        public UserModel UpdateUser(UserModel userModel);
    }
}
