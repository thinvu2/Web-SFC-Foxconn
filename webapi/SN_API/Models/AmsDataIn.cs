using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class AmsDataIn
    {
        public string ApplicationID { set; get; }
        public List<item> EmpList { set; get; }
        public string Token { set; get; }
    }

    public class item
    {
        public string EMPNO { get; set; }
    }
}