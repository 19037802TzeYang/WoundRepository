
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using WoundImgRepo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace WoundImgRepo.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("LoginPage", "Account");
        }

        [AllowAnonymous]
        public IActionResult LoginPage(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Login

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginPage(LogInUser user)
        {


            int status = 0;

            //getting the status of users
            string getuserstatus = "SELECT * FROM useracc  WHERE username = '" + user.Username + "' ";
            List<User> List = DBUtl.GetList<User>(getuserstatus);
            foreach (User account in List)
            {
                status = account.status;
            }
            System.Diagnostics.Debug.WriteLine("user status is" + status);
            if (status != 1)
            {
                ViewData["Message"] = "Account is deactivated , please contact your supervisor for support.";
                ViewData["MsgType"] = "danger";
                return View();
            }
            



        if (!AuthenticateUser(user.Username, user.Password,
                                  out ClaimsPrincipal principal))
            {


                ViewData["Message"] = "Incorrect Username or Password";
                ViewData["MsgType"] = "danger";
                return View();
            }
            else
            {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal);

                

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

               

                
                
                    // Update the Last Login Timestamp of the User
                    string update = "UPDATE useracc SET last_login=GETDATE() WHERE username='{0}' AND password='{1}'";
                    DBUtl.ExecSQL(update, user.Username, user.Password);
           
                
                System.Diagnostics.Debug.WriteLine("login succss!");
                return RedirectToAction("TheWounds", "Wound");
            }
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }

        private bool AuthenticateUser(string usname, string pw,

                                         out ClaimsPrincipal principal)
        {
            principal = null;


            string sql = @"SELECT * FROM useracc 
                         WHERE username = '{0}' AND password = HASHBYTES('SHA1', '{1}')";

            // TODO: L09 Task 1 - Make Login Secure, use the new way of calling DBUtl
            //string select = String.Format(sql, uid, pw);
            DataTable ds = DBUtl.GetTable(sql, usname, pw);
            //DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, usname),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["username"].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0]["user_role"].ToString())
                         },
                         CookieAuthenticationDefaults.AuthenticationScheme));
                return true;
            }
            return false;
        }

    }
}
