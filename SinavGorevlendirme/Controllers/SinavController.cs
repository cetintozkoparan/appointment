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
using System.Web.UI.WebControls;
using SG_BLL;
using SG_BLL.Tools;
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

        public ActionResult SinavSecimi()
        {
            var sinavlar = SinavManager.SinavListe("Onaylanmadı");
            return View(sinavlar);
        }

        public ActionResult SinavGorevlendirme(int SinavOturumId)
        {
            var sinavokullar = SchoolManager.GetSinavOturumOkullari(SinavOturumId);
            var setting = SettingManager.GetSettings();
            var ogrt = new List<Teacher>();
            var oturum = SinavManager.GetSinavOturum(SinavOturumId);
            var snvOtrOkl = SinavManager.GetSinavOturumOkullari(SinavOturumId);

            if (setting.GenelBasvuru)
            {
                var gorevliler = SinavManager.GetSinavGorevliler(SinavOturumId);

                ogrt = TeacherManager.GetTeacherListForGenelBasvuru();

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
            }
            else
            {
                ogrt = TeacherManager.GetTeacherList();
            }
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
            return RedirectToAction("SinavGorevlendirme", new { SinavOturumId = snvOturmId });
        }

        public ActionResult SinavListe()
        {
            var items = new List<SinavDurumHelper>();
            ResourceManager rm = new ResourceManager("SinavGorevlendirme.Resources.Genel", typeof(SinavController).Assembly);
            foreach (var enmDurum in Enum.GetValues(typeof(EnumSinavDurum)))
            {
                items.Add(new SinavDurumHelper((int)enmDurum, rm.GetString(enmDurum.ToString()), ""));
            }

            if (RouteData.Values["DurumId"] != null)
            {
                var durumlar = new SelectList(items, "SinavDurumId", "Durum", RouteData.Values["DurumId"].ToString());
                ViewBag.SinavDurumalar = durumlar;
                var oturumlar = new List<SinavOturum>();

                oturumlar = SinavManager.SinavListe(Convert.ToInt32(RouteData.Values["DurumId"].ToString()));

                var sinavlist = new SinavListeWrapperModel(new List<Sinav>(), oturumlar);
                return View(sinavlist);
            }
            else
            {
                var durumlar = new SelectList(items, "SinavDurumId", "Durum");

                ViewBag.SinavDurumalar = durumlar;
                var oturumlar = new List<SinavOturum>();

                oturumlar = SinavManager.SinavListe();

                var sinavlist = new SinavListeWrapperModel(new List<Sinav>(), oturumlar);
                return View(sinavlist);
            }

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
            int goreveliIndex = 1;

            var baskan = SinavManager.GetSinavOkulKomisyonGorevliler(snvOturmId, okulID, (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani);
            
            if (baskan.Count() > 0)
	        {
		        var list1 = new SelectList(idareciForDDL, "TeacherId", "AdSoyad", baskan.First().TeacherId.ToString());
                ViewBag.KomisyonBask = list1;
	        }else
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
	        }else
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
