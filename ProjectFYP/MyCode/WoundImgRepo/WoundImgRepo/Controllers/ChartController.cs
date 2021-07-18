﻿using System;
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
            List<Wound> list = DBUtl.GetList<Wound>("SELECT * FROM wound");
            List<WoundLocation> locations = DBUtl.GetList<WoundLocation>("SELECT * FROM wound_location");
            int[] locationId = new int[locations.Count];
            for (int i = 0; i < locationId.Length; i++)
            {
                locationId[i] = locations[i].wound_location_id;
            }
            int totalLeg = 0;
            int totalArm = 0;
            foreach (Wound w in list)
            {
                if (w.wound_location_id.Equals(locationId[0]))
                {
                    totalLeg++;
                }
                else if (w.wound_location_id.Equals(locationId[1]))
                {
                    totalArm++;
                }
            }
            if (totalLeg > totalArm)
            {
                ViewData["Maximum"] = "Foot";
            }
            else
            {
                ViewData["Maximum"] = "Buttock";
            }
            PrepareWoundsData(1);
            ViewData["Chart"] = "bar";
            ViewData["Title"] = "Location";
            ViewData["ShowLegend"] = "false";
            WoundCategoryRecords(1);
            ViewData["CatChart"] = "pie";
            ViewData["CatTitle"] = "Category";
            ViewData["CatShowLegend"] = "false";
            DisplayUsers(1);
            ViewData["UserChart"] = "bar";
            ViewData["UserTitle"] = "Category";
            ViewData["UserShowLegend"] = "false";
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
            int[] version = new int[3] { 0, 0, 0 };
            List<WVersion> list = DBUtl.GetList<WVersion>("SELECT * FROM version");
            foreach (WVersion wversion in list)
            {
                version[calculatePosition(wversion.version_id)]++;
                /*
                if (wversion.version_id == 1) version[0]++;
                else if (wversion.version_id == 2) version[1]++;
                else version[2]++;
                */
            }

            if (x == 1)
            {
                ViewData["Legend"] = "Wound Version";
                ViewData["Colors"] = new string[3] { "grey", "brown", "black" };
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
                ViewData["Colors"] = colors;
                ViewData["Labels"] = labels;
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
    }
}
