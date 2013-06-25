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
using System.IO;
using System.Web;
using SG_BLL.Tools.Report;

namespace SG_BLL
{
    public class TeacherManager
    {
        public static Result result;

        public static Result updateTeacher(User newUser, Teacher teacher)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var userRepository = new Repository<User>(db);
                    var tchRepo = new Repository<Teacher>(db);
                    var user = userRepository.First(d => d.UserId == newUser.UserId);
                    var tch = tchRepo.First(d => d.TeacherId == teacher.TeacherId);
                    
                    user.Ad = newUser.Ad;
                    user.Email = newUser.Email;
                    user.Soyad = newUser.Soyad;
                    user.TCKimlik = newUser.TCKimlik;
                    user.Tel = newUser.Tel;

                    tch.GenelBasvuru = teacher.GenelBasvuru;
                    tch.Kidem = teacher.Kidem;
                    tch.SchoolId = teacher.SchoolId;
                    tch.Unvan = teacher.Unvan;

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

        public static Result addTeacher(User newUser, Teacher teacher)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var userRepository = new Repository<User>(db);
                    var user = userRepository.Find(d => d.TCKimlik == newUser.TCKimlik);

                    if (user.Count() > 0)
                    {
                        result = new Result(SystemRess.Messages.hata_ayniTcSistemdeMevcut.ToString(), SystemRess.Messages.hatali_durum.ToString());
                        return result;
                    }
                    else if (string.IsNullOrEmpty(newUser.Sifre))
                    {
                        newUser.Sifre = newUser.TCKimlik.ToString();
                    }
                    var okulRepo = new Repository<School>(db);
                    int okulID = teacher.Okul.SchoolId;
                    var okul = okulRepo.Find(d => d.SchoolId == okulID);
                    teacher.Okul = okul.First();
                    if (teacher.Unvan == 1)
                        newUser.Rol = (int)SG_DAL.Enums.EnumRol.ogretmen;
                    else
                        if (teacher.Unvan == 2 || teacher.Unvan == 3)
                            newUser.Rol = (int)SG_DAL.Enums.EnumRol.idareci;

                    teacher.User = newUser;
                    teacher.GorevSayisi = 0;
                    var teacherRepo = new Repository<Teacher>(db);
                    teacherRepo.Add(teacher);
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

        public static Result addTeacher(HttpPostedFileBase file)
        {
            string path = "~/Content/files/";
            string retval = FileManager.FileUpload(file, path);
            if (!retval.Equals(""))
            {
                List<Teacher> ogretmenler = FileManager.ReadTeachersFromExcel(HttpContext.Current.Server.MapPath(retval));

                if (TeacherManager.addTeachers(ogretmenler))
                    result = new Result(SystemRess.Messages.basarili_kayit.ToString(), SystemRess.Messages.basarili_durum.ToString());
            }
            else
            {
                result = new Result(SystemRess.Messages.hatali_kayit.ToString(), SystemRess.Messages.hatali_durum.ToString());
            }
            return result;
        }

        private static bool addTeachers(List<Teacher> ogretmenler)
        {
            using (SGContext db = new SGContext())
            {
                try
                {
                    var userRepository = new Repository<Teacher>(db);

                    foreach (var item in ogretmenler)
                        userRepository.Add(item);
                    //db.Teacher.Add(item);
                    //db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static List<Teacher> GetTeacherList()
        {
            using (SGContext db = new SGContext())
            {
                var list = db.Teacher.Include("User").Include("Okul").Where(d => d.User.IsDeleted == false).ToList();
                return list;
            }
        }

        public static List<rptOgretmenListe> GetTeacherListForReport()
        {
            using (SGContext db = new SGContext())
            {
                var list = (from ogtLer in db.Teacher
                            join o in db.User on ogtLer.User.UserId equals o.UserId
                            where ogtLer.IsDeleted == false
                            select new
                            {
                                Ad = o.Ad,
                                Soyad = o.Soyad,
                                TCKimlik = o.TCKimlik,
                                Email = o.Email,
                                Tel = o.Tel,
                                Kidem = ogtLer.Kidem,
                                Unvan = ogtLer.Unvan
                            }).ToList();

                List<rptOgretmenListe> ogtler = new List<rptOgretmenListe>();

                foreach (var item in list)
                {
                    rptOgretmenListe ogt = new rptOgretmenListe(item.Ad, item.Soyad, item.TCKimlik, item.Email, item.Tel, item.Kidem, item.Unvan);
                    ogtler.Add(ogt);
                }
                return ogtler;
            }
        }

        public static List<Teacher> GetTeacherListForGenelBasvuru()
        {
            using (SGContext db = new SGContext())
            {
                var list = db.Teacher.Include("User")
                                     .Include("Okul")
                                     .Where(d => d.User.IsDeleted == false && d.GenelBasvuru == true && d.Unvan == (int)SG_DAL.Enums.EnumUnvan.Ogretmen)
                                     .ToList();
                return list;
            }
        }

        public static Teacher GetTeacherForGenelBasvuru(int TeacherId)
        {
            using (SGContext db = new SGContext())
            {
                var teacher = db.Teacher.Include("User")
                                     .Include("Okul")
                                     .First(d => d.User.IsDeleted == false && d.GenelBasvuru == true && d.TeacherId == TeacherId);
                return teacher;
            }
        }


        public static List<Teacher> GetTeacherListForSinav()
        {
            using (SGContext db = new SGContext())
            {
                var list = new List<Teacher>();

                if (db.Setting.First().GenelBasvuru)
                {
                    list = db.Teacher.Include("User").Where(d => d.User.IsDeleted == false).ToList();
                }

                return list;
            }
        }

        public static Teacher GetTeacher(int TeacherId)
        {
            using (SGContext db = new SGContext())
            {
                var teacher = db.Teacher.Include("SinavGorevli").Include("Okul").First(d => d.TeacherId == TeacherId);
                return teacher;
            }
        }

        public static List<Teacher> GetOkulIdarecileri(int okulID)
        {
            using (SGContext db = new SGContext())
            {
                var teachers = db.Teacher.Include("User").Where(d => d.Okul.SchoolId == okulID && (d.Unvan == (int)(SG_DAL.Enums.EnumUnvan.Mudur) || d.Unvan == (int)(SG_DAL.Enums.EnumUnvan.MudurYardimcisi)));
                return teachers.ToList();
            }

        }

        public static List<Teacher> GetTeacherListBySchoolId(int schID)
        {
            using (SGContext db = new SGContext())
            {
                var list = db.Teacher.Include("User").Include("Okul").Where(d => d.User.IsDeleted == false & d.SchoolId == schID).ToList();
                return list;
            }
        }

        public static Teacher GetTeacherByTCNo(long tcno)
        {
            using (SGContext db = new SGContext())
            {
                var list = db.Teacher.Include("User").Include("Okul").FirstOrDefault(d => d.User.TCKimlik == tcno);
                return list;
            }
        }

        public static void GenelBasvuruGuncelle(bool gnlBasvuru)
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = HttpContext.Current.Request.Cookies["LoginCookie"];
            Int64 tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());

            using (SGContext db = new SGContext())
            {
                db.Teacher.Include("User").FirstOrDefault(d => d.User.TCKimlik == tcno).GenelBasvuru = gnlBasvuru;
                db.SaveChanges();
            }
        }

        public static Result SifreDegistir(string sifre)
        {
            HttpCookie myCookie = new HttpCookie("LoginCookie");
            myCookie = HttpContext.Current.Request.Cookies["LoginCookie"];
            long tcno = Convert.ToInt64(myCookie.Value.Split('=')[1].ToString());

            using (SGContext db = new SGContext())
            {
                try
                {
                    User us = TeacherManager.GetTeacherByTCNo(tcno).User;
                    us.Sifre = sifre;
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
