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
    public class TransactionViewModel : PageModel
    {
        public List<Transaction> TransactionList { get; set; }

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
            else if (!Admin)
            {
                return RedirectToPage("/UserPages/UserIndex");
            }
            //get users
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM [Transactions]";
                SqlDataReader reader = command.ExecuteReader();

                TransactionList = new List<Transaction>();
                while (reader.Read())
                {
                    Transaction transaction = new Transaction();
                    transaction.TransactionID = reader.GetInt32(0);
                    transaction.SenderID = reader.GetInt32(1);
                    transaction.ReceiverID = reader.GetInt32(2);
                    transaction.TransferAmount = reader.GetDecimal(3);
                    transaction.TransactionTime = reader.GetDateTime(4);

                    TransactionList.Add(transaction);
                }
                reader.Close();
            }
            conn.Close();

            return Page();
        }
    }
}
