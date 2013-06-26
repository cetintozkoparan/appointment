using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class CreateTeacherWrapperModel
    {
        public Teacher teacher { get; set; }
        public List<Teacher> teachers { get; set; }

        public CreateTeacherWrapperModel(Teacher teacher, List<Teacher> teachers)
        {
            this.teachers = teachers;
            this.teacher = teacher;
        }
    }
}