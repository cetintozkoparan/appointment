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
        public static User Login(Int64 tckimlikno, string password)
        {
            using (SGContext db = new SGContext())
            {
                var userRepository = new Repository<User>(db);
                var us = userRepository.Find(d => d.TCKimlik == tckimlikno && d.Sifre == password);

                if (us.Count() > 0)
                {
                    User record = new User();
                    SG_DAL.Enums.EnumRol rol = (SG_DAL.Enums.EnumRol)(us.FirstOrDefault().Rol);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, us.FirstOrDefault().Ad + " " + us.FirstOrDefault().Soyad, DateTime.Now, DateTime.Now.AddMinutes(120), false, rol.ToString(), FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    HttpCookie myCookie = new HttpCookie("LoginCookie");
                    myCookie["tcno"] = us.FirstOrDefault().TCKimlik.ToString();
                    myCookie.Expires = DateTime.Now.AddDays(1d);
                    HttpContext.Current.Response.Cookies.Add(myCookie);

                    return us.FirstOrDefault();
                }
                else
                    return null;

            }

        }
    }
}
