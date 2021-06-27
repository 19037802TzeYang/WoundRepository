
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using WoundImgRepo.Models;
using Microsoft.AspNetCore.Authorization;

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
            if (!AuthenticateUser(user.Username, user.Password,
                                  out ClaimsPrincipal principal))
            {


                ViewData["Message"] = "Incorrect Username or Password";

                return View();
            }
            else
            {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal);

                // Update the Last Login Timestamp of the User
                string update = "UPDATE useracc SET last_login=GETDATE() WHERE username='{0}' AND password='{1}'";
                DBUtl.ExecSQL(update, user.Username, user.Password);

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

                System.Diagnostics.Debug.WriteLine("login succss!");
                return RedirectToAction("Index", "Wound");
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