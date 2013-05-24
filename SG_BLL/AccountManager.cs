using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using SG_DAL.Context;
using SG_DAL.Entities;
using SG_DAL.Pattern;

namespace SG_BLL
{
    public class AccountManager
    {
        public static bool Login(Int64 tckimlikno, string password, bool isadmin)
        {
            using (SGContext db = new SGContext())
            {
                var userRepository = new Repository<User>(db);
                var us = userRepository.Find(d => d.TCKimlik == tckimlikno && d.Sifre == password && d.IsAdmin == isadmin);

                if (us != null)
                {
                    User record = new User();
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, record.Ad + "" + record.Soyad, DateTime.Now, DateTime.Now.AddMinutes(120), false, "Admin", FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    return true;
                }
                else
                    return false;

            }

        }
    }
}
