using WoundImgRepo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;



namespace WoundImgRepo.Controllers
{
    public class LoginController : Controller
    {



        //returns page

        public IActionResult Index()
        {
            return View("~/Views/Login/Index.cshtml");
        }

        public IActionResult Index1dssd()
        {
            return View("~/Views/Login/Index1dssd.cshtml");
        }
        // check if the user should be permitted
        [HttpPost]
        public IActionResult Index(string username, string password)
        {



            // detects if all fields are filled
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewData["Message"] = "Not all fields are detected";
                ViewData["MsgType"] = "warning";
                return View("~/Views/Login/Index.cshtml");

            }





            // If all is filled , Then
            else
            {
                //string of SQL
                string LOGIN_SQL =
       @"SELECT user_id FROM useracc 
                      WHERE username = '{0}' 
                        AND password = HASHBYTES('SHA1', '{1}')";


                String userN = username;
                string psswrd = password;


                //testing
                DataTable match = DBUtl.GetTable(LOGIN_SQL, userN, psswrd);






                //check if it is null 
                if (match.Rows.Count > 0)
                {

                    //return the view of the home page if fields are correct
                    return RedirectToAction("Index", "wound");


                }
                else
                {//if the fields are incorrect
                    ViewData["Message"] = "Incorrect inputs detected ,try again.";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Login/Index.cshtml");





                }


            }


        }
    }
}

