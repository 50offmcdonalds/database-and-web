using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace terrible.Pages.AdminPages
{
    public class AdminIndexModel : PageModel
    {
        public string Username;
        public const string SessionKeyName1 = "username";

        public string Name;
        public const string SessionKeyName2 = "name";

        public decimal Balance;
        public const string SessionKeyName3 = "balance";

        public bool Admin;
        public const string SessionKeyName4 = "admin";

        public string SessionID;
        public const string SessionKeyName5 = "sessionID";

        public IActionResult OnGet()
        {

            Username = HttpContext.Session.GetString(SessionKeyName1);
            Name = HttpContext.Session.GetString(SessionKeyName2);
            Balance = decimal.Round(Convert.ToDecimal(HttpContext.Session.GetString(SessionKeyName3)), 2);
            Admin = Convert.ToBoolean(HttpContext.Session.GetString(SessionKeyName4));
            SessionID = HttpContext.Session.GetString(SessionKeyName5);

            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Login/Login");
            }
            else if (!Admin)
            {
                return RedirectToPage("/UserPages/UserIndex");
            }
            return Page();
        }
    }
}
