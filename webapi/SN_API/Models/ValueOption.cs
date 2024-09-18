using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ValueOption
    {
        public string database { set; get; }
        public string option { set; get; }
        public string value_input { set; get; }
        public string table_name { set; get; }
    }
}