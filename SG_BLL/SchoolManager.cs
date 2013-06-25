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

namespace SG_BLL
{
    public class SchoolManager
    {
        public static Result result;

        public static Result add(School school)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var repo = new Repository<School>(db);
                    var sch = repo.Find(d => d.Ad == school.Ad && d.IsDeleted == false);
                    var schmeb = repo.Find(d => d.MebKodu == school.MebKodu && d.IsDeleted == false);

                    if (sch.Count() > 0)
                    {
                        result = new Result(SystemRess.Messages.hata_ayniOkulSistemdeMevcut.ToString(), SystemRess.Messages.hatali_durum.ToString());
                        return result;
                    }
                    else if (schmeb.Count() > 0)
                    {
                        result = new Result(SystemRess.Messages.hata_ayniMebKoduSistemdeMevcut.ToString(), SystemRess.Messages.hatali_durum.ToString());
                        return result;
                    }
                    repo.Add(school);
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

        public static List<School> GetSchools()
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var repo = new Repository<School>(db);
                    var schoolList = repo.Listele().Where(d=>d.IsDeleted == false);
                    return schoolList.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<School> GetSinavOturumOkullari(int SinavOturumId)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var sinavokullar = db.SinavOturum.Include("Okullar").FirstOrDefault(d => d.SinavOturumId == SinavOturumId);
                    var okulListesi = new List<School>();
                    
                    okulListesi.AddRange(sinavokullar.Okullar);
                    foreach (var item in okulListesi)
                    {
                        var oturumokulu = SinavManager.GetSinavOturumOkulu(item.SchoolId, SinavOturumId);
                        if (oturumokulu == null)
                        {
                            item.SalonSayisi = 0;
                        }
                        else
                        {
                            item.SalonSayisi = oturumokulu.SalonSayisi;
                        }
                    }

                    return okulListesi.OrderBy(d=>d.MebKodu).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static School GetSchoolByTCNo(Int64 tcno)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    Teacher us = db.Teacher.Include("Okul").Include("User").FirstOrDefault(d=>d.User.TCKimlik == tcno);

                    return us.Okul;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
