using NewDrZinoLanding.Classes;
using NewDrZinoLanding.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewDrZinoLanding.Controllers
{
    [RoutePrefix("UserPanel")]
    public class UserPanelController : Controller
    {
        Models.DrZinoV2Entities db = new DrZinoV2Entities();
        // partial view

        [Route("_RightSidebar")]
        public PartialViewResult _RightSidebar()
        {
            return PartialView();
        }

        // GET: UserPanel

        [Route("register")]
        public ActionResult Register()
        {
            if (TempData["Step2"] != null)
            {
                ViewBag.Step2 = TempData["Step2"];
            }
            if (TempData["Step3"] != null)
            {
                ViewBag.Step3 = TempData["Step3"];
            }
            if (TempData["Errorphone"] != null)
            {
                ViewBag.ErrorPhone = TempData["ErrorPhone"];
            }
            if (TempData["ErrorPhoneCounter"] != null)
            {
                ViewBag.ErrorPhone = TempData["ErrorPhoneCounter"];
            }
            if (TempData["ErrorPhoneEmpty"] != null)
            {
                ViewBag.ErrorPhoneEmpty = TempData["ErrorPhoneEmpty"];
            }
            if (TempData["ErrorFullNameEmpty"] != null)
            {
                ViewBag.ErrorFullNameEmpty = TempData["ErrorFullNameEmpty"];
            }
            if (TempData["ErrorPassEmpty"] != null)
            {
                ViewBag.ErrorPassEmpty = TempData["ErrorPassEmpty"];
            }
            if (TempData["ErrorPasscount"] != null)
            {
                ViewBag.ErrorPasscount = TempData["ErrorPasscount"];
            }
            if (TempData["ErrorPass"] != null)
            {
                ViewBag.ErrorPass = TempData["ErrorPass"];
            }
            if (TempData["ErrorVerifyCodeIsIncorrect"] != null)
            {
                ViewBag.ErrorVerifyCodeIsIncorrect = TempData["ErrorVerifyCodeIsIncorrect"];
            }
            if (TempData["ErrorVerifyCodeIsEmpty"] != null)
            {
                ViewBag.ErrorVerifyCodeIsEmpty = TempData["ErrorVerifyCodeIsEmpty"];
            }
            if (TempData["PassNull"] != null)
            {
                ViewBag.PassNull = TempData["PassNull"];

            }
            if (TempData["PassLengthNull"] != null)
            {
                ViewBag.PassLengthNull = TempData["PassLengthNull"];

            }
            if (TempData["PassCkeck"] != null)
            {
                ViewBag.PassCkeck = TempData["PassCkeck"];

            }
            if (TempData["OpenModal"] != null)
            {
                ViewBag.OpenModal = TempData["OpenModal"];
            }
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("register")]
        //public ActionResult Register([Bind(Include = "FullName, PhoneNumber, HiddenPhoneNumber, Token, Password, passwordConfirm, VerifyCode, ReapPass")] User user)
        //{
        //    if (TempData["step"] == null)
        //    {
        //        var UserPhoneNumberExist = (from a in db.Users where a.PhoneNumber == user.PhoneNumber select a).SingleOrDefault();
        //        bool FormHasError = false;
        //        if (UserPhoneNumberExist != null)
        //        {
        //            FormHasError = true;
        //            TempData["ErrorPhone"] = ("شماره تلفن قبلا ثبت شده است");

        //        }
        //        if (!string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumber.Length < 11)
        //        {
        //            FormHasError = true;
        //            TempData["ErrorPhoneCounter"] = ("شماره تلفن وارد شده اشتباه است");

        //        }
        //        if (user.PhoneNumber == null)
        //        {
        //            FormHasError = true;
        //            TempData["ErrorPhoneEmpty"] = ("پر کردن این فیلد اجباری است.");

        //        }
        //        if (user.FullName == null)
        //        {
        //            FormHasError = true;
        //            TempData["ErrorFullNameEmpty"] = ("پر کردن این فیلد اجباری است.");

        //        }

        //        if (FormHasError)
        //        {
        //            return RedirectToAction("register", "UserPanel");
        //        }
        //        if (FormHasError == false)
        //        {
        //            var User = new User();
        //            Random nums = new Random();
        //            int VerifyCode = nums.Next(1000, 9999);
        //            user.VerifyCode = VerifyCode;
        //            User.FullName = user.FullName;
        //            User.PhoneNumber = user.PhoneNumber;
        //            string Token = Guid.NewGuid().ToString();
        //            user.Token = Token;
        //            db.Users.Add(user);
        //            db.SaveChanges();
        //            TempData["Step2"] = "var Box1 = document.getElementById('First'); Box1.style.display = 'none';var Box2 = document.getElementById('verify'); Box2.style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + user.PhoneNumber + "'";
        //            TempData["step"] = "1";
        //            return RedirectToAction("register", "UserPanel");
        //        }
        //    }
        //    else if (TempData["step"].ToString() == "1")
        //    {
        //        string PhoneNumber = user.HiddenPhoneNumber;
        //        var UserRow = (from a in db.Users where a.VerifyCode == user.VerifyCode && a.PhoneNumber == PhoneNumber select a).SingleOrDefault();

        //        if (user.VerifyCode == null)
        //        {
        //            TempData["Step2"] = "document.getElementById('First').style.display = 'none'; document.getElementById('verify').style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + PhoneNumber + "'";
        //            TempData["ErrorVerifyCodeIsEmpty"] = ("پر کردن فیلد اجباری است.");
        //            return RedirectToAction("register", "UserPanel");
        //        }
        //        if (UserRow != null)
        //        {
        //            TempData["Step2"] = "document.getElementById('First').style.display = 'none'; document.getElementById('verify').style.display = 'none'; document.getElementById('Password').style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + PhoneNumber + "'";
        //            TempData["step"] = "2";
        //            return RedirectToAction("register", "UserPanel");
        //        }
        //        if (UserRow == null)
        //        {
        //            TempData["Step2"] = "document.getElementById('First').style.display = 'none'; document.getElementById('verify').style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + PhoneNumber + "'";
        //            TempData["ErrorVerifyCodeIsIncorrect"] = ("کد وارد شده صحیح نمی باشد");
        //            return RedirectToAction("register", "UserPanel");
        //        }
        //    }
        //    else if (TempData["Step"].ToString() == "2")
        //    {
        //        string PhoneNumber = user.HiddenPhoneNumber;
        //        var UserRow = (from a in db.Users where a.PhoneNumber == PhoneNumber select a).SingleOrDefault();
        //        if (user.Password != user.Password)
        //        {

        //            TempData["Step2"] = "document.getElementById('First').style.display = 'none'; document.getElementById('verify').style.display = 'none'; document.getElementById('Password').style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + PhoneNumber + "'";
        //            TempData["ErrorPass"] = "تکرار رمز عبور اشتباه است";
        //            return RedirectToAction("register", "UserPanel");

        //        }
        //        else if (user.Password.Length < 8)
        //        {

        //            TempData["Step2"] = "document.getElementById('First').style.display = 'none'; document.getElementById('verify').style.display = 'none'; document.getElementById('Password').style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + PhoneNumber + "'";
        //            TempData["ErrorPassCount"] = "رمز وارد شده باید بیش از هشت کاراکتر باشد.";
        //            return RedirectToAction("register", "UserPanel");

        //        }
        //        else if (user.Password == null || user.ReapPass == null)
        //        {

        //            TempData["Step2"] = "document.getElementById('First').style.display = 'none'; document.getElementById('verify').style.display = 'none'; document.getElementById('Password').style.display = 'block'; document.getElementById('HiddenPhoneNumber').value='" + PhoneNumber + "'";
        //            TempData["ErrorPassEmpty"] = "پر کردن هر دو فیلد اجباری است.";
        //            return RedirectToAction("register", "UserPanel");

        //        }
        //        else
        //        {

        //            UserRow.Password = user.Password;
        //            user.FullName = UserRow.FullName;
        //            user.RTime = UserRow.RTime;
        //            user.Token = UserRow.Token;
        //            user.VerifyCode = UserRow.VerifyCode;
        //            db.SaveChanges();
        //            bool AllTrue = true;
        //            if (AllTrue)
        //            {

        //                TempData["OpenModal"] = "$('#Modal').modal('show');";
        //                return RedirectToAction("register", "UserPanel");

        //            }

        //            return RedirectToAction("Profile", "UserPanel");
        //        }
        //    }

        //    return View();
        //}

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("GetToLogin")]
        public ActionResult GetToLogin(User usr)
        {
            var obj = db.Users.Where(a => a.PhoneNumber.Equals(usr.PhoneNumber) && a.Password.Equals(usr.Password)).SingleOrDefault();
            if (obj != null)
            {
                Session["ID"] = obj.ID.ToString();
                Session["FullName"] = obj.FullName.ToString();
                Session["Password"] = obj.Password.ToString();
                Session["Token"] = obj.Token;
                Session.Timeout = 300;
                return RedirectToAction("profile", "UserPanel");
            }


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

        [Route("Profile")]
        public ActionResult profile()
        {
            if (Session["Token"] != null)
            {
                string Token = Session["Token"].ToString();
                var UserLogin = from a in db.Users where a.Token == Token select a;
                if (UserLogin == null)
                {
                    return RedirectToAction("Login", "UserPanel");
                }

            }
            else if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "UserPanel");

            }
            string token = Session["Token"].ToString();
            var User = (from a in db.Users where a.Token == token select a).SingleOrDefault();
            var UserEdit = (from a in db.UserEdits where a.UsersID == User.ID orderby a.RTime descending select a).FirstOrDefault();
            var DiseaseInfo = (from a in db.Diseases select a).ToList();

            var DiseaseQustion = (from a in db.DisaseQuestions select a).ToList();
            ViewBag.FullName = User.FullName;
            ViewBag.PhoneNumber = User.PhoneNumber;
            ViewBag.Token = User.Token;
            ViewBag.Diseases = DiseaseInfo;// pass Diseases' IDs to View
            if (UserEdit != null)
            {
                var SelectedDiseases = (from a in db.Answers
                                        join b in db.DisaseQuestions on a.DiseaseQuestionsID equals b.ID
                                        join c in db.Diseases on b.DiseaseID equals c.ID
                                        where a.UserID == User.ID && a.Token == UserEdit.Token
                                        select c).ToList().Distinct().ToList();
                string SelectedDiseasesStr = "";
                foreach (var item in SelectedDiseases)
                {
                    SelectedDiseasesStr = SelectedDiseasesStr + "document.getElementById(\"Dis_" + item.ID + "\").checked = true;\n";
                }
                ViewBag.SelectedDiseases = SelectedDiseasesStr;

                Dictionary<Question, string> QuestionAnswers = new Dictionary<Question, string>();
                Dictionary<string, string> QuestionAnswersForJSON = new Dictionary<string, string>();
                var DiseaseAnswers = (from a in db.Answers
                                      join b in db.DisaseQuestions on a.DiseaseQuestionsID equals b.ID
                                      join c in db.Questions on b.QuestionID equals c.ID
                                      where a.UserID == User.ID && a.Token == UserEdit.Token
                                      select c).ToList();
                foreach (var item in DiseaseAnswers)
                {
                    var Answers = (from a in db.Answers
                                   join b in db.DisaseQuestions on a.DiseaseQuestionsID equals b.ID
                                   join c in db.Questions on b.QuestionID equals c.ID
                                   where c.ID == item.ID && a.UserID == User.ID && a.Token == UserEdit.Token
                                   select a).ToList();
                    foreach (var item1 in Answers)
                    {
                        QuestionAnswers.Add(item, item1.AnswerText);
                        QuestionAnswersForJSON.Add("Answer_" + item.ID.ToString(), item1.AnswerText);
                    }
                }
                string DiseaseAnswersStr = "";
                foreach (var item in QuestionAnswers)
                {
                    DiseaseAnswersStr = DiseaseAnswersStr + "var AppendHTML_" + item.Key.ID + " = '<div id=\"QuestionBody_" + item.Key.ID + "\" class=\"form-group col-lg-6 col-md-6 col-sm-12 align-self-center mt-3 mb-3\"><label class=\"adomx-checkbox\">" + item.Key.Context + "</label><textarea onChange=\"PreEdit()\" id=\"Answer_" + item.Key.ID + "\" class=\"form-control DisQuesAns\" cols=\"20\" name=\"DisQuesAnswers[]\" rows=\"2\"></textarea></div>';\n$('#QuestionsAnswer').append(AppendHTML_" + item.Key.ID + ");\ndocument.getElementById(\"Answer_" + item.Key.ID + "\").value = \"" + item.Value + "\";\n";
                }
                string QuestionAnswersForJSONStr = "document.getElementById(\"JsonQuestionsAns\").value ='" + JsonConvert.SerializeObject(QuestionAnswersForJSON) + "';";
                ViewBag.DiseaseAnswersStr = DiseaseAnswersStr + QuestionAnswersForJSONStr;
            }
            UserData UD = new UserData();
            List<string> AU = new List<string>();
            AU.Add("زیاد");
            AU.Add("متوسط");
            AU.Add("کم");
            ViewBag.AU = AU;
            if (User.UserData != null && UserEdit != null)
                UD = JsonConvert.DeserializeObject<UserData>(UserEdit.UsersEditData);
            ViewBag.SelectActivity = UD.PhysicalActivity;
            return View(UD);
        }




        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("profile")]
        public ActionResult profile(UserData user, UserEdit usersEdit, string JsonQuestionsAns)
        {
            if (Session["Token"] != null)
            {
                string Token = Session["Token"].ToString();
                var UserLogin = from a in db.Users where a.Token == Token select a;
                if (UserLogin == null)
                {
                    return RedirectToAction("Login", "UserPanel");
                }

            }
            else if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "UserPanel");

            }
            if (ModelState.IsValid)
            {
                string UserEditToken = Guid.NewGuid().ToString();
                UserData userData = new UserData();
                userData.AllergicFoods = user.AllergicFoods;
                userData.Birthday = user.Birthday;
                userData.DissLikeFoods = user.DissLikeFoods;
                userData.DrugHistory = user.DrugHistory;
                userData.FoodsLike = user.FoodsLike;
                userData.Height = user.Height;
                if (!user.IsMan)
                {
                    userData.IsMan = false;
                }
                else
                {
                    userData.IsMan = true;
                }
                if (!user.IsVegetarian)
                {
                    userData.IsVegetarian = false;
                }
                else
                {
                    userData.IsVegetarian = true;
                }
                userData.Job = user.Job;
                userData.SurgeryHistory = user.SurgeryHistory;
                userData.Waist = user.Waist;
                userData.Weight = user.Weight;
                userData.Wrist = user.Wrist;
                userData.IllnessRelatives = user.IllnessRelatives;
                userData.HasDigestiveProblem = user.HasDigestiveProblem;
                userData.PhysicalActivity = user.PhysicalActivity;
                string userdataJson = JsonConvert.SerializeObject(userData);
                string Token = Session["Token"].ToString();
                var User = (from a in db.Users where a.Token == Token select a).SingleOrDefault();
                int UserID = User.ID;
                var UesrSEdit = new UserEdit();
                usersEdit.UsersID = UserID;
                UesrSEdit.UsersID = usersEdit.UsersID;
                usersEdit.UsersEditData = userdataJson;
                UesrSEdit.UsersEditData = usersEdit.UsersEditData;
                usersEdit.Token = UserEditToken;
                db.UserEdits.Add(usersEdit);

                if (JsonQuestionsAns.Length!=0)
                {
                    List<Ans> MyAns = new List<Ans>();
                    var Answers = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonQuestionsAns);
                    foreach (var item in Answers)
                    {
                        string Key = item.Key.Replace("Answer_", "");

                        Ans A = new Ans();
                        A.ID = Key;
                        A.Value = item.Value;

                        Answer Answ = new Answer();
                        Answ.DiseaseQuestionsID = int.Parse(Key);
                        Answ.UserID = UserID;
                        Answ.AnswerText = item.Value;
                        Answ.Token = UserEditToken;
                        db.Answers.Add(Answ);

                        MyAns.Add(A);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("profile", "UserPanel");
            }

            return View();
        }

        [Route("order")]
        public ActionResult Order()
        {
            if (Session["Token"].ToString() != null)
            {
                var UserLogin = (from a in db.Users where a.Token == Session["Token"].ToString() select a);
                if (UserLogin == null)
                {
                    return RedirectToAction("Login", "UserPanel");
                }
            }
            else if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "UserPanel");

            }

            return View();
        }

        [Route("diets")]
        public ActionResult Diets()
        {
            if (Session["Token"].ToString() != null)
            {
                var UserLogin = (from a in db.Users where a.Token == Session["Token"].ToString() select a);
                if (UserLogin == null)
                {
                    return RedirectToAction("Login", "UserPanel");
                }
            }
            else if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "UserPanel");

            }

            return View();
        }

        [Route("Support")]
        public ActionResult Support()
        {
            if (Session["Token"].ToString() != null)
            {
                var UserLogin = (from a in db.Users where a.Token == Session["Token"].ToString() select a);
                if (UserLogin == null)
                {
                    return RedirectToAction("Login", "UserPanel");
                }
            }
            else if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "UserPanel");

            }

            return View();
        }
    }
}