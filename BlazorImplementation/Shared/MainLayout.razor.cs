using BlazorImplementation.Shared.providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components.Drawer;

namespace BlazorImplementation.Shared
{
    public partial class MainLayout
    {

        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject]
        public SessionStorageAccessor SessionStorageAccessor { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private string UserDisplayName(List<Claim> claims)
        {
            var email = claims.Where(_ => _.Type == "email").Select(_ => _.Value).FirstOrDefault();
            return email;
        }

        public async Task Logout()
        {
            await SessionStorageAccessor.RemoveAsync("jwt-access-token");
            (authenticationStateProvider as CustomAuthProvider).NotifyAuthState();
            NavigationManager.NavigateTo("/login");
        }
    }
}
