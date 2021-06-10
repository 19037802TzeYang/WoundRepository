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
        public IActionResult Details(int id)
        {
            string selectSql = @"SELECT w.name as woundname, wc.name as woundcategoryname,wl.name as woundlocation,
                                v.name as woundversionname, t.name as tissuename, i.img_file as imagefile, i.image_id as imageid
                                FROM wound w
                                INNER JOIN image i ON i.image_id = w.image_id
                                INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                INNER JOIN version v ON v.version_id = w.version_id
                                WHERE wound_id={0}";

            List<CombineClass2> recordFound = DBUtl.GetList<CombineClass2>(selectSql, id);
            if (recordFound.Count == 1)
            {
                CombineClass2 patientRecord = recordFound[0];
                string selectAnnotations = @"SELECT i.img_file as annotationimagefile, im.img_file as maskimagefile
                                FROM annotation an
                                INNER JOIN image i ON an.annotation_image_id = i.image_id
                                INNER JOIN image im ON an.mask_image_id = im.image_id
                                INNER JOIN wound w ON an.wound_id = w.wound_id
                                WHERE w.wound_id={0}";
                List<AnnotationImage> annotationimages = DBUtl.GetList<AnnotationImage>(selectAnnotations, id);
                patientRecord.AnnotationImages = new List<AnnotationImage>();
                patientRecord.AnnotationImages.AddRange(annotationimages);
                return View(patientRecord);
            }
            else
            {
                TempData["Msg"] = "Patient record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
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
        public IActionResult Create(CombineClass cc)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Msg"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Create");
            }
            else
            {
                string picfilename = DoPhotoUpload(cc.image.Photo);
                string anpicfilename = DoPhotoUpload(cc.annotationimage.Photo);
                string maskpicfilename = DoPhotoUpload(cc.maskimage.Photo);

                //image table
                string imageSql = @"INSERT INTO image(name, type, img_file)
                                    VALUES('{0}','{1}','{2}')";
                int imageRowsAffected = DBUtl.ExecSQL(imageSql, cc.image.name, "Original Wound Image", picfilename);
                Image img = DBUtl.GetList<Image>("SELECT image_id FROM image ORDER BY image_id DESC")[0];

                string animageSql = @"INSERT INTO image(name, type, img_file)
                                    VALUES('{0}','{1}','{2}')";
                int animageRowsAffected = DBUtl.ExecSQL(animageSql, cc.annotationimage.name, "Annotation Image", anpicfilename);
                Image anImg = DBUtl.GetList<Image>("SELECT image_id FROM image ORDER BY image_id DESC")[0];

                string amaskimageSql = @"INSERT INTO image(name, type, img_file)
                                    VALUES('{0}','{1}','{2}')";
                int maskimageRowsAffected = DBUtl.ExecSQL(amaskimageSql, cc.maskimage.name, "Mask Image", maskpicfilename);
                Image maskImg = DBUtl.GetList<Image>("SELECT image_id FROM image ORDER BY image_id DESC")[0];

                //wound_category table
                string wCSql = @"INSERT INTO wound_category(name)
                                 VALUES('{0}')";
                int wCRowsAffected = DBUtl.ExecSQL(wCSql, cc.woundc.name);
                WoundCategory wc = DBUtl.GetList<WoundCategory>("SELECT wound_category_id FROM wound_category ORDER BY wound_category_id DESC")[0];

                //wound_location table
                string wLSql = @"INSERT INTO wound_location(name)
                                 VALUES('{0}')";
                int wLRowsAffected = DBUtl.ExecSQL(wLSql, cc.woundl.name);
                WoundLocation wl = DBUtl.GetList<WoundLocation>("SELECT wound_location_id FROM wound_location ORDER BY wound_location_id DESC")[0];

                //tissue table
                string tLSql = @"INSERT INTO tissue(name)
                                 VALUES('{0}')";
                int tRowsAffected = DBUtl.ExecSQL(tLSql, cc.tissue.name);
                Tissue t = DBUtl.GetList<Tissue>("SELECT tissue_id FROM Tissue ORDER BY tissue_id DESC")[0];

                //version table
                string wVLSql = @"INSERT INTO version(name)
                                  VALUES('{0}')";
                int wVRowsAffected = DBUtl.ExecSQL(wVLSql, cc.woundv.name);
                WVersion v = DBUtl.GetList<WVersion>("SELECT version_id FROM Version ORDER BY version_id DESC")[0];

                //wound table 
                string wSql = @"INSERT INTO wound(name, wound_stage, remarks, 
                                wound_category_id, wound_location_id, tissue_id, version_id, image_id)
                                VALUES('{0}','{1}','{2}',{3},{4},{5},{6},{7})";
                int wRowsAffected = DBUtl.ExecSQL(wSql, cc.wound.name, cc.wound.wound_stage, cc.wound.remarks,
                    wc.wound_category_id, wl.wound_location_id, t.tissue_id, v.version_id, img.image_id);
                Wound w = DBUtl.GetList<Wound>("SELECT wound_id FROM wound ORDER BY wound_id DESC")[0];

                //annotation table
                string anSql = @"INSERT INTO annotation(mask_image_id, wound_id, annotation_image_id)
                                  VALUES({0},{1},{2})";
                int anRowsAffected = DBUtl.ExecSQL(anSql, maskImg.image_id, w.wound_id, anImg.image_id);

                if (imageRowsAffected == 1 && wRowsAffected == 1 &&
                    wCRowsAffected == 1 && wLRowsAffected == 1 &&
                    tRowsAffected == 1 && wRowsAffected == 1 &&
                    wVRowsAffected == 1 && anRowsAffected == 1 &&
                    animageRowsAffected == 1 && maskimageRowsAffected == 1)
                {
                    TempData["Msg"] = "New wound created";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("Index");
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
