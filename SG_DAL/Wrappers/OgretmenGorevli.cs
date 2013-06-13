using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Wrappers
{
    public class OgretmenGorevli
    {
        public int TeacherId { get; set; }
        public int SiraNo { get; set; }
        
        public OgretmenGorevli(int TeacherId, int SiraNo)
        {
            this.SiraNo = SiraNo;
            this.TeacherId = TeacherId;
        }
    }
}
