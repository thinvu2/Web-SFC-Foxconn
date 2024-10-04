using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ErrorDataQry
    {
        public string IN_IP { get; set; }
        public string IN_DB { get; set; }
        public string IN_SP { get; set; }
        public string IN_EVENT { get; set; }
        public ERROR_DATA IN_DATA { get; set; }
    }
    public class ERROR_DATA
    {
        public List<string> MODEL_NAME { get; set; }
        public string FROM_DATE { get; set; }
    }
}