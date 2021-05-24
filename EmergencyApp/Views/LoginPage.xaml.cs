using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        ViewModel ViewModel;
		public LoginPage (ViewModel viewModel)
		{
			InitializeComponent ();
            this.ViewModel = viewModel;
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.UserName == null || this.ViewModel.UserName.Equals(string.Empty))
            {
                await App.Current.MainPage.DisplayAlert("!!", "Please enter user name", "OK");
            }
            else
            {
                btn.IsVisible = false;
                indicator.IsVisible = true;
                await Task.Delay(2000);
                (this.BindingContext as ViewModel).database.Query<User>("INSERT INTO User (UserName) values('" + this.ViewModel.UserName + "')");
                App.Current.MainPage = new NavigationPage(new MainPage(this.ViewModel) { BindingContext = this.ViewModel });
            }

        }
    }
}