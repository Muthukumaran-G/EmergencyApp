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
    }
}