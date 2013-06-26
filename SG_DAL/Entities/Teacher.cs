using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public virtual User User { get; set; }
        public virtual School Okul { get; set; }
        public int SchoolId { get; set; }

        public virtual SinavGorevli SinavGorevli { get; set; }
        public virtual ICollection<SinavBasvuru> SinavBasvuru { get; protected set; }

        public string Kidem { get; set; }
        public int Unvan { get; set; }
        public bool IsDeleted { get; set; }
        public bool GenelBasvuru { get; set; }
        public int GorevSayisi { get; set; }
    }
}
