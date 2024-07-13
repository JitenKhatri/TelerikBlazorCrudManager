using BlazorImplementation.Models;
using BlazorImplementation.Models.FormModels;
using BlazorImplementation.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace BlazorImplementation.Shared
{
    public partial class AddEditUser
    {
        [Parameter]
        public RegisterFormModel User { get; set; } = new RegisterFormModel();

        [Parameter]
        public EventCallback<RegisterFormModel> OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public bool IsVisible { get; set;} = false;

        [Parameter]
        public string title { get; set; }

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; } = null!;

        public List<string> Roles = new List<string>() { "Admin", "Customer" };

        public bool HidePassword { get; set; } = true;
        public bool HideConfirmPassword { get; set; } = true;

        public List<CountryModel> CountryList { get; set; }

        public List<CityModel> CityList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CountryList = GetCountries();
        }
        public async Task HandleValidSubmit()
        {
            await OnSave.InvokeAsync(User);
        }

        public async Task OnCancelform()
        {
            await OnCancel.InvokeAsync();
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

        private void OnCountrySelect(long countryId)
        {
            User.CountryId = countryId;
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
        }
    }
}
