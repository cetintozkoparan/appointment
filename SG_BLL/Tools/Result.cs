using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_BLL.Tools
{
    public class Result
    {
        public string Message { get; set; }
        public string Status { get; set; }

        public Result(string retMessage, string retStatus)
        {
            this.Status = retStatus;
            this.Message = retMessage;
        }
    }
}
