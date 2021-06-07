using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecipients : ContentPage
    {
        ViewModel AddRecipientViewModel;
        public AddRecipients()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext == null)
                return;

            AddRecipientViewModel = this.BindingContext as ViewModel;

            if (AddRecipientViewModel.OrderList.Count > 0)
                label.Text = "Added contacts";
            else
                label.Text = "No contact added..!";
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var table = from i in (this.AddRecipientViewModel).database.Table<RecipientModel>() select i;
                AddRecipientViewModel.OrderList.Remove(e.Item as RecipientModel);
                AddRecipientViewModel.database.Query<RecipientModel>("DELETE from RecipientModel where Recipient =" + (e.Item as RecipientModel).Recipient).FirstOrDefault();
                AddRecipientViewModel.contactNames = new string[table.Count()];

                if (table.Count() > 0)
                {
                    for (int i = 0; i < table.Count(); i++)
                    {
                        AddRecipientViewModel.contactNames[i] = table.ToList().ToArray()[i].Recipient;
                    }
                }
                else
                    label.Text = "No contact added..!";
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Unable to delete contact, please try re-installing the application.", "OK");
            }
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            try
            {
                AddRecipientViewModel.database.Query<RecipientModel>("INSERT INTO RecipientModel (Recipient) values('" + entry.Text + "')");
                AddRecipientViewModel.OrderList.Add(new RecipientModel() { Recipient = entry.Text });
                AddRecipientViewModel.contactNames = new string[AddRecipientViewModel.OrderList.Count()];

                for (int i = 0; i < AddRecipientViewModel.OrderList.Count(); i++)
                {
                    AddRecipientViewModel.contactNames[i] = AddRecipientViewModel.OrderList[i].Recipient;
                }
                entry.Text = null;
                label.Text = "Added contacts";
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Unable to add contact, please try re-installing the application.", "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var contact = await Contacts.PickContactAsync();
        }
    }
}