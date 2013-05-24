using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        
        public string Ad { get; set; }

        public string Soyad { get; set; }

        public Int64 TCKimlik { get; set; }
        
        public string Email { get; set; }

        public string Sifre { get; set; }
        
        public bool IsAdmin { get; set; }


    }
}
