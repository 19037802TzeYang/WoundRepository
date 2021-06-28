using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoundImgRepo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace WoundImgRepo.Controllers
{
    public class AdminController : Controller
    {
        // TODO: L09 Task 6 - Add only the admin role to the [Authorize] attributes (4x)
        [Authorize(Roles = "admin")]
        public IActionResult Users()
        {
            List<User> list = DBUtl.GetList<User>("SELECT * FROM useracc");
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateUser(User usr)
        {
            // TODO: L09 Task 5 - Write secure code to insert TravelUser into database
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("CreateUser");
            }
            else
            {
                string insert =
                    @"INSERT INTO useracc(user_id, username, email, password, user_role, last_login)
                    VALUES('{0}', '{1}', '{2}', HASHBYTES('SHA1', '{3}'), '{4}', '{5}')";
                if (DBUtl.ExecSQL(insert, usr.user_id, usr.username, usr.email,
                                    usr.password, usr.user_role, usr.last_Login) == 1)
                {
                    TempData["Message"] = "User Created";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return RedirectToAction("Users");
            }
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(string id)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userid.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            {
                TempData["Message"] = "Own ID cannot be deleted";
                TempData["MsgType"] = "warning";
            }
            else
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
