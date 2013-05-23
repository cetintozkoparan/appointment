using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SG_BLL;
using SG_BLL.Tools;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            Result result = UserManager.CreateUser(user);
            TempData["Status"] = result.Status;
            TempData["Message"] = result.Message;
            return View();
        }
    }
}
