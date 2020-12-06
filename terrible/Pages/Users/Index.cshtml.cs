using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using terrible.Models;
using terrible.Pages.DatabaseConnection;

namespace terrible.Pages.Users
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public User userAccount { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            //string DbConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO [User] ([Name], [Username], [Password], [Balance]) VALUES (@Name, @Username, @Password, @Balance)";


                //command.Parameters.AddWithValue("@UID", userAccounts.Id);
                command.Parameters.AddWithValue("@Name", userAccount.Name);
                command.Parameters.AddWithValue("@Username", userAccount.Username);
                command.Parameters.AddWithValue("@Password", userAccount.Password);
                command.Parameters.AddWithValue("@Balance", userAccount.Balance);

                //Console.WriteLine(userAccounts.Id);
                Console.WriteLine(userAccount.Username);
                Console.WriteLine(userAccount.Password);
                Console.WriteLine(userAccount.Balance);

                Console.WriteLine(command.CommandText);
                command.ExecuteNonQuery();
            }
            conn.Close();
                return RedirectToPage("/Index");
        }
    }
}
