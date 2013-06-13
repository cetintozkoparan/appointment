using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_BLL.Tools
{
    public class UnvanHelper
    {
        public int UnvanId { get; set; }
        public string Unvan { get; set; }

        public UnvanHelper(int UnvanId, string Unvan)
        {
            this.Unvan = Unvan;
            this.UnvanId = UnvanId;
        }
    }
}
