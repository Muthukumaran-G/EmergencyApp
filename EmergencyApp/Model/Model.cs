using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class RecipientModel: NotificationObject
    {
        public string recipientNumber;
        public string recipientName;
        public string userName;
        public string serial;


        [PrimaryKey]
        public string Serial
        {
            get
            {
                return serial;
            }
            set
            {
                serial = value;
                RaisePropertyChanged();
            }
        }

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
    }

    public class User : NotificationObject
    {
        private string userName;
        private string helpText;
        private string language;
        private bool isSelected;

        [PrimaryKey]
        public string UserName {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                RaisePropertyChanged();
            }
        }

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

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }
    }
}
