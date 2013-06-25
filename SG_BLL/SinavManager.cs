using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SG_DAL.Context;
using SG_DAL.Entities;
using System.Data;
using System.Data.Entity;
using SG_DAL.Pattern;
using SG_BLL.Tools;
using System.Collections.Specialized;
using SG_DAL.Wrappers;
using SG_BLL.Tools.Report;

namespace SG_BLL
{
    public class SinavManager
    {
        public static Result result;

        public static Result SinavOlustur(Sinav sinav, NameValueCollection collection)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    SinavOturum otur = new SinavOturum();
                    School sch = new School();
                    otur.Okullar = new List<School>();

                    sinav.SinavOturum = new List<SinavOturum>();
                    int oturumNo = 1;
                    //sinav.SinavDurum = db.SinavDurum.FirstOrDefault(d => d.KisaDurum == "Onaylanmadı");
                   
                    foreach (var item in collection.AllKeys)
                    {
                        if (item.Contains("sinavoturum_Tarih"))
                        {
                            otur = new SinavOturum();
                            otur.Tarih = Convert.ToDateTime(collection.GetValues(item)[0]);
                        }
                        else if (item.Contains("sinavoturum_Saat"))
                        {
                            otur.Saat = collection.GetValues(item)[0];
                            otur.OturumNo = oturumNo++;
                            otur.SinavOturumDurumId = (int)SG_DAL.Enums.EnumSinavDurum.OnaylanmamisSinav;
                            sinav.SinavOturum.Add(otur);
                        }
                        else if (item.Contains("duallistbox_okullar"))
                        {
                            foreach (var okulid in collection.GetValues(item))
                            {
                                var okulRepo = new Repository<School>(db);
                                int id = Convert.ToInt32(okulid);
                                var okul = okulRepo.First(d => d.SchoolId == id);
                                //////////////////////

                                //////////////////////
                                List<SinavOturumOkullari> oturumokullist = new List<SinavOturumOkullari>();
                                foreach (var sinavoturumlari in sinav.SinavOturum)
                                {                                    
                                    sinavoturumlari.Okullar.Add(okul);
                                }
                            }

                        }
                    }

                    var sinavRepo = new Repository<Sinav>(db);
                    sinavRepo.Add(sinav);

                    var sinavOturumlari = SinavManager.GetSinavOturumlari(sinav.SinavId);


                    foreach (var oturum in sinavOturumlari)
                    {
                        var sinavoturumokulu = new SinavOturumOkullari();

                        var sinavOtrOkul = SchoolManager.GetSinavOturumOkullari(oturum.SinavOturumId);

                        foreach (var okul in sinavOtrOkul)
	                    {
		                    sinavoturumokulu.SchoolId = okul.SchoolId;
                            sinavoturumokulu.SinavOturumId = oturum.SinavOturumId;

                            sinavoturumokulu.AsilGozetmenSayisi = 0;
                            sinavoturumokulu.YedekGozetmenSayisi = 0;
                            db.SinavOturumOkullari.Add(sinavoturumokulu);
                            db.SaveChanges();
	                    }
                    }

                    result = new Result(SystemRess.Messages.basarili_kayit.ToString(), SystemRess.Messages.basarili_durum.ToString());
                    return result;
                }
                catch (Exception)
                {
                    result = new Result(SystemRess.Messages.hatali_kayit.ToString(), SystemRess.Messages.hatali_durum.ToString());
                    return result;
                }
            }
        }

        public static List<SinavOturum> GetSinavOturumlari(int SinavId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturumlar = db.SinavOturum.Where(d => d.SinavId == SinavId);
                    return oturumlar.ToList();
                }
                catch
                {
                    return new List<SinavOturum>().ToList();
                }
            }
        }

        public static List<SinavOturum> SinavListe(int SinavOturumDurumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturumlar = db.SinavOturum.Include("Sinav").Where(d => d.SinavOturumDurumId == SinavOturumDurumId);
                    return oturumlar.ToList();
                }
                catch
                {
                    return new List<SinavOturum>().ToList();
                }
            }
        }

        public static List<SinavOturum> SinavListe()
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturumlar = db.SinavOturum.Include("Sinav");
                    return oturumlar.ToList();
                }
                catch
                {
                    return new List<SinavOturum>().ToList();
                }
            }
        }

        public static List<SinavDurum> GetSinavDurumlar()
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var durumalar = db.SinavDurum;
                    return durumalar.ToList();
                }
                catch
                {
                    return new List<SinavDurum>().ToList();
                }
            }
        }

        public static object SinavListeByOturumDurum(int SinavOturumDurumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturumlar = db.SinavOturum.Include("Sinav").Where(d => d.SinavOturumDurumId == SinavOturumDurumId);
                    return oturumlar.ToList();
                }
                catch
                {
                    return new List<SinavOturum>().ToList();
                }
            }
        }

        public static SinavOturum GetSinavOturum(int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturum = db.SinavOturum.Include("Sinav").First(d => d.SinavOturumId == SinavOturumId);
                    return oturum;
                }
                catch
                {
                    return new SinavOturum();
                }
            }
        }

        public static Result SinavGorevlendir(string snvOturmId, string[] ogretmenler, string[] txtSalonSayi, string[] hdnPersonelSayi)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    //var gorevliRepo = new Repository<SinavGorevli>(db);

                    var okullar = SchoolManager.GetSinavOturumOkullari(Convert.ToInt32(snvOturmId));
                    var ogtRepo = new Repository<Teacher>(db);
                    var grvRepo = new Repository<SinavGorevli>(db);
                    var snvOtrOklRepo = new Repository<SinavOturumOkullari>(db);

                    int genelSira = 1;
                    int okulIndex = 1;


                    foreach (var item in okullar)
                    {
                        var komisyon = SinavManager.GetSinavGorevliler(Convert.ToInt32(snvOturmId), (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani, item.SchoolId);

                        if (komisyon.Count == 0)
                        {
                            var idareciler = TeacherManager.GetOkulIdarecileri(item.SchoolId);
                            // bu kısımda idarecilerden ilk olanlarını bina komisyon görevlisi olarak otomatik olarak ata
                            var mdr = idareciler.FirstOrDefault(d => d.Unvan == (int)SG_DAL.Enums.EnumUnvan.Mudur);
                            if (mdr != null)
                            {
                                var gorevli = new SinavGorevli();
                                gorevli.SinavOturumId = Convert.ToInt32(snvOturmId);
                                gorevli.SiraNo = 1;
                                gorevli.TeacherId = Convert.ToInt32(mdr.TeacherId);
                                gorevli.SchoolId = item.SchoolId;
                                gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani;
                                db.SinavGorevli.Add(gorevli);
                                db.SaveChanges();
                            }

                            var mdryrd = idareciler.FirstOrDefault(d => d.Unvan == (int)SG_DAL.Enums.EnumUnvan.MudurYardimcisi);
                            if (mdryrd != null)
                            {
                                var gorevli = new SinavGorevli();
                                gorevli.SinavOturumId = Convert.ToInt32(snvOturmId);
                                gorevli.SiraNo = 1;
                                gorevli.TeacherId = Convert.ToInt32(mdryrd.TeacherId);
                                gorevli.SchoolId = item.SchoolId;
                                gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani;
                                db.SinavGorevli.Add(gorevli);
                                db.SaveChanges();
                            }

                            mdryrd = idareciler.LastOrDefault(d => d.Unvan == (int)SG_DAL.Enums.EnumUnvan.MudurYardimcisi);
                            if (mdryrd != null)
                            {
                                var gorevli = new SinavGorevli();
                                gorevli.SinavOturumId = Convert.ToInt32(snvOturmId);
                                gorevli.SiraNo = 1;
                                gorevli.TeacherId = Convert.ToInt32(mdryrd.TeacherId);
                                gorevli.SchoolId = item.SchoolId;
                                gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani;
                                db.SinavGorevli.Add(gorevli);
                                db.SaveChanges();
                            }
                        }
                    }

                    var gorevliler = SinavManager.GetSinavGorevliler(Convert.ToInt32(snvOturmId), (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen);

                    foreach (var item in gorevliler)
                    {
                        grvRepo.Delete(item);
                    }

                    var oturumlar = SinavManager.GetSinavOturumOkullari(Convert.ToInt32(snvOturmId));

                    foreach (var item in oturumlar)
                    {
                        snvOtrOklRepo.Delete(item);
                    }
                    int okulogretmensira = 1;
                    foreach (var okul in okullar)
                    {
                        for (int i = 0; i < Convert.ToInt32(hdnPersonelSayi[okulIndex - 1]); i++)
                        {
                            int ogtID = Convert.ToInt32(ogretmenler[genelSira - 1]);
                            int sinavOturumID = Convert.ToInt32(snvOturmId);

                            var gorevli = new SinavGorevli();
                            gorevli.SinavOturumId = Convert.ToInt32(snvOturmId);
                            gorevli.SiraNo = genelSira;
                            gorevli.OkulSiraNo = okulogretmensira;
                            gorevli.TeacherId = Convert.ToInt32(ogretmenler[genelSira - 1]);
                            gorevli.SchoolId = okul.SchoolId;
                            gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen;
                            db.SinavGorevli.Add(gorevli);

                            db.SaveChanges();
                            genelSira++;
                            okulogretmensira++;
                        }
                        okulogretmensira = 1;
                        SinavOturumOkullari oturumokul = new SinavOturumOkullari();
                        oturumokul.SchoolId = okul.SchoolId;
                        oturumokul.SinavOturumId = Convert.ToInt32(snvOturmId);
                        oturumokul.AsilGozetmenSayisi = Convert.ToInt32(hdnPersonelSayi[okulIndex - 1]);
                        oturumokul.SalonSayisi = Convert.ToInt32(txtSalonSayi[okulIndex - 1]);
                        db.SinavOturumOkullari.Add(oturumokul);
                        db.SaveChanges();

                        okulIndex++;
                    }


                    result = new Result(SystemRess.Messages.basarili_kayit.ToString(), SystemRess.Messages.basarili_durum.ToString());
                    return result;
                }
                catch
                {
                    result = new Result(SystemRess.Messages.hatali_kayit.ToString(), SystemRess.Messages.hatali_durum.ToString());
                    return result;
                }
            }
        }

        public static List<SinavOturumOkullari> GetSinavOturumOkullari(int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var sinavoturumokullari = db.SinavOturumOkullari.Where(d => d.SinavOturumId == SinavOturumId);
                    return sinavoturumokullari.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<SinavGorevli> GetSinavGorevliler(int SinavOturumId, int GorevId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevliler = gorevliRepo.Find(d => d.SinavOturumId == SinavOturumId && d.SinavGorevId == GorevId);

                    return gorevliler.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<rptSinavGorevlendirme> GetKatilimOgretmenleri(int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                var list = (from grv in db.SinavGorevli
                            join otrm in db.SinavOturum on grv.SinavOturumId equals otrm.SinavOturumId
                            join tch in db.Teacher on grv.TeacherId equals tch.TeacherId
                            join tchUsr in db.User on tch.User.UserId equals tchUsr.UserId
                            join otrokl in db.SinavOturumOkullari on grv.SinavOturumId equals otrokl.SinavOturumId
                            join ogtokl in db.School on tch.SchoolId equals ogtokl.SchoolId
                            join okl in db.School on grv.SchoolId equals okl.SchoolId
                            join snv in db.Sinav on otrm.SinavId equals snv.SinavId
                            where otrm.SinavOturumId == SinavOturumId &&
                                    grv.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen
                            select new
                            {
                                SinavAdi = snv.SinavAdi,
                                SinavTarihi = (DateTime)otrm.Tarih,
                                SinavSaati = otrm.Saat,
                                SinavOkulAdi = okl.Ad,
                                SinavOkulMebKodu = okl.MebKodu,
                                KadroluOlduguOkulAdi = ogtokl.Ad,
                                PersonelSira = grv.SiraNo,
                                PersonelAdSoyad = tchUsr.Ad + " " + tchUsr.Soyad,
                                PersonelTC = tchUsr.TCKimlik,
                                PersonelGorev = (SG_DAL.Enums.EnumSinavGorev)grv.SinavGorevId,
                                SinavOkulId = okl.SchoolId,
                                KatilimDurumu = grv.SinavKatilimi,
                                OgretmenId = tch.TeacherId,
                                KomisyonBaskani =
                                                (from u1 in db.User
                                                 join t1 in db.Teacher on u1.UserId equals t1.User.UserId
                                                 join g1 in db.SinavGorevli on t1.TeacherId equals g1.TeacherId
                                                 where g1.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani
                                                 && g1.SinavOturumId == SinavOturumId
                                                 && g1.SchoolId == okl.SchoolId
                                                 && g1.SiraNo == 1
                                                 select new { AdSoyad = u1.Ad + " " + u1.Soyad }
                                                   ).FirstOrDefault().AdSoyad
                                                   ,
                                KomisyonUyesi =
                                                (from u1 in db.User
                                                 join t1 in db.Teacher on u1.UserId equals t1.User.UserId
                                                 join g1 in db.SinavGorevli on t1.TeacherId equals g1.TeacherId
                                                 where g1.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi
                                                 && g1.SinavOturumId == SinavOturumId
                                                 && g1.SchoolId == okl.SchoolId
                                                 && g1.SiraNo == 2
                                                 select new { AdSoyad2 = u1.Ad + " " + u1.Soyad }
                                                   ).FirstOrDefault().AdSoyad2
                                                   ,
                                KomisyonUyesi2 =
                                                 (from u1 in db.User
                                                  join t1 in db.Teacher on u1.UserId equals t1.User.UserId
                                                  join g1 in db.SinavGorevli on t1.TeacherId equals g1.TeacherId
                                                  where g1.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi
                                                  && g1.SinavOturumId == SinavOturumId
                                                  && g1.SchoolId == okl.SchoolId
                                                  && g1.SiraNo == 3
                                                  select new { AdSoyad3 = u1.Ad + " " + u1.Soyad }
                                                   ).FirstOrDefault().AdSoyad3

                            }).Distinct().OrderBy(d => d.SinavOkulId).ToList();

                List<rptSinavGorevlendirme> sg = new List<rptSinavGorevlendirme>();

                foreach (var item in list)
                {
                    rptSinavGorevlendirme snv = new rptSinavGorevlendirme(item.SinavAdi, item.SinavTarihi, item.SinavSaati, item.SinavOkulAdi,
                        item.KomisyonBaskani, item.KomisyonUyesi, item.KomisyonUyesi2, item.SinavOkulMebKodu, item.KadroluOlduguOkulAdi,
                        item.PersonelTC, item.PersonelAdSoyad, item.PersonelSira, item.PersonelGorev.ToString(), item.SinavOkulId, item.KatilimDurumu, item.OgretmenId);
                    sg.Add(snv);
                }
                return sg;
            }
            //using (SGContext db = new SGContext())
            //{
            //    try
            //    {
            //        var list = (from tch in db.Teacher
            //                    join user in db.User on tch.User.UserId equals user.UserId
            //                    join grv in db.SinavGorevli on tch.TeacherId equals grv.TeacherId
            //                    join otrm in db.SinavOturum on grv.SinavOturumId equals otrm.SinavOturumId
            //                    where otrm.SinavOturumId == SinavOturumId &&
            //                            grv.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen
            //                    select new
            //                    {
            //                        tch.TeacherId,
            //                        tch.Unvan,
            //                        tch.GorevSayisi,
            //                        tch.Kidem,
            //                        user.UserId,
            //                        user.Ad,
            //                        user.Soyad,
            //                        user.TCKimlik,
            //                        user.Tel,
            //                        user.Email
            //                    });

            //        List<Teacher> tchList = new List<Teacher>();

            //        foreach (var item in list)
            //        {
            //            Teacher tc = new Teacher();
            //            tc.TeacherId = item.TeacherId;
            //            tc.GorevSayisi = item.GorevSayisi;
            //            tc.Kidem = item.Kidem;
            //        }

            //        return tchList.ToList();
            //    }
            //    catch (Exception)
            //    {
            //        return null;
            //    }
            //}
        }

        public static List<SinavGorevli> GetSinavGorevliler(int SinavOturumId, int GorevId, int OkulId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevliler = gorevliRepo.Find(d => d.SinavOturumId == SinavOturumId && d.SinavGorevId == GorevId && d.SchoolId == OkulId);

                    return gorevliler.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static SinavGorevli GetSinavGorevli(int SinavOturumId, int TeacherId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevli = gorevliRepo.First(d => d.SinavOturumId == SinavOturumId && d.TeacherId == TeacherId);

                    return gorevli;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static SinavOturumOkullari GetSinavOturumOkulu(int okulID, int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var repo = new Repository<SinavOturumOkullari>(db);

                    var okul = repo.First(d => d.SchoolId == okulID && d.SinavOturumId == SinavOturumId);

                    return okul;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static bool KomisyonGorevlisiEkle(int snvOturmId, int okulId, int komisyonBaskani, int komisyonUyesi, int komisyonUyesi2)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var komisyonGorevlileri = SinavManager.GetSinavOkulKomisyonGorevliler(snvOturmId, okulId);
                    var komisyonRepo = new Repository<SinavGorevli>(db);

                    foreach (var item in komisyonGorevlileri)
                        komisyonRepo.Delete(item);

                    var gorevli = new SinavGorevli();
                    gorevli.SinavOturumId = snvOturmId;
                    gorevli.TeacherId = komisyonBaskani;
                    gorevli.SchoolId = okulId;
                    gorevli.SiraNo = 1;
                    gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani;
                    komisyonRepo.Add(gorevli);

                    gorevli = new SinavGorevli();
                    gorevli.SinavOturumId = snvOturmId;
                    gorevli.TeacherId = komisyonUyesi;
                    gorevli.SiraNo = 2;
                    gorevli.SchoolId = okulId;
                    gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi;
                    komisyonRepo.Add(gorevli);

                    gorevli = new SinavGorevli();
                    gorevli.SinavOturumId = snvOturmId;
                    gorevli.TeacherId = komisyonUyesi2;
                    gorevli.SiraNo = 3;
                    gorevli.SchoolId = okulId;
                    gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi;
                    komisyonRepo.Add(gorevli);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static List<SinavGorevli> GetSinavOkulGorevliler(int snvOturmId, int okulId, int SinavGorevId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevliler = gorevliRepo.Find(d => d.SinavOturumId == snvOturmId && d.SinavGorevId == SinavGorevId && d.SchoolId == okulId);

                    return gorevliler.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<SinavGorevli> GetSinavOkulKomisyonGorevliler(int snvOturmId, int okulId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevliler = gorevliRepo.Find(d => d.SinavOturumId == snvOturmId && d.SchoolId == okulId && ((d.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani) || (d.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi)));

                    return gorevliler.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<SinavGorevli> GetSinavOkulKomisyonGorevliler(int snvOturmId, int okulId, int GorevId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevliler = gorevliRepo.Find(d => d.SinavOturumId == snvOturmId && d.SchoolId == okulId && d.SinavGorevId == GorevId);

                    return gorevliler.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<rptSinavGorevlendirme> GetSinavGorevlendirmeForReport(int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                var list = (from grv in db.SinavGorevli
                            join otrm in db.SinavOturum on grv.SinavOturumId equals otrm.SinavOturumId
                            join tch in db.Teacher on grv.TeacherId equals tch.TeacherId
                            join tchUsr in db.User on tch.User.UserId equals tchUsr.UserId
                            join otrokl in db.SinavOturumOkullari on grv.SinavOturumId equals otrokl.SinavOturumId
                            join ogtokl in db.School on tch.SchoolId equals ogtokl.SchoolId
                            join okl in db.School on grv.SchoolId equals okl.SchoolId
                            join snv in db.Sinav on otrm.SinavId equals snv.SinavId
                            where   otrm.SinavOturumId == SinavOturumId && 
                                    grv.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen
                            select new
                            {
                                SinavAdi = snv.SinavAdi,
                                SinavTarihi = (DateTime)otrm.Tarih,
                                SinavSaati = otrm.Saat,
                                SinavOkulAdi = okl.Ad,
                                SinavOkulMebKodu = okl.MebKodu,
                                KadroluOlduguOkulAdi = ogtokl.Ad,
                                PersonelSira = grv.SiraNo,
                                PersonelAdSoyad = tchUsr.Ad + " " + tchUsr.Soyad,
                                PersonelTC = tchUsr.TCKimlik,
                                PersonelGorev = (SG_DAL.Enums.EnumSinavGorev)grv.SinavGorevId,
                                SinavOkulId = okl.SchoolId,
                                KatilimDurumu = grv.SinavKatilimi,
                                OgretmenId = grv.TeacherId,
                                KomisyonBaskani =
                                                (from u1 in db.User
                                                 join t1 in db.Teacher on u1.UserId equals t1.User.UserId
                                                 join g1 in db.SinavGorevli on t1.TeacherId equals g1.TeacherId
                                                 where  g1.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani
                                                 &&     g1.SinavOturumId == SinavOturumId
                                                 &&     g1.SchoolId == okl.SchoolId
                                                 &&     g1.SiraNo == 1
                                                 select new { AdSoyad = u1.Ad + " " + u1.Soyad }
                                                   ).FirstOrDefault().AdSoyad
                                                   ,
                                KomisyonUyesi = 
                                                (from u1 in db.User
                                                 join t1 in db.Teacher on u1.UserId equals t1.User.UserId
                                                 join g1 in db.SinavGorevli on t1.TeacherId equals g1.TeacherId
                                                 where g1.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi
                                                 && g1.SinavOturumId == SinavOturumId
                                                 && g1.SchoolId == okl.SchoolId
                                                 && g1.SiraNo == 2
                                                 select new { AdSoyad2 = u1.Ad + " " + u1.Soyad }
                                                   ).FirstOrDefault().AdSoyad2
                                                   ,
                                KomisyonUyesi2 =
                                                 (from u1 in db.User
                                                  join t1 in db.Teacher on u1.UserId equals t1.User.UserId
                                                  join g1 in db.SinavGorevli on t1.TeacherId equals g1.TeacherId
                                                  where g1.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi
                                                  &&    g1.SinavOturumId == SinavOturumId
                                                  &&    g1.SchoolId == okl.SchoolId
                                                  &&    g1.SiraNo == 3
                                                  select new { AdSoyad3 = u1.Ad + " " + u1.Soyad }
                                                   ).FirstOrDefault().AdSoyad3

                            }).Distinct().OrderBy(d => d.SinavOkulId).ToList();

                List<rptSinavGorevlendirme> sg = new List<rptSinavGorevlendirme>();

                foreach (var item in list)
                {
                    rptSinavGorevlendirme snv = new rptSinavGorevlendirme(item.SinavAdi, item.SinavTarihi, item.SinavSaati, item.SinavOkulAdi, 
                        item.KomisyonBaskani, item.KomisyonUyesi, item.KomisyonUyesi2, item.SinavOkulMebKodu, item.KadroluOlduguOkulAdi, 
                        item.PersonelTC, item.PersonelAdSoyad, item.PersonelSira, item.PersonelGorev.ToString(), item.SinavOkulId, item.KatilimDurumu, item.OgretmenId);
                    sg.Add(snv);
                }
                return sg;
            }
        }

        private static string GetKomisyonUyesi(int SinavOturumId, int okulID, int uyeSira)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    if (uyeSira == 1)
                    {
                        var gorevli = db.SinavGorevli.First(d => d.SinavOturumId == SinavOturumId && d.SchoolId == okulID && d.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi);
                        var tch = db.Teacher.Include("User").First(d => d.TeacherId == gorevli.TeacherId);
                        return tch.User.Ad + " " + tch.User.Soyad;
                    }
                    else
                    {
                        var gorevli = db.SinavGorevli.Last(d => d.SinavOturumId == SinavOturumId && d.SchoolId == okulID && d.SinavGorevId == (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi);
                        var tch = db.Teacher.Include("User").First(d => d.TeacherId == gorevli.TeacherId);
                        return tch.User.Ad + " " + tch.User.Soyad;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static bool GorevdenCikar(int tchID, int snOturumID)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevli = db.SinavGorevli.FirstOrDefault(d => d.TeacherId == tchID && d.SinavOturumId == snOturumID);

                    if (gorevli != null)
                    {
                        var setting = SettingManager.GetSettings();

                        if (setting.GenelBasvuru)
                        {
                            var gorevliler = SinavManager.GetSinavGorevliler(snOturumID, (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen);

                            var ogrt = TeacherManager.GetTeacherListForGenelBasvuru();

                            if (gorevliler.Count() > 0)
                            {
                                foreach (var item in ogrt)
                                {
                                    var yenigorevli = SinavManager.GetSinavGorevli(snOturumID, item.TeacherId);

                                    if (yenigorevli == null)
                                    {
                                        gorevli.TeacherId = item.TeacherId;
                                        db.SaveChanges();
                                        break;                                        
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        return false;
                    }
                    
                    /*if (gorevli != null)
	                {
                        var okulgorevlileri = db.SinavGorevli.Where(d => d.SchoolId == gorevli.SchoolId && d.SinavOturumId == snOturumID);
                        foreach (var item in okulgorevlileri)
                        {
                            item.si
                        }
	                }*/
                    
                    return true;
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }


        public static bool SinavOturumDurumGuncelle(int snvOturmId, int ddlSinavDurum)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    db.SinavOturum.FirstOrDefault(d => d.SinavOturumId == snvOturmId).SinavOturumDurumId = ddlSinavDurum;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static void KatilimGuncelle(List<SinavGorevli> teachers)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    foreach (var item in teachers)
                    {
                        SinavGorevli sg = db.SinavGorevli.FirstOrDefault(d => d.TeacherId == item.TeacherId & d.SinavOturumId == item.SinavOturumId);
                        sg.SinavKatilimi = item.SinavKatilimi;
                        db.SaveChanges();
                        List<SinavGorevli> sgList = db.SinavGorevli.Where(d => d.TeacherId == item.TeacherId && d.SinavKatilimi == true).ToList();
                        db.Teacher.FirstOrDefault(d => d.TeacherId == item.TeacherId).GorevSayisi = sgList.Count();
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
