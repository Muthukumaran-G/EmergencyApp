using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ViewModel ViewModel;
        Location CurrentLocation { get; set; }

        public MainPage(ViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
            this.Title = "Hello " + viewModel.UserName;
        }

        private void About_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }

        private void AddContacts_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new AddRecipients() { BindingContext = ViewModel });
        }

        protected async override void OnAppearing()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
            this.ViewModel.FrameVisibility = true;
            parentGrid.Opacity = 0.2;
            if (CurrentLocation == null)
                await GetLocation();
            this.ViewModel.FrameVisibility = false;
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, true);
            parentGrid.Opacity = 1;
        }

        private async void ToolBar_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.FrameVisibility)
                return;

            this.ViewModel.FrameVisibility = true;
            parentGrid.Opacity = 0.2;
            await Task.Delay(2000);
            ViewModel.database.DeleteAll<RecipientModel>();
            ViewModel.database.DeleteAll<User>();
            await Navigation.PopToRootAsync();
            this.ViewModel = null;
            this.BindingContext = null;
            await DisplayAlert("Success", "Logout successful. Account deleted successfully.", "OK");
            var newViewModel = new ViewModel();
            App.Current.MainPage = new LoginPage(newViewModel) { BindingContext = newViewModel };
        }

        private async void SendSMS()
        {
            if (CurrentLocation != null)
            {
                var uri = string.Empty;
                uri = $"https://" + $"maps.google.com/maps?q=@{CurrentLocation.Latitude},{CurrentLocation.Longitude},{CurrentLocation.Altitude}";

                System.Diagnostics.Debug.WriteLine(uri);
                await SMS.SendSms($"This is {ViewModel.UserName}. I need your help. I am currently here " + System.Environment.NewLine + " " + uri, ViewModel.contactNames);
            }
            else
            {
                await GetLocation();
                SendSMS();
            }
        }

        private async Task<Location> GetLocation()
        {
            this.ViewModel.FrameVisibility = true;
            parentGrid.Opacity = 0.2;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                CurrentLocation = await Geolocation.GetLocationAsync(request).ConfigureAwait(false);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Feature not supported", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Feature not enabled", "OK");
            }
            catch (PermissionException pEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Permission not granted", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Unable to get location", "OK");
            }

            this.ViewModel.FrameVisibility = false;
            parentGrid.Opacity = 1;
            return CurrentLocation;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.FrameVisibility)
                return;

            if (ViewModel.OrderList.Count() > 0)
            {
                SendSMS();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Please add the recipients", "OK");
            }
        }
    }
}
