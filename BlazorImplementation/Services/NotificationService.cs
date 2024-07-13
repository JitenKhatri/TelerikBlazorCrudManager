namespace BlazorImplementation.Services
{
    public class NotificationService
    {
        public event Action<string, string> OnShow;

        public void ShowNotification(string message, string theme)
        {
            OnShow?.Invoke(message, theme);
        }
    }
}
