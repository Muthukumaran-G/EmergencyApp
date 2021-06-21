using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyApp
{
    public class Blob : Grid
    {
        public static readonly BindableProperty IsTagShownProperty =
           BindableProperty.Create(nameof(IsTagShown), typeof(bool), typeof(Blob), false, BindingMode.TwoWay, null, OnIsTagShownChanged);

        public static readonly BindableProperty IconSourceProperty =
           BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(Blob), null, BindingMode.TwoWay, null, OnImageSourceChanged);

        public static readonly BindableProperty TagNameProperty =
           BindableProperty.Create(nameof(TagName), typeof(string), typeof(Blob), null, BindingMode.TwoWay, null, OnTagNameChanged);

        public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(
            "TapCommand",
            typeof(ICommand),
            typeof(Blob),
            null,
            BindingMode.Default,
            null);

        public static readonly BindableProperty TagDirectionProperty = BindableProperty.Create(
            "TagDirection",
            typeof(TagDirection),
            typeof(Blob),
            TagDirection.Right,
            BindingMode.TwoWay,
            null,
            OnTagDirectionChanged);

        public bool IsTagShown
        {
            get { return (bool)this.GetValue(IsTagShownProperty); }
            set { this.SetValue(IsTagShownProperty, value); }
        }

        public ImageSource IconSource
        {
            get { return (ImageSource)this.GetValue(IconSourceProperty); }
            set { this.SetValue(IconSourceProperty, value); }
        }

        public TagDirection TagDirection
        {
            get { return (TagDirection)this.GetValue(TagDirectionProperty); }
            set { this.SetValue(TagDirectionProperty, value); }
        }

        public string TagName
        {
            get { return (string)this.GetValue(TagNameProperty); }
            set { this.SetValue(TagNameProperty, value); }
        }

        public ICommand TapCommand
        {
            get
            {
                return (ICommand)GetValue(TapCommandProperty);
            }

            set
            {
                SetValue(TapCommandProperty, value);
            }
        }

        private Image Icon { get; set; }
        private Frame TagFrame { get; set; }
        private BoxView Tip { get; set; }
        private Label TagLabel { get; set; }
        private ColumnDefinition ImageColumnDefinition { get; set; }
        private ColumnDefinition TagColumnDefinition { get; set; }


        public Blob()
        {
            ImageColumnDefinition = new ColumnDefinition() { Width = 30 };
            TagColumnDefinition = new ColumnDefinition() { Width = GridLength.Auto };
            var tipColumn = new ColumnDefinition() { Width = 10 };
            this.ColumnDefinitions.Add(ImageColumnDefinition);
            this.ColumnDefinitions.Add(tipColumn);
            this.ColumnDefinitions.Add(TagColumnDefinition);
            Icon = new Image();
            Icon.Source = this.IconSource;
            TagFrame = new Frame() { Padding = 1, CornerRadius = 2, IsVisible = false, Opacity = 0, BorderColor = Color.Transparent, BackgroundColor = Color.AntiqueWhite };
            Tip = new BoxView() { Rotation = 45, HeightRequest = 20, Color = Color.AntiqueWhite, TranslationX = 10, WidthRequest = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Opacity = 0, IsVisible = false };
            TagLabel = new Label() { FontFamily = "ElMessiri-Regular", Text = TagName, Padding = new Thickness(10,0,10,0), TextColor = Color.Black, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            TagFrame.Content = TagLabel;
            this.Children.Add(Icon);
            this.Children.Add(Tip);
            this.Children.Add(TagFrame);

            if(this.TagDirection == TagDirection.Left)
            {
               
                Grid.SetColumn(Icon, 2);
                Grid.SetColumn(TagFrame, 0);
                this.ColumnDefinitions.Reverse();
            }
            else
            {
                Grid.SetColumn(Icon, 0);
                Grid.SetColumn(TagFrame, 2);
            }


            Grid.SetColumn(Tip, 1);
            Grid.SetColumnSpan(Tip, 2);

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += TapGesture_Tapped;
            this.GestureRecognizers.Add(tapGesture);
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            if(IsTagShown)
            {
                IsTagShown = false;
                if (TapCommand != null)
                {
                    this.TapCommand.Execute(this);
                }
            }
            else
            {
                IsTagShown = true;
            }
        }

        private static void OnTagDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var blob = bindable as Blob;
            blob.TagDirection = (TagDirection)newValue;
            if (blob.TagDirection == TagDirection.Left)
            {
                blob.ColumnDefinitions.RemoveAt(2);
                blob.ColumnDefinitions.RemoveAt(0);
                blob.ColumnDefinitions.Insert(0, blob.TagColumnDefinition);
                blob.ColumnDefinitions.Add(blob.ImageColumnDefinition);
                Grid.SetColumn(blob.Icon, 2);
                Grid.SetColumn(blob.TagFrame, 0);
                blob.Tip.TranslationX = -20;
            }
            else
            {
                blob.ColumnDefinitions.RemoveAt(2);
                blob.ColumnDefinitions.RemoveAt(0);
                blob.ColumnDefinitions.Insert(0, blob.ImageColumnDefinition);
                blob.ColumnDefinitions.Add(blob.TagColumnDefinition);
                Grid.SetColumn(blob.Icon, 0);
                Grid.SetColumn(blob.TagFrame, 2);
                blob.Tip.TranslationX = 10;
            }
        }

        private static void OnIsTagShownChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var blob = bindable as Blob;
            if(newValue is true)
            {
                blob.TagFrame.IsVisible = true;
                blob.Tip.IsVisible = true;
                blob.Icon.ScaleTo(1.5, 500, Easing.SpringIn);
                blob.TagFrame.FadeTo(1, 200, Easing.Linear);
                blob.Tip.FadeTo(1, 200, Easing.Linear);
            }
            else if(newValue is false)
            {
                blob.Icon.ScaleTo(1, 500, Easing.SpringIn);
                blob.TagFrame.FadeTo(0, 200, Easing.Linear);
                blob.Tip.FadeTo(0, 200, Easing.Linear);
                blob.TagFrame.IsVisible = false;
                blob.Tip.IsVisible = false;
            }

        }

        private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var blob = bindable as Blob;
            blob.Icon.Source = newValue as ImageSource;

        }

        private static void OnTagNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var blob = bindable as Blob;
            blob.TagLabel.Text = newValue.ToString();
        }
    }

    public enum TagDirection
    {
        Left,Right
    }
}