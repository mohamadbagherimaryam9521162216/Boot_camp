using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test3.Models;

namespace test3.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        [Route()]
        [ActionName("index")]
        public ActionResult Index()
        {
            ViewData["Message"] = "Login ";
            return (View());
        }

        my_db_users db1 = new my_db_users();

        [HttpPost]
        [Route("input_")]
        public ActionResult input_(login_1 lgn)// chera vaghti neveshte boodam FormCollection kar nakard?
        {
            //  string username = Convert.ToString(formCollection.GetValue("username"));
            //   string password = Convert.ToString(formCollection.GetValue("password"));

            //      username = Server.HtmlEncode(username);
            //    password = Server.HtmlEncode(password);

            //   string us = "maryam";
            string UserName = lgn.username;
            string password = lgn.password;

            var sth = (from x in db1.login_1 where (x.username == UserName) && (x.password == password) select x).FirstOrDefault();


            if (sth != null)
            {
                //Session["username"] = sth.username.ToString();
                //Session["password"] = sth.password.ToString();
                Session["TokenID"] = sth.TokenID.ToString();
                Session.Timeout = 60;
                return RedirectToAction("profile");
            }
            else
            {
                string error = "username or password is wrong";

                return (Content(error));
            }





            //  return RedirectToAction("Index");



        }

        [Route("profile")]
        public ActionResult profile()
        {
            if (Session["username"] != null && Session["password"] != null)
            {

                return (View());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [Route("view_students")]
        public ActionResult view_students()
        {
            var sth = (from s in db1.students select s).ToList();

            return View(sth);

        }


        [Route("add_students")]
        public ActionResult add_students()
        {


            return View();

        }



        [Route("add_")]
        public ActionResult add_(int StdId, string FullName, string Field)
		{          
               

                student st = new student();
                st.StdID = StdId;
                st.Rtime = DateTime.Now;
                st.name = FullName;
                st.Field = Field;
                db1.students.Add(st);
                db1.SaveChanges();
                return RedirectToAction("profile");


         


        }
        [Route("remove/{id}")]
        public ActionResult remove(int id)
        {
            var sth = (from s in db1.students where s.ID == id select s).FirstOrDefault();
            db1.students.Remove(sth);
            db1.SaveChanges();
            return RedirectToAction("view_students");
        }


        [Route("edit/{id}")]
        public ActionResult edit(int id)
        {
            var sth = (from s in db1.students where s.ID == id select s).FirstOrDefault();
            db1.students.Remove(sth);
            db1.SaveChanges();
            return View(sth);


        }
        [Route("exit")]
        public ActionResult exit()
        {
            Session.Clear();
            return RedirectToAction("index");


        }

    }
}