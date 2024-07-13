using BlazorImplementation.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorImplementation.Shared
{
    public partial class LoaderComponent
    {
        [Inject]
        private LoaderService LoadingService { get; set; }

        private bool IsLoading { get; set; }

        protected override void OnInitialized()
        {
            LoadingService.OnLoadingChanged += HandleLoadingChanged;
        }

        private void HandleLoadingChanged(bool isLoading)
        {
            IsLoading = isLoading;
            InvokeAsync(StateHasChanged); // Thread safety
        }
    }
}
