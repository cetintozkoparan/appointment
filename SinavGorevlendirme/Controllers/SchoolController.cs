using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SG_BLL;
using SG_BLL.Tools;
using SG_DAL.Entities;
using SinavGorevlendirme.Filters;
using SinavGorevlendirme.Models;
using WebMatrix.WebData;

namespace SinavGorevlendirme.Controllers
{
    public class SchoolController : Controller
    {
        //
        // GET: /Home/
        [AuthenticateUser]

        public ActionResult Create()
        {
            List<School> sc = SchoolManager.GetSchools();
            CreateSchoolWrapperModel model = new CreateSchoolWrapperModel(new School(), sc);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(School school)
        {
            TempData["EventResult"] = SchoolManager.add(school);
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult OkulGuncelle(int SchoolId)
        {
            School sch = SchoolManager.GetSchoolBySchoolId(SchoolId);
            return View(sch);
        }

        [HttpPost]
        public ActionResult OkulGuncelle(School okul)
        {
            TempData["EventResult"] = SchoolManager.OkulGuncelle(okul);
            return RedirectToAction("Create");
        }

        public ActionResult SchoolList()
        {
            List<School> sc = SchoolManager.GetSchools();
            return View(sc);
        }

        [HttpPost]
        public ActionResult MultiCreateSchool(HttpPostedFileBase uploadfile)
        {
            TempData["EventResult"] = SchoolManager.addSchool(uploadfile);

            return RedirectToAction("Create");
        }

        public ActionResult OkulSil(int OkulId)
        {
            TempData["EventResult"] = SchoolManager.deleteSchool(OkulId);

            return RedirectToAction("Create", "School");
        }
    }
}
