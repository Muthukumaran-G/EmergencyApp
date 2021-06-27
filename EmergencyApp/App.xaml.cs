using SQLite;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Montserrat-Bold.ttf",Alias="Montserrat-Bold")]
     [assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
     [assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
     [assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
     [assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]
namespace EmergencyApp
{
    public partial class App : Application
    {
        public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App()
        {
            InitializeComponent();

            var viewModel = new ViewModel();

            if (string.IsNullOrEmpty(viewModel.UserName))
            {
                viewModel.UserName = string.Empty;
                MainPage = new NavigationPage(new LoginPage() { BindingContext = viewModel });
            }
            else
            {
                MainPage = new NavigationPage(new MainPage(viewModel) { BindingContext = viewModel });
            }

            CheckAndRequestLocationPermission();
        }

        public async void CheckAndRequestLocationPermission()
        {
            var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var contactStatus = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            var smsStatus = await Permissions.CheckStatusAsync<Permissions.Sms>();
            locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            contactStatus = await Permissions.RequestAsync<Permissions.ContactsRead>();
            smsStatus = await Permissions.RequestAsync<Permissions.Sms>();

            if (locationStatus != PermissionStatus.Granted || smsStatus != PermissionStatus.Granted)
                await App.Current.MainPage.Navigation.PushAsync(new GPSDeniedPage());
        }

        private async void GPSPermissionCheck()
        {
            var locationAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();

            if(locationAlways == PermissionStatus.Denied)
            {
                await App.Current.MainPage.Navigation.PushAsync(new GPSDeniedPage());
            }
        }

        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
