using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using terrible.Models;
using terrible.Pages.DatabaseConnection;

namespace terrible.Pages.UserPages
{
    public class UserIndexModel : PageModel
    {
        [BindProperty]
        public IFormFile userFile { get; set; }

        public readonly IWebHostEnvironment _env;
        public string userProfileName;

        public int? UserID;
        public const string SessionKeyName1 = "userID";

        public string Username;
        public const string SessionKeyName2 = "username";

        public string Name;
        public const string SessionKeyName3 = "name";

        public bool Admin;
        public const string SessionKeyName4 = "admin";

        public decimal Balance;
        public const string SessionKeyName5 = "balance";

        public string SessionID;
        public const string SessionKeyName6 = "sessionID";

       

        public UserIndexModel (IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult OnGet()
        {
            UserID = HttpContext.Session.GetInt32(SessionKeyName1);
            Username = HttpContext.Session.GetString(SessionKeyName2);
            Name = HttpContext.Session.GetString(SessionKeyName3);
            Admin = Convert.ToBoolean(HttpContext.Session.GetString(SessionKeyName4));
            Balance = decimal.Round(Convert.ToDecimal(HttpContext.Session.GetString(SessionKeyName5)),2);
            SessionID = HttpContext.Session.GetString(SessionKeyName6);

            

            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Login/Login");
            }

            //set profile image
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT [Filename] FROM [UserFiles] WHERE [UserID] = @UserID";
                command.Parameters.AddWithValue("@UserID", UserID);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userProfileName = reader.GetString(0);
                }
                if (string.IsNullOrEmpty(userProfileName))
                {
                    userProfileName = "default.png";
                    
                }
                Console.WriteLine(userProfileName);
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var file = Path.Combine(_env.WebRootPath, "imageUpload", userFile.FileName);

            Console.WriteLine(userFile.FileName);

            using (var FStream = new FileStream(file, FileMode.Create))
            {
                userFile.CopyTo(FStream);
            }

            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO [UserFiles] (UserID, Filename) VALUES (@UserID, @Filename)";
                command.Parameters.AddWithValue("@UserID", HttpContext.Session.GetInt32(SessionKeyName1));
                command.Parameters.AddWithValue("@Filename", userFile.FileName);
                command.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToPage("/UserPages/UserIndex");
        }

    }
}
