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
    public class UserManager
    {
        public static Result result;

        public static Result CreateUser(User newUser)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var userRepository = new Repository<User>(db);
                    var user = userRepository.Find(d => d.TCKimlik == newUser.TCKimlik);

                    if (user.Count() < 1)
                    {
                        if (string.IsNullOrEmpty(newUser.Sifre))
                        {
                            newUser.Sifre = newUser.TCKimlik.ToString();
                        }
                        
                        userRepository.Add(newUser);
                        result = new Result(SystemRess.Messages.basarili_kayit.ToString(), SystemRess.Messages.basarili_durum.ToString());
                        return result;
                    }
                    else
                    {
                        result = new Result(SystemRess.Messages.hata_ayniTcSistemdeMevcut.ToString(), SystemRess.Messages.hatali_durum.ToString());
                        return result;
                    }
                }
                catch (Exception)
                {
                    result = new Result(SystemRess.Messages.hatali_kayit.ToString(), SystemRess.Messages.hatali_durum.ToString());
                    return result;
                }
            }
        }

        public static User GetUserByTeacherId(int TeacherId)
        {
            using (SGContext db = new SGContext())
            {
                var usr = db.Teacher.Include("User").First(d => d.TeacherId == TeacherId);

                return usr.User;
            }
        }
    }
}
