using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class SinavGorevli
    {
        [Key]
        public int SinavGorevliId { get; set; }

        public int SinavOturumId { get; set; }

        public int SchoolId { get; set; }

        public int TeacherId { get; set; }

        public int SiraNo { get; set; }

        public int SinavGorevId { get; set; }
    }
}
