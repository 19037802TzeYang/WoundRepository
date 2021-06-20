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
using System.Security.Claims;

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
        public ActionResult Registry(string username, string password, string UserPw2, string email, String user_role)


        {


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(UserPw2) || string.IsNullOrEmpty(email) || String.IsNullOrEmpty(user_role))
            {

                //stores in password 1


                ViewData["Message"] = "One or more fields are missing";
                ViewData["MsgType"] = "warning";
                return View();
            }
            else
            {
                //password checker for hackable passwords
                if (badPasswords.Contains(password) || password.Length < 5)
                {
                    ViewData["Message"] = "main Password too weak.";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

                // compare passwords
                if (password.Equals(UserPw2) != true || UserPw2.Length < 5)
                {
                    ViewData["Message"] = "second password : error detected! ";
                    ViewData["MsgType"] = "warning";
                    return View();
                }



                Regex list_of_caps = new Regex(@"[A-Z]");

                //check if at least 1 character is in uppercase
                MatchCollection matches = list_of_caps.Matches(password);
                if (matches.Count == 0)
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








                //check if email addressed is in use
                string emailcheck_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE username = '{0}'";

                DataTable matchname = DBUtl.GetTable(emailcheck_SQL, username);


              
                if (matchname.Rows.Count == 1)
                {
                    ViewData["Message"] = "User currently exist , try using another name ";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Register/Registry.cshtml");
                }
                


                //check if insert is done
                 string INSERT = @"INSERT INTO useracc( username, email, password, user_role) 
                VALUES ( '{0}', '{1}', HASHBYTES('SHA1', '{2}'), '{3}')";
                 int rowsAffected = DBUtl.ExecSQL(INSERT, username, email, password, user_role);

                if (rowsAffected == 1)
                {
                    //replace dis wif ur homepage
                    TempData["Message"] = "Successful Registration !";
                    ViewData["MsgType"] = "success";
                    return RedirectToAction("Userlist");
                }
                else
                {
                    TempData["Message"] = "not workin";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

            }
        }

       
        //gets back userlist
        public IActionResult Userlist()
        {
            List<User> List = DBUtl.GetList<User>("SELECT * FROM useracc");
            return View(List);
        }


        public IActionResult Delete(string username)
        {
           
            {
                string delete = "DELETE FROM useracc WHERE username='{0}'";
                int res = DBUtl.ExecSQL(delete, username);
                if (res == 1)
                {
                    TempData["Message"] = "User Record Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("Userlist");
        }

    }
}
