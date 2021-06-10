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

        public IActionResult LoginPage()
        {
            return View("~/Views/Login/LoginPage.cshtml");
        }

    
        [HttpPost]
        public IActionResult LoginPage(string Username, string Password)
        {

            // detects if all fields are filled
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ViewData["Message"] = "Not all fields are detected";
                ViewData["MsgType"] = "warning";
                return View("~/Views/Login/LoginPage.cshtml");

            }





            // If all is filled , Then
            else
            {
                //string of SQL
                string LOGIN_SQL =
       @"SELECT user_id FROM useracc 
                      WHERE username = '{0}' 
                        AND password = HASHBYTES('SHA1', '{1}')";


                String userN = Username;
                string psswrd = Password;


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
                    return View("~/Views/Login/LoginPage.cshtml");
                }


            }


        }
    }
}

