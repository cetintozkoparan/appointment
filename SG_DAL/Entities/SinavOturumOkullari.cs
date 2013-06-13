using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class SinavOturumOkullari
    {
        [Key]
        public int SinavOturumOkulId { get; set; }

        public int SchoolId { get; set; }

        public int SinavOturumId { get; set; }

        public int SalonSayisi { get; set; }

        public int AsilGozetmenSayisi { get; set; }
        public int YedekGozetmenSayisi { get; set; }
    }
}
