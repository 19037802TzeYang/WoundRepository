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

namespace WoundImgRepo.Controllers
{
    public class WoundController : Controller
    {
        public IActionResult Index()
        {
            List<Wound> list = DBUtl.GetList<Wound>("SELECT * FROM wound");
            return View("Index", list);
        }

        #region Details()
        public IActionResult Details()
        {
            string selectSql = @"SELECT * FROM image";
            List<Image> listImage = DBUtl.GetList<Image>(selectSql);
            return View(listImage);
        }
        #endregion

        #region Create()
        public IActionResult Create()
        {
            var versions = DBUtl.GetList("SELECT version_id, name FROM version ORDER BY name");
            ViewData["versions"] = new SelectList(versions, "version_id", "name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Wound wound, Image image)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Msg"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Create");
            }
            else
            {

                string picfilename = DoPhotoUpload(image.Photo);

                string woundSql = @"INSERT INTO wound(name, wound_stage, remarks,
                                    wound_category_id, wound_location_id, 
                                    tissue_id, version_id, image_id, user_id)
                                    VALUES('{0}','{1}','{2}',{3},{4},{5},{6},{7},{8})";
                int woundRowsAffected = DBUtl.ExecSQL(woundSql, wound.name, wound.wound_stage, wound.remarks,
                    wound.wound_category_id, wound.wound_location_id, wound.tissue_id,
                    wound.version_id, wound.image_id, wound.user_id);

                string imageSql = @"INSERT INTO image(name, type, img_file)
                                    VALUES('{0}','{1}','{2}')";
                int imageRowsAffected = DBUtl.ExecSQL(imageSql, image.name, image.type, picfilename);

                if (woundRowsAffected == 1 && imageRowsAffected == 1)
                {
                    TempData["Msg"] = "New wound created";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("Details");
                }
                else
                {
                    TempData["Msg"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                    return View("Create");
                }
            }
        }
        #endregion

        #region DoPhotoUpload()
        private string DoPhotoUpload(IFormFile photo)
        {
            string fext = Path.GetExtension(photo.FileName);
            string uname = Guid.NewGuid().ToString();
            string fname = uname + fext;
            string fullpath = Path.Combine(_env.WebRootPath, "photos/" + fname);
            using (FileStream fs = new FileStream(fullpath, FileMode.Create))
            {
                photo.CopyTo(fs);
            }
            return fname;
        }

        private IWebHostEnvironment _env;
        public WoundController(IWebHostEnvironment environment)
        {
            _env = environment;
        }
        #endregion
    }

}
