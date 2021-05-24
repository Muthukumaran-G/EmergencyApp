using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyApp
{
    public static class SMS
    {
        public static async Task SendSms(string messageText, string[] recipients)
        {
            try
            {
                DependencyService.Get<ISendMessage>().SendMessage(messageText, recipients);
            }
            catch (FeatureNotSupportedException ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Feature not supported", "OK");
            }
            catch (Exception ex)
            {
                SmsMessage message = new SmsMessage(messageText, recipients);
                await Sms.ComposeAsync(message);
            }
        }
    }
}
