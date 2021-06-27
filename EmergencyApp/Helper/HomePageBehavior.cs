using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class HomePageBehavior : Behavior<ContentPage>
	{
        Frame Settings;
        Blob HelpTextBlob;
        Blob ContactListBlob;
        Blob TranslateBlob;
        bool isOpen;
        bool isAnimating;
        ViewModel ViewModel;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            Settings = bindable.FindByName<Frame>("Settings");
            HelpTextBlob = bindable.FindByName<Blob>("HelpTextBlob");
            ContactListBlob = bindable.FindByName<Blob>("ContactListBlob");
            TranslateBlob = bindable.FindByName<Blob>("TranslateBlob");
            var gesture = new TapGestureRecognizer();
            gesture.Tapped += Gesture_Tapped;
            Settings.GestureRecognizers.Add(gesture);
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            MessagingCenter.Subscribe<object, string>(this, "BackgroundTapped", ((object sender, string args) =>
            {
                CollapseTags(args);
                CollapseSettings();
            }));
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            ViewModel = (sender as ContentPage).BindingContext as ViewModel;
        }

        private async void CollapseSettings()
        {
            HelpTextBlob.ScaleTo(0, 1000, Easing.Linear);
            ContactListBlob.ScaleTo(0, 1000, Easing.Linear);
            TranslateBlob.ScaleTo(0, 1000, Easing.Linear);

            HelpTextBlob.FadeTo(0, 1000, Easing.Linear);
            ContactListBlob.FadeTo(0, 1000, Easing.Linear);
            TranslateBlob.FadeTo(0, 1000, Easing.Linear);

            HelpTextBlob.RotateTo(-360, 1500, Easing.BounceOut);
            ContactListBlob.RotateTo(-360, 1500, Easing.BounceOut);
            TranslateBlob.RotateTo(-360, 1500, Easing.BounceOut);

            HelpTextBlob.TranslateTo(0, 0, 1000, Easing.Linear);
            ContactListBlob.TranslateTo(0, 0, 1000, Easing.Linear);
            TranslateBlob.TranslateTo(0, 0, 1000, Easing.Linear);

            Settings.RotateTo(-360, 1000, Easing.Linear);
            await System.Threading.Tasks.Task.Delay(1500);
            HelpTextBlob.IsVisible = false;
            ContactListBlob.IsVisible = false;
            TranslateBlob.IsVisible = false;
            isOpen = false;
        }

        private void CollapseTags(string property)
        {
            if (!property.Equals("AboutTagVisible"))
                ViewModel.AboutTagVisible = false;

            if (!property.Equals("LogoutTagVisible"))
                ViewModel.LogoutTagVisible = false;
        }

        private async void Gesture_Tapped(object sender, EventArgs e)
        {
            if (isAnimating)
            {
                System.Diagnostics.Debug.WriteLine("Animating");
                return;
            }

            CollapseTags("All");

            //HelpTextBlob.InputTransparent = true;
            //ContactListBlob.InputTransparent = true;
            //TranslateBlob.InputTransparent = true;
            //Settings.InputTransparent = true;
            ViewModel.ContactTagVisible = false;
            ViewModel.HelpTextTagVisible = false;
            ViewModel.TranslateTagVisible = false;
            isAnimating = true;

            if (isOpen)
            {
                CollapseSettings();
            }
            else
            {
                HelpTextBlob.IsVisible = true;
                ContactListBlob.IsVisible = true;
                TranslateBlob.IsVisible = true;
                HelpTextBlob.ScaleTo(1, 1000, Easing.Linear);
                ContactListBlob.ScaleTo(1, 1000, Easing.Linear);
                TranslateBlob.ScaleTo(1, 1000, Easing.Linear);

                HelpTextBlob.FadeTo(1, 100, Easing.Linear);
                ContactListBlob.FadeTo(1, 100, Easing.Linear);
                TranslateBlob.FadeTo(1, 100, Easing.Linear);

                HelpTextBlob.RotateTo(360, 1500, Easing.BounceOut);
                ContactListBlob.RotateTo(360, 1500, Easing.BounceOut);
                TranslateBlob.RotateTo(360, 1500, Easing.BounceOut);

                HelpTextBlob.TranslateTo(-50, -50, 1000, Easing.Linear);
                ContactListBlob.TranslateTo(-100, 0, 1000, Easing.Linear);
                TranslateBlob.TranslateTo(0, -100, 1000, Easing.Linear);

                Settings.RotateTo(360, 1000, Easing.Linear);
                await System.Threading.Tasks.Task.Delay(1500);
                isOpen = true;
            }

            isAnimating = false;
            //HelpTextBlob.InputTransparent = false;
            //ContactListBlob.InputTransparent = false;
            //TranslateBlob.InputTransparent = false;
            //Settings.InputTransparent = false;
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);
        }
	}
}
