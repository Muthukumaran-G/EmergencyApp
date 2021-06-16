using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EmergencyApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessageRenderer))]
namespace EmergencyApp.Droid
{
    internal class ToastMessageRenderer : IToastMessage
    {
        public void ShowToast(string message, AlertDuration Alert = AlertDuration.ShortAlert)
        {
            MainActivity.Activity.RunOnUiThread(() =>
            {
                var toast = Toast.MakeText(Application.Context, message, ToastLength.Short);
                toast.Duration = Alert == AlertDuration.ShortAlert ? ToastLength.Short : ToastLength.Long;
                Color c = Color.Gray;
                ColorMatrixColorFilter CM = new ColorMatrixColorFilter(new float[]
                    {
        0,0,0,0,c.R,
        0,0,0,0,c.G,
        0,0,0,0,c.B,
        0,0,0,1,0
                    });
                toast.View.Background.SetColorFilter(CM);
                var messageView = (toast.View as LinearLayout).GetChildAt(0);
                (messageView as TextView).SetTextColor(Color.White);
                toast.Show();
            });
        }
    }
}