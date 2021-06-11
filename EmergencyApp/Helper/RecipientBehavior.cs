using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class RecipientBehavior : Behavior<ContentPage>
    {
        Frame Frame;
        StackLayout MainStack;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            Frame = bindable.FindByName<Frame>("frame");
            MainStack = bindable.FindByName<StackLayout>("mainStack");
            Frame.PropertyChanged += Frame_PropertyChanged; ;
        }

        private async void Frame_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Height" && Frame.IsVisible)
            {
                Frame.IsVisible = false;
            }

            if (e.PropertyName == "IsEnabled" && Frame.Height > 0)
            {
                if (Frame.IsEnabled)
                {
                    Frame.IsVisible = true;
                    Frame.ScaleTo(1, 1000, Easing.BounceOut);
                    await MainStack.FadeTo(0.2, 1000);
                }
                else
                {
                    Frame.ScaleTo(0, 1000, Easing.BounceIn);
                    await MainStack.FadeTo(1, 1000);
                    Frame.IsVisible = false;
                }
            }

        }

        private void Frame_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);
        }
    }
}
