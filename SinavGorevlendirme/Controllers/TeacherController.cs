using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SG_BLL;
using SG_BLL.Tools;
using SG_DAL.Entities;
using SG_DAL.Enums;
using SinavGorevlendirme.Filters;
using SinavGorevlendirme.Models;

namespace SinavGorevlendirme.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/
        
        [AuthenticateUser]
        public ActionResult CreateTeacher()
        {
            var schools = SchoolManager.GetSchools();
            var list = new SelectList(schools, "SchoolId", "Ad");
            ViewBag.SchoolList = list;

            var items = new List<UnvanHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);
            
            foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
            {
                items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
            }

            var unvanList = new SelectList(items, "UnvanId", "Unvan");

            ViewBag.UnvanList = unvanList;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTeacher(User user, Teacher teacher)
        {
            TempData["EventResult"] = TeacherManager.addTeacher(user, teacher);
            if (((SG_BLL.Tools.Result)TempData["EventResult"]).Status.Equals("error"))
            {
                var schools = SchoolManager.GetSchools();
                var list = new SelectList(schools, "SchoolId", "Ad");
                ViewBag.SchoolList = list;

                var items = new List<UnvanHelper>();
                ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);

                foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
                {
                    items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
                }

                var unvanList = new SelectList(items, "UnvanId", "Unvan");

                ViewBag.UnvanList = unvanList;
                return View();
            }

            return RedirectToAction("CreateTeacher");
        }

        [HttpPost]
        public ActionResult MultiCreateTeacher(HttpPostedFileBase uploadfile)
        {
            TempData["EventResult"] = TeacherManager.addTeacher(uploadfile);

            if (((SG_BLL.Tools.Result)TempData["EventResult"]).Status.Equals("error"))
            {
                var schools = SchoolManager.GetSchools();
                var list = new SelectList(schools, "SchoolId", "Ad");
                ViewBag.SchoolList = list;
                return View();
            }

            return RedirectToAction("CreateTeacher");
        }

        [HttpPost]
        public ActionResult CreateTeacherForIdari(User user, Teacher teacher)
        {
            TempData["EventResult"] = TeacherManager.addTeacher(user, teacher);
            if (((SG_BLL.Tools.Result)TempData["EventResult"]).Status.Equals("error"))
            {
                var schools = SchoolManager.GetSchools();
                var list = new SelectList(schools, "SchoolId", "Ad");
                ViewBag.SchoolList = list;

                var items = new List<UnvanHelper>();
                ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);

                foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
                {
                    items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
                }

                var unvanList = new SelectList(items, "UnvanId", "Unvan");

                ViewBag.UnvanList = unvanList;
                return View();
            }

            return RedirectToAction("CreateTeacherForIdari");
        }

        public ActionResult OgretmenSil(int OgretmenId)
        {
            TempData["EventResult"] = TeacherManager.deleteTeacher(OgretmenId);

            if (((FormsIdentity)User.Identity).Ticket.UserData == "idareci")
            {
                return RedirectToAction("TeacherListForIdari", "Teacher");
            }
            else
            {
                return RedirectToAction("TeacherList", "Teacher");
            }
        }

        [HttpPost]
        public ActionResult MultiCreateTeacherForIdari(HttpPostedFileBase uploadfile)
        {
            TempData["EventResult"] = TeacherManager.addTeacher(uploadfile);

            if (((SG_BLL.Tools.Result)TempData["EventResult"]).Status.Equals("error"))
            {
                var schools = SchoolManager.GetSchools();
                var list = new SelectList(schools, "SchoolId", "Ad");
                ViewBag.SchoolList = list;
                return View();
            }

            return RedirectToAction("CreateTeacherForIdari");
        }

        [HttpPost]
        public ActionResult TeacherUpdate(User user, Teacher teacher)
        {
            TempData["EventResult"] = TeacherManager.updateTeacher(user, teacher);

            if (((FormsIdentity)User.Identity).Ticket.UserData == "idareci")
            {
                return RedirectToAction("TeacherEditForIdari", "Teacher", new { OgretmenId = teacher.TeacherId });
            }
            else if (((FormsIdentity)User.Identity).Ticket.UserData == "ogretmen")
            {
                return RedirectToAction("PersonelBilgi", "Teacher", new { OgretmenId = teacher.TeacherId });
            }else
	        {
                return RedirectToAction("TeacherEdit", "Teacher", new { OgretmenId = teacher.TeacherId });
	        }
        }

        [HttpGet]
        public ActionResult PersonelBilgi()
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = Request.Cookies["LoginCookie"];
            Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
            School sch = SchoolManager.GetSchoolByTCNo(tcno);

            List<School> schList = new List<School>();
            schList.Add(sch);

            var list = new SelectList(schList, "SchoolId", "Ad");

            ViewBag.SchoolList = list;

            if (myCookie != null)
            {
                ViewBag.School = sch.Ad;
                ViewBag.SchoolId = sch.SchoolId;
            }

            var items = new List<UnvanHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);

            Teacher tch = TeacherManager.GetTeacherByTCNo(tcno);
            User usr = tch.User;

            //var Kidemler = new SelectList(new[]
            //                              {
            //                                  new{Text="1",Value="1"},
            //                                  new{Text="2",Value="2"},
            //                                  new{Text="3",Value="3"},
            //                              },
            //                "Text", "Value", tch.Kidem);

            //ViewBag.Kidemler = Kidemler;

            //foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
            //{
            //    items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
            //}

            foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
            {
                if ((int)unv == tch.Unvan)
                {
                    ViewBag.Unvan = rm.GetString(unv.ToString());
                }
            }

            //var unvanList = new SelectList(items, "UnvanId", "Unvan", tch.Unvan);

            //ViewBag.UnvanList = unvanList;

            //School sch = SchoolManager.GetSchoolByTCNo(usr.TCKimlik);
            
            TeacherWrapperModel model = new TeacherWrapperModel(usr, tch, sch);
            return View(model);
        }

        [HttpGet]
        public ActionResult TeacherEditForIdari(int OgretmenId)
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = Request.Cookies["LoginCookie"];
            Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
            School sch = SchoolManager.GetSchoolByTCNo(tcno);

            List<School> schList = new List<School>();
            schList.Add(sch);

            var list = new SelectList(schList, "SchoolId", "Ad");

            ViewBag.SchoolList = list;

            if (myCookie != null)
            {
                ViewBag.School = sch.Ad;
                ViewBag.SchoolId = sch.SchoolId;
            }

            var items = new List<UnvanHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);

            Teacher tch = TeacherManager.GetTeacher(OgretmenId);
            User usr = UserManager.GetUserByTeacherId(OgretmenId);

            var Kidemler = new SelectList(new[]
                                          {
                                              new{Text="1",Value="1"},
                                              new{Text="2",Value="2"},
                                              new{Text="3",Value="3"},
                                          },
                            "Text", "Value", tch.Kidem);

            ViewBag.Kidemler = Kidemler;

            foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
            {
                items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
            }

            var unvanList = new SelectList(items, "UnvanId", "Unvan", tch.Unvan);

            ViewBag.UnvanList = unvanList;

            //School sch = SchoolManager.GetSchoolByTCNo(usr.TCKimlik);

            TeacherWrapperModel model = new TeacherWrapperModel(usr, tch, sch);
            return View(model);
        }

        [HttpGet]
        public ActionResult TeacherEdit(int OgretmenId)
        {
            
            List<School> schList = SchoolManager.GetSchools();

            var list = new SelectList(schList, "SchoolId", "Ad");

            ViewBag.SchoolList = list;

            var items = new List<UnvanHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);

            Teacher tch = TeacherManager.GetTeacher(OgretmenId);
            User usr = UserManager.GetUserByTeacherId(OgretmenId);

            var Kidemler = new SelectList(new[]
                                          {
                                              new{Text="1",Value="1"},
                                              new{Text="2",Value="2"},
                                              new{Text="3",Value="3"},
                                          },
                            "Text", "Value", tch.Kidem);

            ViewBag.Kidemler = Kidemler;

            foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
            {
                items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
            }

            var unvanList = new SelectList(items, "UnvanId", "Unvan", tch.Unvan);

            ViewBag.UnvanList = unvanList;

            //School sch = SchoolManager.GetSchoolByTCNo(usr.TCKimlik);

            TeacherWrapperModel model = new TeacherWrapperModel(usr, tch, new School());
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateTeacherForIdari()
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = Request.Cookies["LoginCookie"];
            Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
            School sch = SchoolManager.GetSchoolByTCNo(tcno);

            List<School> schList = new List<School>();
            schList.Add(sch);

            var list = new SelectList(schList, "SchoolId", "Ad");

            ViewBag.SchoolList = list;


            if (myCookie != null)
            {
                ViewBag.School = sch.Ad;
                ViewBag.SchoolId = sch.SchoolId;
            }

            var items = new List<UnvanHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);

            foreach (var unv in Enum.GetValues(typeof(EnumUnvan)))
            {
                items.Add(new UnvanHelper((int)unv, rm.GetString(unv.ToString())));
            }

            var unvanList = new SelectList(items, "UnvanId", "Unvan");

            ViewBag.UnvanList = unvanList;
            return View();
        }

        public ActionResult TeacherList()
        {
            List<Teacher> teacher = TeacherManager.GetTeacherList();

            //ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);
            //foreach (var tch in teacher)
            //{
            //    tch.Unvan = rm.GetString((SG_DAL.Enums.EnumUnvan)tch.Unvan);
            //}

            return View(teacher);
        }

        public ActionResult TeacherListForIdari()
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = Request.Cookies["LoginCookie"];
            Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
            School sch = SchoolManager.GetSchoolByTCNo(tcno);

            List<Teacher> teacher = TeacherManager.GetTeacherListBySchoolId(sch.SchoolId);

            //ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(TeacherController).Assembly);
            //foreach (var tch in teacher)
            //{
            //    tch.Unvan = rm.GetString((SG_DAL.Enums.EnumUnvan)tch.Unvan);
            //}

            return View(teacher);
        }

        public PartialViewResult _ogretmenler()
        {
            List<Teacher> teachers = TeacherManager.GetTeacherList(SG_DAL.Enums.EnumUnvan.Ogretmen);
            return PartialView(teachers);
        }
    }
}
