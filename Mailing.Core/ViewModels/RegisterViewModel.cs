using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookApp.Core.ViewModels
{
    public class RegisterViewModel
    {

        [Required()]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginViewModel
    {

        [Required()]
        public string Username { get; set; }

   

        [Required]
        public string Password { get; set; }
    }
}
