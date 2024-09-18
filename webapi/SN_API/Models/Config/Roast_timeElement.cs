using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Roast_timeElement
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string DEFAULT_GROUP { get; set; }
        public string END_GROUP { get; set; }
        public string CONTROL_TIME { get; set; }
    }
}