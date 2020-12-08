using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terrible.Pages
{
    public class IndexModel : PageModel
    {
        public string Username;
        public const string SessionKeyName2 = "username";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString(SessionKeyName2);
            if (string.IsNullOrEmpty(Username))
            {
                return Page();
            }
            return RedirectToPage("UserPages/Userindex");
        }
    }
}
