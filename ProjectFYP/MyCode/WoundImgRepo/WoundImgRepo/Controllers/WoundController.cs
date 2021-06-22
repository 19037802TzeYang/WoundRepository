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
            List<WoundRecord> list = DBUtl.GetList<WoundRecord>(@"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename,
                                      v.name as versionname, i.img_file as imagefile, i.image_id as imageid
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id");
            return View("Index", list);
        }

        #region Details()
        public IActionResult Details(int id)
        {
            string selectWoundSql = @"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename, 
                                      v.name as versionname, i.img_file as imagefile, i.image_id as imageid
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id
                                      WHERE wound_id={0}";
            List<WoundRecord> recordFound = DBUtl.GetList<WoundRecord>(selectWoundSql, id);

            if (recordFound.Count == 1)
            {
                WoundRecord woundRecord = recordFound[0];
                string selectAnnotationSql = @"SELECT i.img_file as annotationimagefile, im.img_file as maskimagefile
                                               FROM annotation an
                                               INNER JOIN image i ON an.annotation_image_id = i.image_id
                                               INNER JOIN image im ON an.mask_image_id = im.image_id
                                               INNER JOIN wound w ON an.wound_id = w.wound_id
                                               WHERE w.wound_id={0}";
                List<AnnotationMaskImage> annotationMaskImageList = DBUtl.GetList<AnnotationMaskImage>(selectAnnotationSql, id);
                woundRecord.annotationMaskImage = new List<AnnotationMaskImage>();
                woundRecord.annotationMaskImage.AddRange(annotationMaskImageList);
                return View(woundRecord);
            }
            else
            {
                TempData["Msg"] = "Wound record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Create()
        public IActionResult Create()
        {
            List<SelectListItem> versions = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "Version 1.16", Value = "v1.16"
                },
                new SelectListItem {
                    Text = "Version 1.17", Value = "v1.17"
                },
                new SelectListItem {
                    Text = "Version 1.18", Value = "v1.18"
                },
                new SelectListItem {
                    Text = "Version 2.0", Value = "v2.0"
                }
            };
            ViewData["versions"] = new SelectList(versions, "Text", "Value");
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
                //image table
                string picfilename = DoPhotoUpload(cc.image.Photo);
                string imageSql = @"INSERT INTO image(name, type, img_file)
                                    VALUES('{0}','{1}','{2}')";
                int imageRowsAffected = DBUtl.ExecSQL(imageSql, cc.wound.name, "Original Wound Image", picfilename);
                Image img = DBUtl.GetList<Image>("SELECT image_id FROM image ORDER BY image_id DESC")[0];

                string anpicfilename = DoPhotoUpload(cc.annotationimage);
                string animageSql = @"INSERT INTO image(name, type, img_file)
                                      VALUES('{0}','{1}','{2}')";
                int animageRowsAffected = DBUtl.ExecSQL(animageSql, cc.wound.name, "Annotation Image", anpicfilename);
                var anImg = DBUtl.GetList<Image>("SELECT image_id FROM image WHERE name='" + cc.wound.name + "'AND type='Annotation Image'");

                string maskpicfilename = DoPhotoUpload(cc.maskimage);
                string maskimageSql = @"INSERT INTO image(name, type, img_file)
                                        VALUES('{0}','{1}','{2}')";
                int maskimageRowsAffected = DBUtl.ExecSQL(maskimageSql, cc.wound.name, "Mask Image", maskpicfilename);
                var maskImg = DBUtl.GetList<Image>("SELECT image_id FROM image WHERE name='" + cc.wound.name + "'AND type='Mask Image'");

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
                string tSql = @"INSERT INTO tissue(name)
                                VALUES('{0}')";
                int tRowsAffected = DBUtl.ExecSQL(tSql, cc.tissue.name);
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
                int anRowsAffected = 0;
                int imgCount = 0;
                if (anImg.Count == maskImg.Count)
                {
                    anImg.ForEach(img =>
                    {
                        string anSql = @"INSERT INTO annotation(mask_image_id, wound_id, annotation_image_id)
                                         VALUES({0},{1},{2})";
                        anRowsAffected = DBUtl.ExecSQL(anSql, maskImg[imgCount].image_id, w.wound_id, img.image_id);
                        imgCount += 1;
                    });
                }

                if (imageRowsAffected == 1 && wRowsAffected == 1 &&
                    wCRowsAffected == 1 && wLRowsAffected == 1 &&
                    tRowsAffected == 1 && wRowsAffected == 1 &&
                    wVRowsAffected == 1 && anRowsAffected == 1)
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

        #region UpdateAnnotationMaskImage()
        [HttpPost]
        public IActionResult UpdateAnnotationMaskImage(WoundRecord wr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Msg"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return RedirectToAction("Details", new { id = wr.woundid });
            }
            else
            {
                //image table
                string anpicfilename = DoPhotoUpload(wr.annotationimage);
                string animageSql = @"INSERT INTO image(name, type, img_file)
                                      VALUES('{0}','{1}','{2}')";
                int animageRowsAffected = DBUtl.ExecSQL(animageSql, wr.woundname, "Annotation Image", anpicfilename);
                var anImg = DBUtl.GetList<Image>("SELECT image_id FROM image WHERE name='" + wr.woundname + "'AND type='Annotation Image' AND img_file='" + anpicfilename + "'");

                string maskpicfilename = DoPhotoUpload(wr.maskimage);
                string maskimageSql = @"INSERT INTO image(name, type, img_file)
                                        VALUES('{0}','{1}','{2}')";
                int maskimageRowsAffected = DBUtl.ExecSQL(maskimageSql, wr.woundname, "Mask Image", maskpicfilename);
                var maskImg = DBUtl.GetList<Image>("SELECT image_id FROM image WHERE name='" + wr.woundname + "'AND type='Mask Image' AND img_file='" + maskpicfilename + "'");

                //annotation table
                int anRowsAffected = 0;
                int imgCount = 0;
                if (anImg.Count == maskImg.Count)
                {
                    anImg.ForEach(img =>
                    {
                        string anSql = @"INSERT INTO annotation(mask_image_id, wound_id, annotation_image_id)
                                         VALUES({0},{1},{2})";
                        anRowsAffected = DBUtl.ExecSQL(anSql, maskImg[imgCount].image_id, wr.woundid, img.image_id);
                        imgCount += 1;
                    });
                }

                if (animageRowsAffected == 1 && maskimageRowsAffected == 1)
                {
                    TempData["Msg"] = "Images updated successfully";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("Details", new { id = wr.woundid });
                }
                else
                {
                    TempData["Msg"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                    return RedirectToAction("Details", new { id = wr.woundid });
                }
            }
        }
        #endregion

        #region Update()
        public IActionResult Update(int id)
        {
            string selectWoundSql = @"SELECT wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename, 
                                      v.name as versionname, i.img_file as imagefile, i.image_id as imageid
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id
                                      WHERE wound_id={0}";
            List<WoundRecord> recordFound = DBUtl.GetList<WoundRecord>(selectWoundSql, id);
            if (recordFound.Count == 1)
            {
                WoundRecord woundRecord = recordFound[0];
                var wound = DBUtl.GetList<Wound>($"SELECT * FROM wound WHERE wound_id={id}")[0];
                var img = DBUtl.GetList<Image>($"SELECT img_file FROM image WHERE image_id={woundRecord.woundid}")[0];
                //record - Image(wound image), Wound(id, name, stage, remarks), WoundCategory(id, name), WoundLocation(id, name), Tissue(id, name), WVersion(id, name)
                CombineClass record = new CombineClass()
                {
                    wound = new Wound() { wound_id = id, name = woundRecord.woundname, wound_stage = woundRecord.woundstage, remarks = woundRecord.woundremarks },
                    woundc = new WoundCategory() { wound_category_id = wound.wound_category_id, name = woundRecord.woundcategoryname },
                    woundl = new WoundLocation() { wound_location_id = wound.wound_location_id, name = woundRecord.woundlocationname },
                    tissue = new Tissue() { tissue_id = wound.tissue_id, name = woundRecord.tissuename },
                    woundv = new WVersion() { version_id = wound.version_id, name = woundRecord.versionname },
                    image = new Image() { img_file = img.img_file }
                };

                List<SelectListItem> versions = new List<SelectListItem>() {
                    new SelectListItem {
                        Text = "Version 1.16", Value = "v1.16"
                    },
                    new SelectListItem {
                        Text = "Version 1.17", Value = "v1.17"
                    },
                    new SelectListItem {
                        Text = "Version 1.18", Value = "v1.18"
                    },
                    new SelectListItem {
                        Text = "Version 2.0", Value = "v2.0"
                    }
                };
                ViewData["versions"] = new SelectList(versions, "Text", "Value");
                return View(record);
            }
            else
            {
                TempData["Msg"] = "Wound record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Details");
            }
        }

        [HttpPost]
        public IActionResult Update(CombineClass cc)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Msg"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Index");
            }
            else
            {
                //wound table
                string wSql = @"UPDATE wound
                                SET name='{0}', wound_stage='{1}', remarks='{2}'
                                WHERE wound_id={3}";
                int wRowsAffected = DBUtl.ExecSQL(wSql, cc.wound.name, cc.wound.wound_stage, cc.wound.remarks, cc.wound.wound_id);

                //wound_category table
                string wCSql = @"UPDATE wound_category
                                 SET name='{0}'
                                 WHERE wound_category_id={1}";
                int wCRowsAffected = DBUtl.ExecSQL(wCSql, cc.woundc.name, cc.woundc.wound_category_id);

                //wound_location table
                string wLSql = @"UPDATE wound_location
                                 SET name='{0}'
                                 WHERE wound_location_id={1}";
                int wLRowsAffected = DBUtl.ExecSQL(wLSql, cc.woundl.name, cc.woundl.wound_location_id);

                //tissue table
                string tSql = @"UPDATE Tissue
                                SET name='{0}'
                                WHERE tissue_id={1}";
                int tRowsAffected = DBUtl.ExecSQL(tSql, cc.tissue.name, cc.tissue.tissue_id);

                //version table
                string wVSql = @"UPDATE Version
                                 SET name='{0}'
                                 WHERE version_id={1}";
                int wVRowsAffected = DBUtl.ExecSQL(wVSql, cc.woundv.name, cc.woundv.version_id);

                if (wRowsAffected == 1 && wCRowsAffected == 1 &&
                    wLRowsAffected == 1 && tRowsAffected == 1 &&
                    wVRowsAffected == 1)
                {
                    TempData["Msg"] = "Wound record updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Msg"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("Details", new { id = cc.wound.wound_id });
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
