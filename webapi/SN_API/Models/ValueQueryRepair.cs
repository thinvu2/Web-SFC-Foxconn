using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ValueQueryRepair
    {
        public string database { set; get; }
        public string model_name { set; get; }
        public string mo_number { set; get; }
        public string field { set; get; }
        public string input { set; get; }
        public string date_from { set; get; }
        public string date_to { set; get; }
    }
}