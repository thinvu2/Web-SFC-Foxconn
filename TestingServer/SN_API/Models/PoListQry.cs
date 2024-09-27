using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class PoListQry
    {
        public string IN_IP { get; set; }
        public string IN_DB { get; set; }
        public string IN_SP { get; set; }
        public string IN_EVENT { get; set; }
        public PO_LISTS IN_DATA { get; set; }
    }
    public class PO_LISTS
    {
        public List<string> PO_LIST { get; set; }
    }
}