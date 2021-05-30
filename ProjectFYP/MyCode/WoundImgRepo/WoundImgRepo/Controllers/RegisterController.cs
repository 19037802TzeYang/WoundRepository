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
        public ActionResult Index(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                ViewBag.Failed = true;
                return View();
            }
            else
            {
                ViewBag.Failed = false;
                //ensure to put in the transition page
                return RedirectToAction("#", "#");
            }

        }
    }
}