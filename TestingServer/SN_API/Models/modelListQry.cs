using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class modelListQry
    {
        public string IN_IP { get; set; }
        public string IN_DB { get; set; }
        public string IN_SP { get; set; }
        public string IN_EVENT { get; set; }
        public MODEL_TYPEs IN_DATA { get; set; }
    }
    public class MODEL_TYPEs
    {
        public List<string> MODEL_TYPE { get; set; }
    }
}