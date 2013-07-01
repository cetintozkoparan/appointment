using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SinavGorevlendirme
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("homepage_default", "yonetim", new { action = "Index", Controller = "Admin" });
            routes.MapRoute("loginpage", "yonetim/login", new { action = "Login", Controller = "Admin" });
            routes.MapRoute("homepage", "yonetim/anasayfa", new { action = "Index", Controller = "Admin" });
            routes.MapRoute("ogretmenekle", "yonetim/ogretmenekle", new { action = "CreateTeacher", Controller = "Teacher" });
            routes.MapRoute("xmlogretmenekle", "yonetim/xmlogretmenekle", new { action = "MultiCreateTeacher", Controller = "Teacher" });
            routes.MapRoute("okulekle", "yonetim/okulekle", new { action = "Create", Controller = "School" });

            routes.MapRoute("okulliste", "yonetim/okullistesi", new { action = "SchoolList", Controller = "School" });
            routes.MapRoute("ogretmenliste", "yonetim/ogretmenlistesi", new { action = "TeacherList", Controller = "Teacher" });

            routes.MapRoute("sinavac", "yonetim/sinavac", new { action = "Create", Controller = "Sinav" });
            routes.MapRoute("sinavliste", "yonetim/sinavliste/{DurumId}", new { action = "SinavListe", Controller = "Sinav" });
            routes.MapRoute("sinavlistetumu", "yonetim/sinavliste", new { action = "SinavListe", Controller = "Sinav" });
            routes.MapRoute("sinavgorevlendirme", "yonetim/sinavgorevlendirme/{SinavOturumId}", new { action = "SinavGorevlendirme", Controller = "Sinav" });
            routes.MapRoute("ayarlar", "yonetim/ayarlar", new { action = "Index", Controller = "Setting" });

            routes.MapRoute("ogretmenekleidare", "idari/ogretmenekle", new { action = "CreateTeacherForIdari", Controller = "Teacher" });
            routes.MapRoute("ogretmenlisteidare", "idari/ogretmenlistesi", new { action = "TeacherListForIdari", Controller = "Teacher" });
            routes.MapRoute("sinavlisteidare", "idari/sinavlar", new { action = "SinavListeForIdari", Controller = "Sinav" });

            routes.MapRoute("sinavkatilimlistesi", "yonetim/KatilimListesi/{SinavOturumId}", new { action = "KatilimListesi", Controller = "Sinav" });
            routes.MapRoute("ogretmeneditidari", "idari/ogretmen/{OgretmenId}", new { action = "TeacherEditForIdari", Controller = "Teacher" });
            routes.MapRoute("ogretmenedyonetim", "yonetim/ogretmen/{OgretmenId}", new { action = "TeacherEdit", Controller = "Teacher" });
            //routes.MapRoute("ogretmensilyonetim", "yonetim/ogretmensil/{OgretmenId}", new { action = "OgretmenSil", Controller = "Teacher" });
            routes.MapRoute("ogretmenbilgi", "ogretmenbilgilerim", new { action = "PersonelBilgi", Controller = "Teacher" });
            routes.MapRoute("yonetimokulguncelle", "yonetim/okul/{SchoolId}", new { action = "OkulGuncelle", Controller = "School" });
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}