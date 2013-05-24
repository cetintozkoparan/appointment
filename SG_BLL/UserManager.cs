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
                        userRepository.Add(newUser);
                    }
                    else
                    {
                        result = new Result("Aynı TC Kimlik Numaralı kişi sistemde kayıtlı", "error");
                        return result;
                    }
                }
                catch (Exception)
                {
                    result = new Result("Kayıt sırasında hata oluştu", "error");
                    return result;
                }
                result = new Result("Kayıt başarı ile gerçekleşti", "success");
                return result;
            }
        }
    }
}
