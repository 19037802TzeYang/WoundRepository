using WoundImgRepo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace hostrepository.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        // final check if the user should be permitted
        [HttpPost]
        public ActionResult Index(string username, string password , string email , String user_role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || String.IsNullOrEmpty(user_role))
            {
                ViewData["Message"] = user_role;
                ViewData["MsgType"] = "warning";
                return View();
            }
            else
            {
                ViewData["Message"] = user_role;
                ViewData["MsgType"] = "warning";
                return View();
            }

        }
    }
}