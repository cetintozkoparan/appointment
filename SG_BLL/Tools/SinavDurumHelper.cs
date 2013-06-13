using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_BLL.Tools
{
    public class SinavDurumHelper
    {
        public int SinavDurumId { get; set; }
        public string Durum { get; set; }
        public string KisaDurum { get; set; }

        public SinavDurumHelper(int SinavDurumId, string Durum, string KisaDurum)
        {
            this.Durum = Durum;
            this.KisaDurum = KisaDurum;
            this.SinavDurumId = SinavDurumId;
        }
    }
}
