using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatOnline.Models
{
    public class Message_
    {
        public int UserID { get; set; }
        public int AdminID { get; set; }
        public string MSG { get; set; }
        public string Time_ { get; set; }
        public string Date_ { get; set; }
        public int Type_{ get; set; }
    }
}