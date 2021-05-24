using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyApp
{
    public class RecipientModel
    {
        [PrimaryKey]
        public string Recipients { get; set; }

        public string UserName { get; set; }
    }

    public class User
    {
        [PrimaryKey]
        public string UserName { get; set; }
    }
}
