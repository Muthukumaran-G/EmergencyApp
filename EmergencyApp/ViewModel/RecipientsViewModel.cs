using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class RecipientsViewModel: ViewModel
    {
        private bool isContactAdded;
        public bool IsContactAdded
        {
            get
            {
                return isContactAdded;
            }
            set
            {
                isContactAdded = value;
                RaisePropertyChanged();
            }
        }

        private bool isNameEmpty;
        public bool IsNameEmpty
        {
            get
            {
                return isNameEmpty;
            }
            set
            {
                isNameEmpty = value;
                RaisePropertyChanged();
            }
        }

        private bool isNumberEmpty;
        public bool IsNumberEmpty
        {
            get
            {
                return isNumberEmpty;
            }
            set
            {
                isNumberEmpty = value;
                RaisePropertyChanged();
            }
        }

        private string recipientName;
        public string RecipientName
        {
            get
            {
                return recipientName;
            }
            set
            {
                recipientName = value;
                RaisePropertyChanged();
            }
        }

        private string recipientNumber;
        public string RecipientNumber
        {
            get
            {
                return recipientNumber;
            }
            set
            {
                recipientNumber = value;
                RaisePropertyChanged();
            }
        }

        private string popupTitle;
        public string PopupTitle
        {
            get
            {
                return popupTitle;
            }
            set
            {
                popupTitle = value;
                RaisePropertyChanged();
            }
        }

        private string listTitle;
        public string ListTitle
        {
            get
            {
                return listTitle;
            }
            set
            {
                listTitle = value;
                RaisePropertyChanged();
            }
        }
        private bool showPopup;
        public bool ShowPopup
        {
            get
            {
                return showPopup;
            }
            set
            {
                showPopup = value;
                RaisePropertyChanged();
            }
        }

        private bool isEditPopup;
        public bool IsEditPopup
        {
            get
            {
                return isEditPopup;
            }
            set
            {
                isEditPopup = value;
                RaisePropertyChanged();
            }
        }

        private RecipientModel editContactDetails;
        public RecipientModel EditContactDetails
        {
            get
            {
                return editContactDetails;
            }
            set
            {
                editContactDetails = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<RecipientModel> orderList;

        public ObservableCollection<RecipientModel> OrderList
        {
            get
            {
                return orderList;
            }
            set
            {
                orderList = value;
                RaiseCollectionChanged();
            }
        }

        public Command AddNewContact { get; set; }
        public Command OpenPhonebook { get; set; }
        public Command Done { get; set; }
        public Command BackgroundGesture { get; set; }
        public Command EditInvoked { get; set; }
        public Command DeleteInvoked { get; set; }

        public RecipientsViewModel()
        {
            OrderList = new ObservableCollection<RecipientModel>();
            AddNewContact = new Command(AddNewContactCommand);
            OpenPhonebook = new Command(OpenPhonebookCommand);
            Done = new Command(DoneCommand);
            BackgroundGesture = new Command(BackgroundGestureCommand);
            EditInvoked = new Command(EditInvokedCommand);
            DeleteInvoked = new Command(DeleteInvokedCommand);
            ShowPopup = false;
            if (OrderList.Count > 0)
                IsContactAdded = true;
            AddRecipientsList();
        }

        private void AddRecipientsList()
        {
            if (database != null)
            {
                var recipientsTable = (from i in database.Table<RecipientModel>() select i);

                contactNames = new string[recipientsTable.Count()];

                if(recipientsTable.Count()>0)
                {
                    for (int i = 0; i < recipientsTable.Count(); i++)
                    {
                        OrderList.Add(new RecipientModel()
                        {
                            RecipientNumber = recipientsTable.ToList().ToArray()[i].RecipientNumber,
                            RecipientName = recipientsTable.ToList().ToArray()[i].RecipientName
                        });
                        contactNames[i] = recipientsTable.ToList().ToArray()[i].RecipientNumber;
                    }
                    ListTitle = "Added Contacts";
                }
                else
                    ListTitle = "No contact added..!";

                var userTable = (from i in database.Table<User>() select i);

                if (userTable.Count() > 0)
                    this.UserName = (userTable.ToList().ToArray()[0].UserName);
            }
        }

        private async void DeleteInvokedCommand(object obj)
        {
            try
            {
                var table = from i in database.Table<RecipientModel>() select i;
                OrderList.Remove(obj as RecipientModel);
                database.Delete(obj);
                //database.Query<RecipientModel>("DELETE from RecipientModel where RecipientNumber =" + (e.Item as RecipientModel).RecipientNumber).FirstOrDefault();
                contactNames = new string[table.Count()];

                if (table.Count() > 0)
                {
                    for (int i = 0; i < table.Count(); i++)
                    {
                        contactNames[i] = table.ToList().ToArray()[i].RecipientNumber;
                    }
                }
                else
                    ListTitle = "No contact added..!";
            }
            catch (Exception ex)
            {
                    await App.Current.MainPage.DisplayAlert("Alert", "Unable to delete contact, please try again.", "OK");
            }
        }

        private void EditInvokedCommand(object obj)
        {
            EditContactDetails = obj as RecipientModel;
            this.RecipientName = EditContactDetails.RecipientName;
            this.RecipientNumber = EditContactDetails.RecipientNumber;
            this.ShowPopup = true;
            this.IsEditPopup = true;
            PopupTitle = "Edit Contact";
        }

        private void BackgroundGestureCommand(object obj)
        {
            ShowPopup = false;
        }

        private async void DoneCommand(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(this.RecipientName) || string.IsNullOrEmpty(this.RecipientNumber))
                {
                    IsNameEmpty = string.IsNullOrEmpty(this.RecipientName);
                    IsNumberEmpty = string.IsNullOrEmpty(this.RecipientNumber);
                    return;
                }
                else
                {
                    IsNameEmpty = false;
                    IsNumberEmpty = false;
                }

                if (EditContactDetails != null && IsEditPopup)
                {   
                    EditContactDetails.RecipientName = this.RecipientName;
                    EditContactDetails.RecipientNumber = this.RecipientNumber;
                    database.Update(EditContactDetails);
                }
                else
                {
                    var newContact = new RecipientModel() { RecipientName = this.RecipientName, RecipientNumber = this.RecipientNumber };
                    database.Insert(newContact);
                    OrderList.Add(newContact);
                    contactNames = new string[OrderList.Count()];

                    for (int i = 0; i < OrderList.Count(); i++)
                    {
                        contactNames[i] = OrderList[i].RecipientNumber;
                    }
                }

                this.RecipientName = string.Empty;
                this.RecipientNumber = string.Empty;
                IsContactAdded = false;
                ShowPopup = false;
                ListTitle = "Added Contacts";
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Unable to add contact, please try re-installing the application.", "OK");
                ShowPopup = false;
            }
        }

        private async void OpenPhonebookCommand(object obj)
        {
            try
            {
                var contact = await Contacts.PickContactAsync();
                var newContact = new RecipientModel() { RecipientName = contact.DisplayName, RecipientNumber = contact.Phones[0].PhoneNumber };
                database.Insert(newContact);
                OrderList.Add(newContact);
                contactNames = new string[OrderList.Count()];

                for (int i = 0; i < OrderList.Count(); i++)
                {
                    contactNames[i] = OrderList[i].RecipientNumber;
                }

                ListTitle = "Added Contacts";
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Unable to add contact, please try adding again.", "OK");
            }
        }

        private void AddNewContactCommand(object obj)
        {
            ShowPopup = true;
            PopupTitle = "Add new contact";
        }
    }
}
