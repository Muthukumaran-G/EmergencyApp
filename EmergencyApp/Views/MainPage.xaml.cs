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

        public MainPage(ViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            parentGrid.Opacity = 0.2;
            if (ViewModel.CurrentLocation == null)
                await ViewModel.GetLocation();
            parentGrid.Opacity = 1;
            try
            {
                DependencyService.Get<ILocationService>().RequestLocation();
            }
            catch(Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShowToast("Enable GPS.");
            }
        }

        double previousTime = 0;

        protected override bool OnBackButtonPressed()
        {
            var currentTime = new TimeSpan(DateTime.Now.Ticks).TotalSeconds;
            if (previousTime == 0 || currentTime - previousTime > 7)
            {
                previousTime = currentTime;
                DependencyService.Get<IToastMessage>().ShowToast("Press back again to close");
                return true;
            }
            else
            {
                previousTime = 0;
                return base.OnBackButtonPressed();
            }
        }
    }
}
