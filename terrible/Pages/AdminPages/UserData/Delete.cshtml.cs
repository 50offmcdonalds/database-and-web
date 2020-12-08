using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult OnGet(int? id)
        {
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
