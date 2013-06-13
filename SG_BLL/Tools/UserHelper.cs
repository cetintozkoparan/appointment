using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_BLL.Tools
{
    public class UserHelper
    {
        public int TeacherId { get; set; }
        public string AdSoyad { get; set; }

        public UserHelper(int TeacherId, string AdSoyad)
        {
            this.AdSoyad = AdSoyad;
            this.TeacherId = TeacherId;
        }
    }
}
