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
        Label NameLabel;
        Entry NameEntry;
        Label NumberLabel;
        Entry NumberEntry;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            Frame = bindable.FindByName<Frame>("frame");
            MainStack = bindable.FindByName<StackLayout>("mainStack");
            NameLabel = bindable.FindByName<Label>("nameLabel");
            NameEntry = bindable.FindByName<Entry>("nameEntry");
            NumberLabel = bindable.FindByName<Label>("numberLabel");
            NumberEntry = bindable.FindByName<Entry>("numberEntry");
            NameEntry.Focused += NameEntry_Focused;
            NameEntry.Unfocused += NameEntry_Unfocused;
            NumberEntry.Focused += NumberEntry_Focused;
            NumberEntry.Unfocused += NumberEntry_Unfocused;
            Frame.PropertyChanged += Frame_PropertyChanged;
        }

        private void NumberEntry_Unfocused(object sender, FocusEventArgs e)
        {
            NumberLabel.TranslateTo(0, 0, 500, Easing.Linear);
        }

        private void NumberEntry_Focused(object sender, FocusEventArgs e)
        {
            NumberLabel.TranslateTo(20, -20, 300, Easing.Linear);
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            NameLabel.TranslateTo(0, 0, 500, Easing.Linear);
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            NameLabel.TranslateTo(20, -20, 300, Easing.Linear);
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
