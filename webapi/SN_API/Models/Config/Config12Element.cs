using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config12Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string BOM_NO { get; set; }
        public string KEY_PART_NO { get; set; }
        public int KP_RELATION { get; set; }
        public int KP_COUNT { get; set; }
        public string WORK_DATE { get; set; }
        public string GROUP_NAME { get; set; }
        public string VERSION_CODE { get; set; }
        public string TYPE { get; set; }
        public int KP_USE_QTY { get; set; }
        public string BOM_NAME { get; set; }
        public string type { get; set; }
        public string BOM_COPY { get; set; }
        public string OPTION_LINK { get; set; }
    }
}