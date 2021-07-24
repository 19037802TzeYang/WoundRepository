using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoundImgRepo.Models;

namespace WoundImgRepo.Controllers
{
    public class ChartController : Controller
    {

        
        public IActionResult Version()
        {
            PrepareData(1);
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Version";
            ViewData["ShowLegend"] = "true";
            return View("Summary");

        }
        public IActionResult Dashboard()
        {
            List<WoundRecord> list = DBUtl.GetList<WoundRecord>(@"SELECT TOP 1 wl.name as woundlocationname
                                      FROM wound w
                                      
                                      INNER JOIN wound_location wl ON wl.wound_location_id = w.wound_location_id
                                    GROUP BY wl.name HAVING Count(wl.name) >=1");
            
            ViewData["Maximum"] = list[0].woundlocationname;
            List<WoundRecord> Users = DBUtl.GetList<WoundRecord>(@"SELECT TOP 1  u.username
                                      FROM wound w 
                                        INNER JOIN useracc u ON u.user_id = w.user_id 
                                    GROUP BY u.username HAVING Count(u.username) >=1");
            
            ViewData["MaximumUsers"] = Users[0].username;
            List<Wound> total = DBUtl.GetList<Wound>("SELECT * FROM wound");
            ViewData["TotalRecords"] = total.Count();
            PrepareData(1);
            ViewData["VersionChart"] = "line";
            ViewData["VersionTitle"] = "Version";
            ViewData["VersionShowLegend"] = "false";
            PrepareWoundsData(1);
            ViewData["Chart"] = "bar";
            ViewData["Title"] = "Location";
            ViewData["ShowLegend"] = "false";
            WoundCategoryRecords(1);
            ViewData["CatChart"] = "pie";
            ViewData["CatTitle"] = "Category";
            ViewData["CatShowLegend"] = "true";
            DisplayUsers(1);
            ViewData["UserChart"] = "bar";
            ViewData["UserTitle"] = "User Roles";
            ViewData["UserShowLegend"] = "false";
            locateTissues(1);
            ViewData["TissueChart"] = "bar";
            ViewData["TissueTitle"] = "Tissues";
            ViewData["TissueShowLegend"] = "false";
            return View();
        }
        public IActionResult WoundsLocationRecords()
        {
            PrepareWoundsData(1);
            ViewData["Chart"] = "bar";
            ViewData["Title"] = "Location";
            ViewData["ShowLegend"] = "false";
            return View("WoundsLocationRecords");
        }
        public IActionResult DisplayCategories()
        {
            WoundCategoryRecords(1);
            ViewData["Chart"] = "pie";
            ViewData["Title"]="Category";
            ViewData["ShowLegend"] = "true";
            return View("DisplayCategories");
        }
        private void PrepareData(int x)
        {
            
            List<WVersion> list = DBUtl.GetList<WVersion>("SELECT * FROM version");
            List<WoundRecord> wrList = DBUtl.GetList<WoundRecord>("SELECT * FROM wound");
            int[] version = new int[list.Count];
            for (int i = 0; i < version.Length; i++)
            {
                version[i] = 0;
            }
            foreach (WoundRecord wversion in wrList)
            {
                version[calculatePosition(wversion.versionid)]++;
                /*
                if (wversion.version_id == 1) version[0]++;
                else if (wversion.version_id == 2) version[1]++;
                else version[2]++;
                */
            }

            if (x == 1)
            {
                ViewData["VersionLegend"] = "Wound Version";
                ViewData["VersionColors"] = new string[3] { "grey", "brown", "black" };
                string[] colors = new string[version.Length];
                string[] labels = new string[version.Length];
                for (int i = 0; i < colors.Length; i++)
                {

                    colors[i] = colors[i];

                }
                for (int i = 0; i < list.Count; i++)
                {

                    labels[i] = list[i].name;
                }
                ViewData["VersionColors"] = colors;
                ViewData["VersionLabels"] = labels;
                ViewData["VersionData"] = version;
            }

            else
            {
                ViewData["VersionLegend"] = "Nothing";
                ViewData["VersionColors"] = new[] { "gray", "darkgray", "black" };
                ViewData["VersionLabels"] = new[] { "X", "Y", "Z" };
                ViewData["VersionData"] = new int[] { 1, 2, 3 };
            }
        }
        private void PrepareWoundsData(int x)
        {
            
            
            List<Wound> list = DBUtl.GetList<Wound>("SELECT * FROM wound");
            List<WoundLocation> wLocation = DBUtl.GetList<WoundLocation>("SELECT * FROM wound_location");
            int[] wounds = new int[wLocation.Count];
            foreach (Wound w in list)
            {
                wounds[calculatePosition(w.wound_location_id)]++;
            }

            if (x == 1)
            {
                ViewData["Legend"] = "Wounds";
                string[] knownColors = new string[3] { "grey", "brown", "black" };
                string[] colors = new string[wounds.Length];
                string[] labels = new string[wounds.Length];
                for (int i = 0; i < colors.Length; i++)
                {

                    colors[i] = knownColors[i];
                    
                }
                for (int i = 0; i < wLocation.Count; i++)
                {
                    
                    labels[i] = wLocation[i].name;
                }
                ViewData["Colors"] = colors ;
                ViewData["Labels"] = labels;
                ViewData["Data"] = wounds;
            }

            else
            {
                ViewData["Legend"] = "Nothing";
                ViewData["Colors"] = new[] { "gray", "darkgray", "black" };
                ViewData["Labels"] = new[] { "X", "Y", "Z" };
                ViewData["Data"] = new int[] { 1, 2, 3 };
            }
        }

        private void WoundCategoryRecords(int x)
        {
            int[] categories = new int[] { 0, 0, 0 };
            List<Wound> list = DBUtl.GetList<Wound>("SELECT * FROM wound");
            List<WoundCategory> categoryList = DBUtl.GetList<WoundCategory>("SELECT * FROM wound_category");

            foreach (Wound w in list)
            {
                categories[calculatePosition(w.wound_category_id)]++;

            }
            if (x == 1)
            {
                ViewData["CatLegend"] = "Wounds";
                string[] knownColors = new string[3] { "grey", "brown", "black" };
                string[] colors = new string[categories.Length];
                string[] labels = new string[categories.Length];
                for (int i = 0; i < colors.Length; i++)
                {

                    colors[i] = knownColors[i];

                }
                for (int i = 0; i < categoryList.Count; i++)
                {

                    labels[i] = categoryList[i].name;
                }
                ViewData["CatColors"] = colors;
                ViewData["CatLabels"] = labels;
                ViewData["CatData"] = categories;
            }
        }

        private void DisplayUsers(int x)
        {
            List<User> list = DBUtl.GetList<User>("SELECT * FROM useracc");
            int[] users = new int[3] { 0, 0, 0 };
            foreach (User u in list)
            {
                users[findRole(u.user_role)]++;
            }
            if (x == 1)
            {
                string[] colors = new string[3] { "red", "blue", "yellow" };
                string[] roles = new string[3] { "Doctor", "Annotator", "Admin" };
                ViewData["UserLegend"] = "Users";
                ViewData["UserColors"] = colors;
                ViewData["UserLabels"] = roles;
                ViewData["UserData"] = users;
            }
        }

        private void locateTissues(int x)
        {
            List<Tissue> tissues = DBUtl.GetList<Tissue>("SELECT * FROM tissue");
            List<Wound> wounds = DBUtl.GetList<Wound>("SELECT * FROM wound");
            int[] tissuenames = new int[tissues.Count];
            string[] knowncolors = new string[20] { "red", "blue", "yellow", "green", "orange", "black", "purple", "brown", "violet", "crimson", "grey", "maroon", "olive", "chocolate", "aquamarine", "Turquoise", "#437C17", "#ffdf00", "#614051", "#F70D1A" };

            for (int i = 0; i < tissues.Count; i++)
            {
                tissuenames[i] = 0;
            }
            foreach (Wound w in wounds)
            {
                tissuenames[findTissue(w.tissue_id)]++;
            }
            if (x == 1)
            {
                string[] colors =new string[tissuenames.Length];
                string[] labels = new string[tissuenames.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = knowncolors[i];
                }
                for (int i = 0; i < tissues.Count; i++)
                {
                    labels[i] = tissues[i].name;
                }
                
                ViewData["TissueColors"]=colors;
                ViewData["TissueLabels"]=labels;
                ViewData["TissueData"]=tissuenames;
            }
        }
        private int calculatePosition(int x)
        {
            if (x == 1) return 0;
            else if (x == 2) return 1;
            else return 2;
        }
        private int findRole(string role)
        {
            if (role.Equals("Doctor")) return 0;
            else if (role.Equals("Annotator")) return 1;
            else return 2;

        }
        private int findTissue(int id)
        {
            if (id == 1) return 0;
            else if (id == 2) return 1;
            else if (id == 3) return 2;
            else return 3;
        }
    }
}
