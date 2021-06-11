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
            this.ViewModel.FrameVisibility = true;
            parentGrid.Opacity = 0.2;
            if (ViewModel.CurrentLocation == null)
                await ViewModel.GetLocation();
            this.ViewModel.FrameVisibility = false;
            parentGrid.Opacity = 1;
        }
    }
}
