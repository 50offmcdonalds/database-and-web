using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using terrible.Models;


namespace terrible.Pages.UserPages
{
    public class UserIndexModel : PageModel
    {

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
            return Page();
        }


    }
}
