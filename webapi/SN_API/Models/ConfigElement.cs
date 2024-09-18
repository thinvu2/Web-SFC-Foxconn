using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ConfigElement
    {
        public string database_name { get; set; }
        public string emp_no { get; set; }
        public string fun { set; get; }

    }
    public class ConfigChangePassWord
    {
        public string database_name { get; set; }
        public string EMP_NO { get; set; }
        public string PASSWORD { get; set; }
        public string NEW_PASSWORD { get; set; }
    }
}