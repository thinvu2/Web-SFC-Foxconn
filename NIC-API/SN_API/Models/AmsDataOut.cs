using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class AmsDataOut
    {
        public string ApplicationID { get; set; }

        public List<SetEmpInfo> EmpList { get; set; }
    }

    public class SetEmpInfo
    {
        public string USERID { get; set; }
        public string EMPNO { get; set; }
        public string NAME { get; set; }
        public string GroupName { get; set; }
        public string GroupRemark { get; set; }
        public string EffectTime { get; set; }
    }
}