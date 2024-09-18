using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class ConfigWeightSetupElement
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }

        public string MODEL_NAME { get; set; }
        public string CTN_GROSS { get; set; }
        public string STANDARD_WEIGHT { get; set; }
        public string SKIP_DATA { set; get; }
        public string DOWN_WEIGHT { get; set; }
        public string UP_WEIGHT { get; set; }
    }
}