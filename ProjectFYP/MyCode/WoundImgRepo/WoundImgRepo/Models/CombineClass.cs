using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Image annotationimage { get; set; }
        public Image maskimage { get; set; }
    }

    //refer to columns from selectSql table created
    public class CombineClass2
    {
        public string woundname { get; set; }
        public string woundcategoryname { get; set; }
        public string woundlocation { get; set; }
        public string woundversionname { get; set; }
        public string tissuename { get; set; }
        public string imagefile { get; set; }
        public int imageid { get; set; }
        public List<AnnotationImage> AnnotationImages { get; set; }
    }
}
