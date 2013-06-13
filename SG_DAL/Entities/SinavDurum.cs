using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class SinavDurum
    {
        [Key]
        public int SinavDurumId { get; set; }
        public string Durum { get; set; }
        public string KisaDurum { get; set; }
    }
}
