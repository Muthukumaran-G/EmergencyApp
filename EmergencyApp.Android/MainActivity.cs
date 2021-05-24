using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Xamarin.Forms;
using EmergencyApp.Droid;
using Android.Content;
using Android.Net;
using Android.Support.V4.App;
using Android;
using AndroidX.Core.App;

[assembly: Dependency(typeof(SendMessage))]
namespace EmergencyApp.Droid
{
    [Activity(Label = "EmergencyApp", Icon = "@mipmap/siren", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Activity Activity;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Activity = this;
            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadSms }, 1);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class SendMessage : ISendMessage
    {
        void ISendMessage.SendMessage(string message, string[] recipents)
        {
            SmsManager sms = SmsManager.Default;
            foreach (var contact in recipents)
            {
                sms.SendTextMessage(contact, null, message, null, null);
            }
        }
    }
}