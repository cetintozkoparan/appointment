﻿using System;
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

namespace SG_BLL
{
    public class TeacherManager
    {
        public static Result result;

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
                
                if(TeacherManager.addTeachers(ogretmenler))
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
                var teacher = db.Teacher.Include("SinavGorevli").First(d => d.TeacherId == TeacherId);
                return teacher;
            }
        }

        public static List<Teacher> GetOkulIdarecileri(int okulID)
        {
            using (SGContext db = new SGContext())
            {
                var teachers = db.Teacher.Include("User").Where(d=>d.Okul.SchoolId == okulID && (d.Unvan == (int)(SG_DAL.Enums.EnumUnvan.Mudur) || d.Unvan == (int)(SG_DAL.Enums.EnumUnvan.MudurYardimcisi)));
                return teachers.ToList();
            }

        }
    }
}
