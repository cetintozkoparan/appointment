using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class SinavBasvuru
    {
        [Key]
        public int SinavBasvuruId { get; set; }

        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        
        public int SinavOturumId { get; set; }
    }
}
