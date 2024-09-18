using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config52Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string GROUP_NAME { get; set; }
        public int STATION_TYPE { get; set; }
        public string PORT_TYPE { get; set; }
        public string MO_TYPE { get; set; }
        public string STATUS { get; set; }
        public string EDIT_EMP { get; set; }
        public string VERIFY_BY { get; set; }
        public string EDIT_TIME { get; set; }
    }
}