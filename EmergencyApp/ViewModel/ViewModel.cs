using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class ViewModel : NotificationObject
    {
        public SQLiteConnection database;
        public string[] contactNames;

        private bool frameVisibility;
        public string userName;
        public bool FrameVisibility
        {
            get
            {
                return frameVisibility;
            }
            set
            {
                frameVisibility = value;
                RaisePropertyChanged("FrameVisibility");
            }
        }

        private bool isLoginVisible;

        public bool IsLoginVisible
        {
            get
            {
                return isLoginVisible;
            }
            set
            {
                isLoginVisible = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public Command SOS { get; set; }
        public Command AddContactPage { get; set; }
        public Command AboutPage { get; set; }
        public Command Logout { get; set; }
        public Command LogIn { get; set; }
        internal Location CurrentLocation { get; set; }


        public ViewModel()
        {
            IsLoginVisible = true;
            SOS = new Command(SosCommand);
            AddContactPage = new Command(AddContactPageCommand);
            AboutPage = new Command(AboutPageCommand);
            Logout = new Command(LogoutCommand);
            LogIn = new Command(LogInCommand);
            database = DependencyService.Get<ISQLite>().GetConnection();
            // Create the table
            database.CreateTable<RecipientModel>();
            database.CreateTable<User>();
            if (database != null)
            {
                var recipientsTable = (from i in database.Table<RecipientModel>() select i);
                contactNames = new string[recipientsTable.Count()];

                for (int i = 0; i < recipientsTable.Count(); i++)
                {
                    contactNames[i] = recipientsTable.ToList().ToArray()[i].RecipientNumber;
                }

                var userTable = from i in database.Table<User>() select i;

                if (userTable.Count() > 0)
                    this.UserName = userTable.ToList().ToArray()[0].UserName;
            }
        }

        private async void LogInCommand(object obj)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                await App.Current.MainPage.DisplayAlert("!!", "Please enter user name", "OK");
            }
            else
            {
                IsLoginVisible = false;
                GetLocation();
                await Task.Delay(2000);
                //if (CurrentLocation == null)
                //    await GetLocation();
                database.Insert(new User() { UserName = this.UserName });
                App.Current.MainPage = new NavigationPage(new MainPage(this) { BindingContext = this });
            }

        }

        private async void LogoutCommand(object obj)
        {
            if (this.FrameVisibility)
                return;

            this.FrameVisibility = true;
            await Task.Delay(2000);
            database.DeleteAll<RecipientModel>();
            database.DeleteAll<User>();
            await App.Current.MainPage.Navigation.PopToRootAsync();
            await App.Current.MainPage.DisplayAlert("Success", "Logout successful. Account deleted successfully.", "OK");
            var newViewModel = new ViewModel();
            App.Current.MainPage = new LoginPage() { BindingContext = newViewModel };
        }

        private void AboutPageCommand(object obj)
        {
            App.Current.MainPage.Navigation.PushModalAsync(new AboutPage());
        }

        private void AddContactPageCommand(object obj)
        {
            App.Current.MainPage.Navigation.PushModalAsync(new AddRecipients());
        }

        private async void SosCommand(object obj)
        {
            var recipientsTable = from i in database.Table<RecipientModel>() select i;
            contactNames = new string[recipientsTable.Count()];

            for (int i = 0; i < recipientsTable.Count(); i++)
            {
                contactNames[i] = recipientsTable.ToList().ToArray()[i].RecipientNumber;
            }

            if (contactNames.Count() > 0)
            {
                SendSMS();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Please add the recipients", "OK");
            }
        }


        private async void SendSMS()
        {
            if (CurrentLocation != null)
            {
                var uri = string.Empty;
                uri = $"https://" + $"maps.google.com/maps?q=@{CurrentLocation.Latitude},{CurrentLocation.Longitude},{CurrentLocation.Altitude}";

                System.Diagnostics.Debug.WriteLine(uri);
                await SMS.SendSms($"This is {UserName}. I need your help. I am currently here " + System.Environment.NewLine + " " + uri, contactNames);
            }
            else
            {
                await GetLocation();
                SendSMS();
            }
        }

        internal async Task<Location> GetLocation()
        {
            FrameVisibility = true;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                CurrentLocation = await Geolocation.GetLocationAsync(request).ConfigureAwait(false);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "GPS not supported", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "GPS not enabled", "OK");
            }
            catch (PermissionException pEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Permission not granted", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Unable to get location", "OK");
            }

            FrameVisibility = false;
            return CurrentLocation;
        }
    }

    public class NotificationObject : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public void RaisePropertyChanged([CallerMemberName] string propName = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseCollectionChanged([CallerMemberName] string propName = "")
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
