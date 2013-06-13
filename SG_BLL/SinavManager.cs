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
                    sinav.SinavDurum = db.SinavDurum.FirstOrDefault(d => d.KisaDurum == "Onaylanmadı");
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

        private static List<SinavOturum> GetSinavOturumlari(int SinavId)
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

        public static List<SinavOturum> SinavListe(int DurumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturumlar = db.SinavOturum.Include("Sinav").Where(d => d.Sinav.SinavDurum.SinavDurumId == DurumId);
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

        public static object SinavListe(string durum)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var oturumlar = db.SinavOturum.Include("Sinav").Where(d => d.Sinav.SinavDurum.KisaDurum == durum);
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
                    var oturum = db.SinavOturum.First(d => d.SinavOturumId == SinavOturumId);
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

                    var gorevliler = SinavManager.GetSinavGorevliler(Convert.ToInt32(snvOturmId));

                    foreach (var item in gorevliler)
                    {
                        grvRepo.Delete(item);
                    }

                    var oturumlar = SinavManager.GetSinavOturumOkullari(Convert.ToInt32(snvOturmId));

                    foreach (var item in oturumlar)
                    {
                        snvOtrOklRepo.Delete(item);
                    }

                    foreach (var okul in okullar)
                    {
                        for (int i = 0; i < Convert.ToInt32(hdnPersonelSayi[okulIndex - 1]); i++)
                        {
                            int ogtID = Convert.ToInt32(ogretmenler[genelSira - 1]);
                            int sinavOturumID = Convert.ToInt32(snvOturmId);

                            var gorevli = new SinavGorevli();
                            gorevli.SinavOturumId = Convert.ToInt32(snvOturmId);
                            gorevli.SiraNo = genelSira;
                            gorevli.TeacherId = Convert.ToInt32(ogretmenler[genelSira - 1]);
                            gorevli.SchoolId = okul.SchoolId;
                            gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.Gozetmen;
                            db.SinavGorevli.Add(gorevli);

                            db.SaveChanges();
                            genelSira++;
                        }

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

        public static List<SinavGorevli> GetSinavGorevliler(int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var gorevliRepo = new Repository<SinavGorevli>(db);

                    var gorevliler = gorevliRepo.Find(d => d.SinavOturumId == SinavOturumId);

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
                    gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuBaskani;
                    komisyonRepo.Add(gorevli);

                    gorevli = new SinavGorevli();
                    gorevli.SinavOturumId = snvOturmId;
                    gorevli.TeacherId = komisyonUyesi;
                    gorevli.SchoolId = okulId;
                    gorevli.SinavGorevId = (int)SG_DAL.Enums.EnumSinavGorev.BinaSinavKomisyonuUyesi;
                    komisyonRepo.Add(gorevli);

                    gorevli = new SinavGorevli();
                    gorevli.SinavOturumId = snvOturmId;
                    gorevli.TeacherId = komisyonUyesi2;
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
    }
}
