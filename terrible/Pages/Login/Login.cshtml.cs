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

namespace terrible.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User UserLogin { get; set; }
        public string Message { get; set; }

        public string Username;
        public const string SessionKeyName2 = "username";

        public string SessionID;
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString(SessionKeyName2);
            if (string.IsNullOrEmpty(Username))
            {
                return Page();
            }
            return RedirectToPage("/UserPages/Userindex");
        }
        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Console.WriteLine(UserLogin.Username);
            Console.WriteLine(UserLogin.Password);

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT [Id], [Name], [Username], [Password], [Admin], [Balance] FROM [User] WHERE [Username] = @Username AND [Password] = @Password";

                command.Parameters.AddWithValue("@Username", UserLogin.Username);
                command.Parameters.AddWithValue("@Password", UserLogin.Password);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserLogin.Id = reader.GetInt32(0);
                    UserLogin.Name = reader.GetString(1);
                    UserLogin.Username = reader.GetString(2);
                    UserLogin.Password = reader.GetString(3);
                    UserLogin.Admin = reader.GetBoolean(4);
                    UserLogin.Balance = reader.GetDecimal(5);
                }
            }
            if (!string.IsNullOrEmpty(UserLogin.Name))
            {
                SessionID = HttpContext.Session.Id;
                HttpContext.Session.SetString("sessionID", SessionID);
                HttpContext.Session.SetInt32("userID", UserLogin.Id);
                HttpContext.Session.SetString("username", UserLogin.Username);
                HttpContext.Session.SetString("name", UserLogin.Name);
                HttpContext.Session.SetString("admin", UserLogin.Admin.ToString());
                HttpContext.Session.SetString("balance", UserLogin.Balance.ToString());
                if (UserLogin.Admin == true)
                {
                    return RedirectToPage("/AdminPages/AdminIndex");
                }
                else
                {
                    return RedirectToPage("/UserPages/UserIndex");
                }
            }
            else
            {
                Message = "Invalid Username or Password";
                return Page();
            }
        }
    }
}
