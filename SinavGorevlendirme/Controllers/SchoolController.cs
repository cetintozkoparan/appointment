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
            return View();
        }

        [HttpPost]
        public ActionResult Create(School school)
        {
            TempData["EventResult"] = SchoolManager.add(school);

            return View();
        }

        public ActionResult SchoolList()
        {
            List<School> sc = SchoolManager.GetSchools();
            return View(sc);
        }

    }
}
