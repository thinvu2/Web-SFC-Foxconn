using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class Config4Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string OLD_LINE_NAME { get; set; }
        public string NEW_LINE_NAME { get; set; }
        public string LINE_NAME { get; set; }
        public string SECTION_NAME { get; set; }
        public string MAP_WORK_SECTION { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string CLASS { get; set; }
        public string SERIAL { get; set; }
        public string DAY_DISTINCT { get; set; }
        public string CREATE_DATE { get; set; }
    }
}