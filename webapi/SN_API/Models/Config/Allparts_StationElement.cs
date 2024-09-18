using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Allparts_StationElement
    {

        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }

        public string MODEL_NAME { get; set; }
        public string VERSION { get; set; }
        public string GROUP_NAME { get; set; }
        public string DATA1 { get; set; }
        public string DATA2 { get; set; }
        public List<Group> listGroup;
    }
}