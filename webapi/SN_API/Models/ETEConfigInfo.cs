using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ETEConfigInfo
    {
        public string database_name { get; set; }
        public string model_name { get; set; }
        public string group_name { get; set; }
        public string condition { get; set; }
        public string condition_value { get; set; }
        public string emp_pass { get; set; }
        public string reason { get; set; }
        public string solution { get; set; }
        public string ip_address { set; get; }
    }
}