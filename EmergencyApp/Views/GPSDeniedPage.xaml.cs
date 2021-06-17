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
    public partial class GPSDeniedPage : ContentPage
    {
        public GPSDeniedPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}