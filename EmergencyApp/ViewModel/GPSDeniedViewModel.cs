using System;
using System.Collections.Generic;
using System.Text;
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
            var locationAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();
            if (locationAlways == PermissionStatus.Granted)
            {
                DependencyService.Get<ILocationService>().RequestLocation();
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
