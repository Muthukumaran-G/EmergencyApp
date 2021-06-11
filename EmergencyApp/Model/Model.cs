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

        [PrimaryKey]
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

        public string UserName
        {
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
    }

    public class User
    {
        [PrimaryKey]
        public string UserName { get; set; }
    }
}
