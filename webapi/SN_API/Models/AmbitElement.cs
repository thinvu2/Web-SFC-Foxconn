using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class AmbitElement
    {
        public string database_name { get; set; }
        public string type { get; set; }
        public string mo_number { get; set; }
        public string model_name { get; set; }
        public string line_name { get; set; }
        public bool isNotlink { get; set; }
        public List<Type> mo_value { get; set; }
        public string group_value { get; set; }
        public string motype_value { get; set; }
        public string mo_state { get; set; }
        public List<Type> model_value { get; set; }
        public string section_value { get; set; }
        public string sort_by { get; set; }
        public bool show_line { get; set; }      
        public bool isFG { get; set; }
        public bool isORTWip { get; set; }
        
    }
}