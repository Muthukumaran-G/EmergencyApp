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

[assembly: Xamarin.Forms.Dependency(typeof(GPSLocationService))]
namespace EmergencyApp.Droid
{
    public class GPSLocationService : ILocationService
    {
        public GPSListener Listener;
        private readonly LocationManager _locMgr;

        internal void PushLocation(Location location)
        {
            OnLocationChanged(new LocationEventArgs(location.Latitude, location.Longitude, location.Altitude));
        }

        private readonly Criteria _locationCriteria;

        internal void PushGPSState(string boolString)
        {
            OnGPSStateChanged(new GPSStateEventArgs(boolString));
        }

        //public GPSLocationService()
        //{
        //}

        public GPSLocationService()
        {
            _locMgr = Application.Context.GetSystemService(Context.LocationService) as LocationManager;
            Listener = new GPSListener(this);
            _locationCriteria = new Criteria
            {
                Accuracy = Accuracy.Coarse,
                PowerRequirement = Power.Medium
            };
            if (_locMgr == null)
            {
                throw new Exception("No LocationManager instance!");
            }
        }

        public event EventHandler<LocationEventArgs> LocationChanged;
        public event EventHandler<GPSStateEventArgs> GPSStateChanged;

        public void RequestLocation()
        {
            var provider = _locMgr.GetBestProvider(_locationCriteria, true);
            if (provider == null)
            {
                throw new Exception("No GPS provider could be found for given criteria!");
            }

            _locMgr.RequestLocationUpdates(provider, 2000, 1, Listener);
        }
        public void StopRequests()
        {
            _locMgr.RemoveUpdates(Listener);
        }
        protected virtual void OnLocationChanged(LocationEventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }

        protected virtual void OnGPSStateChanged(GPSStateEventArgs e)
        {
            GPSStateChanged?.Invoke(this, e);
        }
    }
}