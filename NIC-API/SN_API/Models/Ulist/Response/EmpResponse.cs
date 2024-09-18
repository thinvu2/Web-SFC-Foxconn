using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Ulist.Response
{
    public class EmpResponse
    {
        public string ApplicationID { get; set; }
        public List<EmpList> EmpList { get; set; }
        public string Token { get; set; }
    }
    public class EmpList
    {
        public string EMPNO { get; set; }
    }

}