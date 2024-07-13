using Blazored.LocalStorage;
using BlazorImplementation.Models.FormModels;
using BlazorImplementation.Services;
using BlazorImplementation.Shared.providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
namespace BlazorImplementation.Pages
{
    [AllowAnonymous]
    public partial class Login
    {
        public bool HidePassword { get; set; } = true;
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        //[Inject]
        //public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        private LoaderService _LoadingService { get; set; }

        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager _NavigationManager { get; set; }

        [Inject]
        public SessionStorageAccessor SessionStorageAccessor { get; set; }

        [Inject]
        public NotificationService NotificationService { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> authState { get; set; }
        public LoginFormModel tokenModel { get; set; } = new();

        public string APIErrorMessage { get; set; } = string.Empty;

        TelerikNotification telerikNotification { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var user = (await authState).User;
            if (user.Identity.IsAuthenticated)
            {
                _NavigationManager.NavigateTo("/");
            }
            base.OnInitializedAsync();
        }

        public async Task RevealPassword()
        {
            HidePassword = false;
            await Task.Delay(700);
            HidePassword = true;
        }

        public async Task SubmitLogin()
        {
            _LoadingService.StartLoading();
            var httpClient = HttpClientFactory.CreateClient("API");
            var response = await httpClient.PostAsJsonAsync<object>("User/login", tokenModel);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = await response.Content.ReadAsStringAsync();
                // await LocalStorage.SetItemAsync<string>("jwt-access-token", token);
                await SessionStorageAccessor.SetValueAsync("jwt-access-token", token);
                (authenticationStateProvider as CustomAuthProvider).NotifyAuthState();
                _NavigationManager.NavigateTo("/fetchdata", forceLoad:true);
                NotificationService.ShowNotification("Log in Success!!", "Success");
                _LoadingService.StopLoading();
            }
            else
            {
                NotificationService.ShowNotification("Please enter a valid Username and password combination", "error");
                _LoadingService.StopLoading();
                //APIErrorMessage = "Please enter a valid Username and password combination";
            }
        }
    }
}
