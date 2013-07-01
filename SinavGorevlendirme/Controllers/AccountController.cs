using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SG_BLL;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                User us = AccountManager.Login(user.TCKimlik, user.Sifre);
                if (us == null)
                {
                    TempData["GirisBasarisiz"] = "true";
                }
                else
                {
                    TempData["GirisBasarisiz"] = "false";
                    if (us.IsAdmin == true)
                        return RedirectToAction("SinavListe", "Sinav");
                    else
                        return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["GirisBasarisiz"] = "true";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
