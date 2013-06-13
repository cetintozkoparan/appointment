using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
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
    }
}
