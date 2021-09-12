using EmergencyApp.Resx;
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

        private bool isAccessDenied;
        public bool IsAccessDenied
        {
            get
            {
                return isAccessDenied;
            }
            set
            {
                isAccessDenied = value;
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
            CheckPhoneBookAccess();
        }

        private async void CheckPhoneBookAccess()
        {
            var contactAccess = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();

            if (contactAccess != PermissionStatus.Granted)
            {
                this.IsAccessDenied = true;
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.ContactAccessDenied);
            }
            else
            {
                this.IsAccessDenied = false;
            }
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
                            RecipientName = recipientsTable.ToList().ToArray()[i].RecipientName,
                            Serial = recipientsTable.ToList().ToArray()[i].Serial
                        });
                        contactNames[i] = recipientsTable.ToList().ToArray()[i].RecipientNumber;
                    }
                }

                //var userTable = (from i in database.Table<User>() select i);

                //if (userTable.Count() > 0)
                //    this.UserDetails.UserName = (userTable.ToList().ToArray()[0].UserName);
            }
        }

        private void DeleteInvokedCommand(object obj)
        {
            try
            {
                var table = from i in database.Table<RecipientModel>() select i;
                OrderList.Remove(obj as RecipientModel);
                database.Delete(obj);
                contactNames = new string[table.Count()];

                if (table.Count() > 0)
                {
                    for (int i = 0; i < table.Count(); i++)
                    {
                        contactNames[i] = table.ToList().ToArray()[i].RecipientNumber;
                    }
                }

                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.RecipientDeleted);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.TryDeleteContact);
            }
        }

        private void EditInvokedCommand(object obj)
        {
            EditContactDetails = obj as RecipientModel;
            this.RecipientName = EditContactDetails.RecipientName;
            this.RecipientNumber = EditContactDetails.RecipientNumber;
            this.ShowPopup = true;
            this.IsEditPopup = true;
            PopupTitle = EmergencyAppResources.EditRecipient;
        }

        private void BackgroundGestureCommand(object obj)
        {
            ShowPopup = false;
            IsNumberEmpty = false;
            IsNameEmpty = false;
            this.RecipientName = string.Empty;
            this.RecipientNumber = string.Empty;
        }

        private void DoneCommand(object obj)
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
                    DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.RecipientUpdated);
                }
                else
                {
                    var newContact = new RecipientModel() { RecipientName = this.RecipientName, RecipientNumber = this.RecipientNumber };
                    OrderList.Add(newContact);
                    newContact.Serial = Guid.NewGuid().ToString();
                    database.Insert(newContact);
                    contactNames = new string[OrderList.Count()];

                    for (int i = 0; i < OrderList.Count(); i++)
                    {
                        contactNames[i] = OrderList[i].RecipientNumber;
                    }

                    DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.RecipientAdded);
                }

                this.RecipientName = string.Empty;
                this.RecipientNumber = string.Empty;
                IsContactAdded = false;
                ShowPopup = false;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.TryAddContact);
                ShowPopup = false;
            }
        }

        private async void OpenPhonebookCommand(object obj)
        {
            try
            {
                if(IsAccessDenied)
                {
                    CheckPhoneBookAccess();
                    return;
                }

                var contact = await Contacts.PickContactAsync();

                if (contact == null)
                    return;

                if (string.IsNullOrEmpty(contact.DisplayName) || contact.Phones.Count <= 0 || string.IsNullOrEmpty(contact.Phones[0].PhoneNumber))
                    return;

                var newContact = new RecipientModel() { RecipientName = contact.DisplayName, RecipientNumber = contact.Phones[0].PhoneNumber };
                OrderList.Add(newContact);
                newContact.Serial = Guid.NewGuid().ToString();
                database.Insert(newContact);

                contactNames = new string[OrderList.Count()];

                for (int i = 0; i < OrderList.Count(); i++)
                {
                    contactNames[i] = OrderList[i].RecipientNumber;
                }

                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.PhoneContactAdded);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShowToast(EmergencyAppResources.TryAddContact);
            }
        }

        private void AddNewContactCommand(object obj)
        {
            ShowPopup = true;
            IsEditPopup = false;
            PopupTitle = EmergencyAppResources.AddRecipient;
        }
    }
}
