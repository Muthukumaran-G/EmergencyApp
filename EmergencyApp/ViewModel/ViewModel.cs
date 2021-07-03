using EmergencyApp.Resx;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        private bool isSmsAccessDenied;

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

        public bool showLanguagePopup;
        public bool ShowLanguagePopup
        {
            get
            {
                return showLanguagePopup;
            }
            set
            {
                showLanguagePopup = value;
                RaisePropertyChanged();
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

        private bool contactTagVisible;

        public bool ContactTagVisible
        {
            get
            {
                return contactTagVisible;
            }
            set
            {
                contactTagVisible = value;
                RaisePropertyChanged();
                if (contactTagVisible)
                {
                    AboutTagVisible = false;
                    LogoutTagVisible = false;
                    HelpTextTagVisible = false;
                    TranslateTagVisible = false;
                }
            }
        }

        private bool aboutTagVisible;

        public bool AboutTagVisible
        {
            get
            {
                return aboutTagVisible;
            }
            set
            {
                aboutTagVisible = value;
                RaisePropertyChanged();
                if(aboutTagVisible)
                {
                    ContactTagVisible = false;
                    LogoutTagVisible = false;
                    HelpTextTagVisible = false;
                    TranslateTagVisible = false;
                    MessagingCenter.Send<object, string>(this, "BackgroundTapped", "AboutTagVisible");
                }
            }
        }

        private bool logoutTagVisible;

        public bool LogoutTagVisible
        {
            get
            {
                return logoutTagVisible;
            }
            set
            {
                logoutTagVisible = value;
                RaisePropertyChanged();
                if(logoutTagVisible)
                {
                    AboutTagVisible = false;
                    ContactTagVisible = false;
                    HelpTextTagVisible = false;
                    TranslateTagVisible = false;
                    MessagingCenter.Send<object, string>(this, "BackgroundTapped", "LogoutTagVisible");
                }
            }
        }

        private bool helpTextTagVisible;

        public bool HelpTextTagVisible
        {
            get
            {
                return helpTextTagVisible;
            }
            set
            {
                helpTextTagVisible = value;
                RaisePropertyChanged();
                if (helpTextTagVisible)
                {
                    AboutTagVisible = false;
                    ContactTagVisible = false;
                    LogoutTagVisible = false;
                    TranslateTagVisible = false;
                }
            }
        }

        private bool translateTagVisible;

        public bool TranslateTagVisible
        {
            get
            {
                return translateTagVisible;
            }
            set
            {
                translateTagVisible = value;
                RaisePropertyChanged();
                if (translateTagVisible)
                {
                    AboutTagVisible = false;
                    ContactTagVisible = false;
                    HelpTextTagVisible = false;
                    LogoutTagVisible = false;
                }
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

        public string helpText;
        public string HelpText
        {
            get
            {
                return helpText;
            }
            set
            {
                helpText = value;
                RaisePropertyChanged();
            }
        }

        public string language;
        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                RaisePropertyChanged();
            }
        }

        public Command SOS { get; set; }
        public Command AddContactPage { get; set; }
        public Command AboutPage { get; set; }
        public Command Logout { get; set; }
        public Command LogIn { get; set; }
        public Command BackgroundTapped { get; set; }
        public Command Translate { get; set; }
        public Command HelpTextPage { get; set; }
        public Command LanguageChanged { get; set; }
        public Command HelpTextChanged { get; set; }
        internal Location CurrentLocation { get; set; }

        public ObservableCollection<User> LanguageList { get; set; }
        public object SelectedLanguage { get; set; }


        public ViewModel()
        {
            IsLoginVisible = true;
            SOS = new Command(SosCommand);
            AddContactPage = new Command(AddContactPageCommand);
            AboutPage = new Command(AboutPageCommand);
            Logout = new Command(LogoutCommand);
            LogIn = new Command(LogInCommand);
            BackgroundTapped = new Command(BackgroundTappedCommand);
            Translate = new Command(TranslateCommand);
            HelpTextPage = new Command(HelpTextPageCommand);
            LanguageChanged = new Command(LanguageChangedCommand);
            HelpTextChanged = new Command(HelpTextChangedCommand);
            PopulateLanguageList();

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
                {
                    this.UserName = userTable.ToList().ToArray()[0].UserName;
                    this.HelpText = userTable.ToList().ToArray()[0].HelpText;
                    var language = userTable.ToList().ToArray()[0].Language;
                    this.Language = language;
                    App.ChangeLanguage(language);
                    if (language != null)
                        LanguageList.FirstOrDefault(x => x.Language.Equals(language)).IsSelected = true;
                    else
                        LanguageList.FirstOrDefault(x => x.Language.Equals("English")).IsSelected = true;
                }
            }
            
            DependencyService.Get<ILocationService>().LocationChanged += App_LocationChanged;
            DependencyService.Get<ILocationService>().GPSStateChanged += App_GPSStateChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void HelpTextChangedCommand(object obj)
        {
            database.Update(new User() { HelpText = this.HelpText, UserName = this.UserName, Language = this.Language });
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void PopulateLanguageList()
        {
            LanguageList = new ObservableCollection<User>();
            LanguageList.Add(new User() { Language = "English" });
            LanguageList.Add(new User() { Language = "தமிழ்" });
            LanguageList.Add(new User() { Language = "తెలుగు" });
            LanguageList.Add(new User() { Language = "മലയാളം" });
            LanguageList.Add(new User() { Language = "हिंदी" });
            LanguageList.Add(new User() { Language = "Français" });
            LanguageList.Add(new User() { Language = "Italiano" });
            LanguageList.Add(new User() { Language = "日本語" });
            LanguageList.Add(new User() { Language = "中国人" });
        }

        private async void LanguageChangedCommand(object obj)
        {
            if (obj == null)
            {
                ShowLanguagePopup = false;
                return;
            }

            var alertTitle = EmergencyAppResources.AlertTitle;
            var alertMessage = EmergencyAppResources.AlertMessage;
            var yes = EmergencyAppResources.Yes;
            var cancel = EmergencyAppResources.Cancel;

            var response = await App.Current.MainPage.DisplayAlert(alertTitle, alertMessage, yes, cancel);

            if (!response)
            {
                return;
            }

            ShowLanguagePopup = false;
            await Task.Delay(1100);
            FrameVisibility = true;
            await Task.Delay(2000);

            var user = (obj as User);
            var previousLanguage = LanguageList.FirstOrDefault(x => x.IsSelected == true);
            if (previousLanguage != null)
                previousLanguage.IsSelected = false;

            user.IsSelected = true;
            this.Language = language;
            App.ChangeLanguage(user.Language);
            database.Update(new User() { UserName = this.UserName, HelpText = this.HelpText, Language = user.Language });

            App.Current.MainPage = new NavigationPage(new MainPage(this) { BindingContext = this });
            FrameVisibility = false;
        }


        private void HelpTextPageCommand(object obj)
        {
            App.Current.MainPage.Navigation.PushModalAsync(new HelpTextPage() { BindingContext = this });

        }

        private void TranslateCommand(object obj)
        {
            ShowLanguagePopup = true;
            MessagingCenter.Send<object, string>(this, "BackgroundTapped", "Background");
        }

        private void BackgroundTappedCommand(object obj)
        {
            AboutTagVisible = false;
            ContactTagVisible = false;
            LogoutTagVisible = false;
            HelpTextTagVisible = false;
            TranslateTagVisible = false;
            MessagingCenter.Send<object, string>(this, "BackgroundTapped", "Background");
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.NetworkConnected);
            }
            else
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.NetworkDisconnected);
            }
        }

        private async void LogInCommand(object obj)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.MissingUserName);
            }
            else
            {
                IsLoginVisible = false;
                await Task.Delay(2000);
                //if (CurrentLocation == null)
                //    await GetLocation();
                database.Insert(new User() { UserName = this.UserName, HelpText = $"This is {UserName}. I need your help. I am currently here ", Language = this.Language });
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
            var title = EmergencyAppResources.Success;

            await App.Current.MainPage.DisplayAlert(EmergencyAppResources.Success, EmergencyAppResources.LogoutSuccess, EmergencyAppResources.Okay);
            var newViewModel = new ViewModel();
            App.Current.MainPage = new LoginPage() { BindingContext = newViewModel };
        }


        private void App_LocationChanged(object sender, LocationEventArgs e)
        {
            if (CurrentLocation != null)
            {
                CurrentLocation.Altitude = e.Altitude;
                CurrentLocation.Latitude = e.Latitude;
                CurrentLocation.Longitude = e.Longitude;
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.LocationUpdated);
            }
        }

        private void App_GPSStateChanged(object sender, GPSStateEventArgs e)
        {
            if (e.IsGPSEnabled)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.GPSEnabled);
            }
            else
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.GPSDisabled);
            }
        }


        private void AboutPageCommand(object obj)
        {
            App.Current.MainPage.Navigation.PushModalAsync(new AboutPage());
        }

        private void AddContactPageCommand(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new AddRecipients());
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
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.NoRecipients);
            }
        }

        private async void SendSMS()
        {
            if (CurrentLocation != null)
            {
                var uri = string.Empty;
                uri = $"https://" + $"maps.google.com/maps?q=@{CurrentLocation.Latitude},{CurrentLocation.Longitude}";

                System.Diagnostics.Debug.WriteLine(uri);
                await SMS.SendSms(HelpText + System.Environment.NewLine + " " + uri, contactNames);
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
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.LocationAccquired);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.GPSNotSupported);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.GPSNotEnabled);
            }
            catch (PermissionException pEx)
            {
                await App.Current.MainPage.Navigation.PushAsync(new GPSDeniedPage());
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.CheckConnection);
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
