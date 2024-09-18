using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class ConfigJigElement
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string TYPE_VALUE { get; set; }
        public string VERSION { get; set; }
        public string ATTRIBUTE_VALUE { get; set; }
        public string ATTRIBUTE_DESC1 { get; set; }
    }
}