using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SN_API.Models.Config
{
    public class UploadDataElement
    {
        public string EMP_NO { get; set; }
        public string database_name { get; set; }
        public string MO_NUMBER { get; set; }
        public string MO_TYPE { get; set; }
        public string MODEL_NAME { get; set; }
        public string VERSION_CODE { get; set; }
        public string TARGET_QTY { get; set; }
        public string MO_CREATE_DATE { get; set; }
        public string MO_START_DATE { get; set; }
        public string MO_CLOSE_DATE { get; set; }
    }
}