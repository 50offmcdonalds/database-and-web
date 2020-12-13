using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.Rendering;
using terrible.Models;
using terrible.Pages.DatabaseConnection;

namespace terrible.Pages.AdminPages
{
    public class ViewModel : PageModel
    {
        public List<User> UserList { get; set; }

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
        public IActionResult OnGet(string pdf)
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
                command.CommandText = @"SELECT * FROM [User]";
                SqlDataReader reader = command.ExecuteReader();

                UserList = new List<User>();
                while (reader.Read())
                {
                    User userItem = new User();
                    userItem.Id = reader.GetInt32(0);
                    userItem.Name = reader.GetString(1);
                    userItem.Username = reader.GetString(2);
                    userItem.Password = reader.GetString(3);
                    userItem.Admin = reader.GetBoolean(4);
                    userItem.Balance = Decimal.Round(reader.GetDecimal(5),2);

                    UserList.Add(userItem);
                }
                reader.Close();
            }
            conn.Close();

            if (pdf == "1")
            {
                Document doc = new Document();
                Section sec = doc.AddSection();
                Paragraph para = sec.AddParagraph();

                para.Format.Font.Name = "Arial";
                para.Format.Font.Size = 14;
                para.Format.Font.Color = Color.FromCmyk(0, 0, 0, 100);
                para.AddFormattedText("List of Users", TextFormat.Bold);
                para.Format.SpaceAfter = "1.0cm";

                Table tab = new Table();
                tab.Borders.Width = 0.75;
                tab.TopPadding = 5;
                tab.BottomPadding = 5;

                Column col = tab.AddColumn(Unit.FromCentimeter(1.5));
                col.Format.Alignment = ParagraphAlignment.Justify;
                tab.AddColumn(Unit.FromCentimeter(3.5));
                tab.AddColumn(Unit.FromCentimeter(3.5));
                tab.AddColumn(Unit.FromCentimeter(3.5));
                tab.AddColumn(Unit.FromCentimeter(1.5));
                tab.AddColumn(Unit.FromCentimeter(4));
                Row row = tab.AddRow();
                row.Shading.Color = Colors.NavajoWhite;

                Cell cell = new Cell();
                cell = row.Cells[0];
                cell.AddParagraph("ID");
                cell = row.Cells[1];
                cell.AddParagraph("Name");
                cell = row.Cells[2];
                cell.AddParagraph("Username");
                cell = row.Cells[3];
                cell.AddParagraph("Password");
                cell = row.Cells[4];
                cell.AddParagraph("Admin");
                cell = row.Cells[5];
                cell.AddParagraph("Balance");

                for (int i=0; i<UserList.Count; i++)
                {
                    row = tab.AddRow();
                    cell = row.Cells[0];
                    cell.AddParagraph(Convert.ToString(UserList[i].Id));
                    cell = row.Cells[1];
                    cell.AddParagraph(UserList[i].Name);
                    cell = row.Cells[2];
                    cell.AddParagraph(UserList[i].Username);
                    cell = row.Cells[3];
                    cell.AddParagraph(UserList[i].Password);
                    cell = row.Cells[4];
                    cell.AddParagraph(Convert.ToString(UserList[i].Admin));
                    cell = row.Cells[5];
                    cell.AddParagraph(Convert.ToString(UserList[i].Balance));
                }

                tab.SetEdge(0, 0, 6, (UserList.Count + 1), Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
                sec.Add(tab);

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = doc;
                pdfRenderer.RenderDocument();

                MemoryStream stream = new MemoryStream();
                pdfRenderer.PdfDocument.Save(stream);
                Response.Headers.Add("content-disposition", new[] { "inline; filename = ListOfUsers.pdf" });
                return File(stream, "application/pdf");
            }

            return Page();
        }
    }
}
