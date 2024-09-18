using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class RecipientConfigElement
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MO_NUMBER { get; set; }
        public string SPID { get; set; }
        public string LOCATION { get; set; }
        public string QTY { get; set; }
        public string FLAG { get; set; }
        public string EMP_EDIT { get; set; }
        public string EDIT_TIME { get; set; }
        public string DATA1 { get; set; }
        public string DATA2 { get; set; }
    }
}