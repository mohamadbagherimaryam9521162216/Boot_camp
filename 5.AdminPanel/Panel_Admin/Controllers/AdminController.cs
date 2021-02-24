using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft;
using System.Web.Script.Serialization;
using Panel_Admin.Models;
using Panel_Admin.Models.Classes;

namespace Panel_Admin.Controllers
{
    public class AdminController : Controller
    {
        DrZinoV2Entities db = new DrZinoV2Entities();

            [Route("MainPanel")]
            public ActionResult MainPanel()
            {

            var DiseaseInfo = (from a in db.Diseases select a).ToList();
            ViewBag.Diseases = DiseaseInfo;// pass Diseases' IDs to View
            return View();
            }


        [HttpPost]
        [Route("GetQuestion")]
        public string GetQuestion(int DiseaseID)
        {
            var DisaseQuestions = (from a in db.DisaseQuestions where a.DiseaseID == DiseaseID select a).ToList();

            List<Question> OurQuestions = new List<Question>();
            foreach (var item in DisaseQuestions)
            {
                Question Ques = (from a in db.Questions where a.ID == item.QuestionID select a).FirstOrDefault();
                OurQuestions.Add(Ques);
            }

            string jsonString = JsonConvert.SerializeObject(OurQuestions);
            return (jsonString);

        }



        [HttpPost]
        [Route("Edit")]
        public void Edit(string jsonstring, string DisID)
        {

            var QustionList = JsonConvert.DeserializeObject<Root>(jsonstring);
            var IDs = (from s in db.Questions select s).ToList();

            foreach (var item in QustionList.questions)
            {
                string x = item.QID;
                int status = x.IndexOf("NEW");//If it has NEW it's a new Question otherwise it just has to be edited in the table
                if (status != -1)//NEW question
                {
                    Question qt = new Question();
                    qt.Context = item.Context;
                    db.Questions.Add(qt);
                    db.SaveChanges();

                    //Findout what is ID of Question which is stored in Question Table
                    var result = (from s in db.Questions
                                  where item.Context == s.Context
                                  select s).FirstOrDefault();

                    var QID = result.ID;//Now we have QID
                                        //Get Disease ID correctly
                    string DiseID = DisID;
                    DiseID = DiseID.Replace("Dis_", "");

                    DisaseQuestion DQ = new DisaseQuestion();
                    DQ.DiseaseID = Int32.Parse(DiseID.Trim());
                    DQ.QuestionID = QID;
                    db.DisaseQuestions.Add(DQ);
                    db.SaveChanges();


                }

                else
                //An editable question
                {
                    int Q_ID;
                    Q_ID = Int32.Parse(item.QID.Trim());//convert to int
                    var result = (from p in db.Questions where Q_ID == p.ID select p).FirstOrDefault();
                    if (result != null)//when ID exist in table
                    {
                        result.Context = item.Context;
                        db.SaveChanges();
                    }
                }
            }
            RedirectToAction("MainPanel");
        }




    }
}