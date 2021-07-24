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
using System.Diagnostics;
using System.Dynamic;
using System.Security.Claims;

namespace WoundImgRepo.Controllers
{
    public class WoundController : Controller
    {
        #region Index()
        [Authorize(Roles = "Admin, Annotator")]
        public IActionResult Index()
        {
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

            ViewBag.keyword = "";
            ViewBag.selection = "nothing";
            List<WoundRecord> list = DBUtl.GetList<WoundRecord>(@"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename,
                                      v.name as versionname, i.img_file as imagefile, i.image_id as imageid, u.username
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id
                                        INNER JOIN useracc u ON u.user_id = w.user_id");
            return View("Index", list);
        }
        #endregion

        #region Indexpost()
        public IActionResult Indexpost( )
        {
            IFormCollection form = HttpContext.Request.Form;
            string searchedsection = form["searchedsection"].ToString();
            string searchedobj = form["searchedobj"].ToString().Trim();
            Debug.WriteLine("doin index search with "+ searchedsection);

            String listinput = @"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename,
                                      v.name as versionname, i.img_file as imagefile, i.image_id as imageid, u.username
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id
                                        INNER JOIN useracc u ON u.user_id = w.user_id
                                        WHERE "+ searchedsection + " LIKE '%" + searchedobj + "%'";



            Debug.WriteLine(listinput);
            ViewBag.keyword = searchedobj;
            ViewBag.selection = searchedsection;

            List<WoundRecord> list = DBUtl.GetList<WoundRecord>(listinput);

            return View("Index", list );
        }
        #endregion

        #region Details()
        public IActionResult Details(int id)
        {
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

            var wound = DBUtl.GetList<Wound>($"SELECT * FROM wound WHERE wound_id={id}")[0];
            string selectWoundSql = @"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename, 
                                      v.name as versionname, v.version_id as versionid, i.img_file as imagefile, i.image_id as imageid
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id
                                      WHERE w.name='{0}'";
            //retrieve all wound record
            List<WoundRecord> recordFound = DBUtl.GetList<WoundRecord>(selectWoundSql, wound.name);
            //create new list of WoundRecord
            var woundRecordList = new List<WoundRecord>();
            if (recordFound.Count > 0)
            {
                //check if any of the wound record that has been found has the same version name in the list
                foreach (var woundRecord in recordFound)
                {
                    string selectAnnotationSql = @"SELECT i.img_file as annotationimagefile, im.img_file as maskimagefile, annotation_id as annotationid
                                                   FROM annotation an
                                                   INNER JOIN image i ON an.annotation_image_id = i.image_id
                                                   INNER JOIN image im ON an.mask_image_id = im.image_id
                                                   INNER JOIN wound w ON an.wound_id = w.wound_id
                                                   WHERE w.wound_id={0} AND w.version_id={1}";
                    List<AnnotationMaskImage> annotationMaskImageList = DBUtl.GetList<AnnotationMaskImage>(selectAnnotationSql, woundRecord.woundid, woundRecord.versionid);

                    if (woundRecordList.Any(x => x.versionname.Equals(woundRecord.versionname, StringComparison.OrdinalIgnoreCase)))
                    {
                        //get the wound record that has the same version name and add in the additional annotation/mask image
                        var sameVersionNameWR = woundRecordList.FirstOrDefault(x => x.versionname.Equals(woundRecord.versionname, StringComparison.OrdinalIgnoreCase));
                        sameVersionNameWR.annotationMaskImage.AddRange(annotationMaskImageList);
                    }
                    else
                    {
                        woundRecord.annotationMaskImage = new List<AnnotationMaskImage>();
                        woundRecord.annotationMaskImage.AddRange(annotationMaskImageList);
                        woundRecordList.Add(woundRecord);
                    }
                }
                //set version data dropdown list
                SetVersionViewData();
                //assign value to properties and pass to view
                var woundDetailsViewModel = new WoundDetailsViewModel()
                {
                    woundRecordList = woundRecordList,
                    woundRecord = recordFound[0]
                };
                return View(woundDetailsViewModel);
            }
            else
            {
                TempData["Msg"] = "Wound record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region TheWounds()
        public IActionResult TheWounds()
        {
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

            List<WoundRecord> list = DBUtl.GetList<WoundRecord>(@"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                                                  wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename,
                                                                  v.name as versionname, i.img_file as imagefile, i.image_id as imageid, u.username
                                                                  FROM wound w
                                                                  INNER JOIN image i ON i.image_id = w.image_id
                                                                  INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                                                  INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                                                  INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                                                  INNER JOIN version v ON v.version_id = w.version_id
                                                                  INNER JOIN useracc u ON u.user_id = w.user_id");
            return View(list);
        }
        #endregion

        #region ZoomImage()
        public IActionResult ZoomImage(int id)
        {
            var wound = DBUtl.GetList<Wound>($"SELECT * FROM wound WHERE wound_id={id}")[0];
            string selectWoundSql = @"SELECT w.wound_id as woundid, w.name as woundname, w.wound_stage as woundstage, w.remarks as woundremarks, 
                                      wc.name as woundcategoryname, wl.name as woundlocationname, t.name as tissuename, 
                                      v.name as versionname, v.version_id as versionid, i.img_file as imagefile, i.image_id as imageid
                                      FROM wound w
                                      INNER JOIN image i ON i.image_id = w.image_id
                                      INNER JOIN wound_category wc ON wc.wound_category_id = w.wound_category_id
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                      INNER JOIN tissue t ON t.tissue_id = w.tissue_id
                                      INNER JOIN version v ON v.version_id = w.version_id
                                      WHERE w.name='{0}'";
            List<WoundRecord> recordFound = DBUtl.GetList<WoundRecord>(selectWoundSql, wound.name);
            return View(recordFound);
        }
        #endregion

        #region SetWoundCategoryViewData()
        public void SetWoundCategoryViewData()
        {
            var getWoundCategory = DBUtl.GetList<WoundCategory>("SELECT * FROM wound_category");
            List<SelectListItem> wcSelectList = new List<SelectListItem>();
            foreach (var wc in getWoundCategory)
            {
                wcSelectList.Add(new SelectListItem()
                {
                    Text = wc.name,
                    Value = wc.name
                });
            }
            ViewData["woundCategory"] = new SelectList(wcSelectList, "Text", "Value");
        }
        #endregion

        #region SetTissueViewData()
        public void SetTissueViewData()
        {
            var getTissue = DBUtl.GetList<Tissue>("SELECT * FROM tissue");
            List<SelectListItem> tSelectList = new List<SelectListItem>();
            foreach (var t in getTissue)
            {
                tSelectList.Add(new SelectListItem()
                {
                    Text = t.name,
                    Value = t.name
                });
            }
            ViewData["tissue"] = new SelectList(tSelectList, "Text", "Value");
        }
        #endregion

        #region SetVersionViewData()
        public void SetVersionViewData()
        {
            var getVersion = DBUtl.GetList<WVersion>("SELECT * FROM version");
            List<SelectListItem> versionsSelectList = new List<SelectListItem>();
            foreach (var version in getVersion)
            {
                versionsSelectList.Add(new SelectListItem()
                {
                    Text = version.name,
                    Value = version.name
                });
            }
            ViewData["version"] = new SelectList(versionsSelectList, "Text", "Value");
        }
        #endregion

        #region Create()
        public IActionResult Create()
        {
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LoginPage", "Account", new { returnUrl = "/Wound/Create" });
            }
            //set wound category, tissue, version data dropdown list
            SetWoundCategoryViewData();
            SetVersionViewData();
            SetTissueViewData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CombineClass cc)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LoginPage", "Account", new { returnUrl = "/Wound/Create" });
            }

            //set wound category, tissue, version data dropdown list
            SetWoundCategoryViewData();
            SetVersionViewData();
            SetTissueViewData();
            if (!ModelState.IsValid)
            {
                ViewData["Msg"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Create");
            }
            else
            {
                //check wound name not exist
                var wound = DBUtl.GetList<Wound>($"SELECT * FROM wound");
                if (wound.Any(x => x.name.Equals(cc.wound.name, StringComparison.OrdinalIgnoreCase)))
                {
                    TempData["Msg"] = "Wound name already exist. Try giving unique name";
                    TempData["MsgType"] = "warning";
                    return View("Create");
                }

                //useracc table
                var userDetail = DBUtl.GetList<User>("SELECT * FROM useracc WHERE username = '" + User.Identity.Name + "'")[0];

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
                WoundCategory wc = DBUtl.GetList<WoundCategory>($"SELECT wound_category_id FROM wound_category WHERE name='{cc.woundc.name}'")[0];

                //wound_location table
                string wLSql = @"INSERT INTO wound_location(name)
                                 VALUES('{0}')";
                int wLRowsAffected = DBUtl.ExecSQL(wLSql, cc.woundl.name);
                WoundLocation wl = DBUtl.GetList<WoundLocation>("SELECT wound_location_id FROM wound_location ORDER BY wound_location_id DESC")[0];

                //tissue table
                Tissue t = DBUtl.GetList<Tissue>($"SELECT tissue_id FROM Tissue WHERE name='{cc.tissue.name}'")[0];

                //version table
                WVersion v = DBUtl.GetList<WVersion>($"SELECT version_id FROM Version WHERE name='{cc.woundv.name}'")[0];

                //wound table 
                string wSql = @"INSERT INTO wound(name, wound_stage, remarks, 
                                                  wound_category_id, wound_location_id, tissue_id, version_id, image_id, user_id)
                                VALUES('{0}','{1}','{2}',{3},{4},{5},{6},{7},{8})";
                int wRowsAffected = DBUtl.ExecSQL(wSql, cc.wound.name, cc.wound.wound_stage, cc.wound.remarks,
                                                        wc.wound_category_id, wl.wound_location_id, t.tissue_id, 
                                                        v.version_id, img.image_id, userDetail.user_id);
                Wound w = DBUtl.GetList<Wound>("SELECT wound_id FROM wound ORDER BY wound_id DESC")[0];

                //annotation table
                int anRowsAffected = 0;
                int imgCount = 0;
                if (anImg.Count == maskImg.Count)
                {
                    anImg.ForEach(img =>
                    {
                        string anSql = @"INSERT INTO annotation(mask_image_id, wound_id, annotation_image_id, user_id)
                                         VALUES({0},{1},{2},{3})";
                        anRowsAffected = DBUtl.ExecSQL(anSql, maskImg[imgCount].image_id, w.wound_id, img.image_id, userDetail.user_id);
                        imgCount += 1;
                    });
                }

                if (imageRowsAffected == 1 &&
                    animageRowsAffected == 1 &&
                    maskimageRowsAffected == 1 &&
                    wLRowsAffected == 1 &&
                    wRowsAffected == 1 &&
                    anRowsAffected == 1)
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

        #region DeleteAnnotationMaskImage()
        public IActionResult DeleteAnnotationMaskImage(int woundid, int annotationid)
        {       
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

            var getAnnotation = DBUtl.GetList<Annotation>($"SELECT * FROM annotation WHERE annotation_id={annotationid}")[0];

            string annotationSql = "DELETE FROM annotation WHERE annotation_id={0}";
            int annotationRowsAffected = DBUtl.ExecSQL(annotationSql, annotationid);

            string maskImgSql = "DELETE FROM image WHERE image_id={0}";
            int maskImgRowsAffected = DBUtl.ExecSQL(maskImgSql, getAnnotation.mask_image_id);

            string annotationImgSql = "DELETE FROM image WHERE image_id={0}";
            int annotationImgRowsAffected = DBUtl.ExecSQL(annotationImgSql, getAnnotation.annotation_image_id);

            if (maskImgRowsAffected == 1 && 
                annotationImgRowsAffected == 1 && 
                annotationRowsAffected == 1)
            {
                TempData["Msg"] = "Annotation and Mask image deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Details", new { id = woundid });
        }
        #endregion

        #region Delete()
        public IActionResult Delete(int id)
        {
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

            string deletewoundandannotationSQL = "DELETE FROM annotation WHERE wound_id={0} DELETE FROM wound WHERE wound_id={0}";
            if (DBUtl.ExecSQL(deletewoundandannotationSQL,id) == 1)
            {
                TempData["Msg"] = "Wound record deleted!";
                TempData["MsgType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Msg"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region UpdateAnnotationMaskImage()
        [HttpPost]
        public IActionResult UpdateAnnotationMaskImage(WoundDetailsViewModel details)
        {
            var wr = details.woundRecord;
            if (!ModelState.IsValid)
            {
                ViewData["Msg"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return RedirectToAction("Details", new { id = wr.woundid });
            }
            else
            {
                //useracc table
                var userDetail = DBUtl.GetList<User>("SELECT * FROM useracc WHERE username = '" + User.Identity.Name + "'")[0];

                var version = DBUtl.GetList<WVersion>($"SELECT * FROM version WHERE name='{wr.versionname}'")[0];
                var woundList = DBUtl.GetList<Wound>($"SELECT * FROM wound WHERE wound_id={wr.woundid} AND version_id={version.version_id}");
                var wound = new Wound();
                if (woundList.Any())
                {
                    wound = woundList.FirstOrDefault();
                }
                else
                {
                    var getWound = DBUtl.GetList<Wound>($"SELECT * FROM wound WHERE wound_id={wr.woundid}")[0];
                    //wound table 
                    string wSql = @"INSERT INTO wound(name, wound_stage, remarks, 
                                                      wound_category_id, wound_location_id, tissue_id, version_id, image_id, user_id)
                                    VALUES('{0}','{1}','{2}',{3},{4},{5},{6},{7},{8})";
                    int wRowsAffected = DBUtl.ExecSQL(wSql, getWound.name, getWound.wound_stage, getWound.remarks,
                                                            getWound.wound_category_id, getWound.wound_location_id, getWound.tissue_id, 
                                                            version.version_id, getWound.image_id, userDetail.user_id);
                    wound = DBUtl.GetList<Wound>("SELECT * FROM wound ORDER BY wound_id DESC")[0];
                }
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
                        string anSql = @"INSERT INTO annotation(mask_image_id, wound_id, annotation_image_id, user_id)
                                         VALUES({0},{1},{2},{3})";
                        anRowsAffected = DBUtl.ExecSQL(anSql, maskImg[imgCount].image_id, wound.wound_id, img.image_id, userDetail.user_id);
                        imgCount += 1;
                    });
                }

                if (animageRowsAffected == 1 && 
                    maskimageRowsAffected == 1 && 
                    anRowsAffected == 1)
                {
                    TempData["Msg"] = "Annotation and Mask images added successfully";
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
            #region checkuserrole
            int checktheuserrole = 0;
            if (User.IsInRole("Admin"))
            {
                checktheuserrole = 1;
            }
            else if (User.IsInRole("Doctor"))
            {
                checktheuserrole = 2;
            }
            else if (User.IsInRole("Annotator"))
            {
                checktheuserrole = 3;
            }
            if (checktheuserrole == 0)
            {
                return View("~/Views/Account/Forbidden.cshtml");
            }
            #endregion

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
                var img = DBUtl.GetList<Image>($"SELECT img_file FROM image WHERE image_id={woundRecord.imageid}")[0];
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
                //set wound category, tissue, version data dropdown list
                SetWoundCategoryViewData();
                SetVersionViewData();
                SetTissueViewData();
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
                //wound_category table
                WoundCategory wc = DBUtl.GetList<WoundCategory>($"SELECT wound_category_id FROM wound_category WHERE name='{cc.woundc.name}'")[0];

                //wound_location table
                string wLSql = @"UPDATE wound_location
                                 SET name='{0}'
                                 WHERE wound_location_id={1}";
                int wLRowsAffected = DBUtl.ExecSQL(wLSql, cc.woundl.name, cc.woundl.wound_location_id);
                WoundLocation wl = DBUtl.GetList<WoundLocation>($"SELECT wound_location_id FROM wound_location WHERE name='{cc.woundl.name}'")[0];

                //tissue table
                Tissue t = DBUtl.GetList<Tissue>($"SELECT tissue_id FROM Tissue WHERE name='{cc.tissue.name}'")[0];

                //version table
                WVersion v = DBUtl.GetList<WVersion>($"SELECT version_id FROM Version WHERE name='{cc.woundv.name}'")[0];

                //wound table
                string wSql = @"UPDATE wound
                                SET name='{0}', wound_stage='{1}', remarks='{2}', wound_category_id={3}, wound_location_id={4}, tissue_id={5}, version_id={6}
                                WHERE wound_id={7}";
                int wRowsAffected = DBUtl.ExecSQL(wSql, cc.wound.name, cc.wound.wound_stage, cc.wound.remarks, wc.wound_category_id, wl.wound_location_id, t.tissue_id, v.version_id, cc.wound.wound_id);

                if (wRowsAffected == 1 && 
                    wLRowsAffected == 1)
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
