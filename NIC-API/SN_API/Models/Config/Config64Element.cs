using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config64Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }

        public string TYPE_NAME { get; set; }
        public string DEFAULT_PREFIX { get; set; }
        public string DEFAULT_POSTFIX { get; set; }
        public string DEFAULT_STRING { get; set; }
        public string DEFAULT_LENGHT { get; set; }
        public string DATA_LOCATION { get; set; }
        public string CHECK_UNIQUE { get; set; }
        public string UPLOAD_TO { get; set; }
        public string USE_TOKEN { get; set; }
        public string CAM_QTY { get; set; }
        public string ACTION_TYPE { get; set; }

    }
    public class CopyModel_TmmReportt
    {
        public string database_name { get; set; }
        public string MODEL_NAME_OLD { get; set; }
        public string MODEL_NAME { get; set; }
        public string EMP { get; set; }

    }
}