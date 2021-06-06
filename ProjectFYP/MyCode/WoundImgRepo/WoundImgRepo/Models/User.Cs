
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WoundImgRepo.Models
{
    public class User
    {
       
            public int user_id { get; set; }
           

            [Required]
            public string username { get; set; }

        [Required(ErrorMessage = "Email field cannot be empty")]
        public string email { get; set; }

            [Required]
            [MinLength(8, ErrorMessage = "This password is too short")]
            public string password { get; set; }

            [Required]
            [RegularExpression("(Doctor|Annotator|Admin)", ErrorMessage = "Select option")]
            public String user_role { get; set; }
        
    }
}