using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class Sinav
    {
        [Key]
        public int SinavId { get; set; }
        public string SinavAdi { get; set; }
        public SinavDurum SinavDurum { get; set; }
        public virtual List<SinavOturum> SinavOturum { get; set; }
    }
}
