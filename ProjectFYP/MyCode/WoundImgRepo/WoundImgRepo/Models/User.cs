using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WoundImgRepo.Models
{
    public class User
    {

        [Required(ErrorMessage = "Please enter User ID")]
        [Remote(action: "VerifyUserID", controller: "Account")]
        public string UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email field cannot be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5 characters or more")]
        public string UserPw { get; set; }

        [Compare("UserPw", ErrorMessage = "Passwords do not match")]
        public string UserPw2 { get; set; }

        [Required]
        [RegularExpression("(Doctor|Annotator|Admin)", ErrorMessage = "Select option")]
        public string UserRole { get; set; }

        public DateTime LastLogin { get; set; }
    }
}