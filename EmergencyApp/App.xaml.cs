using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var viewModel = new ViewModel();
            if (viewModel.UserName == null || viewModel.UserName.Equals(string.Empty) || viewModel.OrderList.Count() <= 0)
            {
                MainPage = new LoginPage(viewModel) { BindingContext = viewModel };
            }
            else
            {
                MainPage = new NavigationPage(new MainPage(viewModel) { BindingContext = viewModel });
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
