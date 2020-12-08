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

namespace terrible.Pages.UserPages
{
    public class DepositModel : PageModel
    {
        [BindProperty]
        public Transaction Transaction { get; set; }

        public int? UserID;
        public const string SessionKeyName1 = "userID";

        public string Username;
        public const string SessionKeyName2 = "username";

        public string Name;
        public const string SessionKeyName3 = "name";

        public decimal Balance;
        public const string SessionKeyName4 = "balance";

        public bool Admin;
        public const string SessionKeyName5 = "admin";

        public string SessionID;
        public const string SessionKeyName6 = "sessionID";
        public IActionResult OnGet()
        {
            UserID = HttpContext.Session.GetInt32(SessionKeyName1);
            Username = HttpContext.Session.GetString(SessionKeyName2);
            Name = HttpContext.Session.GetString(SessionKeyName3);
            Balance = decimal.Round(Convert.ToDecimal(HttpContext.Session.GetString(SessionKeyName4)), 2);
            Admin = Convert.ToBoolean(HttpContext.Session.GetString(SessionKeyName5));
            SessionID = HttpContext.Session.GetString(SessionKeyName6);
            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Login/Login");
            }

            return Page();
        }
        public int getUserID()
        {
            int userID = HttpContext.Session.GetInt32(SessionKeyName1).Value;
            return userID;
        }
        public decimal getBalance()
        {
            decimal balance = decimal.Round(Convert.ToDecimal(HttpContext.Session.GetString(SessionKeyName4)), 2);
            return balance;
        }
        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE [User] SET [Balance] = [Balance] + @NewBalance WHERE [Id] = @UserID";

                command.Parameters.AddWithValue("@NewBalance", Transaction.TransferAmount);
                command.Parameters.AddWithValue("@UserID", getUserID());

                command.ExecuteNonQuery();

                //refresh balance of logged-in account
                HttpContext.Session.SetString("balance", (getBalance() + Transaction.TransferAmount).ToString());
            }
            return RedirectToPage("UserIndex");
        }
    }
}
