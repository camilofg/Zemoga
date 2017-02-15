using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZemogaDevTest.Models
{
    public class skill
    {
        public skill() {

        }

        public int SkillID { get; set; }
        public string SkillDesc { get; set; }
        public bool SkillChecked { get; set; }
    }
    
}