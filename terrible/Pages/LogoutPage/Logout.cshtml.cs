using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace terrible.Pages.LogoutPage
{
    public class LogoutModel : PageModel
    {
        public bool Admin;
        public const string SessionKeyName1 = "admin";

        public string Username;
        public const string SessionKeyName2 = "username";
        public void OnGet()
        {
            HttpContext.Session.Clear();
        }
    }
}
