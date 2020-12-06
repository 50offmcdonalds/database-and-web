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
        public string SessionID;
        public void OnGet()
        {
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
                command.CommandText = @"SELECT [Name], [Username], [Password], [Admin] FROM [User] WHERE [Username] = @Username AND [Password] = @Password";

                command.Parameters.AddWithValue("@Username", UserLogin.Username);
                command.Parameters.AddWithValue("@Password", UserLogin.Password);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserLogin.Name = reader.GetString(0);
                    UserLogin.Username = reader.GetString(1);
                    UserLogin.Password = reader.GetString(2);
                    UserLogin.Admin = reader.GetBoolean(3);
                    //UserLogin.Balance = reader.GetFloat(4);
                }
            }
            if (!string.IsNullOrEmpty(UserLogin.Name))
            {
                SessionID = HttpContext.Session.Id;
                HttpContext.Session.SetString("sessionID", SessionID);
                HttpContext.Session.SetString("username", UserLogin.Username);
                HttpContext.Session.SetString("name", UserLogin.Name);
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
