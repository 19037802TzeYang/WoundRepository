﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WoundImgRepo.Models
{
    public class CombineClass
    {
        public Wound wound { get; set; }
        public WoundCategory woundc { get; set; }
        public WoundLocation woundl { get; set; }
        public Tissue tissue { get; set; }
        public WVersion woundv { get; set; }
        public Image image { get; set; }
<<<<<<< HEAD
        public IFormFile annotationimage { get; set; }
        public IFormFile maskimage { get; set; }
=======
        [Required(ErrorMessage ="Annotation images are required!")]
        public List<IFormFile> annotationimages { get; set; }
        [Required(ErrorMessage = "Mask images are required!")]
        public List<IFormFile> maskimages { get; set; }
>>>>>>> b3429f8ced17bf6834f81844aeda92cb75b00086
    }

    //get/set data from database
    public class WoundRecord
    {
        public int woundid { get; set; }
        public string woundname { get; set; }
        public string woundstage { get; set; }
        public string woundremarks { get; set; }
        public string woundcategoryname { get; set; }
        public string woundlocationname { get; set; }
        public string versionname { get; set; }
        public string tissuename { get; set; }
        public string imagefile { get; set; }
        public int imageid { get; set; }
<<<<<<< HEAD
        public IFormFile annotationimage { get; set; }
        public IFormFile maskimage { get; set; }
=======

        public string createdBy { get; set; }
>>>>>>> b3429f8ced17bf6834f81844aeda92cb75b00086
        public List<AnnotationMaskImage> annotationMaskImage { get; set; }
    }
}
