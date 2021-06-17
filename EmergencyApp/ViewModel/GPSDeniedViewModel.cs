using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class GPSDeniedViewModel: NotificationObject
    {
        private string imagePath;

        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
                RaisePropertyChanged();
            }
        }

        public Command Retry { get; set; }

        public GPSDeniedViewModel()
        {
            Retry = new Command(RetryCommand);
            ImagePath = "LocationAccessDenied.svg";
        }

        private async void RetryCommand(object obj)
        {
            var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            //var contactStatus = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            var smsStatus = await Permissions.CheckStatusAsync<Permissions.Sms>();

            if (locationStatus != PermissionStatus.Granted || smsStatus != PermissionStatus.Granted)
            {
                AppInfo.ShowSettingsUI();
            }

            if (locationStatus == PermissionStatus.Granted && smsStatus == PermissionStatus.Granted)
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        internal async Task<Location> GetLocation()
        {
            Location CurrentLocation = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                CurrentLocation = await Geolocation.GetLocationAsync(request).ConfigureAwait(false);
                DependencyService.Get<IToastMessage>().ShowToast("Location accquired");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                DependencyService.Get<IToastMessage>().ShowToast("GPS not supported");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                DependencyService.Get<IToastMessage>().ShowToast("GPS not enabled");
            }
            catch (PermissionException pEx)
            {
                DependencyService.Get<IToastMessage>().ShowToast("Permission denied");
                //await App.Current.MainPage.Navigation.PushAsync(new GPSDeniedPage());
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShowToast("Unable to get location. Check connection.");
            }

            return CurrentLocation;
        }
    }
}
