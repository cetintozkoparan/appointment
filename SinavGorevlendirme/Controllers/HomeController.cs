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
using SinavGorevlendirme.Models;
using WebMatrix.WebData;

namespace SinavGorevlendirme.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var items = new List<SinavDurumHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(SinavController).Assembly);
            foreach (var enmDurum in Enum.GetValues(typeof(EnumSinavDurum)))
            {
                items.Add(new SinavDurumHelper((int)enmDurum, rm.GetString(enmDurum.ToString()), ""));
            }

            var oturumlar = new List<SinavOturum>();

            oturumlar = SinavManager.SinavListe((int)SG_DAL.Enums.EnumSinavDurum.OnaylanmisSinav);

            var sinavlist = new SinavListeWrapperModel(new List<Sinav>(), oturumlar);
            if (User.Identity.IsAuthenticated)
            {
                if (((FormsIdentity)User.Identity).Ticket.UserData == "ogretmen")
                {
                    HttpCookie myCookie = new HttpCookie("LoginCookie");
                    myCookie = Request.Cookies["LoginCookie"];
                    Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
                    Teacher tcm = TeacherManager.GetTeacherByTCNo(tcno);
                    if (tcm.GenelBasvuru)
                    {
                        ViewBag.isaretli = "checked";
                    }
                    else
                    {
                        ViewBag.isaretli = string.Empty;
                    }
                }
            }
            return View(sinavlist);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifreDegistir(string sifre1, string sifre2)
        {
            if (sifre2.Equals(sifre1))
            {
                TempData["EventResult"] = TeacherManager.SifreDegistir(sifre1);
            }
            else
            {
                TempData["EventResult"] = new Result("Şifreler farklı olmalıdır!", SystemRess.Messages.hatali_durum.ToString());
            }
            return RedirectToAction("PersonelBilgi", "Teacher");
        }

        [HttpPost]
        public ActionResult BasvuruDurumGuncelle(string gnlBasvuru)
        {
            bool durum = false;
            if (gnlBasvuru != null && gnlBasvuru.Equals("on"))
                durum = true;
            TeacherManager.GenelBasvuruGuncelle(durum);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            Result result = UserManager.CreateUser(user);
            TempData["Status"] = result.Status;
            TempData["Message"] = result.Message;
            return View();
        }

        public PartialViewResult _onaysizSinavlar()
        {
            var sinavlar = SinavManager.SinavListeByOturumDurum((int)SG_DAL.Enums.EnumSinavDurum.OnaylanmamisSinav);
            return PartialView(sinavlar);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }
    }
}
