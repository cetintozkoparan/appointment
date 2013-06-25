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
    public class SettingManager
    {
        public static Result result;

        public static Result UpdateSettings(Setting newsettings)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var settings = db.Setting.First();

                    settings.SalonPersonelSayisi = newsettings.SalonPersonelSayisi;
                    settings.GenelBasvuru = newsettings.GenelBasvuru;
                    db.SaveChanges();

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

        public static Setting GetSettings()
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var settingRepo = new Repository<Setting>(db);
                    var settings = settingRepo.First();
                    return settings;
                }catch(Exception){
                    return new Setting();
                }
            }
        }

        public static object UpdateSiralama(Setting newsettings)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var settings = db.Setting.First();

                    settings.GozetmenSiralama1 = newsettings.GozetmenSiralama1;
                    settings.GozetmenSiralama2 = newsettings.GozetmenSiralama2;
                    settings.GozetmenSiralama3 = newsettings.GozetmenSiralama3;
                    db.SaveChanges();

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
    }
}
