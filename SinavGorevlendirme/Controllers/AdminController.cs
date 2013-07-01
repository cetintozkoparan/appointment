using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["GirisBasarisiz"] = null;
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                User us = AccountManager.Login(user.TCKimlik, user.Sifre);
                if (us != null)
                {
                    return RedirectToAction("SinavListe", "Sinav");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı!");
                    ViewBag.HataliGiris = "Kullanıcı Adı veya Şifre Hatalı!";
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
