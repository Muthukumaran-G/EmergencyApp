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
            //locationService = new GPSLocationService();
            //_locMgr = GetSystemService(LocationService) as LocationManager;
            //var locationCriteria = new Criteria
            //{
            //    Accuracy = Accuracy.Coarse,
            //    PowerRequirement = Power.Medium
            //};
            //if (_locMgr != null)
            //{
            //    _locationProvider = _locMgr.GetBestProvider(locationCriteria, true);
            //    if (_locationProvider != null)
            //    {
            //        _locMgr.RequestLocationUpdates(_provider, 2000, 1, this);
            //    }
            //    else
            //    {
            //        throw new Exception();
            //    }
            //}
        }

        protected override void OnPause()
        {
            base.OnPause();
            //_locMgr.RemoveUpdates(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            //_locMgr.RequestLocationUpdates(_provider, 2000, 1, this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnLocationChanged(Location location)
        {
        }

        //public void OnProviderDisabled(string provider)
        //{
        //    if(provider.Equals("gps"))
        //    {
        //        //locationService.PushGPSState(Boolean.FalseString);
        //        MessagingCenter.Send<Object, string>(this, "GPSState", "false");
        //        Toast.MakeText(Activity, "GPS disabled", ToastLength.Short);
        //    }
        //}

        //public void OnProviderEnabled(string provider)
        //{
        //    if (provider.Equals("gps"))
        //    {
        //        MessagingCenter.Send<Object, string>(this, "GPSState", "true");

        //        //locationService.PushGPSState(Boolean.TrueString);
        //        Toast.MakeText(Activity, "GPS disabled", ToastLength.Short);
        //    }
        //}

        //public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        //{
        //}
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