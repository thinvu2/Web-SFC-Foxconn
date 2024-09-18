using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ValueQueryModel
    {
        public string database { set; get; }
        public string table { set; get; }
        public string model { set; get; }
        public string mo_number { set; get; }
        public string version { set; get; }
        public string group { set; get; }
        public string location { set; get; }
        public string RB_check { set; get; }
        public bool period { set; get; }
        public string date_from { set; get; }
        public string date_to { set; get; }
        public bool checkbymo { set; get; }
        public string cust_po { set; get; }
        public bool visible { set; get; }
        public string CSQ { get; set; }
        public string model_name { get; set; }
        public string where_condition { get; set; }
        public string where_value { get; set; }
        public string where_fiel1 { get; set; }
        public string where_fiel2 { get; set; }
        public string where_fiel3 { get; set; }
    }
}