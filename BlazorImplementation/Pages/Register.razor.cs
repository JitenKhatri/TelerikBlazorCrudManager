using Blazored.FluentValidation;
using BlazorImplementation.Models;
using BlazorImplementation.Models.FormModels;
using BlazorImplementation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Telerik.Blazor.Components;

namespace BlazorImplementation.Pages
{
    [AllowAnonymous]
    public partial class Register
    {
        public int Value { get; set; }
        public bool HidePassword { get; set; } = true;
        public bool HideConfirmPassword { get; set; } = true;
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [CascadingParameter]
        public Task<AuthenticationState> authState { get; set; } = null!;
        public RegisterFormModel User { get; set; } = new();

        public EditContext FormContext { get; set; } = null!;

        public ValidationMessageStore ValidationMessageStore { get; set; } = null!;

        public List<string> Roles = new List<string>() { "Admin","Customer"};

        public List<CountryModel> CountryList { get; set; }

        public List<CityModel> CityList { get; set; }

        [Inject]
        public NotificationService notificationService { get; set; } = null!;

        [Inject]
        public LoaderService _LoaderService { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            FormContext = new(User);
            CountryList = GetCountries();
            ValidationMessageStore = new(FormContext);
            var user = (await authState).User;
            if (user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/");
            }
            base.OnInitializedAsync();
        }

        private void InitRegistration()
        {
            FormContext = new(User);
        }

        private void ResetRegisterForm()
        {
            User = new RegisterFormModel();
            InitRegistration();
        }
        public void OnRegistrationStepChange()
        {
            var isFormValid = FormContext.Validate();
            if (isFormValid)
            {
                Value += 1;
            }
        }
        public async Task RevealPassword()
        {
            HidePassword = false;
            await Task.Delay(700);
            HidePassword = true;
        }
        public async Task RevealConfirmPassword()
        {
            HideConfirmPassword = false;
            await Task.Delay(700);
            HideConfirmPassword = true;
        }

        private List<CountryModel> GetCountries()
        {
            var httpClient = HttpClientFactory.CreateClient("API");
            var response = httpClient.GetFromJsonAsync<List<CountryModel>>("Country/GetCountries").GetAwaiter().GetResult();
            return response;
        }
        public async Task HandleValidSubmit()
        {
            _LoaderService.StartLoading();
            try
            {
                var httpClient = HttpClientFactory.CreateClient("API");
                var response = await httpClient.PostAsJsonAsync<object>("User/register", User);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NavigationManager.NavigateTo("/login");
                    notificationService.ShowNotification("Register Successfully Please login to continue", "Success");
                    _LoaderService.StopLoading();
                }
                else
                {
                    notificationService.ShowNotification("Failed To Register User Please Try After SomeTime", "error");
                    _LoaderService.StopLoading();
                }
            }
            catch (Exception ex)
            {
                _LoaderService.StopLoading();
                notificationService.ShowNotification(ex.Message, "error");
            }
        }

        private void OnCountrySelect(long countryId)
        {
            User.CountryId = countryId;
            _LoaderService.StartLoading();
            if (countryId != null && countryId != 0)
            {
                var httpClient = HttpClientFactory.CreateClient("API");
                var result = httpClient.GetFromJsonAsync<List<CityModel>>($"City/GetCities/?CountryId={countryId}").GetAwaiter().GetResult();
                CityList = result;
            }
            else
            {
                CityList = new List<CityModel>();
            }
            _LoaderService.StopLoading();
        }
    }
}
