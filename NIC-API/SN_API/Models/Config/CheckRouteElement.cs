using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class CheckRouteElement
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string MO_NUMBER { get; set; }
        public string ROUTE_CODE { get; set; }
        public string ROUTE_NAME { get; set; }
        public string ROUTE_DESC { get; set; }
    }
}