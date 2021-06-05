using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WoundImgRepo.Models
{
    public class Wound
    {
        public int id { get; set; }
        public string name { get; set; }
        public string wound_stage { get; set; }
        public string remarks { get; set; }
        public int wound_category_id { get; set; }
        public int wound_location_id { get; set; }
        public int tissue_id { get; set; }
        public int version_id { get; set; }
        public int image_id { get; set; }
        public int user_id { get; set; }
    }
}
