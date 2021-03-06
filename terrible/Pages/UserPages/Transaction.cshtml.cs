using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using terrible.Pages.DatabaseConnection;
using terrible.Models;

namespace terrible.Pages.UserPages
{
    public class TransactionModel : PageModel
    {
        [BindProperty]
        public Transaction Transaction { get; set; }

        //public List<Transaction> TransactionHistory { get; set; }

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

        public int getSenderID()
        {
            int senderID = HttpContext.Session.GetInt32(SessionKeyName1).Value;
            return senderID;
        }
        public decimal getBalance()
        {
            decimal balance = decimal.Round(Convert.ToDecimal(HttpContext.Session.GetString(SessionKeyName4)), 2);
            return balance;
        }
        public IActionResult OnPost()
        {
            Console.WriteLine(Transaction.TransferAmount);
            Console.WriteLine(Transaction.ReceiverID);
            Console.WriteLine(Transaction.SenderID);
            if (getBalance() - Transaction.TransferAmount < 0)
            {
                Message = "Insufficient funds to make transfer";
                return Page();
            }
            if (Transaction.ReceiverID == 0 || Transaction.TransferAmount == 0)
            {
                Message = "Please enter an ID and amount";
                return Page();
            }

            int rowsAffected;
            Transaction.SenderID = getSenderID();
            DatabaseConnect dbstring = new DatabaseConnect(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using(SqlCommand command = new SqlCommand())
            {
                
                command.Connection = conn;
                command.CommandText = @"IF EXISTS (SELECT [Id] FROM [User] WHERE [Id] = @TransferID)
                                        BEGIN
                                            UPDATE [User] SET [Balance] = [Balance] + @NewBalance WHERE [Id] = @TransferID;
                                            UPDATE [User] SET [Balance] = [Balance] - @NewBalance WHERE [Id] = @SenderID;
                                            INSERT INTO[Transactions] ([SenderID], [ReceiverID], [Amount], [Date]) VALUES(@SenderID, @TransferID, @NewBalance, @Date)
                                        END";

                command.Parameters.AddWithValue("@NewBalance", Transaction.TransferAmount);
                command.Parameters.AddWithValue("@TransferID", Transaction.ReceiverID);
                command.Parameters.AddWithValue("@SenderID", Transaction.SenderID);
                command.Parameters.AddWithValue("@Date", DateTime.Now);

                if (string.IsNullOrWhiteSpace(Convert.ToString(Transaction.ReceiverID)) || Transaction.ReceiverID == 0)
                {
                    Message = "Receiver account does not exist";
                    return Page();
                }

                Console.WriteLine(Transaction.TransferAmount);
                Console.WriteLine(Transaction.ReceiverID);
                Console.WriteLine(Transaction.SenderID);

                rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Rows " + rowsAffected);
            }
            conn.Close();
            if (rowsAffected == -1)
            {
                Message = "Receiver account does not exist";
                return Page();
            }
            else
            {
                //refresh balance of logged in account
                HttpContext.Session.SetString("balance", (getBalance() - Transaction.TransferAmount).ToString());
            }
            return RedirectToPage("UserIndex");
        }
    }
}
