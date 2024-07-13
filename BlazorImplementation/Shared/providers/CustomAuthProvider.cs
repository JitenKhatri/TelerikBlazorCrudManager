using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
 
namespace BlazorImplementation.Shared.providers
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        // private readonly ILocalStorageService _localStorageService;

        private readonly SessionStorageAccessor sessionStorageAccessor;
        public CustomAuthProvider(//ILocalStorageService localStorageService,
            SessionStorageAccessor sessionStorageAccessor)
        {
            // _localStorageService = localStorageService;
            this.sessionStorageAccessor = sessionStorageAccessor;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // var jwtToken = await _localStorageService.GetItemAsync<string>("jwt-access-token");
            var jwtToken = await sessionStorageAccessor.GetValueAsync<string>("jwt-access-token");
            if (string.IsNullOrEmpty(jwtToken) || !TokenIsValid(jwtToken))
            {
                return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity()));
            }
            return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(jwtToken), "jwtAuth")));
        }
 
        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
 
            var jsonBytes = ParseBase64WithoutPadding(payload);
 
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
 
        public void NotifyAuthState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public static bool TokenIsValid(string token)
        {
            var payload = token.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(keyValuePairs["exp"].ToString())).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }
    }
}