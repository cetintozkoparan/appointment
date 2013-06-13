using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class TeacherWrapperModel
    {
        public User user { get; set; }
        public Teacher teacher { get; set; }
        public School okul { get; set; }

        public TeacherWrapperModel(User user, Teacher teacher, School okul)
        {
            this.user = user;
            this.teacher = teacher;
            this.okul = okul;
        }
    }
}