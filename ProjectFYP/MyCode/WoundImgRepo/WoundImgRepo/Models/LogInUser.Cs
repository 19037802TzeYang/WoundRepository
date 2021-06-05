using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WoundImgRepo.Models
{
    public class LogInUser
    {
       
            public int user_id { get; set; }
           
            [Required]
            public string username { get; set; }

            [Required]
            public string password { get; set; }

          
            public bool RememberMe { get; set; }

    }
}