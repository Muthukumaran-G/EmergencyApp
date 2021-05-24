using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyApp
{
    public interface ISendMessage
    {
        void SendMessage(string message, string[] recipents);
    }
}
