using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using terrible.Models;
using terrible.Pages.DatabaseConnection;

namespace terrible.Pages.AdminPages
{
    public class StatsModel : PageModel
    {
        public int totalAccounts;
        public decimal storedMoney;
        //public decimal totalTransactions;
        //public decimal largestTransaction;

        public string Username;
        public const string SessionKeyName1 = "username";

        public bool Admin;
        public const string SessionKeyName2 = "admin";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString(SessionKeyName1);
            Admin = Convert.ToBoolean(HttpContext.Session.GetString(SessionKeyName2));
            SessionID = HttpContext.Session.GetString(SessionKeyName3);
            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Login/Login");
            }
            else if (!Admin)
            {
                return RedirectToPage("/UserPages/UserIndex");
            }

            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT COUNT(DISTINCT [Id]), SUM([Balance]) FROM [User]";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    totalAccounts = reader.GetInt32(0);
                    storedMoney = decimal.Round(reader.GetDecimal(1), 2);
                }
                reader.Close();
            }
            conn.Close();

            return Page();
        }
    }
}
