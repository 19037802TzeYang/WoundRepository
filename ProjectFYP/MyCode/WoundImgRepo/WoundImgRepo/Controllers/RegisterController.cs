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
using System.Diagnostics;

namespace hostrepository.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Registry()
        {
            return View();
        }

        public IActionResult doneregistry()
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
        public ActionResult Registry(string Username, string UserPw , string UserPw2, string Email , String UserRole)


        {

          
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(UserPw) || string.IsNullOrEmpty(UserPw2)  || string.IsNullOrEmpty(Email) || String.IsNullOrEmpty(UserRole))
            {

                //stores in password 1
               

                ViewData["Message"] = "One or more fields are missing";
                ViewData["MsgType"] = "warning";
                return View();
            }
            else
            {
                //password checker for hackable passwords
                if (badPasswords.Contains(UserPw) || UserPw.Length<5)
                {
                    ViewData["Message"] = "main Password too weak.";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

                // compare passwords
                if ( UserPw.Equals(UserPw2) != true || UserPw2.Length < 5)
                {
                    ViewData["Message"] = UserPw;
                    ViewData["MsgType"] = "warning";
                    return View();
                }



                Regex list_of_caps = new Regex(@"[A-Z]" );

                //check if at least 1 character is in uppercase
                MatchCollection matches = list_of_caps.Matches(UserPw);
                if (matches.Count ==0)
                {
                    ViewData["Message"] = "Password Has no capital";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

                Regex numbercheck = new Regex(@"[1-9]");

                //check if at least 1 character is in uppercase
                MatchCollection matchnum = numbercheck.Matches(UserPw);
                if (matchnum.Count == 0)
                {
                    ViewData["Message"] = "Password Has no numbers";
                    ViewData["MsgType"] = "warning";
                    return View();
                }
                string emailcheck_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE email = '{0}'";

               DataTable matchmail = DBUtl.GetTable(emailcheck_SQL, Email);

           
                //check if email addressed is in use
                if (matchmail.Rows.Count == 1)
                {
                    ViewData["Message"] = "Re-occuring email addressed detected ! ";
                    ViewData["MsgType"] = "warning";
                }

                //sets the ID

                int newregi = 1;

                string checkID_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE user_id = '{0}'";

                DataTable match = DBUtl.GetTable(checkID_SQL, newregi);
                // check if a new ID is acceptable , if not +1 (ngl but we need to string this down by A LOT)
                while (match.Rows.Count == 1)
                {
                 
                    newregi +=1; //sets up the ID)
                    match = DBUtl.GetTable(checkID_SQL, newregi);

                }

                string INSERT = @"INSERT INTO useracc(user_id, username, email, password, user_role) VALUES
                                        ('{0}' , '{1}' , '{2}' , HASHBYTES('SHA1', '{3}'), '{4}'";

                int rowsAffected = DBUtl.ExecSQL(INSERT, newregi, Username, Email, UserPw, UserRole);


                //insert test
                if (rowsAffected == 0)
                {
                    ViewData["Message "] = "doesn't work";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

                //replace dis wif ur homepage
                ViewData["Message"] = "ye m8";
                ViewData["MsgType"] = "warning";
                return View();
            }

        }



    }
}
