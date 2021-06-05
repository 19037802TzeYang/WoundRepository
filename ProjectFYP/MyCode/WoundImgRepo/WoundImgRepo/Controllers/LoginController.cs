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

            // detects if password is filled
            if (string.IsNullOrEmpty(username))
            {
                ViewData["Message"] = "No username detected";
                ViewData["MsgType"] = "warning";



                return View("~/Views/Login/Index.cshtml");

            }
            //detects if username is filled
            else if (string.IsNullOrEmpty(password))
            {
                ViewData["Message"] = "No password detected";
                ViewData["MsgType"] = "warning";

                return View("~/Views/Login/Index.cshtml");
            }




            else
            {
                //string of SQL
                //          string LOGIN_SQL =
                //@"SELECT user_id FROM [user] 
                //      WHERE username] = '{0}' 
                //        AND [password] = HASHBYTES('SHA1', '{1}')";


                //String userN = username;
                //string psswrd = password;

                string INSERTSQL = @"INSERT INTO user(user_id, username, email, password, user_role, last_login) VALUES " +
                   "(1, 'Benny', 'benny@gmail,com', HASHBYTES('SHA1', 'adminpw'), 'Admin', 'null')";

                //testing string wif nothing
                string LOGINSQL = "SELECT * FROM [user]";

               

                int rowsAffected = DBUtl.ExecSQL(INSERTSQL);
                //testing if insert works
                if (rowsAffected == 1)
                {
                    return View("~/Views/Login/Index1dssd.cshtml");
                }
                else
                {
                    ViewData["Message"] = "insert not working m8.";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Login/Index.cshtml");
                }

                //testing
                //DataTable match = DBUtl.GetTable(LOGIN_SQL, userN, psswrd);


                //testing the GetTable
                DataTable matcht = DBUtl.GetTable(LOGINSQL);

                //convert results into string
                

                

                //check if it is null 
                if (matcht.Rows.Count>0 )
                {

                    //return the view of the home page if fields are correct
                    return View("~/Views/Login/Index1dssd.cshtml");


                }
                else
                {//if the fields are incorrect
                    ViewData["Message"] = "Incorrect fields detected ,try again.";
                    ViewData["MsgType"] = "warning";
                    return View("~/Views/Login/Index.cshtml");





                }

                return View(); //delete after completed
            }


        }
    }
}

