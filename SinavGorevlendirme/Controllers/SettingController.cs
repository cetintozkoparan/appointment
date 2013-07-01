using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SG_BLL;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Controllers
{
    public class SettingController : Controller
    {
        //
        // GET: /Setting/

        public ActionResult Index()
        {
            return View(SettingManager.GetSettings());
        }

        [HttpPost]
        public ActionResult UpdateSettings(Setting settings)
        {
            TempData["EventResult"] = SettingManager.UpdateSettings(settings);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SiralamaGuncelle(Setting settings)
        {
            TempData["EventResult"] = SettingManager.UpdateSiralama(settings);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult KurumGuncelle(Setting settings)
        {
            TempData["EventResult"] = SettingManager.UpdateKurumAdi(settings);

            return RedirectToAction("Index");
        }
    }
}
