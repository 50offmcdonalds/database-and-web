using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace terrible.Models
{
    public class User
    {
        [Display(Name = "User ID")]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Admin")]
        public bool Admin { get; set; }
        [Display(Name = "Account Balance")]
        public decimal Balance { get; set; }
    }
}
