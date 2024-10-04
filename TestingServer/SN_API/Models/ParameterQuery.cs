using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ParameterQuery
    {
        public string IN_IP { get; set; }
        public string IN_DB { get; set; }
        public string IN_SP { get; set; }
        public string IN_EVENT { get; set; }
        public IN_DATA IN_DATA { get; set; }
    }

    public class IN_DATA
    {
        public List<string> MODEL_NAME { get; set; }
        public List<string> GROUP_NAME { get; set; }
        public string FROM_DATE { get; set; }
    }


}