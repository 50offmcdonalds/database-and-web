using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terrible.Pages.DatabaseConnection
{
    public class DatabaseConnect
    {
        public string DatabaseString()
        {
            string DbString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\shit for uni\really good project\terrible\terrible\Data\ProjectDB.mdf;Integrated Security=True";
            return DbString;
        }
    }
}
