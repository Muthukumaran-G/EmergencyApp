using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyApp
{
    public interface IToastMessage
    {
        void ShowToast(string message, AlertDuration Alert = AlertDuration.ShortAlert);
    }

    public enum AlertDuration
    {
        LongAlert,
        ShortAlert
    };
}
