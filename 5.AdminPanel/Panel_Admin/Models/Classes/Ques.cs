using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Panel_Admin.Models.Classes
{
    public class Question1
    {
        public string QID { get; set; }
        public string Context { get; set; }
    }


    public class Root
    {
        public Question1[] questions { get; set; }
    }
}