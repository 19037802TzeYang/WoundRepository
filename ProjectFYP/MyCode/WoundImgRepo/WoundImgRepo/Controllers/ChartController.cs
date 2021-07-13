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

        [Authorize(Roles = "Admin")]
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
            int[] version = new int[2] { 0, 0 }; 
            List<WVersion> list = DBUtl.GetList<WVersion>("SELECT * FROM version");
            foreach (WVersion wversion in list)
            {
                if (wversion.version_id == 1) version[0]++;
                else if (wversion.version_id == 2) version[1]++;
                else version[2]++;
            }

            if (x == 1)
            {
                ViewData["Legend"] = "Wound Version";
                ViewData["Colors"] = new[] { "violet", "orange" };
               //static
                ViewData["Labels"] = new[] { "Version 1.16", "Version 1.17" };
                ViewData["Data"] = version;
            }

            else
            {
                ViewData["Legend"] = "Nothing";
                ViewData["Colors"] = new[] { "gray", "darkgray", "black" };
                ViewData["Labels"] = new[] { "X", "Y", "Z" };
                ViewData["Data"] = new int[] { 1, 2, 3 };
            }
        }
        private void PrepareWoundsData(int x)
        {
            int[] wounds = new int[] { 0, 0, 0 };
            
            List<Wound> list = DBUtl.GetList<Wound>("SELECT * FROM wound");
            List<WoundLocation> wLocation = DBUtl.GetList<WoundLocation>("SELECT * FROM wound_location");
            
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
                ViewData["Legend"] = "Wounds";
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
                ViewData["Colors"] = colors;
                ViewData["Labels"] = labels;
                ViewData["Data"] = categories;
            }
        }
        private int calculatePosition(int x)
        {
            if (x == 1) return 0;
            else if (x == 2) return 1;
            else return 2;
        }
    }
}
