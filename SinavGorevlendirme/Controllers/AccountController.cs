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
                if (!AccountManager.Login(user.TCKimlik, user.Sifre, false))
                {
                    TempData["GirisBasarisiz"] = "true";
                }
                else
                {
                    TempData["GirisBasarisiz"] = "false";
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
