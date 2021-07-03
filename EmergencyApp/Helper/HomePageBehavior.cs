using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class HomePageBehavior : Behavior<ContentPage>
	{
        Frame Settings;
        Frame Frame;
        Grid ParentGrid;
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
            Frame = bindable.FindByName<Frame>("frame");
            ParentGrid = bindable.FindByName<Grid>("parentGrid");
            Frame.PropertyChanged += Frame_PropertyChanged;
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

        private async void Frame_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" && Frame.IsVisible)
            {
                Frame.IsVisible = false;
            }

            if (e.PropertyName == "IsEnabled" && Frame.Height > 0)
            {
                if (Frame.IsEnabled)
                {
                    Frame.IsVisible = true;
                    Frame.ScaleTo(1, 1000, Easing.BounceOut);
                    await ParentGrid.FadeTo(0.2, 1000);
                }
                else
                {
                    Frame.ScaleTo(0, 1000, Easing.BounceIn);
                    await ParentGrid.FadeTo(1, 1000);
                    Frame.IsVisible = false;
                }
            }

        }


        private async void CollapseSettings()
        {
            await Task.WhenAll(
                HelpTextBlob.ScaleTo(0, 1500, Easing.BounceOut),
            ContactListBlob.ScaleTo(0, 1500, Easing.BounceOut),
            TranslateBlob.ScaleTo(0, 1500, Easing.BounceOut),

            HelpTextBlob.FadeTo(0, 800, Easing.CubicIn),
            ContactListBlob.FadeTo(0, 800, Easing.CubicIn),
            TranslateBlob.FadeTo(0, 800, Easing.CubicIn),

            HelpTextBlob.RotateTo(-360, 1500, Easing.BounceOut),
            ContactListBlob.RotateTo(-360, 1500, Easing.BounceOut),
            TranslateBlob.RotateTo(-360, 1500, Easing.BounceOut),

            HelpTextBlob.TranslateTo(-20, -20, 1000, Easing.Linear),
            ContactListBlob.TranslateTo(-20, -20, 1000, Easing.Linear),
            TranslateBlob.TranslateTo(-20, -20, 1000, Easing.Linear),

            Settings.RotateTo(-360, 1000, Easing.Linear)
            
                );

            HelpTextBlob.TranslationX = 0;
            HelpTextBlob.TranslationY = 0;
            ContactListBlob.TranslationX = 0;
            ContactListBlob.TranslationY = 0;
            TranslateBlob.TranslationX = 0;
            TranslateBlob.TranslationY = 0;
                //HelpTextBlob.IsVisible = false;
                //ContactListBlob.IsVisible = false;
                //TranslateBlob.IsVisible = false;
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
            //ViewModel.ContactTagVisible = false;
            //ViewModel.HelpTextTagVisible = false;
            //ViewModel.TranslateTagVisible = false;
            isAnimating = true;

            if (isOpen)
            {
                CollapseSettings();
            }
            else
            {
                //HelpTextBlob.IsVisible = true;
                //ContactListBlob.IsVisible = true;
                //TranslateBlob.IsVisible = true;
                HelpTextBlob.TranslationX = -20;
                HelpTextBlob.TranslationY = -20;
                ContactListBlob.TranslationX = -20;
                ContactListBlob.TranslationY = -20;
                TranslateBlob.TranslationX = -20;
                TranslateBlob.TranslationY = -20;

                await Task.WhenAll(
                HelpTextBlob.ScaleTo(1, 1500, Easing.BounceOut),
                ContactListBlob.ScaleTo(1, 1500, Easing.BounceOut),
                TranslateBlob.ScaleTo(1, 1500, Easing.BounceOut),

                HelpTextBlob.FadeTo(1, 200, Easing.CubicIn),
                ContactListBlob.FadeTo(1, 200, Easing.CubicIn),
                TranslateBlob.FadeTo(1, 200, Easing.CubicIn),

                HelpTextBlob.RotateTo(360, 1500, Easing.BounceOut),
                ContactListBlob.RotateTo(360, 1500, Easing.BounceOut),
                TranslateBlob.RotateTo(360, 1500, Easing.BounceOut),

                HelpTextBlob.TranslateTo(-70, -75, 1000, Easing.Linear),
                ContactListBlob.TranslateTo(-120, -20, 1000, Easing.Linear),
                TranslateBlob.TranslateTo(-20, -120, 1000, Easing.Linear),

                Settings.RotateTo(360, 1000, Easing.Linear)
                );

                //await System.Threading.Tasks.Task.Delay(1500);
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
