using EmergencyApp.Resx;
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
            MessagingCenter.Subscribe<object, int>(this, "SentReceiver", (sender, arg) =>
            {
                switch (arg)
                {
                    case -1:
                        DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.SmsSent);
                        break;
                    case 1:
                        DependencyService.Get<IToastMessage>().ShowToast("Generic failure");

                        break;
                    case 2:
                        DependencyService.Get<IToastMessage>().ShowToast("Radio off");

                        break;
                    case 3:
                        DependencyService.Get<IToastMessage>().ShowToast("NullPdu");
                        
                        break;
                    case 5:
                        DependencyService.Get<IToastMessage>().ShowToast("Limit exceeded");

                        break;
                    case 7:
                        DependencyService.Get<IToastMessage>().ShowToast("Short code not allowed");

                        break;
                    case 8:
                        DependencyService.Get<IToastMessage>().ShowToast("Short Code Never Allowed");

                        break;

                    default:
                    case 4:
                        DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.NoService);
                        break;
                }
            });

            MessagingCenter.Subscribe<object, int>(this, "DeliveredReceiver", (sender, arg) =>
            {
                switch (arg)
                {
                    case -1:
                        DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.SmsDelivered);
                        break;

                    default:
                    case 0:
                        DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.SmsNotDelivered);
                        break;
                }
            });

            var viewModel = new ViewModel();
            if (string.IsNullOrEmpty(viewModel.UserDetails.UserName))
            {
                viewModel.UserDetails.UserName = string.Empty;
                MainPage = new NavigationPage(new LoginPage() { BindingContext = viewModel });
            }
            else
            {
                MainPage = new NavigationPage(new MainPage(viewModel) { BindingContext = viewModel });
            }

            CheckAndRequestLocationPermission();
        }

        internal static void ChangeLanguage(string currentLanguage)
        {
            CultureInfo.CurrentCulture.ClearCachedData();
            CultureInfo.CurrentUICulture.ClearCachedData();
            CultureInfo language;
            switch (currentLanguage)
            {
                default:
                case "English":
                    language = new CultureInfo("en");
                    break;

                case "தமிழ்":
                    language = new CultureInfo("ta");
                    break;

                case "తెలుగు":
                    language = new CultureInfo("te");
                    break;

                case "മലയാളം":
                    language = new CultureInfo("ml");
                    break;

                case "हिंदी":
                    language = new CultureInfo("hi");
                    break;

                case "Français":
                    language = new CultureInfo("fr");
                    break;

                case "Italiano":
                    language = new CultureInfo("it");
                    break;

                case "日本語":
                    language = new CultureInfo("ja");
                    break;

                case "中国人":
                    language = new CultureInfo("zh");
                    break;
            }

            Thread.CurrentThread.CurrentUICulture = language;
            EmergencyAppResources.Culture = language;
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
