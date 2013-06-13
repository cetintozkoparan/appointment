using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class TeacherListWrapperModel
    {
        public List<User> user { get; set; }
        public List<Teacher> teacher { get; set; }

        public TeacherListWrapperModel(List<User> user, List<Teacher> teacher)
        {
            this.user = user;
            this.teacher = teacher;
        }
    }
}