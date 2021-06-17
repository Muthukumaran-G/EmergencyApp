using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EmergencyApp
{
    public interface ILocationService
    {
        event EventHandler<LocationEventArgs> LocationChanged;
        event EventHandler<GPSStateEventArgs> GPSStateChanged;
        void RequestLocation();
        void StopRequests();
    }

    public class LocationEventArgs : EventArgs
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public double Altitude { get; }
        public LocationEventArgs(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }
    }

    public class GPSStateEventArgs : EventArgs
    {
        public GPSStateEventArgs(string boolString)
        {
            IsGPSEnabled = Convert.ToBoolean(boolString);
        }

        public bool IsGPSEnabled { get; }
    }
}
