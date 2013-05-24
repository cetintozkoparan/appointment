using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SG_BLL;
using SG_DAL.Entities;
using SinavGorevlendirme.Filters;

namespace SinavGorevlendirme.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        [AuthenticateUser]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (AccountManager.Login(user.TCKimlik, user.Sifre, true))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı!");
                }

                return View(user);
            }
            else
            {
                return View();
            }
        }
    }
}
