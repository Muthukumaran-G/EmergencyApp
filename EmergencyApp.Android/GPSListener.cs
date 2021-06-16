using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EmergencyApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(GPSListener))]
namespace EmergencyApp.Droid
{
    public class GPSListener : Java.Lang.Object, ILocationListener
    {
        private readonly GPSLocationService locationService;
        public void OnLocationChanged(Location location)
        {
            locationService.PushLocation(location);
        }

        public GPSListener(GPSLocationService gpsLocationService)
        {
            locationService = gpsLocationService;
        }

        public void OnProviderDisabled(string provider)
        {
            locationService.PushGPSState(Boolean.FalseString);
        }

        public void OnProviderEnabled(string provider)
        {
            locationService.PushGPSState(Boolean.TrueString);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}