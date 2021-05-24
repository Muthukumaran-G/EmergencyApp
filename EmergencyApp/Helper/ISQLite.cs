using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyApp
{

    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
