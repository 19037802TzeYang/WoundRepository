
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

            [Required]
            [MinLength(8, ErrorMessage = "This password is too short")]
            public string password { get; set; }

            [Required]
            [Range(1, 3, ErrorMessage= "Id should be 1 to 3 ")]
            public int user_type_id { get; set; }
        
    }
}