using BlazorImplementation.Models;

namespace DataAPIs.Interfaces
{
    public interface IJWTTokenService
    {
        string BuildToken(UserModel userModel);
    }
}
