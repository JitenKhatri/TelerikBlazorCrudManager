using BlazorImplementation.Services;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Resources;

namespace BlazorImplementation.Shared
{
    public partial class Notification
    {
        [Inject]
        private NotificationService notificationService { get; set; }

        TelerikNotification telerikNotification { get; set; }

        protected override void OnInitialized()
        {
            notificationService.OnShow += ShowNotification;
        }

        private void ShowNotification(string message, string theme)
        {
            telerikNotification.Show(new NotificationModel()
            {
                Text = message,
                ThemeColor = theme == "Success" ? ThemeConstants.Notification.ThemeColor.Success : ThemeConstants.Notification.ThemeColor.Error,
                ShowIcon = true,
                CloseAfter = 6000
            });
        }

        public void Dispose()
        {
            notificationService.OnShow -= ShowNotification;
        }
    }
}
