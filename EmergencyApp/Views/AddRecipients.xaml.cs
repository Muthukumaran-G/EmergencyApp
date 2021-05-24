using SQLite;
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
    public partial class AddRecipients : ContentPage
    {
        public AddRecipients()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext == null)
                return;

            if ((this.BindingContext as ViewModel).OrderList.Count > 0)
                label.Text = "Added contacts";
            else
                label.Text = "No contact added..!";
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var table = (from i in (this.BindingContext as ViewModel).database.Table<RecipientModel>() select i);

                (this.BindingContext as ViewModel).OrderList.Remove(e.Item as RecipientModel);
                (this.BindingContext as ViewModel).database.Query<RecipientModel>("DELETE from RecipientModel where Recipients =" + (e.Item as RecipientModel).Recipients).FirstOrDefault();
                (this.BindingContext as ViewModel).contactNames = new string[table.Count()];

                if (table.Count() > 0)
                {
                    for (int i = 0; i < table.Count(); i++)
                    {
                        (this.BindingContext as ViewModel).contactNames[i] = (table.ToList().ToArray()[i].Recipients);
                    }
                }
                else
                    label.Text = "No contact added..!";
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Alert", "Unable to delete contact, please try re-installing the application.", "OK");
            }
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            try
            {
                (this.BindingContext as ViewModel).database.Query<RecipientModel>("INSERT INTO RecipientModel (Recipients) values('" + entry.Text + "')");
                (this.BindingContext as ViewModel).OrderList.Add(new RecipientModel() { Recipients = entry.Text });
                (this.BindingContext as ViewModel).contactNames = new string[(this.BindingContext as ViewModel).OrderList.Count()];

                for (int i = 0; i < (this.BindingContext as ViewModel).OrderList.Count(); i++)
                {
                    (this.BindingContext as ViewModel).contactNames[i] = (this.BindingContext as ViewModel).OrderList[i].Recipients;
                }
                entry.Text = null;
                label.Text = "Added contacts";
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Alert", "Unable to add contact, please try re-installing the application.", "OK");
            }
        }
    }
}