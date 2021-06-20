using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WoundImgRepo.Models
{
    // This is for registration
    public class User
    {
     
         //BEWARE USERID IS FOR DATABASE
        [Required(ErrorMessage = "Please enter User ID")]
     
        public string user_id { get; set; }
   
        [Required]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 characters")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 characters")]
        [DataType(DataType.Password)]
        public string UserPw { get; set; }

        [Compare("UserPw", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string UserPw2 { get; set; }

        [Required]
        [RegularExpression("(Doctor|Annotator|Admin)", ErrorMessage = "Select option")]
        public string user_role { get; set; }

        public DateTime last_Login { get; set; }
    }
}

