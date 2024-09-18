using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class EtEConfigElement
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string GROUP_NAME { get; set; }
        public string TYPE { get; set; }
        public string CONDITION { get; set; }
        public string CREATE_TIME { get; set; }
        public string STATUS { get; set; }
        public int CURRENT_DATA { get; set; }
        public string ERROR_CODE { get; set; }
        public int QTY { get; set; }
        public string DATA { get; set; }
    }
}