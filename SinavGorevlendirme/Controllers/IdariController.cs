using System;
using System.Collections.Generic;
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

namespace SinavGorevlendirme.Controllers
{
    public class IdariController : Controller
    {
        //
        // GET: /Idari/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }
        
        [AuthenticateUser]
        public ActionResult CreateTeacherForIdari()
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = Request.Cookies["LoginCookie"];

            //Int64 tcno = Convert.ToInt64(myCookie.Value.Split('&')[0].Split('=')[1].ToString());
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
        
    }
}
