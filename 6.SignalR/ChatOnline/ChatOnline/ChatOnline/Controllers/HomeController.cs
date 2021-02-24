using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatOnline.Models;
using Newtonsoft.Json;

namespace ChatOnline.Controllers
{
    [RoutePrefix("HomePanel")]
    public class HomeController : Controller
    {
        ChatOnlineEntities2 db = new ChatOnlineEntities2();
        // GET: Home
        [Route()]
        [ActionName("index")]
        public ActionResult Index()
        {
            ViewBag.error = TempData["Message"];
            return View();
        }

        
        [HttpPost]
        [Route("input_")]
        public ActionResult input_(Login_model lgn)// chera vaghti neveshte boodam FormCollection kar nakard?
        {

          var info = (from x in db.User_Info where (x.username_ == lgn.username) && (x.password_ == lgn.password) select x).FirstOrDefault();


            if (info != null)
            {
                var access = (from x in db.User_Access where (x.TokenID == info.TokenID)  select x).FirstOrDefault();

                Session["TokenID"] = info.TokenID.ToString();

                Session.Timeout = 60;
                //We must pass Tokenparameter to actionResult
                if (access.Access==true)
                {
                    return RedirectToAction("profile_Admin", new { TokenID = info.TokenID.ToString() });
                }
                if (access.Access == false)
                {
                    return RedirectToAction("profile_user", new { TokenID = info.TokenID.ToString() });
                }
              
                
            }
            else
            {

                TempData["Message"] = "یوزرنیم یا پسورد اشتباه است";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Route("profile_Admin")]
        public ActionResult profile_Admin(string TokenID)
        {
            //List of users 
            var users = (from x in db.User_Info join
                         y in db.User_Access on x.TokenID equals y.TokenID
                         where (y.Access==false )
                         select x ).ToList();


            TempData["TokenID"] = TokenID;
            return View(users);
      


        }

        [Route("profile_user")]
        public ActionResult profile_user(string TokenID)
        {
            var info = (from x in db.User_Info where (x.TokenID == TokenID) select x).FirstOrDefault();
            var Messages_ = (from x in db.Messages where (x.UserID == info.U_ID) select x).ToList();
           
            TempData["TokenID"] = TokenID;
            if (!Messages_.Equals(null))
                return View(Messages_);
            else
                return View();



        }
        [HttpPost]
        [Route("GetMessages")]
        public string GetMessages(string TokenID)
        {
          
            var info = (from a in db.User_Info where (a.TokenID==TokenID) select a ).FirstOrDefault();
            var messages = (from x in db.Messages where (x.UserID == info.U_ID) select x).ToList();
            string jsonString = JsonConvert.SerializeObject(messages);
            return (jsonString);
        }


    }
}