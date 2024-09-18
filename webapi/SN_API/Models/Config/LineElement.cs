using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class LineElement
    {
        public string database_name { get; set; }
        public string line_name { get; set; }
        public string line_type { get; set; }
        public string line_desc { get; set; }
        public string line_code { get; set; }
        public List<LineName> listLine;
        public class LineName
        {
            public string LINE_NAME { get; set; }
        }
    }
}