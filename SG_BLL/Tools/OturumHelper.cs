using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_BLL.Tools
{
    public class OturumHelper
    {
        public int SinavOturumId { get; set; }
        public string Oturum { get; set; }

        public OturumHelper(int SinavOturumId, string Oturum)
        {
            this.SinavOturumId = SinavOturumId;
            this.Oturum = Oturum;
        }
    }
}
