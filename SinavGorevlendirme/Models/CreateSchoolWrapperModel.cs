using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class CreateSchoolWrapperModel
    {
        public School school { get; set; }
        public List<School> schools { get; set; }

        public CreateSchoolWrapperModel(School school, List<School> schools)
        {
            this.schools = schools;
            this.school = school;
        }
    }
}