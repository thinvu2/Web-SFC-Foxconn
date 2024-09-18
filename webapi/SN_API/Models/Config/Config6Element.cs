using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config6Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string MODEL_SERIAL { get; set; }
        public string MODEL_TYPE { get; set; }
        public string BOM_NO { get; set; }
        public string STANDARD { get; set; }
        public string CUSTOMER { get; set; }
        public string MODEL_RANGE1 { get; set; }
        public string MODEL_RANGE2 { get; set; }
        public string ROUTE_CODE { get; set; }
        public string DEFAULT_GROUP { get; set; }
        public string END_GROUP { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string LEAD_FREE { get; set; }
        public string REPAIR_1A2A_PROCESS { get; set; }
        public string VERSION_CODE { get; set; }
        public string CHECKFLAG { get; set; }
        public string INI_FNAME { get; set; }
        public string INI_FVER { get; set; }
        public string BOARD_TYPE { get; set; }
    }
}