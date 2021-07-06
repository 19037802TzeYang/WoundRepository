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
    public class AdminController : Controller
    {
        //display register page
        [Authorize(Roles = "Admin")]
        public IActionResult Registry()
        {
            return View();
        }

        //display list of users
        [Authorize(Roles = "Admin")]
        public IActionResult Userlist()
        {
            List<User> List = DBUtl.GetList<User>("SELECT * FROM useracc");
            return View(List);
        }
        //stores in bad passwords
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

        //check if the details keyed in are eligable for registration
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Registry(string username, string password, string UserPw2, string email, String user_role)


        {


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(UserPw2) || string.IsNullOrEmpty(email) || String.IsNullOrEmpty(user_role))
            {

                //stores in password 1


                ViewData["Message"] = "One or more fields are missing";
                ViewData["MsgType"] = "warning";
                return View("~/Views/Admin/Registry.cshtml");
            }
            else
            {
                //check for duplicate names
                string dupname = "SELECT * FROM useracc WHERE username = '{0}'";
                DataTable matchdupe = DBUtl.GetTable(dupname, username);
                if (matchdupe.Rows.Count == 1)
                {
                    ViewData["Message"] = "duplicate name detected! ";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                //password checker for hackable passwords
                if (badPasswords.Contains(password) || password.Length < 5)
                {
                    ViewData["Message"] = "main Password too weak.";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                // compare passwords
                if (password.Equals(UserPw2) != true || UserPw2.Length < 5)
                {
                    ViewData["Message"] = "second password : error detected! ";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }



                Regex list_of_caps = new Regex(@"[A-Z]");

                //check if at least 1 character is in uppercase
                MatchCollection matches = list_of_caps.Matches(password);
                if (matches.Count == 0)
                {
                    ViewData["Message"] = "Password Has no capital";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                Regex numbercheck = new Regex(@"[1-9]");

                //check if at least 1 character is in uppercase
                MatchCollection matchnum = numbercheck.Matches(password);
                if (matchnum.Count == 0)
                {
                    ViewData["Message"] = "Password Has no numbers";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }


                //check if email addressed is in use
                string namecheck_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE username = '{0}'";

                DataTable matchname = DBUtl.GetTable(namecheck_SQL, username);
                              
                if (matchname.Rows.Count == 1)
                {
                    ViewData["Message"] = "User currently exist , try using another name ";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }
                


                //check if insert is done
                 string INSERT = @"INSERT INTO useracc( username, email, password, user_role) 
                VALUES ( '{0}', '{1}', HASHBYTES('SHA1', '{2}'), '{3}')";
                 int rowsAffected = DBUtl.ExecSQL(INSERT, username, email, password, user_role);

                if (rowsAffected == 1)
                {
                    //replace dis wif ur homepage
                    TempData["Message"] = "Successful Registration!";
                    ViewData["MsgType"] = "success";
                    return RedirectToAction("Userlist");
                }
                else
                {
                    TempData["Message"] = "Not working";
                    ViewData["MsgType"] = "warning";
                    return View();
                }

            }
        }





        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
      //      Debug.WriteLine("deleting" + id);
        //    string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
          //  if (userid.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            //{
              //  TempData["Message"] = "Own ID cannot be deleted";
                //TempData["MsgType"] = "warning";
        //    }
          //  else
            {
                string delete = "DELETE FROM useracc WHERE user_id='{0}'";
                int res = DBUtl.ExecSQL(delete, id);
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
