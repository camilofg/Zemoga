using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZemogaDevTest.Models
{
    public class Picture
    {
        public HttpPostedFileBase file { get; set; }
    }
}