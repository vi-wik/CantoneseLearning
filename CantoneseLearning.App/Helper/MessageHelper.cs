using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace viwik.CantoneseLearning.App.Helper
{
    public class MessageHelper
    {
        public static async void ShowToastMessage(string message, ToastDuration duration=ToastDuration.Short)
        {
            var toast = Toast.Make(message, duration);

            await toast.Show(CancellationToken.None);
        }
    }
}
