using SQLite;
using System;
using System.Linq;
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
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            
            
            DependencyService.Get<ILocationService>().GPSStateChanged += App_GPSStateChanged;
            DependencyService.Get<ILocationService>().LocationChanged += App_LocationChanged;
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

            SMSAccessCheck();
            GPSPermissionCheck();
        }

        internal static async Task<PermissionStatus> SMSAccessCheck()
        {
            return await Permissions.RequestAsync<Permissions.Sms>();
            
            
        }

        private async void GPSPermissionCheck()
        {
            var locationAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();

            if(locationAlways == PermissionStatus.Denied)
            {
                await App.Current.MainPage.Navigation.PushAsync(new GPSDeniedPage());
            }

            
        }

        private void App_LocationChanged(object sender, LocationEventArgs e)
        {

        }

        private void App_GPSStateChanged(object sender, GPSStateEventArgs e)
        {
            if(e.IsGPSEnabled)
            {
                DependencyService.Get<IToastMessage>().ShowToast("GPS enabled");
            }
            else
            {
                DependencyService.Get<IToastMessage>().ShowToast("GPS disabled");
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                DependencyService.Get<IToastMessage>().ShowToast("Network connected");
            }
            else
            {
                DependencyService.Get<IToastMessage>().ShowToast("Network disconnected");
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
