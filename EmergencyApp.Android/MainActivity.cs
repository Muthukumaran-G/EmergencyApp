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
using Android.Locations;

[assembly: Dependency(typeof(SendMessage))]
namespace EmergencyApp.Droid
{
    [Activity(Label = "EmergencyApp", Icon = "@mipmap/siren", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity//, ILocationListener
    {
        public static Activity Activity;
        private LocationManager _locMgr;
        GPSLocationService locationService;
        private string _provider = LocationManager.GpsProvider;
        private object _locationProvider;
        private SMSSentReceiver _smsSentBroadcastReceiver;
        private SMSDeliveredReceiver _smsDeliveredBroadcastReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Activity = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(_smsSentBroadcastReceiver);
            UnregisterReceiver(_smsDeliveredBroadcastReceiver);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _smsSentBroadcastReceiver = new SMSSentReceiver();
            _smsDeliveredBroadcastReceiver = new SMSDeliveredReceiver();

            RegisterReceiver(_smsSentBroadcastReceiver, new IntentFilter("SMS_SENT"));
            RegisterReceiver(_smsDeliveredBroadcastReceiver, new IntentFilter("SMS_DELIVERED"));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    [BroadcastReceiver(Exported = true, Permission = "//receiver/@android:android.permission.SEND_SMS")]
    public class SMSSentReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch ((int)ResultCode)
            {
                case (int)Result.Ok:
                    Toast.MakeText(context, "SMS has been sent", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.GenericFailure:
                    Toast.MakeText(context, "Generic Failure", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.NoService:
                    Toast.MakeText(context, "No Service", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.NullPdu:
                    Toast.MakeText(context, "Null PDU", ToastLength.Short).Show();
                    break;
                case (int)SmsResultError.RadioOff:
                    Toast.MakeText(context, "Radio Off", ToastLength.Short).Show();
                    break;
            }
        }
    }

    [BroadcastReceiver(Exported = true, Permission = "//receiver/@android:android.permission.SEND_SMS")]
    public class SMSDeliveredReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch ((int)ResultCode)
            {
                case (int)Result.Ok:
                    Toast.MakeText(context, "SMS Delivered", ToastLength.Short).Show();
                    break;
                case (int)Result.Canceled:
                    Toast.MakeText(context, "SMS not delivered", ToastLength.Short).Show();
                    break;
            }
        }
    }

    public class SendMessage : ISendMessage
    {
        void ISendMessage.SendMessage(string message, string[] recipents)
        {
            SmsManager sms = SmsManager.Default;
            foreach (var contact in recipents)
            {
                var piSent = PendingIntent.GetBroadcast(MainActivity.Activity.ApplicationContext, 0, new Intent("SMS_SENT"), 0);
                var piDelivered = PendingIntent.GetBroadcast(MainActivity.Activity.ApplicationContext, 0, new Intent("SMS_DELIVERED"), 0);
                sms.SendTextMessage(contact, null, message, piSent, piDelivered);
            }
        }
    }
}