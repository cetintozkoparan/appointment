using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class Setting
    {
        [Key]
        public int Setting_Id { get; set; }
        public int SalonPersonelSayisi { get; set; }
        public bool GenelBasvuru { get; set; }
        public int GozetmenSiralama1 { get; set; }
        public int GozetmenSiralama2 { get; set; }
        public int GozetmenSiralama3 { get; set; }
        public string KurumAdi { get; set; }
    }
}
