using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        [Required(ErrorMessage="Lütfen okul adı giriniz.")]
        public string Ad { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Lütfen MEB Kodunu giriniz.")]
        public int MebKodu { get; set; }
        public School()
        {
            this.SinavOturum = new List<SinavOturum>();
        }

        public virtual SinavGorevli SinavGorevli { get; set; }
        
        public virtual int SalonSayisi { get; set; }

        public virtual ICollection<SinavOturum> SinavOturum { get; set; }
        public virtual ICollection<SinavOturumOkullari> SinavOturumOkullari { get; set; }
    }
}
