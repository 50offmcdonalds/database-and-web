using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using terrible.Models;

namespace terrible.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User user { get; set; }
        public string Message { get; set; }
        public string SessionID;
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("/Index");
        }
    }
}
