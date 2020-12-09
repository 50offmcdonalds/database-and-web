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

namespace terrible.Pages.AdminPages.UserData
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public User UserRec { get; set; }

        public string Username;
        public const string SessionKeyName1 = "username";

        public bool Admin;
        public const string SessionKeyName2 = "admin";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";
        public IActionResult OnGet(int? id)
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
                command.CommandText = @"SELECT * FROM [User] WHERE [Id] = @UserID";
                command.Parameters.AddWithValue("@UserID", id);

                SqlDataReader reader = command.ExecuteReader();
                UserRec = new User();
                while (reader.Read())
                {
                    UserRec.Id = reader.GetInt32(0);
                    UserRec.Name = reader.GetString(1);
                    UserRec.Username = reader.GetString(2);
                    UserRec.Password = reader.GetString(3);
                    UserRec.Admin = reader.GetBoolean(4);
                    UserRec.Balance = Decimal.Round(reader.GetDecimal(5), 2);
                }
                reader.Close();
            }
            conn.Close();
            return Page();
        }

        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"DELETE [User] WHERE [Id] = @UserID";
                command.Parameters.AddWithValue("@UserID", UserRec.Id);
                command.ExecuteNonQuery();
            }
            return RedirectToPage("View");
        }
    }
}
