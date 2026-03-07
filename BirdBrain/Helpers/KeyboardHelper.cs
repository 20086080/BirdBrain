// File: Helpers/KeyboardHelper.cs
using System.Threading.Tasks;
#if ANDROID
using Android.Views.InputMethods;
using Microsoft.Maui.ApplicationModel;
#endif
#if IOS
using UIKit;
#endif
namespace BirdBrain.Helpers
{
    public static class KeyboardHelper
    {
        public static async Task DismissAsync()
        {
#if ANDROID
            var activity = Platform.CurrentActivity;
            var imm = activity.GetSystemService(Android.Content.Context.InputMethodService) as InputMethodManager;
            var currentFocus = activity.CurrentFocus;

            if (currentFocus != null && imm != null)
            {
                imm.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }

            // Small delay to let the keyboard fully dismiss
            await Task.Delay(50);
#elif IOS
            UIApplication.SharedApplication.KeyWindow?.EndEditing(true);
            await Task.Delay(50); // small delay to let UI settle
#endif
        }
    }
}