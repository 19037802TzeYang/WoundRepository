using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WoundImgRepo.Models
{
    public class AnnotationImage
    {
        public string annotationimagefile { get; set; }
        public string maskimagefile { get; set; }
    }
}
