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
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public User UserDetails { get; set; }

        public string Message;

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

        public int getID()
        {
            int UID = HttpContext.Session.GetInt32(SessionKeyName1).Value;
            return UID;
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(UserDetails.Name) || string.IsNullOrWhiteSpace(UserDetails.Password))
            {
                Message = "Please enter a username and password";
                return Page();
            }
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"";
                if (!string.IsNullOrWhiteSpace(UserDetails.Name))
                {
                    command.CommandText += @"UPDATE [User] SET [Name] = @NewName WHERE [Id] = @UserID;";
                    command.Parameters.AddWithValue("@NewName", UserDetails.Name);
                }
                if (!string.IsNullOrWhiteSpace(UserDetails.Password))
                {
                    command.CommandText += @"UPDATE [User] SET [Password] = @NewPassword WHERE [Id] = @UserID;";
                    command.Parameters.AddWithValue("@NewPassword", UserDetails.Password);
                }

                command.Parameters.AddWithValue("@UserID", getID());

                Console.WriteLine(command.CommandText);

                command.ExecuteNonQuery();

                //refresh name before redirect
                HttpContext.Session.SetString("name", UserDetails.Name);
            }
            return RedirectToPage("UserIndex");
        }
    }
}
