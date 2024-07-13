namespace BlazorImplementation.Services
{
    public class LoaderService
    {
        public event Action<bool> OnLoadingChanged;

        private int _loadingCount = 0;

        public void StartLoading()
        {
            _loadingCount++;
            NotifyLoadingChanged();
        }

        public void StopLoading()
        {
            _loadingCount--;
            NotifyLoadingChanged();
        }

        private void NotifyLoadingChanged()
        {
            OnLoadingChanged?.Invoke(_loadingCount > 0);
        }
    }
}
