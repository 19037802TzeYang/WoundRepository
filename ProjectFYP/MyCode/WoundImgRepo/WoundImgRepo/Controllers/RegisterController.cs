using WoundImgRepo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Dynamic;

using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace hostrepository.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private string[] badPasswords = new[] {
            "111111",
            "12345",
            "123456",
            "1234567",
            "12345678",
            "123456789",
            "abc123",
            "password",
            "password1",
            "qwerty"
        };

        // final check if the user should be permitted
        [HttpPost]
        public ActionResult Index(string username, string password , string email , String user_role)


        {

           



            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || String.IsNullOrEmpty(user_role))
            {
                
             
                ViewData["Message"] = "One or more fields are missing";
                ViewData["MsgType"] = "warning";
                return View();
            }
            else
            {
                //password checker for hackable passwords
                if (badPasswords.Contains(password) || password.Length<5)
                {
                    ViewData["Message"] = "Password too weak";
                    ViewData["MsgType"] = "warning";
                    return View();
                }
                
               

                

                Regex list_of_caps = new Regex(@"[A-Z]" );

                //check if at least 1 character is in uppercase
                MatchCollection matches = list_of_caps.Matches(password);
                if (matches.Count ==0)
                {
                    ViewData["Message"] = "Password Has no capital";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

                Regex numbercheck = new Regex(@"[1-9]");

                //check if at least 1 character is in uppercase
                MatchCollection matchnum = numbercheck.Matches(password);
                if (matchnum.Count == 0)
                {
                    ViewData["Message"] = "Password Has no numbers";
                    ViewData["MsgType"] = "warning";
                    return View();
                }



                //sets the ID

                int newregi = 1;

                string LOGIN_SQL =
               @"SELECT user_id FROM [user] 
                      WHERE user_id = '{0}'";

                DataTable match = DBUtl.GetTable(LOGIN_SQL, newregi);
                // check if a new ID is acceptable , if not +1 (ngl but we need to string this down by A LOT)
                while (match != null)
                {
                    newregi +=1;
                    match = DBUtl.GetTable(LOGIN_SQL, newregi);

                }



                ViewData["Message"] = "ye m8";
                ViewData["MsgType"] = "warning";
                return View();
            }

        }
    }
}