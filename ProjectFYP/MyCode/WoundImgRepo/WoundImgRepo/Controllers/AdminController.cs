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
        #region DisplayRegistry()
        //  [Authorize(Roles = "Admin")]
        public IActionResult Registry()
        {
            return View("~/Views/Admin/Registry.cshtml");
        }
        #endregion

        //display list of users
        #region showUserlist()
        //   [Authorize(Roles = "Admin")]
        //   [Authorize(Roles = "Doctor")]
        //   [Authorize(Roles = "Annotator")]
        public IActionResult Userlist()
        {
            List<User> List = DBUtl.GetList<User>("SELECT * FROM useracc");
            return View(List);
        }
        #endregion

        //stores in bad passwords
        #region badPasswords()
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
            "qwerty",
            "Password",
            "Password1",
            "QWERTY"
        };
        #endregion


        //check if the details keyed in are eligable for registration
        #region Registrypost()
        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Registry(string username, string password, string UserPw2, string email, String user_role)
        {

            Debug.WriteLine(username);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(UserPw2) || string.IsNullOrEmpty(email) || String.IsNullOrEmpty(user_role))
            {

                //stores in password 1


                ViewData["Msg"] = "One or more fields are missing";
                ViewData["MsgType"] = "danger";
                return View("~/Views/Admin/Registry.cshtml");
            }
            else
            {
                //check for duplicate names
                string dupname = "SELECT * FROM useracc WHERE username = '{0}'";
                DataTable matchdupe = DBUtl.GetTable(dupname, username);
                if (matchdupe.Rows.Count == 1)
                {
                    ViewData["Msg"] = "duplicate name detected! ";
                    ViewData["MsgType"] = "danger";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                //password checker for hackable passwords
                if (badPasswords.Contains(password) || password.Length < 5)
                {
                    ViewData["Msg"] = "main Password too weak.";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                // compare passwords
                if (password.Equals(UserPw2) != true || UserPw2.Length < 5)
                {
                    ViewData["Msg"] = "second password : error detected! ";
                    ViewData["MsgType"] = "danger";
                    return View("~/Views/Admin/Registry.cshtml");
                }



                Regex list_of_caps = new Regex(@"[A-Z]");

                //check if at least 1 character is in uppercase
                MatchCollection matches = list_of_caps.Matches(password);
                if (matches.Count == 0)
                {
                    ViewData["Msg"] = "Password Has no capital";
                    ViewData["MsgType"] = "danger";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                Regex numbercheck = new Regex(@"[0-9]");

                //check if at least 1 character is in numbers
                MatchCollection matchnum = numbercheck.Matches(password);
                if (matchnum.Count == 0)
                {
                    ViewData["Msg"] = "Password Has no numbers";
                    ViewData["MsgType"] = "danger";
                    return View("~/Views/Admin/Registry.cshtml");
                }


                //check if username is in use
                string namecheck_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE username = '{0}'";

                DataTable matchname = DBUtl.GetTable(namecheck_SQL, username);
                              
                if (matchname.Rows.Count == 1)
                {
                    ViewData["Msg"] = "User currently exist , try using another name ";
                    ViewData["MsgType"] = "danger";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                //check if email is duplicated
                string emailcheck_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE email = '{0}'";

                DataTable matchemail = DBUtl.GetTable(emailcheck_SQL, email);

                if (matchemail.Rows.Count == 1)
                {
                    ViewData["Msg"] = "duplicate email detected";
                    ViewData["MsgType"] = "danger";
                    return View("~/Views/Admin/Registry.cshtml");
                }

                //check if insert is done
                string INSERT = @"INSERT INTO useracc( username, email, password, user_role, status) 
                VALUES ( '{0}', '{1}', HASHBYTES('SHA1', '{2}'), '{3}', 1)";
                 int rowsAffected = DBUtl.ExecSQL(INSERT, username, email, password, user_role);

                if (rowsAffected == 1)
                {
                    //replace dis wif ur homepage
                    TempData["Msg"] = "Successful Registration !";
                    ViewData["MsgType"] = "success";
                    return RedirectToAction("Userlist");
                }
                else
                {
                    TempData["Msg"] = "Registration not working";
                    ViewData["MsgType"] = "danger";
                    return View();
                }

            }
        }
        #endregion


        #region showingedituser()
        //    [Authorize(Roles = "Admin"l]
        public IActionResult EditUser(string id)
        {

            //gets a user back in
            String Getuser = "SELECT * FROM useracc WHERE user_id = " + id;

            List<User> List = DBUtl.GetList<User>(Getuser);
            foreach(User account in List)
            {
                TempData["id"] = account.user_id;
                TempData["username"] = account.username;
                TempData["email"]  = account.email;
                TempData["role"] =   account.user_role;
                TempData["password"] = account.password;
               

            }
            TempData["usernamecurrently"] = "presentnamefirst";
            return View();
        }
        #endregion

        //    [Authorize(Roles = "Admin"l]
        [HttpPost]
        public IActionResult EditUser(int editPW, string username, string password, string UserPw2, string email, String user_role , int id)
        {
            Debug.WriteLine("we are editing on :"+ editPW);
            //<---------------------------------------------------------------------------------------------------->
            //resources for checking names
            string dupname = "SELECT user_id FROM useracc WHERE username = '{0}' AND user_id != {1}";
            DataTable matchdupe = DBUtl.GetTable(dupname, username , id);

            //<---------------------------------------------------------------------------------------------------->
            //fault counter
            int atfault = 0;

            //<---------------------------------------------------------------------------------------------------->
            //Resource to check if email is in use
            string emailcheck_SQL =
               @"SELECT user_id FROM useracc 
                      WHERE email = '{0}' AND user_id != {1}";

            DataTable matchemail = DBUtl.GetTable(emailcheck_SQL, email , id);


            //---------------------------------------------------------------------------------------------------------------------------
            //check if non-password fields are empty first
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                atfault = 1;
                ViewData["Msg"] = "non-password fields not entered";
                ViewData["MsgType"] = "danger";
            }
            //---------------------------------------------------------------------------------------------------------------------------
            //check for duplicate names

            else if (matchdupe.Rows.Count > 0)
            {
                atfault = 1;
                ViewData["Msg"] = "duplicate name detected! ";
                ViewData["MsgType"] = "danger";

            }
            //---------------------------------------------------------------------------------------------------------------------------
            //check for duplicate emails

            else if (matchemail.Rows.Count > 0)
            {
                atfault = 1;
                ViewData["Msg"] = "duplicate email detected! ";
                ViewData["MsgType"] = "danger";

            }
            //---------------------------------------------------------------------------------------------------------------------------
            //fields for checking password
            else if (editPW == 1)
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = "";
                 
                }
                if (string.IsNullOrEmpty(UserPw2))
                {
                   
                    UserPw2 = "";
                }
                
                //---------------------------------------------------------------------------------------------------------------------------
                //check if both passwords are empty
                if (password == "" || UserPw2 == "")
                {
                    atfault = 1;
                    ViewData["Msg"] = "One or more Passwords are empty ";
                    ViewData["MsgType"] = "danger";
                }
                //---------------------------------------------------------------------------------------------------------------------------
                else
                {
                    //check if password contains captial letters
                    Regex list_of_caps = new Regex(@"[A-Z]");
                    MatchCollection matches = list_of_caps.Matches(password);

                    //check if password contains numbers
                    Regex numbercheck = new Regex(@"[0-9]");
                    MatchCollection matchnum = numbercheck.Matches(password);
                    //---------------------------------------------------------------------------------------------------------------------------
                    //check if both passwords are matched up
                    if (password.Equals(UserPw2) != true)
                    {
                        atfault = 1;
                        ViewData["Msg"] = "Passwords do not match ";
                        ViewData["MsgType"] = "danger";
                    }
                    //---------------------------------------------------------------------------------------------------------------------------
                    //check both passwords are up to length
                    else if (password.Length < 5)
                    {
                        atfault = 1;
                        ViewData["Msg"] = "Password(s) length too short ";
                        ViewData["MsgType"] = "danger";
                    }
                    //---------------------------------------------------------------------------------------------------------------------------
                    //check if at least 1 character is in numbers for password
                    else if (matchnum.Count == 0)
                    {
                        atfault = 1;
                        ViewData["Msg"] = "Password Has no numbers";
                        ViewData["MsgType"] = "danger";
                    }
                    //---------------------------------------------------------------------------------------------------------------------------
                    //check if at least 1 character is in uppercase for password
                    else if (matches.Count == 0)
                    {
                        ViewData["Msg"] = "Password Has no capital";
                        ViewData["MsgType"] = "danger";
                        atfault = 1;
                    }
                    //---------------------------------------------------------------------------------------------------------------------------
                    //check if both passwords are presented in badPasswords
                    else if (badPasswords.Contains(password))
                    {
                        atfault = 1;
                        ViewData["Msg"] = "Passwords are not secured";
                        ViewData["MsgType"] = "danger";
                    }
                }

            }
            //---------------------------------------------------------------------------------------------------------------------------
            //triggers if no fault is detected
            if(atfault == 0)
            {
                int rowsAffected = 0;
                //---------------------------------------------------------------------------------------------------------------------------
                //Triggers if user allows password reset
                if (editPW ==1) {
                    String edituserconfirmed = @"UPDATE useracc SET 
                                                        username = '{0}' ,email = '{1}' ,password = HASHBYTES('SHA1', '{2}') ,user_role = '{3}' 
                                                        WHERE user_id = {4}";
                   rowsAffected = DBUtl.ExecSQL(edituserconfirmed ,username , email , password , user_role , id);
                    TempData["Msg"] = "User updated";
                    TempData["MsgType"] = "success";
                }
                //---------------------------------------------------------------------------------------------------------------------------
                //Triggers if user deny password reset
                if (editPW == 0)
                {
                    String edituserconfirmed = @"UPDATE useracc SET 
                                                        username = '{0}' ,email = '{1}' ,user_role = '{2}' 
                                                        WHERE user_id = {3}";
                    rowsAffected = DBUtl.ExecSQL(edituserconfirmed, username, email, user_role, id);
                    TempData["Msg"] = "User updated";
                    TempData["MsgType"] = "success";
                }
                if (rowsAffected !=1)
                {
                    TempData["Msg"] = "User Record failed";
                    TempData["MsgType"] = "danger";
                }
            }
            //---------------------------------------------------------------------------------------------------------------------------
            //brings user details back into form if something isn't right
            if (atfault > 0)
            {
                //inject user details back into the form
                String Getuser = "SELECT * FROM useracc WHERE user_id = " + id;
                List<User> List = DBUtl.GetList<User>(Getuser);
                foreach (User account in List)
                {
                    TempData["id"] = account.user_id;
                    TempData["email"] = account.email;
                    TempData["role"] = account.user_role;
                    TempData["password"] = account.password;
                    TempData["usernamecurrently"] = username;
                    TempData["username"] = account.username;
                }

                return View();
            }


            return RedirectToAction("Userlist");
        }


        //    [Authorize(Roles = "Admin")]
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



        public IActionResult Statusedit(int id)
        {
            //---------------------------------------------------------------------------------------------------------------------------
            //Takes in a user details
            String Getuser = "SELECT * FROM useracc WHERE user_id = " + id;
            List<User> List = DBUtl.GetList<User>(Getuser);
            int status = 0;
            foreach (User account in List)
            {
                status = account.status;
            }
            string update="";
            //---------------------------------------------------------------------------------------------------------------------------
            //edit status
            if (status == 0)
            {
                 update = "UPDATE useracc SET status = 1 WHERE user_id = {0}";
                TempData["Msg"] = "User account activated";
                TempData["MsgType"] = "success";
            } else if (status != 0)
            {
                 update = "UPDATE useracc SET status = 0 WHERE user_id = {0}";
                TempData["Msg"] = "User account de-activated";
                TempData["MsgType"] = "success";
            }

            int rowsAffected = DBUtl.ExecSQL(update, id);

            return RedirectToAction("Userlist");
        }
    }
}
