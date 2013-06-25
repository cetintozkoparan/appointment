using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class SinavOturum
    {
        [Key]
        public int SinavOturumId { get; set; }

        public SinavOturum()
        {
            this.Okullar = new List<School>();
            //this.Ogretmen = new List<Teacher>();
        }

        public virtual ICollection<School> Okullar { get; set; }

        //public virtual SinavGorevli SinavGorevli { get; set; }
        public virtual ICollection<SinavOturumOkullari> SinavOturumOkullari { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}")]
        public Nullable<DateTime> Tarih { get; set; }
        public string Saat { get; set; }
        public int OturumNo { get; set; }
        public int SinavId { get; set; }
        public Sinav Sinav { get; set; }

        public int SinavOturumDurumId { get; set; }
    }
}
