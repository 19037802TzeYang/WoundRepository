using WoundImgRepo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WoundImgRepo.Controllers
{
    public class WoundController : Controller
    {
        #region Index()
        public IActionResult Index()
        {
            List<Trip> list = DBUtl.GetList<Trip>("SELECT * FROM TravelHighlight");
            return View("Index", list);
        }
        #endregion
        public IActionResult Create()
        {
            return View();
        }
        #region Details(int id)
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            string sql =
               @"SELECT h.*, 
                     u.FullName AS SubmittedBy
                FROM TravelHighlight h, TravelUser u
               WHERE h.UserId = u.UserId
                 AND Id={0}";

            List<Trip> lstTrip = DBUtl.GetList<Trip>(sql, id);
            if (lstTrip.Count == 1)
            {
                Trip trip = lstTrip[0];
                return View("Details", trip);
            }
            else
            {
                TempData["Message"] = "Trip Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
        }
        #endregion
    }

}
