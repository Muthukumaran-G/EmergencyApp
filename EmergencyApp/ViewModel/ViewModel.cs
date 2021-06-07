using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class ViewModel : NotificationObject
    {
        ObservableCollection<RecipientModel> orderList;
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

        public ObservableCollection<RecipientModel> OrderList
        {
            get
            {
                return orderList;
            }
            set
            {
                orderList = value;
            }
        }

        public ViewModel()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            // Create the table
            database.CreateTable<RecipientModel>();
            database.CreateTable<User>();
            if (database != null)
            {
                var recipientsTable = (from i in database.Table<RecipientModel>() select i);
                OrderList = new ObservableCollection<RecipientModel>();
                contactNames = new string[recipientsTable.Count()];

                for (int i = 0; i < recipientsTable.Count(); i++)
                {
                    OrderList.Add(new RecipientModel()
                    {
                        Recipient = recipientsTable.ToList().ToArray()[i].Recipient
                    });
                    contactNames[i] = (recipientsTable.ToList().ToArray()[i].Recipient);
                }

                var userTable = (from i in database.Table<User>() select i);

                if (userTable.Count() > 0)
                    this.UserName = (userTable.ToList().ToArray()[0].UserName);
            }
        }
    }

    public class NotificationObject : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseCollectionChanged(string propName)
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
