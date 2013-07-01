using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.EnterpriseServices;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using SG_BLL;
using SG_BLL.Tools;
using SG_BLL.Tools.Report;
using SG_DAL.Entities;
using SG_DAL.Enums;
using SinavGorevlendirme.Models;

namespace SinavGorevlendirme.Controllers
{
    public class SinavController : Controller
    {
        //
        // GET: /Sinav/

        public ActionResult Create()
        {
            var schools = SchoolManager.GetSchools();
            var list = new SelectList(schools, "SchoolId", "Ad");
            ViewBag.SchoolList = list;

            return View();
        }

        [HttpPost]
        public ActionResult KatilimGuncelle(List<SinavGorevli> teachers, int snvOtrmId)
        {
            teachers.ForEach(d => d.SinavOturumId = snvOtrmId);
            SinavManager.KatilimGuncelle(teachers);
            return RedirectToAction("KatilimListesi", new { SinavOturumId = snvOtrmId });
        }

        public ActionResult KatilimListesi()
        {
            string SinavOturumId = RouteData.Values["SinavOturumId"].ToString();

            ViewBag.SinavOturumId = SinavOturumId;

            List<rptSinavGorevlendirme> gorevliler = SinavManager.GetKatilimOgretmenleri(Convert.ToInt32(SinavOturumId));
            
            List<SinavGorevli> tch = new List<SinavGorevli>();

            foreach (var t in gorevliler)
            {
                SinavGorevli tcm = new SinavGorevli();
                tcm.TeacherId = t.OgretmenId;
                tcm.SinavKatilimi = t.KatilimDurumu;
                tch.Add(tcm);
            }

            SinavKatilimWrapperModel model = new SinavKatilimWrapperModel(gorevliler, tch);

            return View(model);
        }

        
        public ActionResult SinavSecimi()
        {
            var sinavlar = SinavManager.SinavListeByOturumDurum((int)SG_DAL.Enums.EnumSinavDurum.OnaylanmamisSinav);
            return View(sinavlar);
        }

        [HttpPost]
        public ActionResult Basvur(int SinavOturumId)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult GorevdenCikar(int tchID, int snOturumID)
        {
            return Json(SinavManager.GorevdenCikar(tchID, snOturumID));
        }

        public ActionResult DurumGuncelle(int snvOturmId, int ddlSinavDurum)
        {
            SinavManager.SinavOturumDurumGuncelle(snvOturmId, ddlSinavDurum);
            return RedirectToAction("SinavGorevlendirme", new { SinavOturumId = snvOturmId });
        }

        public ActionResult YayinDurumuGuncelle(int ddlOturum)
        {
            SinavManager.SinavOturumYayinDurumGuncelle(ddlOturum);
            return RedirectToAction("SinavGorevlendirme", new { SinavOturumId = ddlOturum });
        }

        public ActionResult SinavGorevlendirme(int SinavOturumId)
        {
            var setting = SettingManager.GetSettings();
            string sortlist = string.Empty;
 
            var sinavokullar = SchoolManager.GetSinavOturumOkullari(SinavOturumId);
            var ogrt = new List<Teacher>();
            var oturum = SinavManager.GetSinavOturum(SinavOturumId);

            var tumOturumlar = SinavManager.GetSinavOturumlari(oturum.Sinav.SinavId);

            // sınav DUrumları Ekleniyor
            var durumitems = new List<SinavDurumHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(SinavController).Assembly);
            foreach (var enmDurum in Enum.GetValues(typeof(EnumSinavDurum)))
            {
                durumitems.Add(new SinavDurumHelper((int)enmDurum, rm.GetString(enmDurum.ToString() + "Kisa"), ""));
            }

            var durumlar = new SelectList(durumitems, "SinavDurumId", "Durum", oturum.SinavOturumDurumId);
            ViewBag.SinavDurumalar = durumlar;
            // end sınav durumları 


            var items = new List<OturumHelper>();

            foreach (var otr in tumOturumlar)
                items.Add(new OturumHelper(otr.SinavOturumId, otr.OturumNo + ". Oturum"));

            var oturumlist = new SelectList(items, "SinavOturumId", "Oturum", SinavOturumId);
            ViewBag.OturumList = oturumlist;

            var snvOtrOkl = SinavManager.GetSinavOturumOkullari(SinavOturumId);

            var gorevliler = new List<SinavGorevli>();

            if (setting.GenelBasvuru)
            {
                ogrt = TeacherManager.GetTeacherListForGenelBasvuru();
            }
            else
            {
                ogrt = TeacherManager.GetTeacherListForOzelBasvuru(SinavOturumId);
            }
            
            gorevliler = SinavManager.GetSinavGorevliler(SinavOturumId, (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen);

            if (gorevliler.Count() > 0)
            {
                foreach (var item in ogrt)
                {
                    var tekgorevli = SinavManager.GetSinavGorevli(SinavOturumId, item.TeacherId);

                    if (tekgorevli == null)
                    {
                        tekgorevli = new SinavGorevli();
                        tekgorevli.SiraNo = 9999999;
                    }
                    item.SinavGorevli = tekgorevli;
                }
                ogrt = ogrt.OrderBy(d => d.SinavGorevli.SiraNo).ToList();
            }
            else
            {
                if (setting.GozetmenSiralama1 != 0)
                    sortlist += "[" + setting.GozetmenSiralama1 + ",0], ";

                if (setting.GozetmenSiralama2 != 0)
                    sortlist += "[" + setting.GozetmenSiralama2 + ",0], ";

                if (setting.GozetmenSiralama3 != 0)
                    sortlist += "[" + setting.GozetmenSiralama3 + ",0], ";

                if (sortlist != string.Empty)
                {
                    sortlist = sortlist.Substring(0, sortlist.Length - 2);
                }
            }

            ViewBag.SortList = sortlist;

            var model = new SinavGorevlendirmeWrapperModel(ogrt, sinavokullar, oturum, setting, snvOtrOkl);
            return View(model);
        }

        [HttpPost]
        public ActionResult SinavGorevlendirme(string snvOturmId, string[] ogtSira, string[] txtSalonSayi, string[] hdnPersonelSayi)
        {
            TempData["EventResult"] = SinavManager.SinavGorevlendir(snvOturmId, ogtSira, txtSalonSayi, hdnPersonelSayi);

            return RedirectToAction("SinavGorevlendirme", new { SinavOturumId = snvOturmId });
        }

        [HttpPost]
        public ActionResult KomisyonGorevli(int snvOturmId, int okulId, int komisyonBaskani, int komisyonUyesi, int komisyonUyesi2)
        {
            SinavManager.KomisyonGorevlisiEkle(snvOturmId, okulId, komisyonBaskani, komisyonUyesi, komisyonUyesi2);
            if (((FormsIdentity)User.Identity).Ticket.UserData == "idareci")
            {
                return RedirectToAction("SinavListeForIdari", new { SinavOturumId = snvOturmId });              
            }
            else
            {
                return RedirectToAction("SinavGorevlendirme", new { SinavOturumId = snvOturmId });
            }
        }

        public ActionResult SinavListe()
        {
            var items = new List<SinavDurumHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(SinavController).Assembly);
            foreach (var enmDurum in Enum.GetValues(typeof(EnumSinavDurum)))
            {
                items.Add(new SinavDurumHelper((int)enmDurum, rm.GetString(enmDurum.ToString()), ""));
            }
            //HttpCookie myCookie = new HttpCookie("LoginCookie");
            //myCookie = Request.Cookies["LoginCookie"];
            //Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
            //Teacher tck = TeacherManager.GetTeacherByTCNo(tcno);


            if (RouteData.Values["DurumId"] != null)
            {
                var durumlar = new SelectList(items, "SinavDurumId", "Durum", RouteData.Values["DurumId"].ToString());
                ViewBag.SinavDurumalar = durumlar;
                var oturumlar = new List<SinavOturum>();

                oturumlar = SinavManager.SinavListe(Convert.ToInt32(RouteData.Values["DurumId"].ToString()));

                var sinavlist = new SinavListeWrapperModel(new List<Sinav>(), oturumlar, SettingManager.GetSettings(), new List<SinavOturum>(), new List<SinavOturum>());
                return View(sinavlist);
            }
            else
            {
                var durumlar = new SelectList(items, "SinavDurumId", "Durum");

                ViewBag.SinavDurumalar = durumlar;
                var oturumlar = new List<SinavOturum>();

                oturumlar = SinavManager.SinavListe();

                var sinavlist = new SinavListeWrapperModel(new List<Sinav>(), oturumlar, SettingManager.GetSettings(), new List<SinavOturum>(), new List<SinavOturum>());
                return View(sinavlist);
            }

        }

        public ActionResult SinavListeForIdari()
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = Request.Cookies["LoginCookie"];
            Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());
            School sch = SchoolManager.GetSchoolByTCNo(tcno);
            ViewBag.SchoolId = sch.SchoolId;
            
            var oturumlar = new List<SinavOturum>();
            oturumlar = SinavManager.SinavListe((int)SG_DAL.Enums.EnumSinavDurum.OnaylanmisSinav);

            var sinavlist = new SinavListeWrapperModel(new List<Sinav>(), oturumlar, SettingManager.GetSettings(), new List<SinavOturum>(), new List<SinavOturum>());
            return View(sinavlist);
        }

        [HttpPost]
        public ActionResult Create(Sinav sinav, SinavOturum oturum, FormCollection collection)
        {
            TempData["EventResult"] = SinavManager.SinavOlustur(sinav, collection);

            if (((SG_BLL.Tools.Result)TempData["EventResult"]).Status.Equals("error"))
            {
                var schools = SchoolManager.GetSchools();
                var list = new SelectList(schools, "SchoolId", "Ad");
                ViewBag.SchoolList = list;
                return View();
            }

            return RedirectToAction("Create");
        }

        public PartialViewResult _komisyonGorevlileri(int okulID, int snvOturmId)
        {
            var idareciler = TeacherManager.GetOkulIdarecileri(okulID);

            var idareciForDDL = new List<SG_BLL.Tools.UserHelper>();

            foreach (var item in idareciler)
            {
                var user = new SG_BLL.Tools.UserHelper(item.TeacherId, item.User.Ad + " " + item.User.Soyad);

                idareciForDDL.Add(user);
            }

            var komisyonGorevliler = SinavManager.GetSinavOkulKomisyonGorevliler(snvOturmId, okulID);
            //int goreveliIndex = 1;

            var baskan = SinavManager.GetSinavOkulKomisyonGorevliler(snvOturmId, okulID, (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani);

            if (baskan.Count() > 0)
            {
                var list1 = new SelectList(idareciForDDL, "TeacherId", "AdSoyad", baskan.First().TeacherId.ToString());
                ViewBag.KomisyonBask = list1;
            }
            else
            {
                var list1 = new SelectList(idareciForDDL, "TeacherId", "AdSoyad");
                ViewBag.KomisyonBask = list1;
            }

            var uyeler = SinavManager.GetSinavOkulKomisyonGorevliler(snvOturmId, okulID, (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi);

            if (uyeler.Count() > 0)
            {
                var list1 = new SelectList(idareciForDDL, "TeacherId", "AdSoyad", uyeler.First().TeacherId.ToString());
                var list2 = new SelectList(idareciForDDL, "TeacherId", "AdSoyad", uyeler.Last().TeacherId.ToString());
                ViewBag.KomisyonUye = list1;
                ViewBag.KomisyonUye2 = list2;
            }
            else
            {
                var list1 = new SelectList(idareciForDDL, "TeacherId", "AdSoyad");
                ViewBag.KomisyonUye = list1;
                ViewBag.KomisyonUye2 = list1;
            }

            ViewBag.okulId = okulID;
            var komisyonGorevlileri = SinavManager.GetSinavOkulKomisyonGorevliler(snvOturmId, okulID);

            return PartialView(idareciler);
        }

    }
}
